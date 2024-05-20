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

        public OverlayDisplay Overlay;

        public TextStyleTab()
        {
            InitializeComponent();
            Overlay = new OverlayDisplay();
        }

        public TextStyleTab(OverlayDisplay overlay)
        {
            InitializeComponent();
            Overlay = overlay;
        }

        private void UpdateColorPreview(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RedSlider.Value = (int)RedSlider.Value;
            GreenSlider.Value = (int)GreenSlider.Value;
            BlueSlider.Value = (int)BlueSlider.Value;
            ColorPreview.Background = new SolidColorBrush(Color.FromRgb((byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value));
        }

        
        

    }
}
