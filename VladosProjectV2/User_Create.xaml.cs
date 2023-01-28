using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для User_Create.xaml
    /// </summary>
    public partial class User_Create : Window
    {
        public User_Create()
        {
            InitializeComponent();
        }

        private static ProjectDB dbContext;

        private void txtLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            txtLogin.Text = null;
        }

        private void frmCreateUser_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext = new ProjectDB();
        }

        private static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string password = GetHash(txtPassword.Password);
            var listUsers = dbContext.Users.ToList();
            foreach (var item in listUsers)
            {
                if (item.Login == txtLogin.Text)
                {
                    MessageBox.Show("Такой логин уже существует!");
                }
            }
            DBChanges.User_Change(DBChanges.Operations.Add, 1, txtLogin.Text, password);
            Close();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = null;
        }
    }
}
