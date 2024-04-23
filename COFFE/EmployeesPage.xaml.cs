using Microsoft.AspNet.Identity;
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
    public partial class EmployeesPage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public EmployeesPage()
        {
            InitializeComponent();
            EmployeesDatagrid.ItemsSource = con.Employees.ToList();
            RoleComboBox.ItemsSource = con.Roles.Select(r => r.RoleName).ToList();
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PositionTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!IsLettersOnly(FirstNameTextBox.Text) ||
                !IsLettersOnly(LastNameTextBox.Text) ||
                !IsLettersOnly(PositionTextBox.Text))
            {
                MessageBox.Show("Имя, фамилия и должность должны содержать только буквы.");
                return;
            }

            Employees employees = new Employees();
            employees.FirstName = FirstNameTextBox.Text;
            employees.LastName = LastNameTextBox.Text;
            employees.Position = PositionTextBox.Text;
            employees.Password = PasswordNameTextBox.Text;

            string roleName = RoleComboBox.SelectedItem as string;
            var role = con.Roles.FirstOrDefault(r => r.RoleName == roleName);

            if (role != null)
            {
                employees.Role_ID = role.ID_Role;
                con.Employees.Add(employees);
                con.SaveChanges();
                EmployeesDatagrid.ItemsSource = con.Employees.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите роль для сотрудника.");
            }
        }

        private bool IsLettersOnly(string input)
        {
            return input.All(char.IsLetter);
        }


        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDatagrid.ItemsSource != null)
            {
                con.Employees.Remove(EmployeesDatagrid.SelectedItem as Employees);
                con.SaveChanges();
                EmployeesDatagrid.ItemsSource = con.Employees.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDatagrid.SelectedItem != null)
            {
                if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PositionTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PasswordNameTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                if (!IsLettersOnly(FirstNameTextBox.Text) ||
                    !IsLettersOnly(LastNameTextBox.Text) ||
                    !IsLettersOnly(PositionTextBox.Text))
                {
                    MessageBox.Show("Имя, фамилия и должность должны содержать только буквы.");
                    return;
                }

                Employees selectedEmployee = EmployeesDatagrid.SelectedItem as Employees;
                if (selectedEmployee != null)
                {
                    selectedEmployee.FirstName = FirstNameTextBox.Text;
                    selectedEmployee.LastName = LastNameTextBox.Text;
                    selectedEmployee.Position = PositionTextBox.Text;
                    selectedEmployee.Password = PasswordNameTextBox.Text;
                    string roleName = RoleComboBox.SelectedItem as string;
                    var role = con.Roles.FirstOrDefault(r => r.RoleName == roleName);
                    if (role != null)
                    {
                        selectedEmployee.Role_ID = role.ID_Role;
                        con.SaveChanges();
                        EmployeesDatagrid.ItemsSource = con.Employees.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите роль для сотрудника.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для обновления.");
            }
        }



        private void EmployeesDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employees selectedEmployee = EmployeesDatagrid.SelectedItem as Employees;

            if (selectedEmployee != null)
            {

                FirstNameTextBox.Text = selectedEmployee.FirstName;
                LastNameTextBox.Text = selectedEmployee.LastName;
                PositionTextBox.Text = selectedEmployee.Position;


                string roleName = con.Roles.FirstOrDefault(r => r.ID_Role == selectedEmployee.Role_ID)?.RoleName;
                RoleComboBox.SelectedItem = roleName;

                PasswordNameTextBox.Text = selectedEmployee.Password;
            }
        }
    }
}
