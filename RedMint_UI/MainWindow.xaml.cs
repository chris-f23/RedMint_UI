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

namespace RedMint_UI
{
    /// <summary>
    public enum Calidad { Alta, Media, Baja }
    public enum Formato { Audio, Video }
    
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

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

            // Comboboxes:
            // cbb_calidad.ItemsSource = Enum.GetValues(typeof(Calidad));
            cbb_formato.ItemsSource = Enum.GetValues(typeof(Formato));
            cbb_formato.SelectedIndex = 0;

            // Progressbar:
            pgb_progress.Value = 0;

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

        private void btn_descargar_Click(object sender, RoutedEventArgs e)
        {
            switch ((Formato) cbb_formato.SelectedValue)
            {
                case Formato.Audio:
                    var audioDownloader = downloadController.ObtenerAudioDownloader(input_direccion.Text, input_directorio_salida.Text);
                    audioDownloader.Execute();

                    break;
                case Formato.Video:
                    var videoDownloader = downloadController.ObtenerVideoDownloader(input_direccion.Text, input_directorio_salida.Text);

                    videoDownloader.DownloadProgressChanged += (sender, args) => UpdateProgressBar(args.ProgressPercentage);
                    videoDownloader.Execute();

                    break;
                default:
                    input_direccion.Text = "Error, formato no seleccionado";
                    break;
            }
        }

        private void UpdateProgressBar(double pogressPercentage)
        {
            pgb_progress.Value = pogressPercentage;
        }
    }
}
