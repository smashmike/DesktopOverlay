using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;
using DesktopOverlayUI.pages;
using DesktopOverlayUI.pages.overlayMenu;
using GameOverlay.Drawing;
using GameOverlay.Windows;
using Point = GameOverlay.Drawing.Point;

namespace DesktopOverlayUI;

public class OverlayDriver
{
    private readonly Graphics _gfx;
    private readonly GraphicsWindow _mainWindow;

    private ImageItem? _imageItem;
    private bool _isSetUp;
    private Process _process;
    private DispatcherTimer _timer;
    private Size _position;
    private Size _offset;

    public OverlayDriver(OverlayDisplay baseDisplay)
    {
        var baseHandle = new WindowInteropHelper(baseDisplay).EnsureHandle();
        baseDisplay.Show();
        _isSetUp = false;
        IsAttached = false;
        _position = new Size(0, 0);
        _offset = new Size(0, 0);
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _timer.Tick += OnTimerTick;
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
        //_stickyWindow.Resize(width, height);
    }

    public void SetPosition(int x, int y)
    {
        _position = new Size(x, y);
        _mainWindow.X = x + _offset.Width;
        _mainWindow.Y = y + _offset.Height;
    }

    private void UpdatePosition()
    {
        _mainWindow.X = _position.Width + _offset.Width;
        _mainWindow.Y = _position.Height + _offset.Height;
    }

    public void SetOffset(int x, int y)
    {
        _offset = new Size(x, y);
        UpdatePosition();
    }

    public void SetTarget(Process process)
    {
        _timer.Stop();
        _process = process;
        IsAttached = true;
        if (!_isSetUp) SetUp();
        if (IsAttached) _timer.Start();
        else _timer.Stop();
    }

    public void SetAttach(bool attach)
    {
        if (!_isSetUp) SetUp();
        IsAttached = attach;
        if (IsAttached) _timer.Start();
        else
        {
            _timer.Stop();
            SetPosition(0,0);
        }

    }

    private void OnTimerTick(object sender, EventArgs e)
    {
        if (IsAttached)
        {
            WindowBounds bounds = new WindowBounds();
            WindowHelper.GetWindowClientBounds(_process.MainWindowHandle, out bounds);
            if (bounds.Top < 0 && bounds.Left < 0)
            {
                SetPosition(0, 0);
                return;
            }
            //_gfx.BeginScene(); //Debugging
            //_gfx.ClearScene();
            //_gfx.DrawText(_gfx.CreateFont("Arial", 12), _gfx.CreateSolidBrush(255, 255, 255), 20, 20, bounds.Top + " " + bounds.Left);
            //_gfx.EndScene();
            WindowHelper.EnableBlurBehind(_process.MainWindowHandle);
            //_mainWindow.FitTo(_process.MainWindowHandle);
            SetPosition(bounds.Left, bounds.Top);
        }
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