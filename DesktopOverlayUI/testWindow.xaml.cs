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
using Wpf.Ui.Controls;
using Button = Wpf.Ui.Controls.Button;
namespace DesktopOverlayUI
{
    /// <summary>
    /// Interaction logic for testWindow.xaml
    /// </summary>
    public partial class testWindow
    {


        public testWindow()
        {
            InitializeComponent();
        }

        private void newItem(object sender, RoutedEventArgs e)
        {
            //Wpf.Ui.Controls.Button btn = new Wpf.Ui.Controls.Button();
            ControlTemplate template = this.FindResource("itemButtonTemplate") as ControlTemplate;
            //btn.Name = "test";
            
            //itemStackPanel.Children.Add(btn);
            NavigationItem btn = new NavigationItem(itemStackPanel,template);
            btn.Name = "item" + itemStackPanel.Children.Count;
            itemStackPanel.Children.Add(btn);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private void selectItem(object sender, RoutedEventArgs e)
        {

        }

        public class NavigationItem : Button
        {
            
            private StackPanel itemStackPanel;
            private Boolean isSelected;

            public NavigationItem(StackPanel stackPanel, ControlTemplate template)
            {
                itemStackPanel = stackPanel;
                isSelected = false;

                ContextMenu cm = new ContextMenu();

                System.Windows.Controls.MenuItem deleteBtn = new();
                deleteBtn.Header = "Delete";
                deleteBtn.Click += deleteItem;
                cm.Items.Add(deleteBtn);

                System.Windows.Controls.MenuItem editBtn = new();
                editBtn.Header = "Edit";
                cm.Items.Add(editBtn);

                this.Template = template;
                this.Name = "item" + stackPanel.Children.Count;
                this.ContextMenu = cm;
            }


            private void deleteItem(object sender, RoutedEventArgs e)
            {
                itemStackPanel.Children.Remove(this);
            }

            private void editItem(object sender, RoutedEventArgs e)
            {
                
            }
        }
    }
}
