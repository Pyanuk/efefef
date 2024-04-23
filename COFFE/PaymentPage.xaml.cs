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
   
    public partial class PaymentPage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public PaymentPage()
        {
            InitializeComponent();
            PayCoffeDataGrid.ItemsSource = con.PaymentMethods.ToList();
        }


        private void insert_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту
            if (string.IsNullOrWhiteSpace(PayTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите название платежного метода.");
                return;
            }

            // Проверка на наличие цифр и специальных символов
            if (PayTextBox.Text.Any(char.IsDigit) || PayTextBox.Text.Any(char.IsPunctuation) || PayTextBox.Text.Any(char.IsSymbol))
            {
                MessageBox.Show("Пожалуйста, убедитесь, что название платежного метода не содержит цифр или специальных символов.");
                return;
            }

            PaymentMethods paymentMethods = new PaymentMethods();
            paymentMethods.MethodName = PayTextBox.Text;
            con.PaymentMethods.Add(paymentMethods);
            con.SaveChanges();
            PayCoffeDataGrid.ItemsSource = con.PaymentMethods.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (PayCoffeDataGrid.SelectedItem != null)
            {
                PaymentMethods paymentMethods = PayCoffeDataGrid.SelectedItem as PaymentMethods;
                con.PaymentMethods.Remove(paymentMethods);
                con.SaveChanges();
                PayCoffeDataGrid.ItemsSource = con.PaymentMethods.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (PayCoffeDataGrid.SelectedItem != null)
            {
                PaymentMethods paymentMethods = PayCoffeDataGrid.SelectedItem as PaymentMethods;

                // Проверка на пустоту
                if (string.IsNullOrWhiteSpace(PayTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, введите название платежного метода.");
                    return;
                }

                // Проверка на наличие цифр и специальных символов
                if (PayTextBox.Text.Any(char.IsDigit) || PayTextBox.Text.Any(char.IsPunctuation) || PayTextBox.Text.Any(char.IsSymbol))
                {
                    MessageBox.Show("Пожалуйста, убедитесь, что название платежного метода не содержит цифр или специальных символов.");
                    return;
                }

                paymentMethods.MethodName = PayTextBox.Text;
                con.SaveChanges();
                PayCoffeDataGrid.ItemsSource = con.PaymentMethods.ToList();
            }
        }

        private void PayCoffeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PayCoffeDataGrid.SelectedItem != null)
            {
                PaymentMethods paymentMethods = PayCoffeDataGrid.SelectedItem as PaymentMethods;
                PayTextBox.Text = paymentMethods.MethodName;
            }
        }
    }
}
