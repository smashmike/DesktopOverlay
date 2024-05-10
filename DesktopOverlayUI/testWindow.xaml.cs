
using Microsoft.UI.Xaml.Controls.Primitives;
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
using Wpf.Ui.Controls;
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
            frameDisplay.Source = new Uri("/pages/template.xaml", UriKind.Relative);

            
        }

        private void newItem(object sender, RoutedEventArgs e)
        {
            //Wpf.Ui.Controls.Button btn = new Wpf.Ui.Controls.Button();
            ControlTemplate template = this.FindResource("itemButtonTemplate") as ControlTemplate;
            //btn.Name = "test";
            
            //itemStackPanel.Children.Add(btn);
            NavigationItem btn = new NavigationItem(itemStackPanel,template,this);
            btn.Name = "item" + itemStackPanel.Children.Count;
            itemStackPanel.Children.Add(btn);
            history.Add(itemStackPanel.Children.IndexOf(btn));
            if (history.Count > 10)
            {
                history.RemoveAt(0);
            }
        }

        public void setView(Uri uri)
        {
            frameDisplay.Source = uri;
        }
        
        public void setView(Page page)
        {
            frameDisplay.Content = page;
        }

        

        
    }
}
