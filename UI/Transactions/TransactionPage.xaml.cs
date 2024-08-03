using PersonalFinanceApp.Model;
using PersonalFinanceApp.Service;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private CategoryService _categoryService;
        public ObservableCollection<Model.Category>? Categories { get; set; }
        private ObservableCollection<Model.Transaction> Transactions { get; set; } 
        public Model.Transaction? Transaction { get; set; }

        private const string DATE_FORMAT = "yyyy-mm-dd";

        public TransactionPage()
        {
            InitializeComponent();
            this._transactionService = new TransactionService();
            this._categoryService = new CategoryService();
            this.LoadCategories();
            this.LoadTransactions();

            DataContext = this;

            PanelEdit.Visibility = Visibility.Collapsed;
        }
        private void LoadTransactions()
        {
            this.Transactions = new ObservableCollection<Model.Transaction>(this._transactionService.FindAll());
            if (this.Categories != null)
            {
                DataGrid.DataContext = this.Transactions;
            }
        }

        private void LoadCategories()
        {
            this.Categories = new ObservableCollection<Model.Category>(this._categoryService.FindAll());
        }

        private void BtnNewTransaction_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Transaction = new Model.Transaction();
            this.Transaction.TransactionId = 0;
            txtBoxDate.Text = "";
            txtBoxDescription.Text = "";
            txtBoxAmount.Text = "";
            comboBoxCategory.SelectedItem = null;

            PanelEdit.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            bool dateValid = DateTime.TryParseExact(
                txtBoxDate.Text, DATE_FORMAT, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime date);

            if (!dateValid)
            {
                MessageBox.Show(
                    string.Format("Provide a valid date. Format: {0}", DATE_FORMAT), 
                    "Validation Error");
                txtBoxDate.Focus();
                return;
            }
            bool amountValid = double.TryParse(txtBoxAmount.Text, out double amount);
            if (!amountValid)
            {
                MessageBox.Show(
                    "Provide a valid amount number", 
                    "Validation Error");
                txtBoxAmount.Focus();
                return;
            }
            if (comboBoxCategory.SelectedValue != null)
            {
                this.Transaction.Category = (Model.Category)comboBoxCategory.SelectedValue;
            }
            this.Transaction.DateTime = date;
            this.Transaction.Description = txtBoxDescription.Text;
            this.Transaction.Amount = amount;
            try
            {
                this._transactionService.Save(this.Transaction);

                this.LoadTransactions();

                PanelEdit.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Error");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            PanelEdit.Visibility = Visibility.Collapsed;
        }

        private void EditRow_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)e.OriginalSource;
            var transaction = (Model.Transaction)btn.DataContext;

            this.Transaction = new Model.Transaction();
            this.Transaction.TransactionId = transaction.TransactionId;

            txtBoxDate.Text = transaction.DateTime.ToString(DATE_FORMAT);
            txtBoxDescription.Text = transaction.Description;
            txtBoxAmount.Text = transaction.Amount.ToString();
            comboBoxCategory.SelectedItem = transaction.Category;

            PanelEdit.Visibility = Visibility.Visible;
        }

        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                "Are you sure?",
                "Delete Confirmation",
                System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var btn = (Button)e.OriginalSource;
                var transaction = (Model.Transaction)btn.DataContext;
                this._transactionService.Delete(transaction);
                this.LoadTransactions();
            }
        }
    }
}
