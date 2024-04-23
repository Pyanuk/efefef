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

    public partial class SupliersPage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();

        public SupliersPage()
        {
            InitializeComponent();
            SupliersDatagrid.ItemsSource = con.Suppliers.ToList();
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CompanyTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(NomerTextBox.Text) || string.IsNullOrWhiteSpace(AdresTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!IsLettersOnly(CompanyTextBox.Text) || !IsLettersOnly(NameTextBox.Text))
            {
                MessageBox.Show("Имя компании и контактное лицо должны содержать только буквы и пробелы.");
                return;
            }

            if (!IsPhoneNumberValid(NomerTextBox.Text))
            {
                MessageBox.Show("Неправильный формат номера телефона. Пожалуйста, введите номер в формате +1234567890.");
                return;
            }

            if (!IsAddressValid(AdresTextBox.Text))
            {
                MessageBox.Show("Адрес не должен содержать специальные символы.");
                return;
            }

            Suppliers suppliers = new Suppliers();
            suppliers.CompanyName = CompanyTextBox.Text;
            suppliers.ContactName = NameTextBox.Text;
            suppliers.Phone = NomerTextBox.Text;
            suppliers.Email = EmailTextBox.Text;
            suppliers.ADDRESS_Suppliers = AdresTextBox.Text;

            con.Suppliers.Add(suppliers);
            con.SaveChanges();
            SupliersDatagrid.ItemsSource = con.Suppliers.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (SupliersDatagrid.SelectedItems != null)
            {
                foreach (var selectedItem in SupliersDatagrid.SelectedItems)
                {
                    Suppliers suppliers = selectedItem as Suppliers;
                    if (suppliers != null)
                    {
                        con.Suppliers.Remove(suppliers);
                    }
                }
                con.SaveChanges();
                SupliersDatagrid.ItemsSource = con.Suppliers.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (SupliersDatagrid.SelectedItem != null)
            {
                if (string.IsNullOrWhiteSpace(CompanyTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(NomerTextBox.Text) || string.IsNullOrWhiteSpace(AdresTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                if (!IsLettersOnly(CompanyTextBox.Text) || !IsLettersOnly(NameTextBox.Text))
                {
                    MessageBox.Show("Имя компании и контактное лицо должны содержать только буквы и пробелы.");
                    return;
                }

                if (!IsPhoneNumberValid(NomerTextBox.Text))
                {
                    MessageBox.Show("Неправильный формат номера телефона. Пожалуйста, введите номер в формате +1234567890.");
                    return;
                }

                if (!IsAddressValid(AdresTextBox.Text))
                {
                    MessageBox.Show("Адрес не должен содержать специальные символы.");
                    return;
                }

                Suppliers suppliers = SupliersDatagrid.SelectedItem as Suppliers;
                suppliers.CompanyName = CompanyTextBox.Text;
                suppliers.ContactName = NameTextBox.Text;
                suppliers.Phone = NomerTextBox.Text;
                suppliers.Email = EmailTextBox.Text;
                suppliers.ADDRESS_Suppliers = AdresTextBox.Text;
                con.SaveChanges();
                SupliersDatagrid.ItemsSource = con.Suppliers.ToList();
            }
        }

        private void SupliersDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SupliersDatagrid.SelectedItem != null)
            {
                Suppliers selectedSupplier = SupliersDatagrid.SelectedItem as Suppliers;
                CompanyTextBox.Text = selectedSupplier.CompanyName;
                NameTextBox.Text = selectedSupplier.ContactName;
                NomerTextBox.Text = selectedSupplier.Phone;
                EmailTextBox.Text = selectedSupplier.Email;
                AdresTextBox.Text = selectedSupplier.ADDRESS_Suppliers;
            }
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            Regex regex = new Regex(@"^\+\d+$");
            return regex.IsMatch(phoneNumber);
        }

        private bool IsLettersOnly(string input)
        {
            Regex regex = new Regex("^[A-Za-z\\s]+$");
            return !string.IsNullOrWhiteSpace(input) && regex.IsMatch(input);
        }

        private bool IsAddressValid(string address)
        {
            Regex regex = new Regex("^[A-Za-z0-9]+$");
            return regex.IsMatch(address);
        }
    }

}
