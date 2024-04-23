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
    public partial class ProductsPage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();

        public ProductsPage()
        {
            InitializeComponent();
            ProductsDatagrid.ItemsSource = con.Products.ToList();
            CategoryComboBox.ItemsSource = con.ProductCategories.Select(r => r.CategoryName).ToList();
            ProductsDatagrid.SelectionChanged += ProductsDatagrid_SelectionChanged;
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (!IsProductNameValid(ProductsNameTextBox.Text) ||
                !IsPriceValid(PriceTextBox.Text) ||
                !IsQuantityValid(QuantityTextBox.Text) ||
                !AreFieldsNotEmpty())
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            Products products = new Products();
            products.ProductName = ProductsNameTextBox.Text;
            products.Price = decimal.Parse(PriceTextBox.Text);
            products.Quantity = int.Parse(QuantityTextBox.Text);

            string category = CategoryComboBox.SelectedItem as string;
            ProductCategories selectedProduct = con.ProductCategories.FirstOrDefault(s => s.CategoryName == category);

            if (selectedProduct != null)
            {
                products.Category_ID = selectedProduct.ID_Category;
            }
            con.Products.Add(products);
            con.SaveChanges();
            ProductsDatagrid.ItemsSource = con.Products.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDatagrid.SelectedItem != null)
            {
                Products selectedProduct = ProductsDatagrid.SelectedItem as Products;
                con.Products.Remove(selectedProduct);
                con.SaveChanges();
                ProductsDatagrid.ItemsSource = con.Products.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDatagrid.SelectedItem != null &&
                IsProductNameValid(ProductsNameTextBox.Text) &&
                IsPriceValid(PriceTextBox.Text) &&
                IsQuantityValid(QuantityTextBox.Text) &&
                AreFieldsNotEmpty())
            {
                Products selectedProduct = ProductsDatagrid.SelectedItem as Products;
                selectedProduct.ProductName = ProductsNameTextBox.Text;
                selectedProduct.Price = decimal.Parse(PriceTextBox.Text);
                selectedProduct.Quantity = int.Parse(QuantityTextBox.Text);

                string selectedCategoryName = CategoryComboBox.SelectedItem as string;
                ProductCategories selectedCategory = con.ProductCategories.FirstOrDefault(c => c.CategoryName == selectedCategoryName);

                if (selectedCategory != null)
                {
                    selectedProduct.Category_ID = selectedCategory.ID_Category;
                }
                con.SaveChanges();

                ProductsDatagrid.ItemsSource = con.Products.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для обновления и заполните все поля корректно.");
            }
        }

        private void ProductsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsDatagrid.SelectedItem != null)
            {
                Products selectedProduct = ProductsDatagrid.SelectedItem as Products;
                ProductsNameTextBox.Text = selectedProduct.ProductName;
                PriceTextBox.Text = selectedProduct.Price.ToString();
                QuantityTextBox.Text = selectedProduct.Quantity.ToString();
                ProductCategories selectedCategory = con.ProductCategories.FirstOrDefault(c => c.ID_Category == selectedProduct.Category_ID);
                if (selectedCategory != null)
                {
                    CategoryComboBox.SelectedItem = selectedCategory.CategoryName;
                }
            }
        }

        private bool IsProductNameValid(string name)
        {
            return !Regex.IsMatch(name, @"[\d\W]");
        }

        private bool IsPriceValid(string price)
        {
            decimal parsedPrice;
            if (!decimal.TryParse(price, out parsedPrice))
                return false;

            return parsedPrice > 0;
        }

        private bool IsQuantityValid(string quantity)
        {
            int parsedQuantity;
            if (!int.TryParse(quantity, out parsedQuantity))
                return false;

            return parsedQuantity > 0;
        }

        private bool AreFieldsNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(ProductsNameTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(PriceTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(QuantityTextBox.Text) &&
                   CategoryComboBox.SelectedItem != null;
        }

    }
}
