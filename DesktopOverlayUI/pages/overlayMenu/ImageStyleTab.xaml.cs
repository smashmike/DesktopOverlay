using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DesktopOverlay.pages.overlayMenu;

/// <summary>
///     Interaction logic for ImageStyleTab.xaml
/// </summary>
public partial class ImageStyleTab : Page
{


    private bool _ignoreSizeChange;

    private readonly OverlayDriver _overlayDriver;

    public ImageStyleTab(OverlayDriver driver)
    {
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
        if (_ignoreSizeChange) return;

        _ignoreSizeChange = true;
        var height = YNumberBox.Value;
        var width = XNumberBox.Value;

        if (AspectRatioToggle.IsChecked != null && AspectRatioToggle.IsChecked.Value)
        {
            if (sender == XNumberBox)
            {
                height = _overlayDriver.GetAspectRatio() * width;
                YNumberBox.Value = height;
            }
            else
            {
                width = height / _overlayDriver.GetAspectRatio();
                XNumberBox.Value = width;
            }
        }
        //_base.OverlayImageItem.Resize((int)width, (int)height);
        //_base.SetImage(_base.OverlayImageItem);

        _overlayDriver.ImageItem.Resize((int)width, (int)height);
        _overlayDriver.SetImage(_overlayDriver.ImageItem);
        //overlayDriver.SetSize((int)width, (int)height);

        _overlayDriver.SetSize((int)width, (int)height);
        _ignoreSizeChange = false;
    }


    private void ResetSizeBoxes(object sender, RoutedEventArgs e)
    {
        if (_overlayDriver.ImageItem == null) return;
        var originalSize = _overlayDriver.ImageItem.ResetSize();
        _ignoreSizeChange = true;
        XNumberBox.Value = originalSize.Width;
        YNumberBox.Value = originalSize.Height;
        _ignoreSizeChange = false;
    }

    private void OpacityValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (OpacitySlider == null) return;
        if (_overlayDriver.ImageItem == null) return;
        OpacityTextBox.Text = ((int)OpacitySlider.Value).ToString();
        _overlayDriver.SetImageOpacity(((int)OpacitySlider.Value / 100f));
        //_overlayDriver.SetImage(_overlayDriver.ImageItem);
    }

    private void OpacityValueChanged(object sender, TextChangedEventArgs e)
    {
        if (OpacityTextBox == null) return;
        if (!Regex.IsMatch(OpacityTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(OpacityTextBox.Text) > 100)
            OpacityTextBox.Text = "";

        if (OpacityTextBox.Text.Length != 0)
        {
            OpacitySlider.Value = int.Parse(OpacityTextBox.Text);
        }
    }
}