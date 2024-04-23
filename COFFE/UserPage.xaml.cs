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
    public partial class UserPage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public UserPage()
        {
            InitializeComponent();
            UserDatagrid.ItemsSource = con.Users.ToList();
            RoleComboBox.ItemsSource = con.Roles.Select(r => r.RoleName).ToList();
            UserDatagrid.SelectionChanged += UserDatagrid_SelectionChanged;
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NasvanieTextBox.Text) && !string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                Users users = new Users();
                users.Username = NasvanieTextBox.Text;
                users.Password_Users = PasswordTextBox.Password;

                string roleName = RoleComboBox.SelectedItem as string;
                var role = con.Roles.FirstOrDefault(r => r.RoleName == roleName);

                if (role != null)
                {
                    users.Role_ID = role.ID_Role;
                    con.Users.Add(users);
                    con.SaveChanges();
                    UserDatagrid.ItemsSource = con.Users.ToList();
                }
                else
                {
                    MessageBox.Show("Указанная роль не существует. Пожалуйста, введите существующую роль.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (UserDatagrid.SelectedItem != null)
            {
                con.Users.Remove(UserDatagrid.SelectedItem as Users);
                con.SaveChanges();
                UserDatagrid.ItemsSource = con.Users.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NasvanieTextBox.Text) && !string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                if (UserDatagrid.SelectedItem != null)
                {
                    Users selectedUser = UserDatagrid.SelectedItem as Users;
                    selectedUser.Username = NasvanieTextBox.Text;
                    selectedUser.Password_Users = PasswordTextBox.Password;
                    string roleName = RoleComboBox.SelectedItem as string;
                    var role = con.Roles.FirstOrDefault(r => r.RoleName == roleName);
                    if (role != null)
                    {
                        selectedUser.Role_ID = role.ID_Role;
                    }
                    else
                    {
                        MessageBox.Show("Указанная роль не существует. Пожалуйста, введите существующую роль.");
                        return;
                    }
                    con.SaveChanges();
                    UserDatagrid.ItemsSource = con.Users.ToList();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
            }
        }

        private void UserDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserDatagrid.SelectedItem != null)
            {
                Users selectedUser = UserDatagrid.SelectedItem as Users;
                NasvanieTextBox.Text = selectedUser.Username;
                PasswordTextBox.Password = selectedUser.Password_Users;
                string roleName = con.Roles.FirstOrDefault(r => r.ID_Role == selectedUser.Role_ID)?.RoleName;
                if (!string.IsNullOrEmpty(roleName))
                {
                    RoleComboBox.SelectedItem = roleName;
                }
            }
        }
    }
}
