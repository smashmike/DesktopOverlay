using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace DesktopOverlayUI.pages.overlayMenu;

/// <summary>
///     Interaction logic for ImagesTab.xaml
/// </summary>
public partial class ImagesTab : Page
{
    private BaseDisplay _base;
    private readonly OverlayDriver _overlayDriver;

    public ImagesTab(BaseDisplay @base, OverlayDriver driver)
    {
        _base = @base;
        _overlayDriver = driver;
        //var test1 = new ImageItem("test1");
        //var test2 = new ImageItem("test2");
        //_imageItemsList.Add(test1);
        //_imageItemsList.Add(test2);
        DataContext = this;
        InitializeComponent();
    }

    public List<ImageItem> ImageItemsList { get; } = [];


    private void AddImage(object sender, RoutedEventArgs e)
    {
        var getImage = new OpenFileDialog
        {
            Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png"
        };
        if (getImage.ShowDialog() != true) return;
        var newImage = new ImageItem(Path.GetFileName(getImage.FileName), new Uri(getImage.FileName));
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
            _overlayDriver.Show();
        else
            _overlayDriver.Hide();
    }

    private void SelectImage(object sender, SelectionChangedEventArgs e)
    {
        if (ImageListView == null) return;
        var selectedImage = (ImageItem)ImageListView.SelectedItem;
        if (selectedImage == null) return;
        _overlayDriver.ClearOverlay();
        ToggleVisibility.IsChecked = true;
        _overlayDriver.Show();
        //_base.SetImage(selectedImage);
        _overlayDriver.SetImage(selectedImage);
        //_overlayDriver.Show();
        //ToggleVisibility.IsChecked = true;
    }
}