using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace RedMint_UI
{
    // private enum DownloadOutputFormat { };
    

    interface IDownloadController
    {
        VideoDownloader ObtenerVideoDownloader(string url, string directorioSalida);
        AudioDownloader ObtenerAudioDownloader(string url, string directorioSalida);
    }

    public class DownloadController : IDownloadController
    {
        private string ParseUrl(string url) 
        {
            throw new NotImplementedException();
        }

        public VideoDownloader ObtenerVideoDownloader(string url, string directorioSalida)
        {
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url, false);

            // Obtener el video con la mejor calidad.
            VideoInfo video = videoInfos
                .Where( info => info.VideoType == VideoType.Mp4)
                .OrderByDescending( info => info.Resolution)
                .First();
            
            // Desencriptar de ser necesario.
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }
            
            // Crear el descargador.
            var directorioFinal = Path.Combine(directorioSalida, video.Title + video.VideoExtension);
            var videoDownloader = new VideoDownloader(video, directorioFinal);

            return videoDownloader;
        }

        public AudioDownloader ObtenerAudioDownloader(string url, string directorioSalida)
        {
            throw new NotImplementedException();
        }
    }
}
