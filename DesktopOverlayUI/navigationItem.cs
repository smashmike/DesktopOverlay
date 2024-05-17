
using DesktopOverlayUI.pages;
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
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;
using Wpf.Ui.Markup;
using Button = Wpf.Ui.Controls.Button;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace DesktopOverlayUI
{
    public class NavigationItem : Button   
    {

        private testWindow currentWindow;

        private StackPanel itemStackPanel;
        private Boolean isSelected;

        private Page page;
        private Uri pageUri;

        


        public NavigationItem(StackPanel stackPanel, testWindow testWindow, string itemType)
        {
            currentWindow = testWindow;
            itemStackPanel = stackPanel;
            isSelected = false;
            ResourceDictionary resources = new ResourceDictionary();
            resources.Source = new Uri("/MenuItemTemplate.xaml", UriKind.Relative);
            ControlTemplate? template = resources["itemButtonTemplate"] as ControlTemplate;


            page = new pages.ItemTemplate(itemType);
            pageUri = new Uri("/pages/ItemTemplate.xaml", UriKind.Relative);


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
            //int testCount = 0;
            //foreach (NavigationItem item in stackPanel.Children.OfType<NavigationItem>())
            //{
            //    if (item.Content.ToString().Contains("New Item"))
            //    {
            //        testCount++;
            //    }
            //}
            //Name = "item" + stackPanel.Children.Count;
            //if (testCount > 0)
            //{
            //    Content = "New Item " + "(" + testCount + ")";
            //}
            //else
            //{
            //    Content = "New Item";
            //}

            Content = "New " + itemType +" Item";//14

            ContextMenu = cm;
            Click += selectItem;

            // Set default unselected appearance
            Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
            Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;


        }


        private void deleteItem(object sender, RoutedEventArgs e)
        {
            itemStackPanel.Children.Remove(this);
            while (testWindow.history.IndexOf(itemStackPanel.Children.IndexOf(this)) != -1)
            {
                testWindow.history.Remove(itemStackPanel.Children.IndexOf(this));
            }
            if (currentWindow.frameDisplay.CanGoBack)
            {
                if (itemStackPanel.Children.Count == 0)
                {
                    currentWindow.frameDisplay.Content = "";
                    return;
                }
                
                if (itemStackPanel.Children.Count > 0)
                {
                    NavigationItem? lastChild = itemStackPanel.Children[itemStackPanel.Children.Count - 1] as NavigationItem;
                    lastChild?.selectItem();
                }

                currentWindow.applyTransition();
            }
        }

        private async void editItem(object sender, RoutedEventArgs e)
        {
            string newName = await renameDialog();
            if (!newName.Equals(""))
            {
                Content = newName;
            } 
        }

        public async Task<string> renameDialog()
        {
            ContentDialogService contentDialogService = new ContentDialogService();
            contentDialogService.SetDialogHost(currentWindow.dialog);
            Frame frame = new Frame();
            Page renameDialogPage = new dialogViews.RenameView();
            frame.Navigate(renameDialogPage);
            TextBox textBox = new TextBox();


            ContentDialogResult result = await contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "Rename Item",
                Content = frame,
                PrimaryButtonText = "Apply",
                CloseButtonText = "Cancel",

            });

            string resultText = result switch
            {
                ContentDialogResult.Primary => "Apply",
                ContentDialogResult.None => "None",
                _ => "None",
            };

            if (resultText == "Apply")
            {
                resultText = ((dialogViews.RenameView)frame.Content).getTextbox();
            } else
            {
                resultText = "";
            }
            return resultText;
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

                        // Set unselected appearance
                        Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                        Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                    }
                }
                // Set selected appearance
                Background = FindResource("ControlStrokeColorDefaultBrush") as SolidColorBrush;
                Foreground = FindResource("AccentTextFillColorSecondaryBrush") as SolidColorBrush;

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
                        // Set unselected appearance
                        Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                        Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                    }
                }
                // Set selected appearance
                Background = FindResource("ControlStrokeColorDefaultBrush") as SolidColorBrush;
                Foreground = FindResource("AccentTextFillColorSecondaryBrush") as SolidColorBrush;

                currentWindow.setView(page);
                
            }
        }

        public bool IsSelected()
        {
            return isSelected;
        }

        public void setSelected(bool selected)
        {

            if (!selected)
            {
                isSelected = false;
                Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
            } else
            {
                selectItem();
            }
        }
        

    }
}
