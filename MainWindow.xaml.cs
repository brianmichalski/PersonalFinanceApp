using PersonalFinanceApp.Model;
using PersonalFinanceApp.Service;
using PersonalFinanceApp.UI.Categories;
using PersonalFinanceApp.UI.Dashboard;
using PersonalFinanceApp.UI.Transactions;
using PersonalFinanceApp.Util;
using System.Windows;
using System.Windows.Navigation;

namespace PersonalFinanceApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NavigationService _navigationService;
        TransactionService _transactionService;
        DashboardPage _dashboardPage;

        public MainWindow()
        {
            InitializeComponent();
            this._navigationService = _NavigationFrame.NavigationService;
            this._transactionService = new TransactionService();
            this._dashboardPage = new DashboardPage(null);
            _NavigationFrame.Navigate(this._dashboardPage);
        }

        private void BtnLoadSample_Click(object sender, RoutedEventArgs e)
        {
            this._navigationService.Navigate(new DashboardPage(LoadSamples()));
        }

        private IEnumerable<Transaction> LoadSamples()
        {
            return ExcelUtils.LoadSpreadSheet("../../../Sample/sample_transactions.xlsx", true);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

        private void BtnTransactions_Click(object sender, RoutedEventArgs e)
        {
            this._navigationService.Navigate(new TransactionPage());
        }

        private void BtnCategories_Click(object sender, RoutedEventArgs e)
        {
            this._navigationService.Navigate(new CategoriesPage());
        }
    }
}