using OverlayForm;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DesktopOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <ControlTemplate  TargetType="TabItem">
//                                <Border Name = "Border" BorderThickness="1,1,1,0" BorderBrush="#FF1B1F23" Background="#FF24292E">
//                                    <ContentPresenter x:Name="ContentSite" VerticalAlignment="Center" TextBlock.FontFamily="Segoe UI
//"  HorizontalAlignment="Center" ContentSource="Header" Margin="12,2"/>
//                                </Border>
//                                <ControlTemplate.Triggers>
//                                    <Trigger Property = "IsSelected" Value="True">
//                                        <Setter TargetName = "Border" Property="Background" Value="#FF1B1F23"/>
//                                    </Trigger>
//                                    <Trigger Property = "IsSelected" Value="False">
//                                        <Setter TargetName = "Border" Property="Background" Value="#FF24292E"/>
//                                    </Trigger>
//                                </ControlTemplate.Triggers>
//                            </ControlTemplate>

    public partial class MainWindow : Window
    {

        public Overlay Overlay;
        

        public MainWindow()
        {
            InitializeComponent();
            OverlayElement.template = templateTab;



            activeElements.Items.Clear();
        }

        public class OverlayElement : TabItem
        {
            public static TabItem template;
            public static Overlay activeOverlay;

            // Element Settings
            public Element activeElement;
            private int id;
            private string elementType;
            private bool isActive;

            // TabControl
            private UIElementCollection templateItems;
            private Label visStatus;
            private Button toggleBtn;
            private Button updateBtn;

            // Element Settings
            private TabControl elementSettings;
            private TabItem currentSetting;
            private Grid settingsGrid;

            // Text Color Settings
            private Grid colorSetting;
            private Slider redSlider;
            private Slider greenSlider;
            private Slider blueSlider;
            private int redVal;
            private int greenVal;
            private int blueVal;


            public OverlayElement(Style t)
            {
                // init TabControl
                TabItem tempCopy = XamlClone<TabItem>(template);
                Header = tempCopy.Header;
                Content = tempCopy.Content;
                Grid templateGrid = Content as Grid;
                templateItems = templateGrid.Children;

                // TabControl Items
                visStatus = templateItems.OfType<Label>().First(label => label.Name.Equals("visStatus"));
                visStatus.Foreground = Brushes.Green;

                toggleBtn = templateItems.OfType<Button>().First(button => button.Name.Equals("toggleElement"));
                toggleBtn.Click += new RoutedEventHandler(toggleOverlay);

                updateBtn = templateItems.OfType<Button>().First(button => button.Name.Equals("updateElement"));
                updateBtn.Click += new RoutedEventHandler(updateOverlay);

                // Element Settings TabControl
                elementSettings = templateItems.OfType<TabControl>().First(tab => tab.Name.Equals("elementSettings"));
                elementSettings.SelectionChanged += new SelectionChangedEventHandler(loadSettings);
                currentSetting = (TabItem)elementSettings.SelectedItem;
                settingsGrid = elementSettings.SelectedContent as Grid;

                // init Overlay Window
                if (activeOverlay == null)
                {
                    activeOverlay = new Overlay();
                    activeOverlay.showOverlay();
                }

                // init Default Element Object
                id = new Random().Next(0, 100);
                activeElement = new Element(id, "1234");
                activeOverlay.addElement(activeElement);
                elementType = "String";
                isActive = true;

                
                

                redVal = 0;
                greenVal = 0;
                blueVal = 0;


            }

            public void onSettingsLoad(Object sender, EventArgs e)
            {
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

            public void updateOverlay(Object sender, RoutedEventArgs e)
            {
                //activeOverlay.updateOverlay(id);
                Console.WriteLine("aaaaaa");
                elementType = currentSetting.Header.ToString();
                if (elementType.Equals("Text"))
                {
                    string textValue = ((Grid)currentSetting.Content).Children.OfType<TextBox>().First(text => text.Name.Equals("textSettings")).Text;
                    //activeOverlay.updateOverlay(id, textValue);
                    //activeOverlay.removeElement(activeElement);
                    //activeElement = new Element(id, textValue);
                    activeElement.updateText(textValue);

                }

            }

            public void updateOverlay()
            {
                activeOverlay.updateOverlay(id);

                elementType = currentSetting.Header.ToString();
                if (elementType.Equals("Text"))
                {
                    string textValue = ((Grid)currentSetting.Content).Children.OfType<TextBox>().First(text => text.Name.Equals("textSettings")).Text;
                    //activeOverlay.updateOverlay(id, textValue);
                    activeElement.updateText(textValue);

                }
            }

            public void loadSettings(Object sender, RoutedEventArgs e)
            {
                currentSetting = (TabItem)elementSettings.SelectedItem;
                settingsGrid = (Grid)elementSettings.SelectedContent;

                colorSetting = settingsGrid.Children.OfType<Grid>().First(grid => grid.Name.Equals("textColorGrid"));
                redSlider = colorSetting.Children.OfType<Slider>().First(slider => slider.Name.Equals("textR"));
                greenSlider = colorSetting.Children.OfType<Slider>().First(slider => slider.Name.Equals("textG"));
                blueSlider = colorSetting.Children.OfType<Slider>().First(slider => slider.Name.Equals("textB"));
                redSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(updateTextColor);
                greenSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(updateTextColor);
                blueSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(updateTextColor);

            }

            public void updateTextColor(Object sender, RoutedEventArgs e)
            {
                redVal = (int)redSlider.Value;
                greenVal = (int)greenSlider.Value;
                blueVal = (int)blueSlider.Value;

                colorSetting.Children.OfType<Label>().First(label => label.Name.Equals("textRValue")).Content = redVal;
                colorSetting.Children.OfType<Label>().First(label => label.Name.Equals("textGValue")).Content = greenVal;
                colorSetting.Children.OfType<Label>().First(label => label.Name.Equals("textBValue")).Content = blueVal;

                

            }

            public void toggleOverlay(Object sender, RoutedEventArgs e)
            {
                if (isActive)
                {
                    visStatus.Foreground = Brushes.Red;
                    activeOverlay.removeElement(activeElement);
                    isActive = false;
                }
                else
                {
                    visStatus.Foreground = Brushes.Green;

                    activeOverlay.addElement(activeElement);
                    isActive = true;
                }
            }

            public void toggleOverlay()
            {
                if (isActive)
                {
                    visStatus.Foreground = Brushes.Red;

                    activeOverlay.removeElement(activeElement);
                    isActive = false;
                }
                else
                {
                    visStatus.Foreground = Brushes.Green;

                    activeOverlay.addElement(activeElement);
                    isActive = true;
                }
            }



        }

        private void newElement_Click(object sender, RoutedEventArgs e)
        {
            
            OverlayElement element = new OverlayElement(activeElements.Resources.FindName("tabStyle") as Style);
            element.Header = "Test";
            //element.Style = new Style();
            Style style = new Style();
            style.TargetType = typeof(TabItem);
            style.BasedOn = activeElements.Resources.FindName("tabStyle") as Style;
            style.Triggers.Add(new Trigger() { Property = TabItem.IsSelectedProperty, Value = true, Setters = { new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromArgb(255, 27, 32, 35))) } });
            style.Triggers.Add(new Trigger() { Property = TabItem.IsSelectedProperty, Value = false, Setters = { new Setter(Border.BackgroundProperty, new SolidColorBrush(Color.FromArgb(255, 36, 41, 46))) } });


            element.Style = style;
            
            activeElements.Items.Add(element);
            activeElements.SelectedIndex = activeElements.Items.Count - 1;
            //testTab.Items.Add(element);


        }

        private void toggleElement_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(1234);
            OverlayElement element = (OverlayElement)activeElements.SelectedItem;
            element.toggleOverlay();
        }

        
    }
}
