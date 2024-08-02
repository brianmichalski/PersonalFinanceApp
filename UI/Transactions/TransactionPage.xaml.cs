using PersonalFinanceApp.Model;
using PersonalFinanceApp.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PersonalFinanceApp.UI.Transactions
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class TransactionPage : Page
    {
        private TransactionService _transactionService;
        private IEnumerable<Model.Transaction> _transactions;

        public TransactionPage()
        {
            InitializeComponent();
            this._transactionService = new TransactionService();
            this._transactions = this._transactionService.FindAll();
            PanelEdit.Visibility = Visibility.Collapsed;
            DataGrid.DataContext = new ObservableCollection<Model.Transaction>(this._transactions);

        }

        private void BtnNewTransaction_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PanelEdit.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Parse(txtBoxDate.Text);
            string description = txtBoxDescription.Text;
            double amount = double.Parse(txtBoxAmount.Text);
            TransactionType transactionType = TransactionType.Output;

            this._transactionService.Add(transactionType, null, description, dateTime, amount);
            this._transactions = this._transactionService.FindAll();
            DataGrid.DataContext = new ObservableCollection<Model.Transaction>(this._transactions);
            PanelEdit.Visibility = Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            PanelEdit.Visibility = Visibility.Collapsed;
        }
    }
}
