using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
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

namespace DesktopOverlayUI.pages.overlayMenu
{
    /// <summary>
    /// Interaction logic for ImageStyleTab.xaml
    /// </summary>
    public partial class ImageStyleTab : Page
    {

        private readonly OverlayDisplay _overlay;

        public ImageStyleTab(OverlayDisplay overlay)
        {
            _overlay = overlay;
            InitializeComponent();
        }

        private void SizeValueChanged(object sender, TextChangedEventArgs e)
        {
            if (SizeXBox == null || SizeYBox == null) return;

            if (!System.Text.RegularExpressions.Regex.IsMatch(SizeXBox.Text, @"\A\b[0-9]+\b\Z") ||
                int.Parse(SizeXBox.Text) > SystemParameters.VirtualScreenWidth)
            {
                SizeXBox.Text = "1";
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(SizeYBox.Text, @"\A\b[0-9]+\b\Z") ||
                int.Parse(SizeYBox.Text) > SystemParameters.VirtualScreenHeight)
            {
                SizeYBox.Text = "1";
            }

            _overlay.OverlayImage.Width = int.Parse(SizeXBox.Text);
            _overlay.OverlayImage.MinWidth = int.Parse(SizeXBox.Text);
            _overlay.OverlayImage.Height = int.Parse(SizeYBox.Text);

           
            var newImage = SixLabors.ImageSharp.Image.Load(_overlay.OverlayImage.Source.ToString());
            newImage.Mutate(x => x.Resize(int.Parse(SizeXBox.Text), int.Parse(SizeYBox.Text)));
            
        }
        


        

    }

}
