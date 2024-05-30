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

        private readonly BaseDisplay _base;
        private readonly OverlayDriver _overlayDriver;
 


        private readonly EditorForm _editor;


        public TextTab(BaseDisplay @base, OverlayDriver overlayDriver)
        {
            _base = @base;
            _overlayDriver = overlayDriver;
            _overlayDriver.SetText("Text Overlay");
            //_base.SetText("Text Base");
            _editor = new EditorForm();
            _editor.OkButton.Click += OkEditor;
            _editor.CancelButton.Click += HideEditor;
            _editor.ApplyButton.Click += ApplyEditor;
            _editor.ValueBox.Document = new FlowDocument(new Paragraph(new Run(_overlayDriver.GetText())));
            InitializeComponent();
        }

        private void UpdateText(object sender, RoutedEventArgs e)
        {
            //_base.OverlayText = TextInputBox.Text;
            _overlayDriver.SetText(TextInputBox.Text);
            _editor.ValueBox.Document = new FlowDocument(new Paragraph(new Run(_overlayDriver.GetText())));
            //Base.Show();
        }

        private void ToggleOverlay(object sender, RoutedEventArgs e)
        {
            var status = ToggleVisibility.IsChecked != null && ToggleVisibility.IsChecked.Value;
            
            if (status)
            {
                _base.Show();
                _overlayDriver.Show();
            }
            else
            {
                _base.Hide();
                _overlayDriver.Hide();
            }
        }

        private void ShowEditor(object sender, RoutedEventArgs e)
        {
            _editor.Show();
        }

        private void OkEditor(object sender, RoutedEventArgs e)
        {
            ApplyEditor(sender, e);
            _editor.Hide();
        }

        private void ApplyEditor(object sender, RoutedEventArgs e)
        {
            var fromRichText = new TextRange(_editor.ValueBox.Document.ContentStart, _editor.ValueBox.Document.ContentEnd).Text;
            TextInputBox.Text = fromRichText;
            _overlayDriver.SetText(fromRichText);
            //_base.OverlayText = fromRichText;
        }

        private void HideEditor(object sender, RoutedEventArgs e)
        {
            _editor.Hide();
        }



    }
}
