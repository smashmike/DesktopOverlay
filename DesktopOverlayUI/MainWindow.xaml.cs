
using System;
using System.Collections;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //frameDisplay.Source = new Uri("/pages/template.xaml", UriKind.Relative);
            var themeService = new ThemeService();
            themeService.SetTheme(themeService.GetSystemTheme());

            
            
        }

        private async void NewItem(object sender, RoutedEventArgs e)
        {
            //Wpf.Ui.Controls.Button btn = new Wpf.Ui.Controls.Button();
            var itemType = await PromptDialog();

            if (itemType.Equals("None"))
            {
                return;
            }


            //ControlTemplate template = this.FindResource("itemButtonTemplate") as ControlTemplate;
            //btn.Name = "test";

            //itemStackPanel.Children.Add(btn);
            var btn = new NavigationItem(ItemStackPanel, this, itemType)
            {
                Name = "item" + ItemStackPanel.Children.Count
            };

            ItemStackPanel.Children.Add(btn);
            btn.SetSelected(true);
        }

        public async Task<string> PromptDialog()
        {
            var contentDialogService = new ContentDialogService();
            contentDialogService.SetDialogHost(Dialog);

            var result = await contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "New Overlay",
                Content = "Select an overlay type.",
                PrimaryButtonText = "Text",
                SecondaryButtonText = "Image",
                CloseButtonText = "Cancel",

            });

            var resultText = result switch
            {
                ContentDialogResult.Primary => "Text",
                ContentDialogResult.Secondary => "Image",
                ContentDialogResult.None => "None",
                _ => "None",
            };
            return resultText;
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


        private void SettingsView(object sender, RoutedEventArgs e)
        {
            foreach (var item in ItemStackPanel.Children.OfType<NavigationItem>())
            {
                item.SetSelected(false);
            }
            
            SetView(new Uri("/pages/SettingsPage.xaml", UriKind.Relative));
        }

       
    }
}
