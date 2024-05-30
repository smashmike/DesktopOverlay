using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using DesktopOverlayUI.pages;
using DesktopOverlayUI.pages.overlayMenu;
using GameOverlay.Drawing;
using GameOverlay.Windows;
using Overlay.NET.Common;
using Overlay.NET.Directx;

namespace DesktopOverlayUI
{
    public class OverlayDriver
    {
        private GraphicsWindow _mainWindow;
        private Graphics _gfx;

        public event EventHandler ImageItemChanged;

        private ImageItem? _imageItem;
        private bool _isSetUp;
        public ImageItem? ImageItem
        {
            get { return _imageItem; }
            set
            {
                _imageItem = value;
                ImageItemChanged.Invoke(this,EventArgs.Empty);
            }
        }

        public bool IsAttatched { get; set; }
        private nint _baseHandle;

        public OverlayDriver(OverlayDisplay baseDisplay)
        {
            _baseHandle = baseDisplay.Handle;
            baseDisplay.Show();
            _isSetUp = false;
            IsAttatched = false;

            _gfx = new Graphics(new WindowInteropHelper(baseDisplay).EnsureHandle())
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true,
                UseMultiThreadedFactories = true,
                Width = 1,
                Height = 1,

            };
            _mainWindow = new GraphicsWindow(_gfx)
            {
                IsTopmost = true,
                IsVisible = true,
            };



            //var stick = new StickyWindow(new WindowInteropHelper(baseDisplay).EnsureHandle(), _gfx)
            //{
            //    IsTopmost = true,
            //    IsVisible = true,
            //};
            //stick.AttachToClientArea = true;
            //stick.Create();

            



        }

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
}
