using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DesktopOverlayUI.pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        private MainWindow _mainWindow;


        public HomePage(MainWindow _main)
        {
            _mainWindow = _main;
            InitializeComponent();
            DataContext = this;

        }

        private void NewTextItem(object sender, RoutedEventArgs e)
        {
            _mainWindow.TriggerNewItem(true);
        }

        private void NewImageItem(object sender, RoutedEventArgs e)
        {
            _mainWindow.TriggerNewItem(false);
        }

        private void OpenGithub(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = "/c start http://github.com/smashmike/DesktopOverlay"
            };
            Process.Start(psi);

        }




    }
}
