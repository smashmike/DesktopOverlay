using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Graphics.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Wpf.Ui.Controls;
using Size = SixLabors.ImageSharp.Size;

namespace DesktopOverlayUI.pages.overlayMenu
{
    /// <summary>
    /// Interaction logic for ImageStyleTab.xaml
    /// </summary>
    public partial class ImageStyleTab : Page
    {

        private readonly OverlayDisplay _overlay;

        private bool _ignoreSizeChange = false;

        public ImageStyleTab(OverlayDisplay overlay)
        {
            _overlay = overlay;
            InitializeComponent();
            overlay.OverlayImageItemChanged += (sender, args) =>
            {
                if (_ignoreSizeChange) return;
                XNumberBox.Value = overlay.OverlayImageItem.Width;
                YNumberBox.Value = overlay.OverlayImageItem.Height;
            };
        }

        private void SizeValueChanged(object sender, RoutedEventArgs e)
        {
            if (XNumberBox == null || YNumberBox == null) return;
            if (XNumberBox.Value == null || YNumberBox.Value == null) return;
            if (_overlay.OverlayImage.Source == null) return;

            _ignoreSizeChange = true;

            _overlay.OverlayImageItem.Resize((int)XNumberBox.Value, (int)YNumberBox.Value);
            _overlay.SetImage(_overlay.OverlayImageItem);

            _ignoreSizeChange = false;
        }


        private void ResetSizeBoxes(object sender, RoutedEventArgs e)
        {
            if (_overlay.OverlayImage.Source == null) return;
            Size originalSize = _overlay.OverlayImageItem.ResetSize();
            XNumberBox.Value = originalSize.Width;
            YNumberBox.Value = originalSize.Height;
        }
    }

}
