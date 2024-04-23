using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;

namespace COFFE
{
    public partial class Category : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public Category()
        {
            InitializeComponent();
            CategoryDatagrid.ItemsSource = con.ProductCategories.ToList();
            CategoryDatagrid.SelectionChanged += CategoryDatagrid_SelectionChanged;
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductsNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите название категории.");
                return;
            }

            if (!IsCategoryNameValid(ProductsNameTextBox.Text))
            {
                MessageBox.Show("Название категории не должно содержать цифры или специальные символы.");
                return;
            }

            ProductCategories productCategories = new ProductCategories();
            productCategories.CategoryName = ProductsNameTextBox.Text;
            con.ProductCategories.Add(productCategories);
            con.SaveChanges();
            CategoryDatagrid.ItemsSource = con.ProductCategories.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryDatagrid.SelectedItem != null)
            {
                ProductCategories selectedOrder = CategoryDatagrid.SelectedItem as ProductCategories;
                con.ProductCategories.Remove(selectedOrder);
                con.SaveChanges();
                CategoryDatagrid.ItemsSource = con.ProductCategories.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryDatagrid.SelectedItem != null)
            {
                if (string.IsNullOrWhiteSpace(ProductsNameTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите название категории.");
                    return;
                }

                if (!IsCategoryNameValid(ProductsNameTextBox.Text))
                {
                    MessageBox.Show("Название категории не должно содержать цифры или специальные символы.");
                    return;
                }

                ProductCategories selectCategories = CategoryDatagrid.SelectedItem as ProductCategories;
                selectCategories.CategoryName = ProductsNameTextBox.Text;
                con.SaveChanges();
                CategoryDatagrid.ItemsSource = con.ProductCategories.ToList();
            }
        }

        private void CategoryDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryDatagrid.SelectedItem != null)
            {
                ProductCategories selectCategories = CategoryDatagrid.SelectedItem as ProductCategories;
                ProductsNameTextBox.Text = selectCategories.CategoryName;
            }
        }

        private bool IsCategoryNameValid(string name)
        {

            return !Regex.IsMatch(name, @"[\d\W]");
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            List<ProductCategories> rolesList = Converter.DeserializeObject<List<ProductCategories>>();
            foreach (var role in rolesList)
            {
                con.ProductCategories.Add(role);
            }
            con.SaveChanges();
            CategoryDatagrid.ItemsSource = null;
            CategoryDatagrid.ItemsSource = con.ProductCategories.ToList();
        }
    }
}
