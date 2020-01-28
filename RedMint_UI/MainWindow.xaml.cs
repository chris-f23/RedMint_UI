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

            // Comboboxes:
            // cbb_calidad.ItemsSource = Enum.GetValues(typeof(Calidad));
            cbb_formato.ItemsSource = Enum.GetValues(typeof(Formato));
            cbb_formato.SelectedIndex = 0;

            // Progressbar:
            pgb_progress.Value = 100;

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

            if (data != null)
            {
                tb_video_title.Text = data.Titulo;
                btn_descargar.IsEnabled = true;
            }
            else {
                ShowErrorMessage("No se encontro el video con la direccion ingresada.");
                input_direccion.Text = string.Empty;
            }
        }

        private async void btn_descargar_ClickAsync(object sender, RoutedEventArgs e)
        {
            switch ((Formato) cbb_formato.SelectedValue)
            {
                case Formato.Audio:
                    try
                    {
                        btn_descargar.IsEnabled = false;
                        await DescargarAudio();
                        ShowSuccessMessage("Descarga completada!");
                    }
                    catch (Exception)
                    {
                        ShowErrorMessage("Ocurrio un error al descargar el video...");
                    }

                    break;
                default:
                    ShowErrorMessage("Error, formato no seleccionado.");
                    return;
            }

            this.data = null;
            tb_video_title.Text = string.Empty;
        }

        private async Task DescargarAudio()
        {
            var videoDownloader = downloadController.DescargarAudio(this.data, input_directorio_salida.Text);

            var progress = new Progress<int>(value => pgb_progress.Value = value);
            videoDownloader.DownloadProgressChanged += (sender, args) => UpdateProgressBar(progress, args.ProgressPercentage);
            
            await Task.Run(() => videoDownloader.Execute());
        }

        private void UpdateProgressBar(Progress<int> progress, double value)
        {
            ((IProgress<int>)progress).Report((int) value);
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
