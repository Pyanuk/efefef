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
using System.Windows.Shapes;

namespace COFFE
{

    public partial class Cashier : Window
    {
        public Cashier()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void Warehouse_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new WarehousePage();
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new ProductsPage();
        }

        private void ProductsCategorie_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new Category();
        }

        private void Castomisehen_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new Castom();
        }

        private void Coffe_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new СoffePage();
        }

        private void TypeCoffe_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new TypeCoffePage();
        }

        private void PayMet_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new PaymentPage();
        }

        private void OrderCoofe_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new OrderCoffePage();   
        }

        private void OrderDetalis_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new OrderDatailsPage();
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new OrdersPage();
        }

        private void Cassa_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new CassaPage();
        }
    }
}
