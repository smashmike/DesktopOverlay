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
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace DesktopOverlay.pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly MainWindow _window;
        private WindowBackdropType _backdropType;


        public SettingsPage(MainWindow window)
        {
            _window = window;
            _backdropType = WindowBackdropType.Mica;
            InitializeComponent();
        }

        private void SetApplicationTheme(object sender, SelectionChangedEventArgs e)
        {

            switch (ThemeComboBox.SelectedIndex)
            {

                case 0:
                    ApplicationThemeManager.ApplySystemTheme();
                    _window.WindowBackdropType = WindowBackdropType.None;
                    _window.WindowBackdropType = _backdropType;
                    break;
                case 1:
                    ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.None, true);
                    _window.WindowBackdropType = WindowBackdropType.None;
                    _window.WindowBackdropType = _backdropType;
                    break;
                case 2:
                    ApplicationThemeManager.Apply(ApplicationTheme.Light, WindowBackdropType.Mica, true);
                    _window.WindowBackdropType = WindowBackdropType.None;
                    _window.WindowBackdropType = _backdropType;
                    break;
            }

        }

        private void SetBackdropMaterial(object sender, SelectionChangedEventArgs e)
        {
            switch (MaterialComboBox.SelectedIndex)
            {
                case 0:
                    _window.WindowBackdropType = WindowBackdropType.Mica;
                    _backdropType = WindowBackdropType.Mica;
                    break;
                case 1:
                    _window.WindowBackdropType = WindowBackdropType.Acrylic;
                    _backdropType = WindowBackdropType.Acrylic;
                    break;
                case 2:
                    _window.WindowBackdropType = WindowBackdropType.None;
                    _backdropType = WindowBackdropType.None;
                    break;
            }
        }



    }
}
