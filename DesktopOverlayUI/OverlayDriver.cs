using System;
using System.Windows.Interop;
using DesktopOverlayUI.pages;
using DesktopOverlayUI.pages.overlayMenu;
using GameOverlay.Drawing;
using GameOverlay.Windows;

namespace DesktopOverlayUI;

public class OverlayDriver
{
    private readonly Graphics _gfx;
    private readonly GraphicsWindow _mainWindow;

    private ImageItem? _imageItem;
    private bool _isSetUp;

    public OverlayDriver(OverlayDisplay baseDisplay)
    {
        var baseHandle = new WindowInteropHelper(baseDisplay).EnsureHandle();
        baseDisplay.Show();
        _isSetUp = false;
        IsAttached = false;

        _gfx = new Graphics(baseHandle)
        {
            MeasureFPS = true,
            PerPrimitiveAntiAliasing = true,
            TextAntiAliasing = true,
            UseMultiThreadedFactories = true,
            Width = 1,
            Height = 1
        };
        _mainWindow = new GraphicsWindow(_gfx)
        {
            IsTopmost = true,
            IsVisible = true
        };


        //var stick = new StickyWindow(new WindowInteropHelper(baseDisplay).EnsureHandle(), _gfx)
        //{
        //    IsTopmost = true,
        //    IsVisible = true,
        //};
        //stick.AttachToClientArea = true;
        //stick.Create();
    }

    public ImageItem? ImageItem
    {
        get => _imageItem;
        private set
        {
            _imageItem = value;
            if (ImageItemChanged != null) ImageItemChanged.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsAttached { get; set; }

    public event EventHandler? ImageItemChanged;

    private void SetUp()
    {
        _mainWindow.Create();
        _gfx.Setup();
        _isSetUp = true;
    }

    public void SetSize(int width, int height)
    {
        _mainWindow.Resize(width, height);
    }

    public void SetPosition(int x, int y)
    {
        _mainWindow.X = x;
        _mainWindow.Y = y;
    }

    public void SetImage(ImageItem image)
    {
        if (!_isSetUp) SetUp();
        ImageItem = image;
        SetSize(ImageItem.Width, ImageItem.Height);
        _gfx.BeginScene();
        _gfx.ClearScene();
        _gfx.DrawImage(new Image(_gfx, image.ImageArray), 0, 0);
        _gfx.EndScene();
    }

    public void ClearOverlay()
    {
        if (!_isSetUp) SetUp();
        _gfx.BeginScene();
        _gfx.ClearScene();
        _gfx.EndScene();
    }

    public void Dispose()
    {
        _mainWindow.Dispose();
        _gfx.Dispose();
        _isSetUp = false;
    }

    public void Show()
    {
        if (!_mainWindow.IsInitialized) return;
        _mainWindow.Show();
    }

    public void Hide()
    {
        if (!_mainWindow.IsInitialized) return;
        _mainWindow.Hide();
    }
}