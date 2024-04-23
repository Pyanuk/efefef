using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    
    public partial class RolePage : Page
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();
        public RolePage()
        {
            InitializeComponent();
            RoleDatagrid.ItemsSource = con.Roles.ToList();
            RoleDatagrid.SelectionChanged += RoleDatagrid_SelectionChanged;
        }

        private bool IsAlpha(string str)
        {
            Regex regex = new Regex("^[a-zA-Z]+$");
            return regex.IsMatch(str);
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RoleTextBox.Text) && IsAlpha(RoleTextBox.Text))
            {
                Roles roles = new Roles();
                roles.RoleName = RoleTextBox.Text;
                con.Roles.Add(roles);
                con.SaveChanges();
                RoleDatagrid.ItemsSource = con.Roles.ToList();
            }
            else
            {
                MessageBox.Show("Имя роли должно содержать только буквы и не должно быть пустым.");
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (RoleDatagrid.SelectedItem != null)
            {
                Roles selectedOrder = RoleDatagrid.SelectedItem as Roles;
                con.Roles.Remove(selectedOrder);
                con.SaveChanges();
                RoleDatagrid.ItemsSource = con.Roles.ToList();
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (RoleDatagrid.SelectedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(RoleTextBox.Text) && IsAlpha(RoleTextBox.Text))
                {
                    Roles selectedRole = RoleDatagrid.SelectedItem as Roles;
                    selectedRole.RoleName = RoleTextBox.Text;
                    con.SaveChanges();
                    RoleDatagrid.ItemsSource = con.Roles.ToList();
                }
                else
                {
                    MessageBox.Show("Имя роли должно содержать только буквы и не должно быть пустым.");
                }
            }
        }


        private void RoleDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoleDatagrid.SelectedItem != null)
            {
                Roles selectedRole = RoleDatagrid.SelectedItem as Roles;
                RoleTextBox.Text = selectedRole.RoleName;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            List<Roles> rolesList = Converter.DeserializeObject<List<Roles>>();
            foreach (var role in rolesList)
            {
                con.Roles.Add(role);
            }
            con.SaveChanges();
            RoleDatagrid.ItemsSource = null;
            RoleDatagrid.ItemsSource = con.Roles.ToList();
        }
    }
}
