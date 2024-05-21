
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

        private readonly MainWindow? _currentWindow;
        private readonly ItemTemplate? _currentItem;
        private bool _isItem;

        private readonly StackPanel _itemStackPanel;
        private bool _isSelected;

        private readonly Page _page;




        public NavigationItem(StackPanel stackPanel, MainWindow mainWindow, string itemType)
        {
            _currentItem = null;
            _isItem = false;
            _currentWindow = mainWindow;
            _itemStackPanel = stackPanel;
            _isSelected = false;
            var resources = new ResourceDictionary
            {
                Source = new Uri("/MenuItemTemplate.xaml", UriKind.Relative)
            };
            var template = resources["ItemButtonTemplate"] as ControlTemplate;


            _page = new ItemTemplate(itemType);


            var cm = new ContextMenu();

            System.Windows.Controls.MenuItem deleteBtn = new()
            {
                Header = "Delete"
            };
            deleteBtn.Click += DeleteItem;
            cm.Items.Add(deleteBtn);

            System.Windows.Controls.MenuItem editBtn = new()
            {
                Header = "Rename"
            };
            editBtn.Click += EditItem;
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

            Content = "New " + itemType + " Item";//14

            ContextMenu = cm;
            Click += SelectItem;

            // Set default unselected appearance
            Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
            Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;


        }

        public NavigationItem(StackPanel stackPanel, ItemTemplate itemWindow, string itemType)
        {
            _currentWindow = null;
            _isItem = true;
            _currentItem = itemWindow;
            _itemStackPanel = stackPanel;
            _isSelected = false;
            var resources = new ResourceDictionary
            {
                Source = new Uri("/MenuItemTemplate.xaml", UriKind.Relative)
            };
            var template = resources["ItemButtonTemplate"] as ControlTemplate;


            _page = new ItemTemplate(itemType);


            var cm = new ContextMenu();

            System.Windows.Controls.MenuItem deleteBtn = new()
            {
                Header = "Delete"
            };
            deleteBtn.Click += DeleteItem;
            cm.Items.Add(deleteBtn);

            System.Windows.Controls.MenuItem editBtn = new()
            {
                Header = "Rename"
            };
            editBtn.Click += EditItem;
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
            if (!_isItem)
            {

                Content = "New " + itemType + " Item"; //14

                ContextMenu = cm;
            }

            Click += SelectItem;

            // Set default unselected appearance
            Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
            Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;


        }


        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (!_isItem)
            {
                _itemStackPanel.Children.Remove(this);

                if (_currentWindow != null && !_currentWindow.FrameDisplay.CanGoBack) return;
                switch (_itemStackPanel.Children.Count)
                {
                    case 0:
                        _currentWindow.FrameDisplay.Content = "";
                        return;
                    case > 0:
                    {
                        var lastChild = _itemStackPanel.Children[^1] as NavigationItem;
                        lastChild?.SelectItem();
                        break;
                    }
                }

                _currentWindow?.ApplyTransition();
            }
            else
            {
                _itemStackPanel.Children.Remove(this);

                if (_currentItem != null && !_currentItem.FrameDisplay.CanGoBack) return;
                switch (_itemStackPanel.Children.Count)
                {
                    case 0:
                        if (_currentItem != null) _currentItem.FrameDisplay.Content = "";
                        return;
                    case > 0:
                    {
                        var lastChild = _itemStackPanel.Children[^1] as NavigationItem;
                        lastChild?.SelectItem();
                        break;
                    }
                }

                _currentItem?.ApplyTransition();
            }
        }

        private async void EditItem(object sender, RoutedEventArgs e)
        {
            var newName = await RenameDialog();
            if (!newName.Equals(""))
            {
                Content = newName;
            } 
        }

        private async Task<string> RenameDialog()
        {
            var contentDialogService = new ContentDialogService();
            if (_currentWindow != null) contentDialogService.SetDialogHost(_currentWindow.Dialog);
            var frame = new Frame();
            Page renameDialogPage = new dialogViews.RenameView();
            frame.Navigate(renameDialogPage);


            var result = await contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "Rename Item",
                Content = frame,
                PrimaryButtonText = "Apply",
                CloseButtonText = "Cancel",

            });

            var resultText = result switch
            {
                ContentDialogResult.Primary => "Apply",
                ContentDialogResult.None => "None",
                _ => "None",
            };

            resultText = resultText == "Apply" ? ((dialogViews.RenameView)frame.Content).GetTextbox() : "";
            return resultText;
        }

        private void SelectItem(object sender, RoutedEventArgs e)
        {
            if (_isSelected) return;
            _isSelected = true;
            
            foreach (var item in _itemStackPanel.Children.OfType<NavigationItem>())
            {
                if (item.Equals(this) || !item.IsSelected()) continue;
                item.SetSelected(false);

                // Set unselected appearance
                Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
            }
            // Set selected appearance
            Background = FindResource("ControlStrokeColorDefaultBrush") as SolidColorBrush;
            Foreground = FindResource("AccentTextFillColorSecondaryBrush") as SolidColorBrush;

            if (!_isItem)
            {
                _currentWindow.SetView(_page);
            }
            else
            {
                _currentItem.SetView(_page);
            }
        }

        private void SelectItem()
        {
            if (_isSelected) return;
            _isSelected = true;

            
                
            foreach (NavigationItem item in _itemStackPanel.Children.OfType<NavigationItem>())
            {
                if (item.Equals(this) || !item.IsSelected()) continue;
                item.SetSelected(false);
                // Set unselected appearance
                Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
            }
            // Set selected appearance
            Background = FindResource("ControlStrokeColorDefaultBrush") as SolidColorBrush;
            Foreground = FindResource("AccentTextFillColorSecondaryBrush") as SolidColorBrush;

            if (!_isItem)
            {
                _currentWindow.SetView(_page);
            }
            else
            {
                _currentItem.SetView(_page);
            }
        }

        private bool IsSelected()
        {
            return _isSelected;
        }

        public void SetSelected(bool selected)
        {

            if (!selected)
            {
                _isSelected = false;
                Background = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
                Foreground = FindResource("ControlFillColorTransparentBrush") as SolidColorBrush;
            } else
            {
                SelectItem();
            }
        }
        

    }
}
