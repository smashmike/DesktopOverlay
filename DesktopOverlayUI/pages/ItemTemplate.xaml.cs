using DesktopOverlayUI.pages.overlayMenu;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace DesktopOverlayUI.pages
{
    /// <summary>
    /// Interaction logic for template.xaml
    /// </summary>
    public partial class ItemTemplate : Page
    {

        private OverlayDisplay Overlay;
        private ImagesTab imagesTab;
        private TextTab textTab;
        private TextStyleTab textStyleTab;

        public ItemTemplate()
        {
            InitializeComponent();
            Overlay = new OverlayDisplay("Test");

            imagesTab = new ImagesTab();
            ImageTab.TargetPageType = imagesTab.GetType();

            TextTab.DataContext = this;
            textTab = new TextTab(Overlay);
            TextTab.TargetPageType = textTab.GetType();

            textStyleTab = new TextStyleTab(Overlay);
            
        }

        public ItemTemplate(string itemType)
        {
            InitializeComponent();
            Overlay = new OverlayDisplay("Test");
            imagesTab = new ImagesTab();
            textTab = new TextTab(Overlay);
            textStyleTab = new TextStyleTab(Overlay);


            switch (itemType)
            {
                case "Image":
                    ImageTab.Visibility = Visibility.Visible;
                    Overlay = new OverlayDisplay("Image");
                    imagesTab = new ImagesTab();
                    ImageTab.TargetPageType = imagesTab.GetType();
                    break;
                case "Text":
                {
                    TextTab.Visibility = Visibility.Visible;
                    Overlay = new OverlayDisplay("Text");
                    textTab = new TextTab(Overlay);
                    TextTab.TargetPageType = textTab.GetType();
                    textStyleTab = new TextStyleTab(Overlay);
                    StyleTab.TargetPageType = textStyleTab.GetType();
                    break;
                }
            }
            Overlay = new OverlayDisplay("Test");
        }


        
    }
}
