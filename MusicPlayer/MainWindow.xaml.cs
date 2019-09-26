using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
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
            mePlayer.Source = App.uris[idx];
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

        private void nextMusic(object sender, RoutedEventArgs e)
        {
            if (idx.Equals(0)) return;

            if (App.uris.Count.Equals(idx))
            {
                idx = 0;
                mePlayer.Source = App.uris[idx];
                mePlayer.Play();
                return;
            }

            mePlayer.Stop();
            mePlayer.Source = App.uris[idx];
            mePlayer.Play();
            idx++;
        }
        private void preMusic(object sender, RoutedEventArgs e)
        {
            if (idx <= 0)
            {
                idx = App.uris.Count - 1;
                mePlayer.Source = App.uris[idx];
                mePlayer.Play();
                return;
            }

            mePlayer.Stop();
            idx--;
            mePlayer.Source = App.uris[idx];
            mePlayer.Play();
        }

        private void replayMusic(object sender, RoutedEventArgs e)
        {
            if (idx.Equals(0)) return;

            idx--;
            mePlayer.Source = App.uris[idx];
        }

        private void shuffleMusic(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            idx = random.Next(0, App.uris.Count);
            mePlayer.Source = App.uris[idx];
            mePlayer.Play();
            return;
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

        private void MusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectName = ((Music)(musicList.SelectedItem)).Name;

            foreach(Music music in App.musicSource.musics)
            {
                if (music.Name.Equals(selectName))
                {
                    idx = music.index - 1;
                    mePlayer.Source = music.uri;
                    mePlayer.Play();
                }
            }
        }

        async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //YoutubeData youtubeData = new YoutubeData();

            var youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyCdYzrgvTCowhvOxk9Yd92RdM5o857e8io", // Api Key 지정
                ApplicationName = "CSharp Music",
            });

            var request = youtube.Search.List("snippet");
            request.Q = txtSearch.Text;
            request.MaxResults = 10;

            var result = await request.ExecuteAsync();

            //int j = searchList.Items.Count;
            //for (int i = 0; i < j; i++)
            //{
            //    searchList.Items.RemoveAt(0);
            //}

            searchList.Items.Clear();


            foreach (var item in result.Items)
            {
                if(item.Id.Kind == "youtube#video" || item.Id.Kind == "youtube#playlist")
                {
                    searchList.Items.Add(item.Snippet.Title);
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
                    if (item.Id.Kind == "youtube#video" || item.Id.Kind == "youtube#playlist")
                    {
                        // YouTube 비디오 Play를 위한 URL 생성
                        videoId = item.Id.VideoId;
                    }
                }

                string youtubeUrl = "http://youtube.com/watch?v=" + videoId;
                web.Source = new Uri(youtubeUrl);
                // 디폴트 브라우져에서 실행
                //Process.Start(youtubeUrl);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}