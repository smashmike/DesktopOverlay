using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using DesktopOverlayUI.pages;
using DesktopOverlayUI.pages.overlayMenu;
using GameOverlay.Drawing;
using GameOverlay.Windows;
using Color = System.Windows.Media.Color;
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

    private string _text;
    private Font _font;
    private float _fontSize;
    private Color _color;
    private SolidBrush _brush;

    private Size _position;
    private Size _offset;


    public OverlayDriver(BaseDisplay baseDisplay)
    {
        var baseHandle = new WindowInteropHelper(baseDisplay).EnsureHandle();
        baseDisplay.Show();
        _isSetUp = false;
        IsAttached = false;
        _position = new Size(0, 0);
        _offset = new Size(0, 0);
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(10)
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
        _text = "testing";
        _font = _gfx.CreateFont("Segoe UI", 12);
        _fontSize = 12;
        _color = Color.FromArgb(255, 255, 255, 255);
        _brush = _gfx.CreateSolidBrush(255, 255, 255, 255);
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
        if (IsAttached)
        {
            _timer.Start();
        }
        else
        {
            _timer.Stop();
            SetPosition(0, 0);
        }
    }

    private void OnTimerTick(object sender, EventArgs e)
    {
        if (IsAttached)
        {
            var bounds = new WindowBounds();
            WindowHelper.GetWindowClientBounds(_process.MainWindowHandle, out bounds);
            var tempX = bounds.Left;
            var tempY = bounds.Top;
            if (bounds.Top < 0) tempY = 0;
            if (bounds.Left < 0) tempX = 0;

            //_gfx.BeginScene(); //Debugging
            //_gfx.ClearScene();
            //_gfx.DrawText(_gfx.CreateFont("Arial", 12), _gfx.CreateSolidBrush(255, 255, 255), 20, 20, bounds.Top + " " + bounds.Left);
            //_gfx.EndScene();
            //_mainWindow.FitTo(_process.MainWindowHandle);
            SetPosition(tempX, tempY);
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

    public void SetText(string text, FontFamily fontFamily, double size, Color color)
    {
        if (!_isSetUp) SetUp();
        _text = fontFamily.ToString();
        _font = _gfx.CreateFont(fontFamily.ToString(), (float)size);
        _brush = _gfx.CreateSolidBrush(color.R, color.G, color.B, color.A);
        _gfx.BeginScene();
        _gfx.ClearScene();
        _gfx.DrawText(_font, _brush, 0, 0, _text);
        _gfx.EndScene();
        UpdatePosition();
    }

    public void SetText(string text)
    {
        if (!_isSetUp) SetUp();
        _text = text;
        _gfx.BeginScene();
        _gfx.ClearScene();
        _gfx.DrawText(_font, _brush, 0, 0, _text);
        _gfx.EndScene();
        UpdatePosition();
    }

    public void SetFontFamily(FontFamily fontFamily)
    {
        if (!_isSetUp) SetUp();
        _font = _gfx.CreateFont(fontFamily.ToString(), _fontSize);
        _gfx.BeginScene();
        _gfx.ClearScene();
        _gfx.DrawText(_font, _brush, 0, 0, _text);
        _gfx.EndScene();
        UpdatePosition();
    }

    public void SetFontSize(float fontSize)
    {
        if (!_isSetUp) SetUp();
        _fontSize = fontSize;
        _font = _gfx.CreateFont(_font.FontFamilyName, _fontSize);
        _gfx.BeginScene();
        _gfx.ClearScene();
        _gfx.DrawText(_font, _brush, 0, 0, _text);
        _gfx.EndScene();
        UpdatePosition();
    }

    public void SetTextColor(Color color)
    {
        if (!_isSetUp) SetUp();
        _color = color;
        _brush = _gfx.CreateSolidBrush(color.R, color.G, color.B, color.A);
        _gfx.BeginScene();
        _gfx.ClearScene();
        _gfx.DrawText(_font, _brush, 0, 0, _text);
        _gfx.EndScene();
        UpdatePosition();
    }

    public string GetText()
    {
        return _text;
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