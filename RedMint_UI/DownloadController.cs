using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace RedMint_UI
{
    public class VideoData
    {
        public string Titulo { get; set; }
        public IEnumerable<VideoInfo> LinksDisponibles { get; set; }
    }

    interface IDownloadController
    {
        VideoData BuscarVideo(string url);
        VideoDownloader DescargarVideo(VideoData videoData, string directorioSalida);
        VideoDownloader DescargarAudio(VideoData videoData, string directorioSalida);
    }

    public class DownloadController : IDownloadController
    {
        public VideoData BuscarVideo(string url) 
        {
            try
            {
                var videoInfos = DownloadUrlResolver.GetDownloadUrls(url, false);

                return new VideoData()
                {
                    Titulo = videoInfos.First().Title,
                    LinksDisponibles = videoInfos
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public VideoDownloader DescargarAudio(VideoData videoData, string directorioSalida)
        {
            // https://github.com/flagbug/YoutubeExtractor/issues/246
            // AudioDownloader esta obsoleto.
            try
            {
                // https://github.com/jphellemons/YoutubeExtractor/blob/805926ebdb521a79439dd4c321101a2d97c20858/readme.md
                VideoInfo link = videoData.LinksDisponibles
                    .Where(info => info.VideoType == VideoType.Mp4 && info.Resolution == 0)
                    .OrderByDescending(info => info.AudioBitrate)
                    .First();

                // Desencriptar de ser necesario.
                if (link.RequiresDecryption)
                {
                    DownloadUrlResolver.DecryptDownloadUrl(link);
                }

                // Crear el descargador.
                var directorioFinal = Path.Combine(directorioSalida, link.Title + ".mp3");
                var videoDownloader = new VideoDownloader(link, directorioFinal);

                return videoDownloader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VideoDownloader DescargarVideo(VideoData videoData, string directorioSalida)
        {
            throw new NotImplementedException();
        }
    }
}
