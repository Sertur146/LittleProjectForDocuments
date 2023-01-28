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

namespace VladosProjectV2
{
    /// <summary>
    /// Логика взаимодействия для ClientAdd_Interface.xaml
    /// </summary>
    public partial class ClientAdd_Interface : Window
    {
        public ClientAdd_Interface()
        {
            InitializeComponent();
        }

        private enum Tables
        {
            Clients = 0
        }

        private static ProjectDB dbContext;

        public bool IsNull(params string[] args)
        {
            bool result = false;
            foreach (var str in args)
            {
                result |= string.IsNullOrEmpty(str);
            }
            return result;
        }

        private void btnClientAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsNull(txtFirst_name_toAdd.Text) || IsNull(txtSurname_toAdd.Text) ||
                       IsNull(txtMiddle_name_toAdd.Text) || IsNull(txtPhone_number_toAdd.Text) || IsNull(txtManager_toAdd.Text))
            {
                MessageBox.Show("Ошибка, не заполнено одно из полей!", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
                if (IsNull(txtFirst_name_toAdd.Text))
                {
                    txtFirst_name_toAdd.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtFirst_name_toAdd.Background = new SolidColorBrush(Colors.White); }
                if (IsNull(txtSurname_toAdd.Text))
                {
                    txtSurname_toAdd.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtSurname_toAdd.Background = new SolidColorBrush(Colors.White); }
                if (IsNull(txtMiddle_name_toAdd.Text))
                {
                    txtMiddle_name_toAdd.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtMiddle_name_toAdd.Background = new SolidColorBrush(Colors.White); }
                if (IsNull(txtPhone_number_toAdd.Text))
                {
                    txtPhone_number_toAdd.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtPhone_number_toAdd.Background = new SolidColorBrush(Colors.White); }
                if (IsNull(txtManager_toAdd.Text))
                {
                    txtManager_toAdd.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtManager_toAdd.Background = new SolidColorBrush(Colors.White); }
            }
            else
            {
                DBChanges.Client_Change(DBChanges.Operations.Add, txtFirst_name_toAdd.Text, txtMiddle_name_toAdd.Text,
                    txtSurname_toAdd.Text, txtPhone_number_toAdd.Text, txtComment_toAdd.Text, txtManager_toAdd.Text);
                MessageBox.Show("Информация о клиенте добавлена", "Сообщение",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
