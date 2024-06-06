using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameOverlay.Drawing;
using WinRT;
using Color = System.Windows.Media.Color;

namespace DesktopOverlayUI.pages.overlayMenu;

/// <summary>
///     Interaction logic for TextStyleTab.xaml
/// </summary>
public partial class TextStyleTab : Page
{
    private readonly OverlayDriver _overlayDriver;

    public TextStyleTab(OverlayDriver overlayDriver)
    {
        _overlayDriver = overlayDriver;
        InitializeComponent();
        FontFamilyComboBox.SelectedItem = new FontFamily("Segoe UI");
    }

    private void UpdateColorPreview(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        RedSlider.Value = (int)RedSlider.Value;
        GreenSlider.Value = (int)GreenSlider.Value;
        BlueSlider.Value = (int)BlueSlider.Value;
        ColorPreview.Background =
            new SolidColorBrush(Color.FromRgb((byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value));
        //_base.OverlayTextBlock.Foreground = new SolidColorBrush(Color.FromRgb((byte)RedSlider.Value,
        //    (byte)GreenSlider.Value, (byte)BlueSlider.Value));
        _overlayDriver.SetTextColor(Color.FromArgb((byte)(OpacitySlider.Value * 2.55), (byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value));
    }

    private void UpdateFontSize(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        FontSizeSlider.Value = (int)FontSizeSlider.Value;
        //_base.OverlayTextBlock.FontSize = FontSizeSlider.Value;
        _overlayDriver.SetFontSize((float)FontSizeSlider.Value);

    }

    private void UpdateFontFamily(object sender, SelectionChangedEventArgs e)
    {
        var fontFamily = (FontFamily)FontFamilyComboBox.SelectedItem;
        //_base.OverlayTextBlock.FontFamily = fontFamily;
    
        if (FontSizeSlider == null) return;

        _overlayDriver.SetFontFamily(fontFamily);
    }

    private void OpacityValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        OpacitySlider.Value = (int)OpacitySlider.Value;
        if (OpacityTextBox == null) return;
        OpacityTextBox.Text = OpacitySlider.Value + "";
        //_base.OverlayTextBlock.Opacity = OpacitySlider.Value / 100;
        _overlayDriver.SetTextColor(Color.FromArgb((byte)(OpacitySlider.Value * 2.55), (byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value));
    }

    private void OpacityValueChanged(object sender, TextChangedEventArgs e)
    {
        if (OpacityTextBox == null) return;
        if (!Regex.IsMatch(OpacityTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(OpacityTextBox.Text) > 100)
            OpacityTextBox.Text = "";

        if (OpacityTextBox.Text.Length != 0)
        {
            OpacitySlider.Value = int.Parse(OpacityTextBox.Text);
            //_base.OverlayTextBlock.Opacity = OpacitySlider.Value / 100;
        }
    }

    private void HexValueChanged(object sender, TextChangedEventArgs e)
    {
        if (!Regex.IsMatch(HexTextBox.Text, @"\A\b[0-9a-fA-F]+\b\Z")) HexTextBox.Text = "";
        if (HexTextBox.Text.Length != 6) return;
        var color = ColorConverter.ConvertFromString("#" + HexTextBox.Text);
        if (color == null) return;
        var rgb = Color.FromRgb(((Color)color).R, ((Color)color).G, ((Color)color).B);
        RedSlider.Value = rgb.R;
        GreenSlider.Value = rgb.G;
        BlueSlider.Value = rgb.B;
        ColorPreview.Background = new SolidColorBrush(rgb);
        //_base.OverlayTextBlock.Foreground = new SolidColorBrush(rgb);
    }

    private void RgbValueChanged(object sender, TextChangedEventArgs e)
    {
        if (RedTextBox == null || GreenTextBox == null || BlueTextBox == null) return;
        if (!Regex.IsMatch(RedTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(RedTextBox.Text) > 255)
            RedTextBox.Text = "";

        if (!Regex.IsMatch(GreenTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(GreenTextBox.Text) > 255)
            GreenTextBox.Text = "";

        if (!Regex.IsMatch(BlueTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(BlueTextBox.Text) > 255)
            BlueTextBox.Text = "";

        if (RedTextBox.Text.Length != 0 && GreenTextBox.Text.Length != 0 && BlueTextBox.Text.Length != 0)
        {
            var rgb = Color.FromRgb(byte.Parse(RedTextBox.Text), byte.Parse(GreenTextBox.Text),
                byte.Parse(BlueTextBox.Text));
            RedSlider.Value = rgb.R;
            GreenSlider.Value = rgb.G;
            BlueSlider.Value = rgb.B;
            ColorPreview.Background = new SolidColorBrush(rgb);
            var toHex = rgb.R.ToString("X2") + rgb.G.ToString("X2") + rgb.B.ToString("X2");
            HexTextBox.Text = toHex;
            //_base.OverlayTextBlock.Foreground = new SolidColorBrush(rgb);
        }
    }
}