using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace OverlayForm
{
    public class OverlayForm : Window
    {
        public OverlayForm()
        {
            WindowStyle = WindowStyle.None;
            BorderBrush = Brushes.Transparent;
            BorderThickness = new Thickness(0);
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            Topmost = true;
            ShowInTaskbar = false;
            Left = 0;
            Top = 0;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            WindowState = WindowState.Maximized;
            
        }
    }
    public class Overlay
    {

        private OverlayForm form;
        public List<TextBox> textItems;
        private List<Element> elements;
        private Grid grid;

        public Overlay()
        {
            form = new OverlayForm();
            textItems = new List<TextBox>();
            elements = new List<Element>();

            grid = new Grid();
            grid.Background = Brushes.Transparent;


            TextBox textBox = new TextBox();
            textBox.Margin = new Thickness(10);
            textBox.FontWeight = FontWeights.Bold;
            textBox.Foreground = Brushes.Red;
            textBox.Background = Brushes.Transparent;
            textBox.BorderBrush = Brushes.Transparent;
            textBox.Text = "test";
            form.Background = Brushes.Transparent;
            textItems.Add(textBox);

            form.Content = grid;
            
            

        }
        public static void Main()
        {
        }

        public void updateOverlay(int id)
        {
            int index = grid.Children.IndexOf(elements.Find(e => e.id == id));
            if (index != -1)
            {
                grid.Children.RemoveAt(index);
                grid.Children.Insert(index, elements.Find(e => e.id == id).image);
            } else
            {
                Element element = new Element(id);
                grid.Children.Add(element);
            }
        }

        public void updateOverlay(int id, String text)
        {
            int index = grid.Children.IndexOf(elements.Find(e => e.id == id));
            if (index != -1)
            {
                grid.Children.RemoveAt(index);
                grid.Children.Insert(index, elements.Find(e => e.id == id));
            }
            else
            {
                Element element = new Element(id, text);
                grid.Children.Add(element);
            }
        }
        public void updateOverlay(Element element, String text)
        {
            grid.Children.Remove(element);
            element.updateText(text);

        }

        public void updateOverlay(int id, Image image)
        {
            int index = grid.Children.IndexOf(elements.Find(e => e.id == id));
            if (index != -1)
            {
                grid.Children.RemoveAt(index);
                grid.Children.Insert(index, elements.Find(e => e.id == id));
            }
            else
            {
                Element element = new Element(id, image);
                grid.Children.Add(element);
            }
        }

        public void addElement(Element element)
        {
            elements.Add(element);
            grid.Children.Add(element);
        }

        public void removeElement(Element element)
        {
            elements.Remove(element);
            grid.Children.Remove(element);
        }

        

        public void showOverlay()
        {
            form.Content = grid;
            form.Show();

        }

        public void hideOverlay()
        {
            form.Hide();
        }

    }

    public class Element : Canvas
    {
        public String text;
        public Image image;
        public bool visible;
        public int id;

        private TextBox textBox;

        public Element(int id)
        {
            Background = Brushes.Transparent;
            text = "";
            image = null;
            visible = true;
            this.id = id;
            
        }

        public Element(int id, String text)
        {
            Background = Brushes.Transparent;
            textBox = new TextBox();
            textBox.Margin = new Thickness(10);
            textBox.FontWeight = FontWeights.Bold;
            textBox.Foreground = Brushes.Red;
            textBox.Background = Brushes.Transparent;
            textBox.BorderBrush = Brushes.Transparent;
            textBox.Text = text;
            Children.Add(textBox);
            this.id = id;
            this.text = text;
            image = null;
            visible = true;
        }

        public Element(int id, Image image)
        {
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = image.Source;
            Background = imageBrush;
            this.id = id;
            text = "";
            this.image = image;
            visible = true;
        }

        public void updateText(String text)
        {
            textBox.Text = text;
        }

        public void updateImage(Image image)
        {
            if (image != null)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = image.Source;
                Background = imageBrush;
            }
        }
        
        
    }
}