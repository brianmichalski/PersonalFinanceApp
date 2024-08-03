using PersonalFinanceApp.Service;
using PersonalFinanceApp.Util;
using System.Windows;
using System.Windows.Controls;

namespace PersonalFinanceApp.UI.Import
{
    /// <summary>
    /// Interaction logic for ImportPage.xaml
    /// </summary>
    public partial class ImportPage : Page
    {
        public ImportPage()
        {
            InitializeComponent();
        }

        private void BtnLoadFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".xlsx"; // Default file extension
            dialog.Filter = "Excel files (.xlsx)|*.xlsx"; // Filter files by extension

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                txtBoxFileName.Text = dialog.FileName;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxFileName.Text == "")
            {
                MessageBox.Show("You need to load a file first", "Warning");
                return;
            }

            MessageBoxResult messageBoxResult = MessageBox.Show(
                "This procedure can cause conflicts with existing data",
                "Confirm import?",
                MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    IEnumerable<Model.Transaction> transactions = ExcelUtils.LoadSpreadSheet(
                        txtBoxFileName.Text, 
                        checkBoxHeader.IsChecked == true);

                    ImportService importService = new ImportService();
                    importService.ImportTransactions(transactions);

                    MessageBox.Show("Importing finished", "Done");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error during import");
                }
            }
        }

        private void btnDeleteDatabase_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxSureDelete.IsChecked == true)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "This procedure will ERASE all existing data",
                    "Confirm clear database?",
                    MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    ImportService importService = new ImportService();
                    importService.DeleteDatabase();
                    MessageBox.Show("Database is empty", "Done");
                }
            } else
            {
                MessageBox.Show("Select the confirmation checkbox", "Confirmation pending");
            }
        }
    }
}
