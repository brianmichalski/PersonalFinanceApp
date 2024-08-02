using PersonalFinance.Model;
using PersonalFinance.Service;
using PersonalFinance.Util;
using ScottPlot;
using ScottPlot.Plottables;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PersonalFinance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TransactionService transactionService;
        IEnumerable<Transaction> sampleTransactions;

        public MainWindow()
        {
            InitializeComponent();
            this.transactionService = new TransactionService();
            this.ComboBoxYear.SelectedItem = DateTime.Now.Year.ToString();
            this.ComboBoxMonth.SelectedItem = DateTime.Now.ToString("MMMM").ToUpper();
        }

        private void BtnLoadSample_Click(object sender, RoutedEventArgs e)
        {
            this.LoadSamples();
        }

        private void LoadSamples()
        {
            if (sampleTransactions == null)
            {
                sampleTransactions = ExcelUtils.LoadSpreadSheet("../../../Sample/sample_transactions.xlsx", true);
            }
            this.RefrehDashboard(sampleTransactions);
        }

        private void RefrehDashboard(IEnumerable<Transaction> transactions)
        {
            int year = int.Parse(ComboBoxYear.SelectedItem.ToString());
            int month = ComboBoxMonth.SelectedIndex + 1;

            IEnumerable<Transaction> monthTransactions = from transaction in transactions
                                                    where transaction.DateTime.Month == month
                                                    && transaction.DateTime.Year == year
                                                    select transaction;

            IEnumerable<Transaction> monthIncomes = from transaction in monthTransactions
                                                    where transaction.Amount > 0
                                                    select transaction;

            IEnumerable<Transaction> monthExpenses = from transaction in monthTransactions
                                                     where transaction.Amount < 0
                                                     select transaction;

            if (monthIncomes.Count() == 0 && monthExpenses.Count() == 0)
            {
                MessageBox.Show("No data for the period selected", "Warning");
                return;
            }
            WpfPlot1.Plot.Clear();
            WpfPlot2.Plot.Clear();

            double[] incomeX = monthIncomes.Select(x => x.DateTime.ToOADate()).ToArray();
            double[] incomeY = monthIncomes.Select(x => x.Amount).ToArray();

            double[] expenseX = monthExpenses.Select(x => x.DateTime.ToOADate()).ToArray();
            double[] expenseY = monthExpenses.Select(x => x.Amount * -1).ToArray();
            
            var incomeLine = WpfPlot1.Plot.Add.ScatterLine(incomeX, incomeY, Colors.Green);
            var expenseLine = WpfPlot1.Plot.Add.ScatterLine(expenseX, expenseY, Colors.Violet);
            incomeLine.Label = "Income";
            expenseLine.Label = "Expense";

            var legend = WpfPlot1.Plot.ShowLegend();
            legend.Location = Alignment.UpperRight;

            WpfPlot1.Plot.Axes.DateTimeTicksBottom();
            WpfPlot1.Plot.XLabel("Day");
            WpfPlot1.Plot.YLabel("Transaction value");
            WpfPlot1.Plot.Title("Cash Flow");
            WpfPlot1.Plot.Axes.AutoScale();
            WpfPlot1.Refresh();

            var expensesCategories = monthExpenses
                .GroupBy(t => new { Label = t.Category.Name })
                .Select(grp => new
                {
                    Label = grp.Key.Label,
                    Total = grp.Sum(t => t.Amount)
                })
                .OrderByDescending(t => t.Total);

            List<PieSlice> slices = new();
            int count = 0;
            double othersTotal = 0;
            Color[] colors = [Colors.Red, Colors.Orange, Colors.Gold, Colors.Green, Colors.Blue];
            foreach (var item in expensesCategories)
            {
                if (count < 5)
                {
                    slices.Add(new PieSlice() { Value = item.Total, Label = item.Label, FillColor = colors[count] });
                }
                else
                {
                    othersTotal += item.Total;
                }
                count++;
            }
            slices.Add(new PieSlice() { Value = othersTotal, Label = "Others", FillColor = Colors.Yellow });

            var pie = WpfPlot2.Plot.Add.Pie(slices);
            pie.DonutFraction = 0.5;

            WpfPlot2.Plot.ShowLegend();
            WpfPlot2.Plot.Title("Expenses By Category");
            WpfPlot2.Plot.Axes.AutoScale();
            WpfPlot2.Refresh();

            DataGrid1.DataContext = new ObservableCollection<Transaction>(monthTransactions);
            DataGrid2.DataContext = new ObservableCollection<Transaction>(monthExpenses);
        }

        private void BtnRefreshDashboard_Click(object sender, RoutedEventArgs e)
        {
            this.LoadSamples();
        }
    }
}