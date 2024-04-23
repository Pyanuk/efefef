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

    public partial class СoffePage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public СoffePage()
        {
            InitializeComponent();
            CoffeeDatagrid.ItemsSource = con.Coffe.ToList();
            Type.ItemsSource = con.Type_Coffee.Select(r => r.Type_Coffee_Name).ToList();
            CoffeeDatagrid.SelectionChanged += CoffeeDatagrid_SelectionChanged;

        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            CoffeeDatagrid.SelectionChanged -= CoffeeDatagrid_SelectionChanged;

            // Проверка на пустоту полей
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(VidTextBox.Text) ||
                string.IsNullOrWhiteSpace(IceTextBox.Text) || string.IsNullOrWhiteSpace(PriceTextBox.Text) || Type.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля и выберите тип кофе.");
                return;
            }

            // Проверка на отсутствие цифр и спец. знаков в названии кофе и виде
            if (NameTextBox.Text.Any(char.IsDigit) || VidTextBox.Text.Any(char.IsDigit) ||
                !IsAlphabetic(NameTextBox.Text) || !IsAlphabetic(VidTextBox.Text))
            {
                MessageBox.Show("Название кофе и вид кофе не должны содержать цифры или специальные символы.");
                return;
            }

            // Проверка на отсутствие цифр и спец. знаков в поле "Лед"
            if (!bool.TryParse(IceTextBox.Text, out _))
            {
                MessageBox.Show("Введите корректное значение для поля 'Лед'.");
                return;
            }

            // Проверка на отрицательные значения, буквы и спец. знаки в цене
            if (!IsNumeric(PriceTextBox.Text) || int.Parse(PriceTextBox.Text) < 0)
            {
                MessageBox.Show("Цена должна быть положительным числом.");
                return;
            }

            Coffe newCoffee = new Coffe();
            newCoffee.Name_Coffe = NameTextBox.Text;
            newCoffee.Type_Of_Coffee = VidTextBox.Text;
            newCoffee.Ice = bool.Parse(IceTextBox.Text);
            newCoffee.Amount_Price = int.Parse(PriceTextBox.Text);
            string selectedTypeCoffeeName = Type.SelectedItem as string;
            Type_Coffee selectedTypeCoffee = con.Type_Coffee.FirstOrDefault(tc => tc.Type_Coffee_Name == selectedTypeCoffeeName);
            if (selectedTypeCoffee != null)
            {
                newCoffee.Type_Coffee_ID = selectedTypeCoffee.ID_Type_Coffee;
            }

            con.Coffe.Add(newCoffee);
            con.SaveChanges();

            CoffeeDatagrid.ItemsSource = con.Coffe.ToList();

            CoffeeDatagrid.SelectionChanged += CoffeeDatagrid_SelectionChanged;
        }



        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (CoffeeDatagrid.SelectedItem != null)
            {
                Coffe selectedCoffee = CoffeeDatagrid.SelectedItem as Coffe;
                con.Coffe.Remove(selectedCoffee);
                con.SaveChanges();
                CoffeeDatagrid.ItemsSource = con.Coffe.ToList();
            }
        }


        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (CoffeeDatagrid.SelectedItem != null && Type.SelectedItem != null)
            {
                Coffe selectedCoffee = CoffeeDatagrid.SelectedItem as Coffe;

                // Проверка на пустоту полей
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(VidTextBox.Text) ||
                    string.IsNullOrWhiteSpace(IceTextBox.Text) || string.IsNullOrWhiteSpace(PriceTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на отсутствие цифр и спец. знаков в названии кофе и виде
                if (NameTextBox.Text.Any(char.IsDigit) || VidTextBox.Text.Any(char.IsDigit) ||
                    !IsAlphabetic(NameTextBox.Text) || !IsAlphabetic(VidTextBox.Text))
                {
                    MessageBox.Show("Название кофе и вид кофе не должны содержать цифры или специальные символы.");
                    return;
                }

                // Проверка на отсутствие цифр и спец. знаков в поле "Лед"
                if (!bool.TryParse(IceTextBox.Text, out _))
                {
                    MessageBox.Show("Введите корректное значение для поля 'Лед'.");
                    return;
                }

                // Проверка на отрицательные значения, буквы и спец. знаки в цене
                if (!IsNumeric(PriceTextBox.Text) || int.Parse(PriceTextBox.Text) < 0)
                {
                    MessageBox.Show("Цена должна быть положительным числом.");
                    return;
                }

                selectedCoffee.Name_Coffe = NameTextBox.Text;
                selectedCoffee.Type_Of_Coffee = VidTextBox.Text;
                selectedCoffee.Ice = bool.Parse(IceTextBox.Text);
                selectedCoffee.Amount_Price = int.Parse(PriceTextBox.Text);

                con.SaveChanges();
                CoffeeDatagrid.ItemsSource = con.Coffe.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для обновления и укажите новые значения.");
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

        private void CoffeeDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoffeeDatagrid.SelectedItem != null)
            {
                Coffe selectedCoffee = CoffeeDatagrid.SelectedItem as Coffe;
                NameTextBox.Text = selectedCoffee.Name_Coffe;
                VidTextBox.Text = selectedCoffee.Type_Of_Coffee;
                IceTextBox.Text = selectedCoffee.Ice.ToString();
                Type.SelectedItem = selectedCoffee.Type_Coffee?.Type_Coffee_Name;
                PriceTextBox.Text = selectedCoffee.Amount_Price.ToString(); 
            }
        }
    }
}
