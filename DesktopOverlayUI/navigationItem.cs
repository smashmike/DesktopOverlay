
using DesktopOverlayUI.pages;
using Microsoft.UI.Xaml.Controls.Primitives;
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
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;
using Button = Wpf.Ui.Controls.Button;

namespace DesktopOverlayUI
{
    public class NavigationItem : Button
    {
        private testWindow currentWindow;

        private StackPanel itemStackPanel;
        private Boolean isSelected;

        private Page page;
        private Uri pageUri;


        public NavigationItem(StackPanel stackPanel, ControlTemplate template, testWindow testWindow)
        {
            currentWindow = testWindow;
            itemStackPanel = stackPanel;
            isSelected = false;

            page = new pages.template();
            pageUri = new Uri("/pages/template.xaml", UriKind.Relative);


            ContextMenu cm = new ContextMenu();

            System.Windows.Controls.MenuItem deleteBtn = new();
            deleteBtn.Header = "Delete";
            deleteBtn.Click += deleteItem;
            cm.Items.Add(deleteBtn);

            System.Windows.Controls.MenuItem editBtn = new();
            editBtn.Header = "Rename";
            editBtn.Click += editItem;
            cm.Items.Add(editBtn);

            Template = template;
            int testCount = 0;
            foreach (NavigationItem item in stackPanel.Children.OfType<NavigationItem>())
            {
                if (item.Content.ToString().Contains("New Item"))
                {
                    testCount++;
                }
            }
            Name = "item" + stackPanel.Children.Count;
            if (testCount > 0)
            {
                Content = "New Item " + "(" + testCount + ")";
            }
            else
            {
                Content = "New Item";
            }
            ContextMenu = cm;
            Click += selectItem;
            ThemeResource theme = ThemeResource.ApplicationBackgroundBrush;
            Background = Resources.FindName("ApplicationBackgroundBrush") as SolidColorBrush;
            
            //Appearance = ControlAppearance.Secondary;


        }


        private void deleteItem(object sender, RoutedEventArgs e)
        {
            itemStackPanel.Children.Remove(this);
            
        }

        private async void editItem(object sender, RoutedEventArgs e)
        {

        }

        private void selectItem(object sender, RoutedEventArgs e)
        {
            if (!isSelected)
            {
                isSelected = true;
                testWindow.history.Add(itemStackPanel.Children.IndexOf(this));
                if (testWindow.history.Count > 10)
                {
                    testWindow.history.RemoveAt(0);
                }
                foreach (NavigationItem item in itemStackPanel.Children.OfType<NavigationItem>())
                {
                    if (!item.Equals(this) && item.IsSelected())
                    {
                        item.setSelected(false);
                        item.Appearance = ControlAppearance.Secondary;
                    }
                }
                //Appearance = ControlAppearance.Primary;
                currentWindow.setView(page);
            }
        }

        private void selectItem()
        {
            if (!isSelected)
            {
                isSelected = true;
                testWindow.history.Add(itemStackPanel.Children.IndexOf(this));
                if (testWindow.history.Count > 10)
                {
                    testWindow.history.RemoveAt(0);
                }
                
                foreach (NavigationItem item in itemStackPanel.Children.OfType<NavigationItem>())
                {
                    if (!item.Equals(this) && item.IsSelected())
                    {
                        item.setSelected(false);
                        item.Appearance = ControlAppearance.Secondary;
                    }
                }
                //Appearance = ControlAppearance.Primary;
                
                currentWindow.setView(page);
                
            }
        }

        public bool IsSelected()
        {
            return isSelected;
        }

        public void setSelected(bool selected)
        {
            isSelected = selected;

            if (!selected)
            {
                Appearance = ControlAppearance.Secondary;
            } else
            {
                selectItem();
            }
        }

    }
}
