using CefSharp;
using CefSharp.Wpf;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool userIsDraggingSlider = false;
        private int idx = 0;
        private ChromiumWebBrowser browser;

        public MainWindow()
        {
            InitializeComponent();
            InitBrowser();
            App.musicSource.Load();
            musicList.ItemsSource = App.musicSource.musics;
            this.mePlayer.MediaEnded += mePlayer_MediaEnded;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void PlayButton(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }

        private void PauseButton(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void StopButton(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }

        void mePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if(idx >= App.uris.Count - 1)
            {
                idx = 0;
                mePlayer.Source = App.uris[idx];
                mePlayer.Play();
                return;
            }

            idx++;
            mePlayer.Source = App.uris[idx];
            mePlayer.Play();
        }
        private void folderOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mePlayer.Source = new Uri(openFileDialog.FileName);
                mePlayer.Play();
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mePlayer.Position.TotalSeconds;
            }
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        async void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            var youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCdYzrgvTCowhvOxk9Yd92RdM5o857e8io", // Api Key 지정
                ApplicationName = "CSharp Music",
            });

            var request = youtube.Search.List("snippet");
            request.Q = txtSearch.Text;
            request.MaxResults = 10;

            var result = await request.ExecuteAsync();

            searchList.Items.Clear();


            foreach (var item in result.Items)
            {
                if(item.Id.Kind == "youtube#video" || item.Id.Kind == "youtube#playlist")
                {
                    //searchList.Items.Add(item.Snippet.Title);
                    Debug.WriteLine(result);

                    Music music = new Music();
                    music.Name = item.Snippet.Title;
                    string youtubeUrl = "http://youtube.com/watch?v=" + item.Id.VideoId;
                    music.uri = new Uri(youtubeUrl, UriKind.Absolute);
                    searchList.Items.Add(music);
                }
            }
            searchList.Items.Refresh();
        }

        private void SearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (searchList.SelectedItem == null) return;

            string selectItem = searchList.SelectedItem.ToString();
            string item = selectItem.ToString();
        }

        async private void SearchList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Music music = (searchList.SelectedItem as Music);
            
            
            var videoId = "";
            if (searchList.SelectedItems.Count > 0)
            {
                var youtube = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyCdYzrgvTCowhvOxk9Yd92RdM5o857e8io", // Api Key 지정
                    ApplicationName = "CSharp Music",
                });

                var request = youtube.Search.List("snippet");
                request.Q = searchList.SelectedItem.ToString();
                request.MaxResults = 5;

                var result = await request.ExecuteAsync();

                foreach (var item in result.Items)
                {
                    if (item.Id.Kind == "youtube#video" /*|| item.Id.Kind == "youtube#playlist"*/)
                    {
                        // YouTube 비디오 Play를 위한 URL 생성
                        videoId = item.Id.VideoId;
                        Debug.WriteLine(item.Id.VideoId);
                    }
                }

                //Console.WriteLine(videoId);
                string youtubeUrl = "http://youtube.com/watch?v=" + videoId;
                web.Source = new Uri(youtubeUrl);
                // 디폴트 브라우져에서 실행
                //Process.Start(youtubeUrl);
            }
        }

        private void ThisIsCalledWhenPropertyIsChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void InitBrowser()
        {
            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs.Add("disable-usb-keyboard-detect", "1");
            Cef.Initialize(settings);

            browser = new ChromiumWebBrowser();
            browser.Address = "http://youtube.com";
            youtube.Children.Add(browser);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string youtubeUrl = "http://youtube.com";
            web.Source = new Uri(youtubeUrl);
        }
    }
}