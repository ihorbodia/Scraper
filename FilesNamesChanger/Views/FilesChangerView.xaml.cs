using FilesNamesChanger.ViewModels;
using System.Windows.Controls;

namespace FilesNamesChanger.Views
{
    /// <summary>
    /// Interaction logic for FilesChangerView.xaml
    /// </summary>
    public partial class FilesChangerView : UserControl
    {
        public FilesChangerView()
        {
            InitializeComponent();
            DataContext = new FilesChangerViewModel();
        }
    }
}
