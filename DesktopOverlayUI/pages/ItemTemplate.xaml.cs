using DesktopOverlayUI.pages.overlayMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using WinRT;
using Wpf.Ui;
using Wpf.Ui.Animations;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using NavigationService = Wpf.Ui.NavigationService;

namespace DesktopOverlayUI.pages
{
    /// <summary>
    /// Interaction logic for template.xaml
    /// </summary>
    public partial class ItemTemplate : Page
    {

        public OverlayDisplay Overlay;
        private ImagesTab imagesTab;
        private TextTab textTab;
        private TextStyleTab textStyleTab;


        public ItemTemplate()
        {
            InitializeComponent();
            Overlay = new OverlayDisplay("Test");

            //imagesTab = new ImagesTab();
            //ImageTab.TargetPageType = imagesTab.GetType();

            //TextTab.DataContext = this;
            //textTab = new TextTab(Overlay);
            //TextTab.TargetPageType = textTab.GetType();


            ////SubscribeToOverlayChange(textTab);

            //textStyleTab = new TextStyleTab(Overlay);

            //TabNavigationView.Navigating += (sender, args) =>
            //{
            //    if (args.Page.GetType().Equals(TextTab))
            //    {
            //        args.Cancel = true; // Cancel the navigation
            //        // Perform your custom navigation logic here
            //    }
            //};
        }

        public ItemTemplate(string itemType)
        {
            InitializeComponent();
            TextMenuButton.Content = "Text";
            Overlay = new OverlayDisplay("Test");
            imagesTab = new ImagesTab();
            textTab = new TextTab(Overlay);
            textStyleTab = new TextStyleTab(Overlay);

            switch (itemType)
            {
                case "Image":
                    //ImageTab.Visibility = Visibility.Visible;
                    //Overlay = new OverlayDisplay("Image");
                    //imagesTab = new ImagesTab();
                    //ImageTab.TargetPageType = imagesTab.GetType();
                    break;
                case "Text":
                    {
                        //TextTab.Visibility = Visibility.Visible;
                        //Overlay = new OverlayDisplay("Text");
                        //textTab = new TextTab(Overlay);
                        //TextTab.TargetPageType = CreateInstance(typeof(TextTab), Overlay).GetType();
                        //TextTab.TargetPageType.GetField("Overlay").SetValue(textTab, Overlay);
                        //textStyleTab = new TextStyleTab(Overlay);
                        //StyleTab.TargetPageType = textStyleTab.GetType();
                        //StyleTab.TargetPageType.GetField("Overlay").SetValue(textStyleTab, Overlay);
                        

                        break;
                    }
            }
            Overlay = new OverlayDisplay("Test");
            //TabNavigationView.Navigating += (sender, args) =>
            //{
            //    if (args.Page.GetType().Equals(TextTab))
            //    {
            //        args.Cancel = true; // Cancel the navigation
            //        // Perform your custom navigation logic here
            //    }
            //};
            //TabNavigationView.ItemInvoked += (sender, args) =>
            //{
            //    if (args.RoutedEvent.Name == "TextTab")
            //    {
            //        TabNavigationView.Navigate(typeof(TextTab), Overlay);
            //    }
            //    else if (args.RoutedEvent.Name == "StyleTab")
            //    {
            //        TabNavigationView.Navigate(typeof(TextStyleTab), this);
            //    }
            //};
        }

        public object CreateInstance(Type type, OverlayDisplay overlay)
        {
            // Find a constructor that takes an Overlay as a parameter
            var constructor = type.GetConstructor(new[] { typeof(OverlayDisplay) });

            if (constructor != null)
            {
                // If found, invoke the constructor with the overlay
                return constructor.Invoke(new object[] { overlay });
            }

            // If no suitable constructor was found, fall back to the parameterless constructor
            return Activator.CreateInstance(type);
        }

        public void SetView(Uri uri)
        {
            ApplyTransition();
            FrameDisplay.Navigate(uri);
        }

        public void SetView(Page page)
        {
            ApplyTransition();
            FrameDisplay.Navigate(page);
        }

        public void ApplyTransition()
        {
            TransitionAnimationProvider.ApplyTransition(FrameDisplay, Transition.FadeInWithSlide, 200);
        }

    }
}
