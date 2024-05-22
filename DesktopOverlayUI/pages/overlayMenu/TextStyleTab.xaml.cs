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

        private readonly OverlayDisplay _overlay;

        public TextStyleTab(OverlayDisplay overlay)
        {
            _overlay = overlay;
            InitializeComponent();
            FontFamilyComboBox.SelectedItem = new FontFamily("Segoe UI");
        }

        private void UpdateColorPreview(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RedSlider.Value = (int)RedSlider.Value;
            GreenSlider.Value = (int)GreenSlider.Value;
            BlueSlider.Value = (int)BlueSlider.Value;
            ColorPreview.Background = new SolidColorBrush(Color.FromRgb((byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value));
            _overlay.OverlayTextBlock.Foreground = new SolidColorBrush(Color.FromRgb((byte)RedSlider.Value, (byte)GreenSlider.Value, (byte)BlueSlider.Value));
        }

        private void UpdateFontSize(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            FontSizeSlider.Value = (int)FontSizeSlider.Value;
            _overlay.OverlayTextBlock.FontSize = FontSizeSlider.Value;
        }

        private void UpdateFontFamily(object sender, SelectionChangedEventArgs e)
        {
            var fontFamily = (FontFamily)FontFamilyComboBox.SelectedItem;
            _overlay.OverlayTextBlock.FontFamily = fontFamily;
        }

        private void HexValueChanged(object sender, TextChangedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(HexTextBox.Text, @"\A\b[0-9a-fA-F]+\b\Z"))
            {
                HexTextBox.Text = "";
            }
            if (HexTextBox.Text.Length != 6) return;
            var color = ColorConverter.ConvertFromString("#" + HexTextBox.Text);
            if (color == null) return;
            var rgb = Color.FromRgb(((Color)color).R, ((Color)color).G, ((Color)color).B);
            RedSlider.Value = rgb.R;
            GreenSlider.Value = rgb.G;
            BlueSlider.Value = rgb.B;
            ColorPreview.Background = new SolidColorBrush(rgb);
            _overlay.OverlayTextBlock.Foreground = new SolidColorBrush(rgb);
        }

        private void RgbValueChanged(object sender, TextChangedEventArgs e)
        {
            if (RedTextBox == null || GreenTextBox == null || BlueTextBox == null) return;
            if (!System.Text.RegularExpressions.Regex.IsMatch(RedTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(RedTextBox.Text) > 255)
            {
                RedTextBox.Text = "";
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(GreenTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(GreenTextBox.Text) > 255)
            {
                GreenTextBox.Text = "";
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(BlueTextBox.Text, @"\A\b[0-9]+\b\Z") || int.Parse(BlueTextBox.Text) > 255)
            {
                BlueTextBox.Text = "";
            }

            if (RedTextBox.Text.Length != 0 && GreenTextBox.Text.Length != 0 && BlueTextBox.Text.Length != 0)
            {
                var rgb = Color.FromRgb(byte.Parse(RedTextBox.Text), byte.Parse(GreenTextBox.Text), byte.Parse(BlueTextBox.Text));
                RedSlider.Value = rgb.R;
                GreenSlider.Value = rgb.G;
                BlueSlider.Value = rgb.B;
                ColorPreview.Background = new SolidColorBrush(rgb);
                string toHex = rgb.R.ToString("X2") + rgb.G.ToString("X2") + rgb.B.ToString("X2");
                HexTextBox.Text = toHex;
                _overlay.OverlayTextBlock.Foreground = new SolidColorBrush(rgb);
            }
        }
        
        

    }
}
