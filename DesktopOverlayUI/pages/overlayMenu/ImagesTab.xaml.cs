using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using GameOverlay.Drawing;
using GameOverlay.Windows;
using Image = GameOverlay.Drawing.Image;

namespace DesktopOverlayUI.pages.overlayMenu
{
    /// <summary>
    /// Interaction logic for ImagesTab.xaml
    /// </summary>
    public partial class ImagesTab : Page
    {

        private OverlayDisplay _overlay;
        private OverlayDriver _overlayDriver;

        private List<ImageItem> _imageItemsList = [];
        public List<ImageItem> ImageItemsList { 
            get => _imageItemsList;
            set => _imageItemsList = value;
        }

        private GraphicsWindow _testWindow;

        public ImagesTab(OverlayDisplay overlay, OverlayDriver driver)
        {
            _overlay = overlay;
            _overlayDriver = driver;
            //var test1 = new ImageItem("test1");
            //var test2 = new ImageItem("test2");
            //_imageItemsList.Add(test1);
            //_imageItemsList.Add(test2);
            DataContext = this;
            InitializeComponent();
        }



        private void AddImage(object sender, RoutedEventArgs e)
        {
            var getImage = new OpenFileDialog
            {
                Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png"
            };
            if (getImage.ShowDialog() != true) return;
            var ImageFile = new BitmapImage(new Uri(getImage.FileName));
            var newImage = new ImageItem(System.IO.Path.GetFileName(getImage.FileName), new Uri(getImage.FileName));
            ImageItemsList.Add(newImage);
            ImageListView.Items.Refresh();
        }

        private void RemoveImage(object sender, RoutedEventArgs e)
        {
            var selectedImage = (ImageItem)ImageListView.SelectedItem;
            ImageItemsList.Remove(selectedImage);
            ImageListView.Items.Refresh();
            _overlayDriver.ClearOverlay();
        }

        private void ToggleOverlay(object sender, RoutedEventArgs e)
        {
            var status = ToggleVisibility.IsChecked != null && ToggleVisibility.IsChecked.Value;

            if (status)
            {
                _overlayDriver.Show();
            }
            else
            {
                _overlayDriver.Hide();
            }
        }

        private void SelectImage(object sender, SelectionChangedEventArgs e)
        {

            if (ImageListView == null) return;
            var selectedImage = (ImageItem)ImageListView.SelectedItem;
            if (selectedImage == null) return;
            _overlayDriver.ClearOverlay();
            ToggleVisibility.IsChecked = true;
            _overlayDriver.Show();
            //_overlay.SetImage(selectedImage);
            _overlayDriver.SetImage(selectedImage);
            //_overlayDriver.Show();
            //ToggleVisibility.IsChecked = true;
        }
    }
}
