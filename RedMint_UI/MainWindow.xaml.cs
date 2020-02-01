using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YoutubeExtractor;

namespace RedMint_UI
{    
    /// <summary>    
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Formato { Audio, Video }

        private VideoData data;
        private IDownloadController downloadController;

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Inputs:
            input_direccion.Text = string.Empty;
            input_directorio_salida.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Combobox:
            cbb_formato.ItemsSource = Enum.GetValues(typeof(Formato));

            cbb_formato.SelectedIndex = 0;

            // Progressbar:
            tb_progress.Text = string.Empty;
            pgb_progress.Value = 0;

            // Textbox:
            tb_video_title.Text = string.Empty;

            // Button:
            btn_descargar.IsEnabled = false;

            // Controlador de Descargas:
            if (downloadController == null)
            {
                downloadController = new DownloadController();
            }
        }

        private void btn_directorio_salida_Click(object sender, RoutedEventArgs e)
        {
            // Abre un dialogo para seleccionar el directorio de salida.
            // Una vez seleccionado, se guarda la ruta del directorio en
            // el control "input_directorio_salida".

            var fbd = new WPFFolderBrowser.WPFFolderBrowserDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (fbd.ShowDialog().Value == true) {
                input_directorio_salida.Text = fbd.FileName;
            }
        }

        private void btn_buscar_Click(object sender, RoutedEventArgs e)
        {
            this.data = downloadController.BuscarVideo(input_direccion.Text);

            if (data == null)
            {
                ShowErrorMessage("No se encontro el video con la direccion ingresada.");
                return;
            }

            // Mostrar el titulo del video.
            tb_video_title.Text = data.Titulo;

            // Desactivar el boton de busqueda y activar el boton de descarga.
            btn_buscar.IsEnabled = false;
            btn_descargar.IsEnabled = true;

            // Resetear la vista del progreso de descarga.
            tb_progress.Text = string.Empty;
            pgb_progress.Value = 0;

        }

        private async void btn_descargar_ClickAsync(object sender, RoutedEventArgs e)
        {
            var errorMsg = "Ocurrio un error al descargar el video...";

            // Desactivar el boton de descarga.
            btn_descargar.IsEnabled = false;

            // Descargar:
            // Crea un descargador dependiendo del formato seleccionado, y luego realiza la descarga. 
            try
            {
                VideoDownloader downloader = null;

                switch ((Formato)cbb_formato.SelectedValue)
                {
                    case Formato.Audio:
                        downloader = downloadController.CrearDescargadorDeAudio(this.data, input_directorio_salida.Text);

                        break;
                    case Formato.Video:
                        downloader = downloadController.CrearDescargadorDeVideo(this.data, input_directorio_salida.Text);

                        break;
                    default:
                        ShowErrorMessage("Error, formato no seleccionado.");
                        return;
                }

                await Descargar(downloader);

                ShowSuccessMessage("Descarga completada!");
            }
            catch (Exception exc)
            {
                // Al ocurrir un error, mostrar el error a traves de un mensaje.
                ShowErrorMessage(string.Format("{0}\n\n{1}", errorMsg, exc.Message));
            }

            // Una vez termina la descarga, se activa el boton de busqueda.
            btn_buscar.IsEnabled = true;

            // Se elimina la data asociada (excepto por la URL).
            this.data = null;
            tb_video_title.Text = string.Empty;
        }

        private async Task Descargar(VideoDownloader videoDownloader)
        {
            var progress = new Progress<float>(value => UpdateProgressView(value));
            
            videoDownloader.DownloadProgressChanged += 
                (sender, args) => ((IProgress<float>)progress).Report((float)args.ProgressPercentage);

            await Task.Run(() => videoDownloader.Execute());
        }

        private void UpdateProgressView(float value)
        {
            value = MathF.Round(value, 1);
            pgb_progress.Value = value;
            tb_progress.Text = string.Concat(value, "%");
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btn_vaciar_Click(object sender, RoutedEventArgs e)
        {
            input_direccion.Text = string.Empty;
            btn_buscar.IsEnabled = true;
            btn_descargar.IsEnabled = false;
            this.data = null;
            tb_video_title.Text = string.Empty;
        }
    }
}
