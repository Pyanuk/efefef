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
using System.Windows.Shapes;

namespace COFFE
{
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void User_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new UserPage();
        }

        private void Role_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new RolePage(); 
        }

        private void Emploues_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new EmployeesPage();
        }

        private void Supliers_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.Content = new SupliersPage();
        }
    }
}
