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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Appearance;
using System.IO;
using System.Windows.Markup;
using DesktopOverlayUI.pages;
using Wpf.Ui.Controls;


namespace DesktopOverlayUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Page page = new Page();
        public List<OverlayElement> elements;
        public List<NavigationViewItem> navigationViews;
        public MainWindow()
        {
        //    SystemThemeWatcher.Watch(this);
        //    InitializeComponent();
            elements = new List<OverlayElement>();
            // clone template
            String path = Directory.GetCurrentDirectory() + "pages/template.xml";
            navigationViews = new List<NavigationViewItem>();

            //Uri uri = new Uri("/pages/template.xaml", UriKind.Relative);
            //((Grid)page.Content).Children.OfType<ToggleSwitch>().First().Content = "test";
            //this.Content = page;
            NavigationViewItem testItem = new NavigationViewItem();
            testItem.Content = "test";
            
            testItem.Visibility = Visibility.Visible;
            //navMenu.MenuItems.Add(testItem);
            //Console.WriteLine("test");
            //navMenu.Navigate("test");
            

        }

        private void newElement_Click(object sender, RoutedEventArgs e)

        {
            OverlayElement element = new OverlayElement(navMenu);
            
            elements.Add(element);
            NavigationViewItem item = new NavigationViewItem();
            item.Template = navMenu.ItemTemplate;
            
            
            navMenu.MenuItems.Add(element.getItem());
            Console.WriteLine(navMenu.MenuItems.Count);
            

        
        }

        

        public class OverlayElement
        {
            public Page elementPage;
            private NavigationViewItem navItem;

            
            
            public OverlayElement(NavigationView views)
            {
                Uri templateUri = new Uri("/pages/template.xaml", UriKind.Relative);
                template template = new template();

                if (templateUri != null)
                {
                    elementPage = new Page();
                    Page templatePage = (Page)Application.LoadComponent(templateUri);
                    ContentControl control = new ContentControl();
                    control.Content = null;

                    // Clone the template page
                    elementPage = XamlClone(templatePage);
                }
                else
                {
                    elementPage = new Page();
                }

                navItem = new NavigationViewItem();
                navItem.NavigationCacheMode = NavigationCacheMode.Required;

                navItem.Template = views.ItemTemplate;
                navItem.TargetPageType = elementPage.GetType();

                navItem.Content = "test" + views.MenuItems.Count;
            }
            public T XamlClone<T>(T source)
            {
                string savedObject = System.Windows.Markup.XamlWriter.Save(source);

                // Load the XamlObject
                StringReader stringReader = new StringReader(savedObject);
                System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader);
                T target = (T)System.Windows.Markup.XamlReader.Load(xmlReader);

                return target;
            }

            public NavigationViewItem getItem()
            {
                return navItem;
            }



        }
    }
}
