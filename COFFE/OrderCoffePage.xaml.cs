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

    public partial class OrderCoffePage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public OrderCoffePage()
        {
            InitializeComponent();
            OrderCoffeDataGrid.ItemsSource = con.OrderCoffe.ToList();
            CoffeID.ItemsSource = con.Coffe.Select(s => s.Name_Coffe).ToList();
            OrdersID.ItemsSource = con.Orders.Select(r => r.OrderDateTime).ToList();

        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту полей
            if (OrdersID.SelectedItem == null || CoffeID.SelectedItem == null || string.IsNullOrWhiteSpace(QyantityTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка на отсутствие специальных символов и отрицательных значений в количестве
            if (!IsNumeric(QyantityTextBox.Text) || int.Parse(QyantityTextBox.Text) < 0)
            {
                MessageBox.Show("Количество должно быть положительным целым числом.");
                return;
            }

            OrderCoffe orderCoffe = new OrderCoffe();
            orderCoffe.Quantity = int.Parse(QyantityTextBox.Text);

            DateTime orderDateTime = (DateTime)OrdersID.SelectedItem;
            string coffeeName = CoffeID.SelectedItem as string;

            var order = con.Orders.FirstOrDefault(o => o.OrderDateTime == orderDateTime);
            var coffee = con.Coffe.FirstOrDefault(c => c.Name_Coffe == coffeeName);

            if (order != null && coffee != null)
            {
                orderCoffe.Order_ID = order.ID_Order;
                orderCoffe.Coffe_ID = coffee.ID_Coffe;

                con.OrderCoffe.Add(orderCoffe);
                con.SaveChanges();

                OrderCoffeDataGrid.ItemsSource = con.OrderCoffe.ToList();
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (OrderCoffeDataGrid.SelectedItem != null)
            {
                OrderCoffe selectedOrderCoffe = OrderCoffeDataGrid.SelectedItem as OrderCoffe;
                if (selectedOrderCoffe != null)
                {
                    con.OrderCoffe.Remove(selectedOrderCoffe);
                    con.SaveChanges();
                    OrderCoffeDataGrid.ItemsSource = con.OrderCoffe.ToList();
                }
            }
        }



        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (OrderCoffeDataGrid.SelectedItem != null)
            {
                // Проверка на пустоту полей
                if (OrdersID.SelectedItem == null || CoffeID.SelectedItem == null || string.IsNullOrWhiteSpace(QyantityTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на отсутствие специальных символов и отрицательных значений в количестве
                if (!IsNumeric(QyantityTextBox.Text) || int.Parse(QyantityTextBox.Text) < 0)
                {
                    MessageBox.Show("Количество должно быть положительным целым числом.");
                    return;
                }

                OrderCoffe selectedOrderCoffe = OrderCoffeDataGrid.SelectedItem as OrderCoffe;
                var orderCoffeToUpdate = con.OrderCoffe.FirstOrDefault(oc => oc.ID_OrderProduct == selectedOrderCoffe.ID_OrderProduct);

                if (orderCoffeToUpdate != null)
                {
                    orderCoffeToUpdate.Quantity = int.Parse(QyantityTextBox.Text);

                    DateTime orderDateTime = (DateTime)OrdersID.SelectedItem;
                    string coffeeName = CoffeID.SelectedItem as string;

                    var order = con.Orders.FirstOrDefault(o => o.OrderDateTime == orderDateTime);
                    var coffee = con.Coffe.FirstOrDefault(c => c.Name_Coffe == coffeeName);

                    if (order != null && coffee != null)
                    {
                        orderCoffeToUpdate.Order_ID = order.ID_Order;
                        orderCoffeToUpdate.Coffe_ID = coffee.ID_Coffe;

                        con.SaveChanges();
                        OrderCoffeDataGrid.ItemsSource = con.OrderCoffe.ToList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для обновления и укажите новые значения.");
            }
        }


        private void OrderCoffeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderCoffeDataGrid.SelectedItem != null)
            {
                OrderCoffe selectedOrderCoffe = OrderCoffeDataGrid.SelectedItem as OrderCoffe;
                OrdersID.SelectedItem = selectedOrderCoffe.Orders.OrderDateTime;
                CoffeID.SelectedItem = selectedOrderCoffe.Coffe.Name_Coffe;
                QyantityTextBox.Text = selectedOrderCoffe.Quantity.ToString();
            }
        }

        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
