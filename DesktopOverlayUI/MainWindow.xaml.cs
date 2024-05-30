﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DesktopOverlayUI.pages;
using Wpf.Ui;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace DesktopOverlayUI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow

{
    private readonly Page _settingsPage;


    public MainWindow()
    {
        InitializeComponent();
        //frameDisplay.Source = new Uri("/pages/template.xaml", UriKind.Relative);
        _settingsPage = new SettingsPage(this);


        var themeService = new ThemeService();
        themeService.SetTheme(themeService.GetSystemTheme());
        ApplicationThemeManager.Apply(
            ApplicationTheme.Dark,
            WindowBackdropType.Acrylic
        );
        //WindowBackdropType = WindowBackdropType.Mica;
    }

    private async void NewItem(object sender, RoutedEventArgs e)
    {
        //Wpf.Ui.Controls.Button btn = new Wpf.Ui.Controls.Button();
        var itemType = await PromptDialog();

        if (itemType.Equals("None")) return;


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

    private async Task<string> PromptDialog()
    {
        var contentDialogService = new ContentDialogService();
        contentDialogService.SetDialogHost(Dialog);

        var result = await contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions
        {
            Title = "New Base",
            Content = "Select an Overlay type.",
            PrimaryButtonText = "Text",
            SecondaryButtonText = "Image",
            CloseButtonText = "Cancel"
        });

        var resultText = result switch
        {
            ContentDialogResult.Primary => "Text",
            ContentDialogResult.Secondary => "Image",
            ContentDialogResult.None => "None",
            _ => "None"
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
        foreach (var item in ItemStackPanel.Children.OfType<NavigationItem>()) item.SetSelected(false);

        SetView(_settingsPage);
    }
}