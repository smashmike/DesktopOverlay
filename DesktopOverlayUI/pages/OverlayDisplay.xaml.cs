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
using System.Windows.Shapes;
using WinRT;
using Wpf.Ui.Controls;
using TextBlock = Wpf.Ui.Controls.TextBlock;

namespace DesktopOverlayUI.pages
{
    /// <summary>
    /// Interaction logic for OverlayDisplay.xaml
    /// </summary>
    public partial class OverlayDisplay
    {

        public Point OriginPoint { get; set; }



        public TextBlock OverlayTextBlock = new TextBlock();

        public string OverlayText {
            get => OverlayTextBlock.Text;
            set => OverlayTextBlock.Text = value;
        }

        public OverlayDisplay()
        {
            InitializeComponent();
            OriginPoint = new Point(Left,Top);
            InitTextOverlay("Test");
        }

        public OverlayDisplay(string overlayText)
        {

            InitializeComponent();
            OriginPoint = new Point(Left,Top);
            InitTextOverlay(overlayText);
        }

        private void InitTextOverlay(string text)
        {
            OverlayText = text;
            OverlayTextBlock.Text = text;
            OverlayTextBlock.FontSize = 24;
            OverlayTextBlock.Foreground = Brushes.Black;
            OverlayTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            OverlayTextBlock.VerticalAlignment = VerticalAlignment.Top;
            Content.As<Grid>().Children.Add(OverlayTextBlock);
        }


    }
}
