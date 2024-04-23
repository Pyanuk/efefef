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
    public partial class OrdersPage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public OrdersPage()
        {
            InitializeComponent();
            OrderDatailsDatagrid.ItemsSource = con.Orders.ToList();
            EmployesID.ItemsSource = con.Employees.Select(r => r.Position).ToList();
            PayID.ItemsSource = con.PaymentMethods.Select(s => s.MethodName).ToList();
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту полей
            if (string.IsNullOrWhiteSpace(TotalPriceTextBox.Text) || string.IsNullOrWhiteSpace(DateTextBox.Text) || EmployesID.SelectedItem == null || PayID.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка на корректность времени
            if (!DateTime.TryParse(DateTextBox.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректное время заказа.");
                return;
            }

            // Проверка на корректность цены
            if (!decimal.TryParse(TotalPriceTextBox.Text, out _) || decimal.Parse(TotalPriceTextBox.Text) < 0)
            {
                MessageBox.Show("Пожалуйста, введите корректную цену.");
                return;
            }

            Orders newOrder = new Orders();
            newOrder.TotalPrice = decimal.Parse(TotalPriceTextBox.Text);
            newOrder.OrderDateTime = DateTime.Parse(DateTextBox.Text);
            string employeePosition = EmployesID.SelectedItem as string;
            string paymentMethod = PayID.SelectedItem as string;

            var employee = con.Employees.FirstOrDefault(emp => emp.Position == employeePosition);
            var payment = con.PaymentMethods.FirstOrDefault(pay => pay.MethodName == paymentMethod);
            if (employee != null && payment != null)
            {
                newOrder.Employee_ID = employee.ID_Employee;
                newOrder.PaymentMethod_ID = payment.ID_PaymentMethod;
                con.Orders.Add(newOrder);
                con.SaveChanges();
                OrderDatailsDatagrid.ItemsSource = con.Orders.ToList();
            }
        }


        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatailsDatagrid.SelectedItem != null)
            {
                Orders selectedOrder = OrderDatailsDatagrid.SelectedItem as Orders;

                var orderToDelete = con.Orders.FirstOrDefault(ord => ord.ID_Order == selectedOrder.ID_Order);

                con.Orders.Remove(orderToDelete);
                con.SaveChanges();
                OrderDatailsDatagrid.ItemsSource = con.Orders.ToList();
            }
        }


        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatailsDatagrid.SelectedItem != null)
            {
                // Проверка на пустоту полей
                if (string.IsNullOrWhiteSpace(TotalPriceTextBox.Text) || string.IsNullOrWhiteSpace(DateTextBox.Text) || EmployesID.SelectedItem == null || PayID.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на корректность времени
                if (!DateTime.TryParse(DateTextBox.Text, out _))
                {
                    MessageBox.Show("Пожалуйста, введите корректное время заказа.");
                    return;
                }

                // Проверка на корректность цены
                if (!decimal.TryParse(TotalPriceTextBox.Text, out _) || decimal.Parse(TotalPriceTextBox.Text) < 0)
                {
                    MessageBox.Show("Пожалуйста, введите корректную цену.");
                    return;
                }

                Orders selectedOrder = OrderDatailsDatagrid.SelectedItem as Orders;
                var orderToUpdate = con.Orders.FirstOrDefault(ord => ord.ID_Order == selectedOrder.ID_Order);

                orderToUpdate.TotalPrice = decimal.Parse(TotalPriceTextBox.Text);
                orderToUpdate.OrderDateTime = DateTime.Parse(DateTextBox.Text);
                string employeePosition = EmployesID.SelectedItem as string;
                string paymentMethod = PayID.SelectedItem as string;

                var employee = con.Employees.FirstOrDefault(emp => emp.Position == employeePosition);
                var payment = con.PaymentMethods.FirstOrDefault(pay => pay.MethodName == paymentMethod);
                if (employee != null && payment != null)
                {
                    orderToUpdate.Employee_ID = employee.ID_Employee;
                    orderToUpdate.PaymentMethod_ID = payment.ID_PaymentMethod;
                    con.SaveChanges();
                    OrderDatailsDatagrid.ItemsSource = con.Orders.ToList();
                }
            }
        }

        private void OrderDatailsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderDatailsDatagrid.SelectedItem != null)
            {
                Orders selectedOrder = OrderDatailsDatagrid.SelectedItem as Orders;

                var employee = con.Employees.FirstOrDefault(emp => emp.ID_Employee == selectedOrder.Employee_ID);
                var payment = con.PaymentMethods.FirstOrDefault(pay => pay.ID_PaymentMethod == selectedOrder.PaymentMethod_ID);

                TotalPriceTextBox.Text = selectedOrder.TotalPrice.ToString();
                DateTextBox.Text = selectedOrder.OrderDateTime.ToString();

                if (employee != null)
                {
                    EmployesID.SelectedItem = employee.Position;
                }

                if (payment != null)
                {
                    PayID.SelectedItem = payment.MethodName;
                }
            }
        }
    }
}
