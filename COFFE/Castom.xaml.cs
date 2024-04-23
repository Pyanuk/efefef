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

namespace COFFE
{
    public partial class Castom : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public Castom()
        {
            InitializeComponent();
            CastomDatagrid.ItemsSource = con.Customization.ToList();
            ProductsID.ItemsSource = con.Coffe.Select(r => r.Name_Coffe).ToList();
            CoffeID.ItemsSource = con.Products.Select(r => r.ProductName).ToList();
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (CoffeID.SelectedItem == null || ProductsID.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите кофе и продукт.");
                return;
            }

            string selectedCoffeName = CoffeID.SelectedItem as string;
            string selectedProductName = ProductsID.SelectedItem as string;

            Coffe selectedCoffe = con.Coffe.FirstOrDefault(c => c.Name_Coffe == selectedCoffeName);
            Products selectedProduct = con.Products.FirstOrDefault(p => p.ProductName == selectedProductName);

            if (selectedCoffe != null && selectedProduct != null)
            {
                Customization customization = new Customization();
                customization.Coffe_ID = selectedCoffe.ID_Coffe;
                customization.Product_ID = selectedProduct.ID_Product;
                con.Customization.Add(customization);
                con.SaveChanges();
                CastomDatagrid.ItemsSource = con.Customization.ToList();
            }
        }



        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (CastomDatagrid.SelectedItem != null)
            {
                Customization selectedCustomization = CastomDatagrid.SelectedItem as Customization;
                con.Customization.Remove(selectedCustomization);
                con.SaveChanges();
                CastomDatagrid.ItemsSource = con.Customization.ToList();
            }
        }


        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (CastomDatagrid.SelectedItem != null && CoffeID.SelectedItem != null && ProductsID.SelectedItem != null)
            {
                Customization selectedCustomization = CastomDatagrid.SelectedItem as Customization;
                selectedCustomization.Coffe_ID = GetCoffeID();
                selectedCustomization.Product_ID = GetProductID();
                con.SaveChanges();
                CastomDatagrid.ItemsSource = con.Customization.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для обновления и укажите новые значения.");
            }
        }

        private int GetCoffeID()
        {
            string selectedCoffeName = CoffeID.SelectedItem as string;
            return con.Coffe.FirstOrDefault(c => c.Name_Coffe == selectedCoffeName)?.ID_Coffe ?? 0;
        }
        private int GetProductID()
        {
            string selectedProductName = ProductsID.SelectedItem as string;
            return con.Products.FirstOrDefault(p => p.ProductName == selectedProductName)?.ID_Product ?? 0;
        }

        private void CastomDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CastomDatagrid.SelectedItem != null)
            {
                Customization selectedCustomization = CastomDatagrid.SelectedItem as Customization;
                CoffeID.SelectedItem = con.Coffe.FirstOrDefault(c => c.ID_Coffe == selectedCustomization.Coffe_ID)?.Name_Coffe;
                ProductsID.SelectedItem = con.Products.FirstOrDefault(p => p.ID_Product == selectedCustomization.Product_ID)?.ProductName;
            }
        }
    }
}
