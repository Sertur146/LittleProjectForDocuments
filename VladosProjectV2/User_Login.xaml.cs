using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace VladosProjectV2
{
    /// <summary>
    /// Логика взаимодействия для User_Login.xaml
    /// </summary>
    public partial class User_Login : Window
    {
        public User_Login()
        {
            InitializeComponent();
        }

        private static ProjectDB dbContext;
        public bool LoginSuccess = false;
        public string user;

        private static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        public void Login()
        {
            var listUsers = dbContext.Users.ToList();
            foreach (var item in listUsers)
            {
                if (item.Login == txtLogin.Text && item.Password == GetHash(txtPassword.Password))
                {
                    LoginSuccess = true;
                    txtLogin.Text = string.Empty;
                    txtPassword.Password = string.Empty;
                }
            }
            if (LoginSuccess == true)
            { MessageBox.Show("Вход выполнен"); }
            else { MessageBox.Show("Неверно введен логин или пароль"); }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == "" || txtPassword.Password == "")
            {
                if (txtLogin.Text == "")
                {
                    MessageBox.Show("Не введен логин!");
                }
                if (txtPassword.Password == "")
                {
                    MessageBox.Show("Не введен пароль!");
                }
            }
            else 
            { 
                Login();
                if (LoginSuccess == true)
                {
                    MainWindow UserInterface = new MainWindow();
                    UserInterface.Show();
                    Hide();
                    UserInterface.Closed += Close;
                }
            }
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            User_Create UserCreate = new User_Create();
            UserCreate.Show();
        }

        private void frmLogin_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext = new ProjectDB();
        }

        private void txtLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            txtLogin.Text = null;
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = null;
        }

        private void Close(object sender, EventArgs e)
        {
            Close();
        }
    }
}
