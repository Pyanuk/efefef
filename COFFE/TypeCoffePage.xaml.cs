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
    public partial class TypeCoffePage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public TypeCoffePage()
        {
            InitializeComponent();
            TypeCoffeDatagrid.ItemsSource = con.Type_Coffee.ToList();

        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту полей
            if (string.IsNullOrWhiteSpace(TypeTextBox.Text) || string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка на отсутствие цифр и спец. знаков в названии типа кофе
            if (TypeTextBox.Text.Any(char.IsDigit) || !IsAlphabetic(TypeTextBox.Text))
            {
                MessageBox.Show("Тип кофе не должен содержать цифры или специальные символы.");
                return;
            }

            // Проверка на отрицательные значения, буквы и спец. знаки в цене
            if (!IsNumeric(PriceTextBox.Text) || int.Parse(PriceTextBox.Text) < 0)
            {
                MessageBox.Show("Цена должна быть положительным числом.");
                return;
            }

            Type_Coffee type_Coffee = new Type_Coffee();
            type_Coffee.Type_Coffee_Name = TypeTextBox.Text;
            type_Coffee.PRICE_Type_Coffee = int.Parse(PriceTextBox.Text);
            con.Type_Coffee.Add(type_Coffee);
            con.SaveChanges();
            TypeCoffeDatagrid.ItemsSource = con.Type_Coffee.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if(TypeCoffeDatagrid.SelectedItem != null)
            {
                Type_Coffee type_Coffee = TypeCoffeDatagrid.SelectedItem as Type_Coffee;
                con.Type_Coffee.Remove(type_Coffee);
                con.SaveChanges();
                TypeCoffeDatagrid.ItemsSource = con.Type_Coffee.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (TypeCoffeDatagrid.SelectedItem != null)
            {
                // Проверка на пустоту полей
                if (string.IsNullOrWhiteSpace(TypeTextBox.Text) || string.IsNullOrWhiteSpace(PriceTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на отсутствие цифр и спец. знаков в названии типа кофе
                if (TypeTextBox.Text.Any(char.IsDigit) || !IsAlphabetic(TypeTextBox.Text))
                {
                    MessageBox.Show("Тип кофе не должен содержать цифры или специальные символы.");
                    return;
                }

                // Проверка на отрицательные значения, буквы и спец. знаки в цене
                if (!IsNumeric(PriceTextBox.Text) || int.Parse(PriceTextBox.Text) < 0)
                {
                    MessageBox.Show("Цена должна быть положительным числом.");
                    return;
                }

                Type_Coffee type_Coffee = TypeCoffeDatagrid.SelectedItem as Type_Coffee;
                type_Coffee.Type_Coffee_Name = TypeTextBox.Text;
                type_Coffee.PRICE_Type_Coffee = int.Parse(PriceTextBox.Text);
                con.SaveChanges();
                TypeCoffeDatagrid.ItemsSource = con.Type_Coffee.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для обновления и укажите новые значения.");
            }
        }

        private void TypeCoffeDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeCoffeDatagrid.SelectedItem != null)
            {
                Type_Coffee type_Coffee = TypeCoffeDatagrid.SelectedItem as Type_Coffee;
                TypeTextBox.Text = type_Coffee.Type_Coffee_Name;
                PriceTextBox.Text = type_Coffee.PRICE_Type_Coffee.ToString();
            }
        }

        private bool IsAlphabetic(string input)
        {
            return input.All(char.IsLetter);
        }

        // Метод для проверки, является ли строка числом
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
