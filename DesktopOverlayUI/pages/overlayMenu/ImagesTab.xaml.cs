using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace DesktopOverlayUI.pages.overlayMenu
{
    /// <summary>
    /// Interaction logic for ImagesTab.xaml
    /// </summary>
    public partial class ImagesTab : Page
    {
        private List<ImageItem> _ImageItemsList = new List<ImageItem>();
        public List<ImageItem> ImageItemsList { 
            get {
                return _ImageItemsList; 
            } 
            set {
                _ImageItemsList = value; 
                OnPropertyChanged("ImageItemsList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        public ImagesTab()
        {
            ImageItem test1 = new ImageItem("test1");
            ImageItem test2 = new ImageItem("test2");
            _ImageItemsList.Add(test1);
            _ImageItemsList.Add(test2);
            //ImageItems = DependencyProperty.Register("imageItems", typeof(ObservableCollection<ImageItem>), typeof(ImagesTab));
            DataContext = this;
            InitializeComponent();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
