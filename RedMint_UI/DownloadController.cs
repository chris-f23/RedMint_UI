using System;
using System.Collections.Generic;
using System.Text;
using YoutubeExtractor;

namespace RedMint_UI
{
    public enum DownloadQuality
    {
        High,
        Medium,
        Low
    }

    interface IDownloadController
    {
        void DownloadVideo(string url, DownloadQuality quality);
        void DownloadAudio(string url, DownloadQuality quality);
    }

    public class DownloadController : IDownloadController
    {
        private string ParseUrl(string url) 
        {
            throw new NotImplementedException();
        }

        public void DownloadVideo(string url, DownloadQuality quality)
        {
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);


        }

        public void DownloadAudio(string url, DownloadQuality quality)
        {
            throw new NotImplementedException();
        }
    }
}
