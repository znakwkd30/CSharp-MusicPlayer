using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
            Load();
            this.mePlayer.MediaEnded += mePlayer_MediaEnded;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void Load()
        {
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\bungee.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\trueshow.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\cheerup.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\feelspecial.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\lost.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\ugotit.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\pretty.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\dama.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\dula.mp3"));
            App.uris.Add(new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\umpahumpah.mp3"));
        }

        private void PlayButton(object sender, RoutedEventArgs e)
        {
            mePlayer.Source = App.uris[idx];
            mePlayer.Play();
            idx++;
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
            if (idx.Equals(0)) return;

            mePlayer.Stop();
            idx--;
            mePlayer.Source = App.uris[idx];
            mePlayer.Play();
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

    }
}
