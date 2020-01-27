using System;
using System.Collections.Generic;
using System.Text;
using YoutubeExtractor;

namespace RedMint_UI
{
    public enum DownloadQuality
    {
        Low,
        Medium,
        High
    }

    interface IDownloadController
    {
        void DownloadVideo(string url, DownloadQuality quality);
        void DownloadAudio(string url, DownloadQuality quality);
    }

    public class DownloadController : IDownloadController
    {
        public void DownloadAudio(string url, DownloadQuality quality)
        {
            throw new NotImplementedException();
        }

        public void DownloadVideo(string url, DownloadQuality quality)
        {
            throw new NotImplementedException();
        }
    }
}
