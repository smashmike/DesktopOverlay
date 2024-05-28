using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Size = SixLabors.ImageSharp.Size;

namespace DesktopOverlayUI.pages.overlayMenu
{
    public class ImageItem : DependencyObject
    {
        public string Name { get; set; }
        public ImageSource Source { get; set; }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            nameof(Width), typeof(int), typeof(NavigationItem), new PropertyMetadata(0));

        public int Width
        {
            get => (int)GetValue(WidthProperty);
            private set => SetValue(WidthProperty, value);
        }

        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
            nameof(Height), typeof(int), typeof(NavigationItem), new PropertyMetadata(0));
        public int Height
        {
            get => (int)GetValue(HeightProperty);
            private set => SetValue(HeightProperty, value);
        }

        private Image _image;
        private Uri _sourceUri;
        public ImageItem(string name, Uri source)
        {
            Name = name;
            Source = new BitmapImage(source);
            _image = Image.Load(source.AbsolutePath);
            Width = _image.Width;
            Height = _image.Height;
            _sourceUri = source;
        }

        public void Resize(int width, int height)
        {
            var newImage = Image.Load(_sourceUri.AbsolutePath);
            newImage.Mutate(x => x.Resize(width, height));
            Source = toBitmapImage(toArray(newImage));
        }

        public Size ResetSize()
        {
            Width = _image.Width;
            Height = _image.Height;
            return new Size(Width, Height);
        }

        private byte[] toArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, PngFormat.Instance);
                return ms.ToArray();
            }
        }

        private BitmapImage toBitmapImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                returnImage.StreamSource = ms;
                returnImage.CacheOption = BitmapCacheOption.OnLoad;
                returnImage.EndInit();
                return returnImage;
            }
        }

    }
}
