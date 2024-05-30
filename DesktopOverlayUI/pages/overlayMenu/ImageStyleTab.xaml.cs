using System.Windows;
using System.Windows.Controls;

namespace DesktopOverlayUI.pages.overlayMenu;

/// <summary>
///     Interaction logic for ImageStyleTab.xaml
/// </summary>
public partial class ImageStyleTab : Page
{
    private readonly BaseDisplay _base;


    private bool _ignoreSizeChange;

    private readonly OverlayDriver _overlayDriver;

    public ImageStyleTab(BaseDisplay @base, OverlayDriver driver)
    {
        _base = @base;
        _overlayDriver = driver;
        InitializeComponent();
        _overlayDriver.ImageItemChanged += (sender, args) =>
        {
            if (_ignoreSizeChange || _overlayDriver.ImageItem == null) return;
            var imageHeight = _overlayDriver.ImageItem.GetSize().Height;
            var imageWidth = _overlayDriver.ImageItem.GetSize().Width;
            XNumberBox.Value = imageWidth;
            YNumberBox.Value = imageHeight;
        };
    }

    private void SizeValueChanged(object sender, RoutedEventArgs e)
    {
        if (XNumberBox == null || YNumberBox == null) return;
        if (XNumberBox.Value == null || YNumberBox.Value == null) return;
        if (AspectRatioToggle == null) return;
        if (_overlayDriver.ImageItem == null) return;

        _ignoreSizeChange = true;
        var height = YNumberBox.Value;
        var width = XNumberBox.Value;


        //_base.OverlayImageItem.Resize((int)width, (int)height);
        //_base.SetImage(_base.OverlayImageItem);

        _overlayDriver.ImageItem.Resize((int)width, (int)height);
        _overlayDriver.SetImage(_overlayDriver.ImageItem);
        //overlayDriver.SetSize((int)width, (int)height);

        _base.Height = (int)height;
        _base.Width = (int)width;
        _overlayDriver.SetSize((int)width, (int)height);
        _ignoreSizeChange = false;
    }


    private void ResetSizeBoxes(object sender, RoutedEventArgs e)
    {
        if (_overlayDriver.ImageItem == null) return;
        var originalSize = _overlayDriver.ImageItem.ResetSize();
        XNumberBox.Value = originalSize.Width;
        YNumberBox.Value = originalSize.Height;
    }
}