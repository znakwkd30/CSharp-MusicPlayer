using DocumentFormat.OpenXml.Wordprocessing;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class MusicSource
    {
        bool isLoaded = false;

        public List<Music> musics;

        public void Load()
        {

            if (isLoaded) return;

            musics = new List<Music>()
            {
                //new Music() { Name = "Bungee", index = 1, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\bungee.mp3") },
                //new Music() { Name = "트루먼 쇼", index = 2, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\trueshow.mp3") },
                //new Music() { Name = "Cheer Up!", index = 3, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\cheerup.mp3") },
                //new Music() { Name = "Feel Special", index = 4, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\feelspecial.mp3") },
                //new Music() { Name = "우리집을 못 찾겠군요", index = 5, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\lost.mp3") },
                //new Music() { Name = "U GOT IT", index = 6, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\ugotit.mp3") },
                //new Music() { Name = "이뻐이뻐", index = 7, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\pretty.mp3") },
                //new Music() { Name = "담아", index = 8, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\dama.mp3") },
                //new Music() { Name = "덜어", index = 9, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\dula.mp3") },
                //new Music() { Name = "음파음파", index = 10, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\umpahumpah.mp3") },
                //new Music() { Name = "팔레트", index = 11, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\palette.mp3") },
                //new Music() { Name = "문제", index = 12, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\problem.mp3") },
                //new Music() { Name = "25", index = 13, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\25.mp3") },
                //new Music() { Name = "워커홀릭", index = 14, uri = new Uri("C:\\Users\\kl\\source\\repos\\MusicPlayer\\MusicPlayer\\music\\workaholic.mp3") },
            };

            Console.WriteLine(musics.Count);

            MusicAdd();
        }

        public void MusicAdd()
        {
            for (int i = 0; i <= musics.Count; i++)
            {
                if (i.Equals(musics.Count)) return;
                App.uris.Add(musics[i].uri);
            }
        }
    }
}
