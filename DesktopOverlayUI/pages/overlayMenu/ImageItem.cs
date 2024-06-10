using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Size = SixLabors.ImageSharp.Size;

namespace DesktopOverlay.pages.overlayMenu;

public class ImageItem : DependencyObject
{
    public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
        nameof(Width), typeof(int), typeof(NavigationItem), new PropertyMetadata(0));

    public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
        nameof(Height), typeof(int), typeof(NavigationItem), new PropertyMetadata(0));

    private readonly Image _image;
    private readonly Uri _sourceUri;

    public ImageItem(string name, Uri source)
    {
        Name = name;
        Source = new BitmapImage(source);
        SourceUri = source;
        _image = Image.Load(source.LocalPath);
        Width = _image.Width;
        Height = _image.Height;
        _sourceUri = source;
        ImageArray = ToArray(_image);
    }

    public string Name { get; }
    public ImageSource Source { get; private set; }
    public Uri SourceUri { get; set; }

    public int Width
    {
        get => (int)GetValue(WidthProperty);
        private set => SetValue(WidthProperty, value);
    }

    public int Height
    {
        get => (int)GetValue(HeightProperty);
        private set => SetValue(HeightProperty, value);
    }

    public byte[] ImageArray { get; private set; }

    public void Resize(int width, int height)
    {
        var newImage = Image.Load(_sourceUri.LocalPath);
        newImage.Mutate(x => x.Resize(width, height));
        Source = ToBitmapImage(ToArray(newImage));
        ImageArray = ToArray(newImage);
        Width = width;
        Height = height;
    }

    public Size GetSize()
    {
        return new Size(Width, Height);
    }

    public Size ResetSize()
    {
        Width = _image.Width;
        Height = _image.Height;
        return new Size(Width, Height);
    }

    private static byte[] ToArray(Image image)
    {
        using var ms = new MemoryStream();
        image.Save(ms, PngFormat.Instance);
        return ms.ToArray();
    }

    private static BitmapImage ToBitmapImage(byte[] byteArray)
    {
        using var ms = new MemoryStream(byteArray);
        var returnImage = new BitmapImage();
        returnImage.BeginInit();
        returnImage.StreamSource = ms;
        returnImage.CacheOption = BitmapCacheOption.OnLoad;
        returnImage.EndInit();
        return returnImage;
    }
}