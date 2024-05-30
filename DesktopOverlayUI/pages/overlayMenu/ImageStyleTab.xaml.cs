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
        private OverlayDriver _overlayDriver;

        private int _imageWidth;
        private int _imageHeight;


        private bool _ignoreSizeChange = false;

        public ImageStyleTab(OverlayDisplay overlay, OverlayDriver driver)
        {
            _overlay = overlay;
            _overlayDriver = driver;
            InitializeComponent();
            _overlayDriver.ImageItemChanged += (sender, args) =>
            {
                if (_ignoreSizeChange || _overlayDriver.ImageItem == null) return;
                _imageHeight = _overlayDriver.ImageItem.GetSize().Height;
                _imageWidth = _overlayDriver.ImageItem.GetSize().Width;
                XNumberBox.Value = _imageWidth;
                YNumberBox.Value = _imageHeight;
            };
        }

        private void SizeValueChanged(object sender, RoutedEventArgs e)
        {
            if (XNumberBox == null || YNumberBox == null) return;
            if (XNumberBox.Value == null || YNumberBox.Value == null) return;
            if (AspectRatioToggle == null) return;
            if (_overlayDriver.ImageItem == null) return;

            _ignoreSizeChange = true;
            double? height = YNumberBox.Value;
            double? width = XNumberBox.Value;


            //_overlay.OverlayImageItem.Resize((int)width, (int)height);
            //_overlay.SetImage(_overlay.OverlayImageItem);

            _overlayDriver.ImageItem.Resize((int)width, (int)height);
            _overlayDriver.SetImage(_overlayDriver.ImageItem);
                //overlayDriver.SetSize((int)width, (int)height);
            
            _overlay.Height = (int)height;
            _overlay.Width = (int)width;
            _overlayDriver.SetSize((int)width, (int)height);
            _ignoreSizeChange = false;
        }


        private void ResetSizeBoxes(object sender, RoutedEventArgs e)
        {
            if (_overlayDriver.ImageItem == null) return;
            Size originalSize = _overlayDriver.ImageItem.ResetSize();
            XNumberBox.Value = originalSize.Width;
            YNumberBox.Value = originalSize.Height;
        }
    }

}
