using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    public class YoutubeDataSource
    {
        public List<YoutubeData> youtubeDatas;

        public void youtubeDataLoad(string plId, string mainTitle)
        {
            youtubeDatas = new List<YoutubeData>()
            {
                new YoutubeData() { videoId=plId , title=mainTitle }
            };
        }
    }
}
