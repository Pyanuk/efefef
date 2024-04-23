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
    public partial class OrderDatailsPage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public OrderDatailsPage()
        {
            InitializeComponent();
            OrderDatailsDatagrid.ItemsSource = con.OrderDetails.ToList();
            DateID.ItemsSource = con.Orders.Select(r => r.OrderDateTime).ToList();
            ProductID.ItemsSource = con.Products.Select(s => s.ProductName).ToList();
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту полей
            if (DateID.SelectedItem == null || ProductID.SelectedItem == null || string.IsNullOrWhiteSpace(QyantityTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка на отсутствие отрицательных значений, букв и специальных символов в количестве
            if (!IsNumeric(QyantityTextBox.Text) || int.Parse(QyantityTextBox.Text) < 0)
            {
                MessageBox.Show("Количество должно быть положительным целым числом.");
                return;
            }

            OrderDetails orderDetails = new OrderDetails();
            orderDetails.Quantity = int.Parse(QyantityTextBox.Text);
            DateTime orderDateTime = (DateTime)DateID.SelectedItem;
            string productName = ProductID.SelectedItem as string;
            var order = con.Orders.FirstOrDefault(o => o.OrderDateTime == orderDateTime);
            var product = con.Products.FirstOrDefault(p => p.ProductName == productName);

            if (order != null && product != null)
            {
                orderDetails.Order_ID = order.ID_Order;
                orderDetails.Product_ID = product.ID_Product;
                con.OrderDetails.Add(orderDetails);
                con.SaveChanges();
                OrderDatailsDatagrid.ItemsSource = con.OrderDetails.ToList();
            }
        }


        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatailsDatagrid.SelectedItem != null)
            {
                OrderDetails selectedOrderDetails = OrderDatailsDatagrid.SelectedItem as OrderDetails;
                con.OrderDetails.Remove(selectedOrderDetails);
                con.SaveChanges();
                OrderDatailsDatagrid.ItemsSource = con.OrderDetails.ToList();
            }
        }


        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatailsDatagrid.SelectedItem != null)
            {
                // Проверка на пустоту полей
                if (DateID.SelectedItem == null || ProductID.SelectedItem == null || string.IsNullOrWhiteSpace(QyantityTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на отсутствие отрицательных значений, букв и специальных символов в количестве
                if (!IsNumeric(QyantityTextBox.Text) || int.Parse(QyantityTextBox.Text) < 0)
                {
                    MessageBox.Show("Количество должно быть положительным целым числом.");
                    return;
                }

                OrderDetails selectedOrderDetails = OrderDatailsDatagrid.SelectedItem as OrderDetails;
                OrderDetails orderDetailsToUpdate = con.OrderDetails.FirstOrDefault(od => od.ID_OrderDetail == selectedOrderDetails.ID_OrderDetail);

                if (orderDetailsToUpdate != null)
                {
                    orderDetailsToUpdate.Quantity = int.Parse(QyantityTextBox.Text);
                    con.SaveChanges();
                    OrderDatailsDatagrid.ItemsSource = con.OrderDetails.ToList();
                }
            }
        }

        private void OrderDatailsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderDatailsDatagrid.SelectedItem != null)
            {
                OrderDetails selectedOrderDetails = OrderDatailsDatagrid.SelectedItem as OrderDetails;
                Orders selectedOrder = con.Orders.FirstOrDefault(o => o.ID_Order == selectedOrderDetails.Order_ID);
                Products selectedProduct = con.Products.FirstOrDefault(p => p.ProductName == selectedOrderDetails.Products.ProductName);
                DateID.SelectedItem = selectedOrder.OrderDateTime;
                ProductID.SelectedItem = selectedProduct.ProductName;
                QyantityTextBox.Text = selectedOrderDetails.Quantity.ToString();
            }
        }

        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
