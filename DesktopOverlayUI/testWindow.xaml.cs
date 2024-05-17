
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
using Wpf.Ui;
using Wpf.Ui.Animations;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using Wpf.Ui.Markup;
using Button = Wpf.Ui.Controls.Button;
namespace DesktopOverlayUI
{
    /// <summary>
    /// Interaction logic for testWindow.xaml
    /// </summary>
    public partial class testWindow
    {
        public static List<int> history = new List<int>();
        public static List<NavigationItem> navigationItems = new List<NavigationItem>();
        
        public testWindow()
        {
            InitializeComponent();
            //frameDisplay.Source = new Uri("/pages/template.xaml", UriKind.Relative);
            ThemeService themeService = new ThemeService();
            //themeService.SetTheme(themeService.GetSystemTheme());

            
            
        }

        private async void newItem(object sender, RoutedEventArgs e)
        {
            //Wpf.Ui.Controls.Button btn = new Wpf.Ui.Controls.Button();
            string itemType = await promptDialog();

            if (itemType.Equals("None"))
            {
                return;
            }


            //ControlTemplate template = this.FindResource("itemButtonTemplate") as ControlTemplate;
            //btn.Name = "test";
            
            //itemStackPanel.Children.Add(btn);
            NavigationItem btn = new NavigationItem(itemStackPanel, this, itemType);
            btn.Name = "item" + itemStackPanel.Children.Count;

            itemStackPanel.Children.Add(btn);
            btn.setSelected(true);
            history.Add(itemStackPanel.Children.IndexOf(btn));
            if (history.Count > 10)
            {
                history.RemoveAt(0);
            }
        }

        public async Task<string> promptDialog()
        {
            ContentDialogService contentDialogService = new ContentDialogService();
            contentDialogService.SetDialogHost(dialog);

            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "New Overlay",
                Content = "Select an overlay type.",
                PrimaryButtonText = "Text",
                SecondaryButtonText = "Image",
                CloseButtonText = "Cancel",

            });

            string resultText = result switch
            {
                ContentDialogResult.Primary => "Text",
                ContentDialogResult.Secondary => "Image",
                ContentDialogResult.None => "None",
                _ => "None",
            };
            return resultText;
        }

        public void setView(Uri uri)
        {
            applyTransition();
            frameDisplay.Navigate(uri);
        }
        
        public void setView(Page page)
        {
            applyTransition();
            frameDisplay.Navigate(page);
        }

        public void applyTransition()
        {
            TransitionAnimationProvider.ApplyTransition(frameDisplay, Transition.FadeInWithSlide, 200);
        }


        private void settingsView(object sender, RoutedEventArgs e)
        {
            foreach (NavigationItem item in itemStackPanel.Children.OfType<NavigationItem>())
            {
                item.setSelected(false);
            }
            
            setView(new Uri("/pages/SettingsPage.xaml", UriKind.Relative));
        }

       
    }
}
