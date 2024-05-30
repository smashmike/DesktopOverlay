using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using DesktopOverlayUI.pages.overlayMenu;
using WinRT;
using Image = Wpf.Ui.Controls.Image;
using TextBlock = Wpf.Ui.Controls.TextBlock;

namespace DesktopOverlayUI.pages;

/// <summary>
///     Interaction logic for BaseDisplay.xaml
/// </summary>
public partial class BaseDisplay
{
    //private readonly Image _overlayImage = new()
    //{
    //    Source = null,
    //    HorizontalAlignment = HorizontalAlignment.Left,
    //    VerticalAlignment = VerticalAlignment.Top,
    //    IsHitTestVisible = false,
    //    Visibility = Visibility.Visible,
    //    Stretch = Stretch.None
    //};

    //public readonly TextBlock OverlayTextBlock = new()
    //{
    //    Text = "",
    //    FontSize = 24,
    //    Foreground = Brushes.Black,
    //    HorizontalAlignment = HorizontalAlignment.Left,
    //    VerticalAlignment = VerticalAlignment.Top,
    //    IsHitTestVisible = false,
    //    Visibility = Visibility.Collapsed
    //};

    //private ImageItem? _overlayImageItem;


    //public BaseDisplay(string overlayType, ImageItem? imageItem, string? str)
    //{
    //    InitializeComponent();
    //    OverlayImageItem = imageItem;
    //    OriginPoint = new Point(Left, Top);
    //    if (str != null) OverlayText = str;
    //    Content.As<Grid>().Children.Add(OverlayTextBlock);
    //    OverlayImageSource = null;
    //    Content.As<Grid>().Children.Add(_overlayImage);

    //    foreach (UIElement item in Content.As<Grid>().Children) item.IsHitTestVisible = false;
    //}

    public BaseDisplay()
    {
        InitializeComponent();
    }

    //public Point OriginPoint { get; set; }
    public nint Handle { get; private set; }

    //private ImageItem? OverlayImageItem
    //{
    //    get => _overlayImageItem;
    //    set
    //    {
    //        _overlayImageItem = value;
    //        OverlayImageItemChanged?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    //public string OverlayText
    //{
    //    get => OverlayTextBlock.Text;
    //    set => OverlayTextBlock.Text = value;
    //}

    //public ImageSource? OverlayImageSource
    //{
    //    get => _overlayImage.Source;
    //    set => _overlayImage.Source = value;
    //}

    //public event EventHandler? OverlayImageItemChanged;


    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        var hwnd = new WindowInteropHelper(this).Handle;
        Handle = Process.GetCurrentProcess().Handle;
        WindowsServices.SetWindowExTransparent(hwnd);
    }


    //public void SetText(string text)
    //{
    //    OverlayText = text;
    //    OverlayTextBlock.Text = OverlayText;
    //    OverlayTextBlock.Visibility = Visibility.Visible;
    //}

    //public void SetImage(ImageItem? image)
    //{
    //    OverlayImageItem = image;
    //    var imageControl = new Image
    //    {
    //        Source = OverlayImageItem?.Source,
    //        HorizontalAlignment = HorizontalAlignment.Left,
    //        VerticalAlignment = VerticalAlignment.Top,
    //        IsHitTestVisible = false,
    //        Visibility = Visibility.Visible,
    //        HorizontalContentAlignment = HorizontalAlignment.Stretch,
    //        VerticalContentAlignment = VerticalAlignment.Stretch,
    //        Stretch = Stretch.None
    //    };
    //    _overlayImage.Source = imageControl.Source;
    //    OverlayTextBlock.Visibility = Visibility.Collapsed;
    //}
}

public static class WindowsServices
{
    private const int WsExTransparent = 0x00000020;
    private const int GwlExstyle = -20;

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    public static void SetWindowExTransparent(IntPtr hwnd)
    {
        var extendedStyle = GetWindowLong(hwnd, GwlExstyle);
        SetWindowLong(hwnd, GwlExstyle, extendedStyle | WsExTransparent);
    }
}