using ScottPlot;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PersonalFinanceApp.UI.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        IEnumerable<Model.Transaction>? Transactions { get; set; }
        public DashboardPage(IEnumerable<Model.Transaction>? _transactions)
        {
            InitializeComponent();

            this.ComboBoxYear.SelectedItem = DateTime.Now.Year.ToString();
            this.ComboBoxMonth.SelectedItem = DateTime.Now.ToString("MMMM").ToUpper();

            var legend = WpfPlot1.Plot.ShowLegend();
            legend.Location = Alignment.UpperRight;

            WpfPlot1.Plot.Axes.DateTimeTicksBottom();
            WpfPlot1.Plot.XLabel("Day");
            WpfPlot1.Plot.YLabel("Model.Transaction value");
            WpfPlot2.Plot.HideLegend();
            WpfPlot2.Plot.HideAxesAndGrid();

            if (_transactions != null && _transactions.Count() > 0)
            {
                this.RefreshDashboard(_transactions);
            }
        }

        private void BtnRefreshDashboard_Click(object sender, RoutedEventArgs e)
        {
            if ((this.Transactions?.Count() ?? 0) == 0)
            {
                MessageBox.Show("No data available to Refresh", "Warning");
                return; 
            }
            this.RefreshDashboard(this.Transactions);
        }

        public void RefreshDashboard(IEnumerable<Model.Transaction> transactions)
        {
            this.Transactions = transactions;
            int year = int.Parse(ComboBoxYear.SelectedItem.ToString());
            int month = ComboBoxMonth.SelectedIndex + 1;

            IEnumerable<Model.Transaction> monthTransactions = from transaction in transactions
                                                         where transaction.DateTime.Month == month
                                                         && transaction.DateTime.Year == year
                                                         select transaction;

            IEnumerable<Model.Transaction> monthIncomes = from transaction in monthTransactions
                                                    where transaction.Amount > 0
                                                    select transaction;

            IEnumerable<Model.Transaction> monthExpenses = from transaction in monthTransactions
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
            Color[] colors = [Colors.Red, Colors.Orange, Colors.Purple, Colors.Green, Colors.Blue];
            foreach (var item in expensesCategories)
            {
                if (count < 5)
                {
                    slices.Add(new PieSlice()
                    {
                        Value = item.Total,
                        Label = item.Label,
                        LabelFontSize = 14,
                        FillColor = colors[count]
                    });
                }
                else
                {
                    othersTotal += item.Total;
                }
                count++;
            }
            slices.Add(new PieSlice()
            {
                Value = othersTotal,
                Label = "Others",
                LabelFontSize = 14,
                FillColor = Colors.Yellow
            });

            var pie = WpfPlot2.Plot.Add.Pie(slices);
            pie.ShowSliceLabels = true;
            pie.LineColor = Colors.White;
            pie.SliceLabelDistance = 1.1;
            pie.DonutFraction = 0.5;

            WpfPlot2.Plot.Axes.AutoScale();
            WpfPlot2.Refresh();

            DataGrid1.DataContext = new ObservableCollection<Model.Transaction>(monthTransactions);
            DataGrid2.DataContext = new ObservableCollection<Model.Transaction>(monthExpenses);
        }
    }
}
