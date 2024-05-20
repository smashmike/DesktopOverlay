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
    /// Interaction logic for TextTab.xaml
    /// </summary>
    public partial class TextTab : Page
    {

        public OverlayDisplay Overlay;

        public TextTab()
        {
            InitializeComponent();
            Overlay = new OverlayDisplay();
        }

        public TextTab(OverlayDisplay overlay)
        {
            InitializeComponent();
            Overlay = overlay;
        }

        public void UpdateText(object sender, RoutedEventArgs e)
        {
            Overlay.OverlayText = TextInputBox.Text;
            Overlay.Show();
        }

    }
}
