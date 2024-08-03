using PersonalFinanceApp.Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PersonalFinanceApp.UI.Categories
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        private CategoryService _categoryService;
        public ObservableCollection<Model.Category>? Categories { get; set; }
        public Model.Category? Category { get; set; }

        public CategoriesPage()
        {
            InitializeComponent();

            this._categoryService = new CategoryService();
            this.LoadCategories();
            this.Category = new Model.Category();

            DataContext = this;

            PanelEdit.Visibility = Visibility.Collapsed;
        }

        private void LoadCategories()
        {
            this.Categories = new ObservableCollection<Model.Category>(this._categoryService.FindAll());
            if (this.Categories != null)
            {
                DataGrid.DataContext = new ObservableCollection<Model.Category>(this.Categories);
            }
        }

        private void BtnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            this.Category = new Model.Category();
            this.Category.CategoryId = 0;
            txtBoxName.Text = "";
            txtBoxLimit.Text = "";
            comboBoxParent.SelectedItem = null;

            PanelEdit.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool limitValid = double.TryParse(txtBoxLimit.Text, out double limit);
            if (!limitValid)
            {
                MessageBox.Show("Provide a valid limit number", "Validation Error");
                txtBoxLimit.Focus();
                return;
            }
            if (comboBoxParent.SelectedValue != null)
            {
                var Parent = (Model.Category)comboBoxParent.SelectedValue;
                if (Parent.CategoryId == Category.CategoryId)
                {
                    MessageBox.Show("The category can not be its own parent", "Validation Error");
                    comboBoxParent.Focus();
                    return;
                }
                this.Category.Parent = (Model.Category)comboBoxParent.SelectedValue;
            }
            else
            {
                this.Category.Parent = null;
            }

            this.Category.Name = txtBoxName.Text;
            this.Category.Limit = limit;

            try
            {
                this._categoryService.Save(this.Category);
                this.LoadCategories();

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
            var category = (Model.Category)btn.DataContext;

            this.Category = new Model.Category();
            this.Category.CategoryId = category.CategoryId;

            txtBoxName.Text = category.Name;
            txtBoxLimit.Text = category.Limit.ToString();
            comboBoxParent.SelectedItem = category.Parent;

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
                var category = (Model.Category)btn.DataContext;
                this._categoryService.Delete(category);
                this.LoadCategories();
            }
        }
    }
}
