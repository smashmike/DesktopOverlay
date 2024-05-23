using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DesktopOverlayUI.pages.overlayMenu
{
    public class ImageItem
    {
        public string Name { get; set; }
        public ImageSource Source { get; set; }
        public ImageItem(string name, ImageSource source)
        {
            Name = name;
            Source = source;
        }

    }
}
