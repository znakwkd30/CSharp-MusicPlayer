using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<Uri> uris = new List<Uri>();
        public static MusicSource musicSource = new MusicSource();
        public static YoutubeDataSource youtubeDataSources = new YoutubeDataSource();
    }
}
