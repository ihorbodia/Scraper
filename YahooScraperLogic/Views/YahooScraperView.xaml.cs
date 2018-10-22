using System.Windows.Controls;
using YahooScraperLogic.ViewModels;

namespace YahooScraperLogic.Views
{
    /// <summary>
    /// Interaction logic for YahooScraper.xaml
    /// </summary>
    public partial class YahooScraperView : UserControl
    {
        public YahooScraperView()
        {
            InitializeComponent();
            DataContext = new YahooScraperViewModel();
        }
    }
}
