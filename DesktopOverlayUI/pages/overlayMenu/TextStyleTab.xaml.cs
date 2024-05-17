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

namespace DesktopOverlayUI.pages.overlayMenu
{
    /// <summary>
    /// Interaction logic for TextStyleTab.xaml
    /// </summary>
    public partial class TextStyleTab : Page
    {
        public TextStyleTab()
        {
            InitializeComponent();
        }

        private void updateColorPreview(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            redSlider.Value = (int)redSlider.Value;
            greenSlider.Value = (int)greenSlider.Value;
            blueSlider.Value = (int)blueSlider.Value;
            colorPreview.Background = new SolidColorBrush(Color.FromRgb((byte)redSlider.Value, (byte)greenSlider.Value, (byte)blueSlider.Value));
        }

        

    }
}
