using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using DesktopOverlayUI.pages;
using Wpf.Ui.Controls;
using TextBlock = Wpf.Ui.Controls.TextBlock;


namespace DesktopOverlayUI;

/// <summary>
///     Interaction logic for OldWindow.xaml
/// </summary>
public partial class OldWindow
{
    public List<OverlayElement> Elements;
    public List<NavigationViewItem> NavigationViews;
    public Page Page = new();

    public OldWindow()
    {
        //    SystemThemeWatcher.Watch(this);
        //    InitializeComponent();
        Elements = new List<OverlayElement>();
        // clone template
        var path = Directory.GetCurrentDirectory() + "pages/template.xml";
        NavigationViews = new List<NavigationViewItem>();

        //Uri uri = new Uri("/pages/template.xaml", UriKind.Relative);
        //((Grid)page.Content).Children.OfType<ToggleSwitch>().First().Content = "test";
        //this.Content = page;
        var testItem = new NavigationViewItem();
        testItem.Content = "test";

        testItem.Visibility = Visibility.Visible;
        //navMenu.MenuItems.Add(testItem);
        //Console.WriteLine("test");
        //navMenu.Navigate("test");
        var testWindow = new MainWindow();
        testWindow.Show();

        Close();
    }

    private void newElement_Click(object sender, RoutedEventArgs e)

    {
        var element = new OverlayElement(NavMenu);

        Elements.Add(element);
        var item = new NavigationViewItem();
        item.Template = NavMenu.ItemTemplate;

        NavMenu.MenuItems.Add(element.GetItem());

        Console.WriteLine(NavMenu.MenuItems.Count);
    }


    public class OverlayElement
    {
        private readonly NavigationViewItem _navItem;
        //private ElementData _data;
        public Page ElementPage;


        public OverlayElement(NavigationView views)
        {
            var templateUri = new Uri("/pages/template.xaml", UriKind.Relative);
            var template = new ItemTemplate();

            //_data = new ElementData();

            if (templateUri != null)
            {
                ElementPage = new Page();
                var templatePage = (Page)Application.LoadComponent(templateUri);
                var control = new ContentControl
                {
                    Content = null
                };
                var grid = new Grid();
            }
            else
            {
                ElementPage = new Page();
            }

            ElementPage = new Page();
            ElementPage.Content = new Grid();

            ElementPage.Template = Application.Current.FindResource("pageTemplate") as ControlTemplate;


            var frame = new Frame();
            frame.Content = ElementPage;
            _navItem = new NavigationViewItem();
            _navItem.NavigationCacheMode = NavigationCacheMode.Required;
            var grid1 = new Grid();
            _navItem.Template = views.ItemTemplate;

            var page = new Page();
            var stackPanel = new StackPanel();
            var textBlock = new TextBlock();
            textBlock.Text = "Hello, world!";
            stackPanel.Children.Add(textBlock);
            page.Content = stackPanel;


            _navItem.TargetPageType = page.GetType();


            _navItem.Content = "test" + views.MenuItems.Count;
        }

        public T XamlClone<T>(T source)
        {
            var savedObject = XamlWriter.Save(source);

            // Load the XamlObject
            var stringReader = new StringReader(savedObject);
            var xmlReader = XmlReader.Create(stringReader);
            var target = (T)XamlReader.Load(xmlReader);

            return target;
        }

        public void TogglePage(NavigationView navigationView)
        {
            if (navigationView.MenuItems.IndexOf(ElementPage) != -1)
                navigationView.MenuItems.Remove(ElementPage);
            else
                navigationView.MenuItems.Add(ElementPage);
        }


        public NavigationViewItem GetItem()
        {
            return _navItem;
        }
    }
}