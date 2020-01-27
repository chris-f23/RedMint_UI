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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDownloadController downloadController;

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();
            downloadController = new DownloadController();
        }

        private void InitializeControls()
        {
            // inputs:
            input_direccion.Text = string.Empty;
            input_directorio_salida.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            cbb_calidad.ItemsSource = Enum.GetValues(typeof(DownloadQuality));
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

        }
    }
}
