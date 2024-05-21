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
using WinRT;
using Wpf.Ui.Controls;

namespace DesktopOverlayUI.pages.overlayMenu
{
    /// <summary>
    /// Interaction logic for TextTab.xaml
    /// </summary>
    public sealed partial class TextTab : Page
    {

        public OverlayDisplay Overlay;
 


        private EditorForm editor;

        public TextTab()
        {
            Overlay = new OverlayDisplay();
            editor = new EditorForm();
            editor.OkButton.Click += OkEditor;
            editor.CancelButton.Click += HideEditor;
            editor.ApplyButton.Click += ApplyEditor;
            editor.ValueBox.Document = new FlowDocument(new Paragraph(new Run(Overlay.OverlayText)));

            InitializeComponent();
        }

        public TextTab(OverlayDisplay overlay)
        {
            Overlay = overlay;
            editor = new EditorForm();
            editor.OkButton.Click += OkEditor;
            editor.CancelButton.Click += HideEditor;
            editor.ApplyButton.Click += ApplyEditor;
            editor.ValueBox.Document = new FlowDocument(new Paragraph(new Run(Overlay.OverlayText)));
            InitializeComponent();
        }

        public void UpdateText(object sender, RoutedEventArgs e)
        {
            Overlay.OverlayText = TextInputBox.Text;
            //Overlay.Show();
        }

        public void ToggleOverlay(object sender, RoutedEventArgs e)
        {
            bool status = ToggleVisibility.IsChecked != null && ToggleVisibility.IsChecked.Value;
            
            if (status)
            {
                Overlay.Show();
            }
            else
            {
                Overlay.Hide();
            }
        }

        private void ShowEditor(object sender, RoutedEventArgs e)
        {
            editor.Show();
        }

        private void OkEditor(object sender, RoutedEventArgs e)
        {
            ApplyEditor(sender, e);
            editor.Hide();
        }

        private void ApplyEditor(object sender, RoutedEventArgs e)
        {
            string fromRichText = new TextRange(editor.ValueBox.Document.ContentStart, editor.ValueBox.Document.ContentEnd).Text;
            TextInputBox.Text = fromRichText;
            Overlay.OverlayText = fromRichText;
        }

        private void HideEditor(object sender, RoutedEventArgs e)
        {
            editor.Hide();
        }



    }
}
