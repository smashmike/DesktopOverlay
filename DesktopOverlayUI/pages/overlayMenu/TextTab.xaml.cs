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
        public TextTab()
        {
            InitializeComponent();
        }

        public void ClearText(object sender, RoutedEventArgs e)
        {
            TextInputBox.Document.Blocks.Clear();
        }

    }
}
