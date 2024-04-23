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
using System.Security.Cryptography;

namespace COFFE
{
    
    public partial class MainWindow : Window
    {
        private Coffee_EmbrasureEntities con = new Coffee_EmbrasureEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string enteredLogin = Login.Text;
            string enteredPassword = Password.Password;


            var user = con.Users.FirstOrDefault(u => u.Username == enteredLogin);

            if (user != null)
            {
                switch (user.Role_ID)
                {
                    case 1:
                        Admin admin = new Admin();
                        admin.Show();
                        break;
                    case 2:
                        Cashier cashier = new Cashier();
                        cashier.Show();
                        break;
                    default:
                        MessageBox.Show("Неправильная роль пользователя.");
                        break;
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

    }
}
