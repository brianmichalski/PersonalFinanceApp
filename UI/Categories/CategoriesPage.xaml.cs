using PersonalFinanceApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PersonalFinanceApp.UI.Categories
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private CategoryService _categoryService;
        private IEnumerable<Model.Category> _categories;
        public CategoriesPage()
        {
            InitializeComponent();
            this._categoryService = new CategoryService();
            this._categories = this._categoryService.FindAll();
            PanelEdit.Visibility = Visibility.Collapsed;
            DataGrid.DataContext = new ObservableCollection<Model.Category>(this._categories);
        }

        private void BtnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            PanelEdit.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this._categoryService.Add(null, txtBoxName.Text, double.Parse(txtBoxLimit.Text));
            this._categories = this._categoryService.FindAll();
            DataGrid.DataContext = new ObservableCollection<Model.Category>(this._categories);
            PanelEdit.Visibility = Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            PanelEdit.Visibility = Visibility.Collapsed;
        }
    }
}
