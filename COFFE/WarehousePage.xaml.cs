using System;
using System.Collections.Generic;
using System.IO;
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

namespace COFFE
{
   
    public partial class WarehousePage : Page
    {

        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public WarehousePage()
        {
            InitializeComponent();
            WarehouseDatagrid.ItemsSource = con.Warehouse.ToList();
            Suppliers.ItemsSource = con.Suppliers.Select(r => r.CompanyName).ToList();
            Products.ItemsSource = con.Products.Select(r => r.ProductName).ToList();
            WarehouseDatagrid.SelectionChanged += WarehouseDatagrid_SelectionChanged;
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {

            if (!IsValidProductName(ProductsTextBox.Text))
            {
                MessageBox.Show("Название продукта должно содержать только буквы и пробелы.");
                return;
            }

            if (!IsValidQuantity(QountityTextBox.Text))
            {
                MessageBox.Show("Количество должно содержать только цифры.");
                return;
            }

            Warehouse warehouse = new Warehouse();
            warehouse.ItemName = ProductsTextBox.Text;
            warehouse.Quantity = int.Parse(QountityTextBox.Text);


            string selectedSupplierName = Suppliers.SelectedItem as string;
            Suppliers selectedSupplier = con.Suppliers.FirstOrDefault(s => s.CompanyName == selectedSupplierName);


            string selectedProductName = Products.SelectedItem as string;
            Products selectedProduct = con.Products.FirstOrDefault(p => p.ProductName == selectedProductName);

            if (selectedSupplier != null)
            {
                warehouse.Supplier_ID = selectedSupplier.ID_Supplier;
            }
            if (selectedProduct != null)
            {
                warehouse.Products_ID = selectedProduct.ID_Product;
            }
            con.Warehouse.Add(warehouse);
            con.SaveChanges();
            WarehouseDatagrid.ItemsSource = con.Warehouse.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (WarehouseDatagrid.SelectedItem != null)
            {
                Warehouse selectedWarehouse = WarehouseDatagrid.SelectedItem as Warehouse;
                con.Warehouse.Remove(selectedWarehouse);
                con.SaveChanges();
                WarehouseDatagrid.ItemsSource = con.Warehouse.ToList();
            }
        }


        private void update_Click(object sender, RoutedEventArgs e)
        {
            Warehouse selectedWarehouse = WarehouseDatagrid.SelectedItem as Warehouse;

            if (!IsValidProductName(ProductsTextBox.Text))
            {
                MessageBox.Show("Название продукта должно содержать только буквы и пробелы.");
                return;
            }

            if (!IsValidQuantity(QountityTextBox.Text))
            {
                MessageBox.Show("Количество должно содержать только цифры.");
                return;
            }

            if (selectedWarehouse != null)
            {
                selectedWarehouse.ItemName = ProductsTextBox.Text;
                selectedWarehouse.Quantity = int.Parse(QountityTextBox.Text);

                string selectedSupplierName = Suppliers.SelectedItem as string;
                Suppliers selectedSupplier = con.Suppliers.FirstOrDefault(s => s.CompanyName == selectedSupplierName);

                string selectedProductName = Products.SelectedItem as string;
                Products selectedProduct = con.Products.FirstOrDefault(p => p.ProductName == selectedProductName);

                if (selectedSupplier != null)
                {
                    selectedWarehouse.Supplier_ID = selectedSupplier.ID_Supplier;
                }
                if (selectedProduct != null)
                {
                    selectedWarehouse.Products_ID = selectedProduct.ID_Product;
                }
                con.SaveChanges();
                WarehouseDatagrid.ItemsSource = con.Warehouse.ToList();
            }
        }

        private void WarehouseDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WarehouseDatagrid.SelectedItem != null)
            {
                Warehouse selectedWarehouse = WarehouseDatagrid.SelectedItem as Warehouse;
                ProductsTextBox.Text = selectedWarehouse.ItemName;
                QountityTextBox.Text = selectedWarehouse.Quantity.ToString();
                Suppliers.SelectedItem = con.Suppliers.FirstOrDefault(s => s.ID_Supplier == selectedWarehouse.Supplier_ID)?.CompanyName;
                Products.SelectedItem = con.Products.FirstOrDefault(p => p.ID_Product == selectedWarehouse.Products_ID)?.ProductName;
            }
        }

        private bool IsValidProductName(string productName)
        {
            return !string.IsNullOrWhiteSpace(productName) && !productName.Any(char.IsDigit) && productName.All(char.IsLetterOrDigit);
        }

        private bool IsValidQuantity(string quantity)
        {

            return !string.IsNullOrWhiteSpace(quantity) && quantity.All(char.IsDigit);
        }
    }
}
