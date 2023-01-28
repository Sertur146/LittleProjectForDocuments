using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Reflection;
using FilePath = System.IO.Path;
using Microsoft.Win32;

namespace VladosProjectV2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static ProjectDB dbContext;

        bool firststart = true; public bool status = true;
        public int clientID; string path;
        public List<string> DocNames, ClientPhones;
        public List<string> FIO = new List<string>();
        BitmapImage bmp;

        private void User_Interface_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext = new ProjectDB();
            FillComboBox();
            FillScanBox();
            cmbPhone_number.SelectedIndex = -1;
            cmbDocSearch.SelectedIndex = -1;
            txtManager.Text = "Ответственный менеджер";
        }

        public bool IsNull(params string[] args)
        {
            bool result = false;
            foreach (var str in args)
            {
                result |= string.IsNullOrEmpty(str);
            }
            return result;
        }

        private void UpdateClientControlStatus()
        {
            cmbPhone_number.Background = IsNull(cmbPhone_number.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217));
            txtFirstName.Background = IsNull(txtFirstName.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Colors.Transparent);
            txtMiddleName.Background = IsNull(txtMiddleName.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Colors.Transparent);
            txtSurname.Background = IsNull(txtSurname.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Colors.Transparent);
            txtManager.Background = IsNull(txtManager.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Colors.Transparent);
            txtPhone_namber.Background = IsNull(txtPhone_namber.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217));
        }

        private void UpdateDocControlStatus()
        {
            cmbDocSearch.Background = IsNull(cmbDocSearch.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217));
            txtDate_start.Background = IsNull(txtDate_start.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Color.FromArgb(0xFF, 191, 191, 191));
            txtDate_end.Background = IsNull(txtDate_end.Text) ? new SolidColorBrush(Colors.IndianRed) :
                new SolidColorBrush(Color.FromArgb(0xFF, 191, 191, 191));
            lstvScan.Background = IsNull(path) ? new SolidColorBrush(Colors.IndianRed) : 
                new SolidColorBrush(Color.FromArgb(0xFF, 238, 238, 238));
        }
        private void LoadData()
        {
            var column = new DataGridTextColumn();
            column.Width = 400;
            clientID = dbContext.Clients.Where(p => p.Phone_number == cmbPhone_number.Text)
                .Select(p => p.Client_ID).FirstOrDefault();
            txtFirstName.Text = dbContext.Clients.Where(p => p.Phone_number == cmbPhone_number.Text)
                .Select(p => p.First_name).FirstOrDefault()?.ToString();
            txtFirstName.IsEnabled = true;
            txtMiddleName.Text = dbContext.Clients.Where(p => p.Phone_number == cmbPhone_number.Text)
                .Select(p => p.Middle_name).FirstOrDefault()?.ToString();
            txtMiddleName.IsEnabled = true;
            txtSurname.Text= dbContext.Clients.Where(p => p.Phone_number == cmbPhone_number.Text)
                .Select(p => p.Surname).FirstOrDefault()?.ToString();
            txtSurname.IsEnabled = true;
            txtComment.Text = dbContext.Clients.Where(p => p.Phone_number == cmbPhone_number.Text)
                   .Select(p => p.Comment).FirstOrDefault()?.ToString();
            txtComment.IsEnabled = true;
            txtPhone_namber.Text = dbContext.Clients.Where(p => p.Phone_number == cmbPhone_number.Text)
                   .Select(p => p.Phone_number).FirstOrDefault()?.ToString();
            txtPhone_namber.IsEnabled = true;
            txtManager.Text = "Менеджер: " + dbContext.Clients.Where(p => p.Phone_number == cmbPhone_number.Text)
                   .Select(p => p.Manager).FirstOrDefault()?.ToString();
            prbStatus.Value = Status_Calculate.CalculateProgress(clientID, true);
            if (!IsNull(cmbPhone_number.Text))
            {
                lblPercent.Content = "Прогресс " + prbStatus.Value.ToString() + " %";
            }
        }

        private void LoadScanData()
        {
            pbScan.Source = null;
            txtDate_start.Text = dbContext.Documents.AsEnumerable().
                Where(p => p.Client_ID == clientID && p.Document_name == cmbDocSearch.Text).
                Select(p => p.Date_of_issue).FirstOrDefault().ToString();
            txtDate_start.IsEnabled = true;
            txtDate_end.Text = dbContext.Documents.AsEnumerable().
                Where(p => p.Client_ID == clientID && p.Document_name == cmbDocSearch.Text).
                Select(p => p.Expiration_date).FirstOrDefault().ToString();
            txtDate_end.IsEnabled = true;
            path = dbContext.Documents.AsEnumerable()
                .Where(p => p.Client_ID == clientID && p.Document_name == cmbDocSearch.Text)
                .Select(p => p.Document_path).FirstOrDefault()?.ToString();
            if (!IsNull(path))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                var directory = FilePath.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                bmp.UriSource = new Uri(FilePath.Combine(directory, path + ".png"));
                bmp.EndInit(); 
                pbScan.Source = bmp;
            }
            status = dbContext.Documents.AsEnumerable().
                Where(p => p.Client_ID == clientID && p.Document_name == cmbDocSearch.Text)
                .Select(p => p.Status).FirstOrDefault();
        }

        private void OpenFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            if (fileDialog.ShowDialog() == true)
            {
                //fileDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
                string fileName = fileDialog.FileName;
                pbScan.Source = new BitmapImage(new Uri(fileName));
            }
        }

        private void FillComboBox()
        {
            var cli = from Client in dbContext.Clients select Client.Phone_number;
            cmbPhone_number.ItemsSource = cli.ToList();
        }

        private void ReFillComboBox(object sender, EventArgs e)
        {
            FillComboBox();
        }

        private void FillScanBox()
        {
            DocNames = dbContext.Documents.AsEnumerable().Where(p => p.Client_ID == clientID).
                Select(p => p.Document_name).ToList();
            cmbDocSearch.ItemsSource = DocNames;
        }

        private void ReFillScanBox(object sender, EventArgs e)
        {
            DocNames = dbContext.Documents.AsEnumerable().Where(p => p.Client_ID == clientID).
                Select(p => p.Document_name).ToList();
            cmbDocSearch.ItemsSource = DocNames;
        }

        private void cmbPhone_number_DropDownClosed(object sender, EventArgs e)
        {
            if (!IsNull(cmbPhone_number.Text))
            {
                LoadData();
                FillScanBox();
                txtManager.IsEnabled = true;
                UpdateClientControlStatus();
            }
        }

        private void cmbDocSearch_DropDownClosed(object sender, EventArgs e)
        {
            if (!IsNull(cmbDocSearch.Text))
            {
                LoadScanData();
                UpdateDocControlStatus();
            }
        }

        private void mnuClientAdd_Click(object sender, RoutedEventArgs e)
        {
            ClientAdd_Interface clientAdd = new ClientAdd_Interface();
            clientAdd.ShowDialog();
            clientAdd.Closed += ReFillComboBox;
        }

        private void mnuDocumentAdd_Click(object sender, RoutedEventArgs e)
        {
            DocAdd_Interface docAdd = new DocAdd_Interface();
            docAdd.ShowDialog();
            docAdd.Closed += ReFillScanBox;
        }

        private void ClientEdit_Leave(object sender, RoutedEventArgs e)
        {
            if (IsNull(txtFirstName.Text) || IsNull(txtSurname.Text) ||
                IsNull(txtPhone_namber.Text) || IsNull(txtManager.Text))
            {
                MessageBox.Show("Ошибка, не заполнено одно из полей!", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
                if (IsNull(txtFirstName.Text))
                {
                    txtFirstName.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtFirstName.Background = new SolidColorBrush(Colors.Transparent); }

                if (IsNull(txtSurname.Text))
                {
                    txtSurname.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtSurname.Background = new SolidColorBrush(Colors.Transparent); }

                if (IsNull(txtPhone_namber.Text))
                {
                    txtPhone_namber.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtPhone_namber.Background = new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217)); }

                if (IsNull(txtManager.Text))
                {
                    txtManager.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtManager.Background = new SolidColorBrush(Colors.Transparent); }
            }
            else
            {
                MessageBoxResult dialogResult = MessageBox.Show("Данные были изменены. Желаете сохранить изменения",
                "Оповещение", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    DBChanges.Client_Change(DBChanges.Operations.Edit, txtFirstName.Text, txtMiddleName.Text,
                        txtSurname.Text, txtPhone_namber.Text, txtComment.Text, txtManager.Text);
                    FillComboBox();
                    UpdateClientControlStatus();
                }
                else if (dialogResult == MessageBoxResult.No)
                {

                }
            }
        }

        private void DocEdit_Leave(object sender, RoutedEventArgs e)
        {
            if (IsNull(txtDate_start.Text) || IsNull(txtDate_end.Text))
            {
                MessageBox.Show("Ошибка, не заполнено одно из полей!", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
                if (IsNull(txtDate_start.Text))
                {
                    txtDate_start.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtDate_start.Background = new SolidColorBrush(Color.FromArgb(0xFF, 191, 191, 191)); }

                if (IsNull(txtDate_end.Text))
                {
                    txtDate_end.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtDate_end.Background = new SolidColorBrush(Color.FromArgb(0xFF, 191, 191, 191)); }
            }
            else
            {
                MessageBoxResult dialogResult = MessageBox.Show("Данные были изменены. Желаете сохранить изменения",
                "Оповещение", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    DBChanges.Document_Change(DBChanges.Operations.Edit, clientID, cmbDocSearch.Text,
                        path, txtDate_start.DisplayDate.Date, txtDate_end.DisplayDate.Date, status);
                    UpdateDocControlStatus();
                }
                else if (dialogResult == MessageBoxResult.No)
                {

                }
            }
        }

        private void mnuClientEdit_Save_Click(object sender, RoutedEventArgs e)
        {
            if (IsNull(cmbPhone_number.Text) || IsNull(txtFirstName.Text) || IsNull(txtSurname.Text) ||
                IsNull(txtPhone_namber.Text) || IsNull(txtManager.Text) || IsNull(cmbDocSearch.Text) ||
                IsNull(txtDate_start.Text) || IsNull(txtDate_end.Text) || pbScan.Source == null)
            {
                MessageBox.Show("Ошибка, не заполнено одно из полей!", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
                if (IsNull(cmbPhone_number.Text))
                {
                    cmbPhone_number.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { cmbPhone_number.Background = new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217)); }

                if (IsNull(txtFirstName.Text))
                {
                    txtFirstName.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtFirstName.Background = new SolidColorBrush(Colors.Transparent); }

                if (IsNull(txtSurname.Text))
                {
                    txtSurname.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtSurname.Background = new SolidColorBrush(Colors.Transparent); }

                if (IsNull(txtPhone_namber.Text))
                {
                    txtPhone_namber.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtPhone_namber.Background = new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217)); }

                if (IsNull(txtManager.Text))
                {
                    txtManager.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtManager.Background = new SolidColorBrush(Colors.Transparent); }
                if (IsNull(cmbDocSearch.Text))
                {
                    cmbDocSearch.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { cmbDocSearch.Background = new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217)); }

                if (IsNull(txtDate_end.Text))
                {
                    txtDate_end.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtDate_end.Background = new SolidColorBrush(Color.FromArgb(0xFF, 191, 191, 191)); }

                if (IsNull(txtDate_start.Text))
                {
                    txtDate_start.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { txtDate_start.Background = new SolidColorBrush(Color.FromArgb(0xFF, 191, 191, 191)); }

                if (pbScan.Source == null)
                {
                    lstvScan.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { lstvScan.Background = new SolidColorBrush(Color.FromArgb(0xFF, 238, 238, 238)); }
            }
            else
            {
                DBChanges.Client_Change(DBChanges.Operations.Edit, txtFirstName.Text, txtMiddleName.Text,
                    txtSurname.Text, txtPhone_namber.Text, txtComment.Text, txtManager.Text);
                DBChanges.Document_Change(DBChanges.Operations.Edit, clientID, cmbDocSearch.Text, path,
                    txtDate_start.DisplayDate.Date, txtDate_end.DisplayDate.Date, status);
                if (File.Exists(path + ".png"))
                {
                    try
                    {
                        File.Delete(path + ".png");
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Не удалось перезаписать файл документа, так как он открыт в другом месте!",
                            e.GetType().Name);
                    }
                }
                //pbScan.Image.Save(path + ".png");
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)pbScan.Source));
                using (FileStream stream = new FileStream(path + ".png", FileMode.Create))
                    encoder.Save(stream);
                UpdateClientControlStatus();
                UpdateDocControlStatus();
            }
        }

        private void mnuDocScan_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (IsNull(cmbDocSearch.Text))
            {
                MessageBox.Show("Не выбран документ!");
            }
            else
            {
                OpenFile();
                if (pbScan.Source == null)
                {
                    MessageBox.Show("Поле скана документа не может быть пустым");
                }

                else
                {
                    MessageBoxResult dialogResult = MessageBox.Show("Данные были изменены. Желаете сохранить изменения",
                        "Оповещение", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        if (File.Exists(path + ".png"))
                        {
                            File.Delete(path + ".png");
                        }
                        //pbScan.Image.Save(path + ".png");
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)pbScan.Source));
                        using (FileStream stream = new FileStream(path + ".png", FileMode.Create))
                            encoder.Save(stream);
                        MessageBox.Show("Скан документа: '" + cmbDocSearch.Text + "' изменен", "Сообщение",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (dialogResult == MessageBoxResult.No)
                    {

                    }

                }
            }
        }

        private void mnuClient_Delete_Click(object sender, RoutedEventArgs e)
        {
            var clientDocs = dbContext.Documents.ToList();
            if (IsNull(cmbPhone_number.Text))
            {
                MessageBox.Show("Ошибка, не выбран клиент!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                if (IsNull(cmbPhone_number.Text))
                {
                    cmbPhone_number.Background = new SolidColorBrush(Colors.IndianRed);
                }
                else { cmbPhone_number.Background = new SolidColorBrush(Color.FromArgb(0xFF, 217, 217, 217)); }
            }
            else
            {
                MessageBoxResult dialogResult = MessageBox.Show("Желаете удалить клиента",
                        "Оповещение", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    MessageBoxResult dialogResult1 = MessageBox.Show("Желаете сохранить данные клиента в архив",
                        "Оповещение", MessageBoxButton.YesNo);
                    if (dialogResult1 == MessageBoxResult.Yes)
                    {
                        DBChanges.ClientArchiv_Change(DBChanges.Operations.Add, txtFirstName.Text, txtMiddleName.Text,
                            txtSurname.Text, txtPhone_namber.Text, txtComment.Text, txtManager.Text);
                        if (dbContext.Documents.AsEnumerable().Where(p => p.Client_ID == clientID).Count() != 0)
                        {
                            foreach (var item in clientDocs)
                            {
                                if (item.Client_ID == clientID)
                                {
                                    DBChanges.DocumentArchiv_Change(DBChanges.Operations.Add, clientID, cmbDocSearch.Text,
                                        path, txtDate_start.DisplayDate.Date, txtDate_end.DisplayDate.Date, false);
                                }
                            }

                            DBChanges.Document_Change(DBChanges.Operations.Delete, clientID, cmbDocSearch.Text,
                                        path, txtDate_start.DisplayDate.Date, txtDate_end.DisplayDate.Date, status);
                        }

                        DBChanges.Client_Change(DBChanges.Operations.Delete, txtFirstName.Text, txtMiddleName.Text,
                                txtSurname.Text, txtPhone_namber.Text, txtComment.Text, txtManager.Text);
                        cmbPhone_number.SelectedIndex = -1;
                    }
                    else
                    {
                        foreach (var item in clientDocs)
                        {
                            if (item.Client_ID == clientID)
                            {
                                DBChanges.Document_Change(DBChanges.Operations.Delete, item.Client_ID, cmbDocSearch.Text,
                                    path, txtDate_start.DisplayDate.Date, txtDate_end.DisplayDate.Date, status);
                                string client_path = Path.Combine("Clients_Scans", txtFirstName.Text + txtMiddleName.Text + txtSurname.Text);
                                if (Directory.Exists(client_path))
                                    Directory.Delete(client_path, true);
                                pbScan.Source = null;
                                cmbDocSearch.SelectedIndex = -1;
                            }
                        }

                        DBChanges.Client_Change(DBChanges.Operations.Delete, txtFirstName.Text, txtMiddleName.Text,
                                txtSurname.Text, txtPhone_namber.Text, txtComment.Text, txtManager.Text);
                        cmbPhone_number.SelectedIndex = -1;
                    }
                }
                LoadData();
                lblPercent.Content = null;
                txtManager.Text = "Ответственный менеджер";
                txtManager.IsEnabled = false;
                LoadScanData();
            }
        }

        private void btnDocDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsNull(cmbDocSearch.Text))
            {
                File.Delete(path + ".png");
                DBChanges.Document_Change(DBChanges.Operations.Delete, clientID, cmbDocSearch.Text, path,
                    txtDate_start.DisplayDate.Date, txtDate_end.DisplayDate.Date, status);
                pbScan.Source = null;
                cmbDocSearch.SelectedIndex = -1;
                FillScanBox();
            }
            else { MessageBox.Show("Не выбран документ!"); }
        }

        private void cmbPhone_number_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !IsNull(cmbPhone_number.Text))
            {
                cmbPhone_number.Text = dbContext.Clients.Where(p => p.Phone_number.Contains(cmbPhone_number.Text))
                   .Select(p => p.Phone_number).FirstOrDefault()?.ToString();
                LoadData();
            }
        }

        private void cmbDocSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !IsNull(cmbDocSearch.Text))
            {
                cmbDocSearch.Text = dbContext.Documents.Where(p => p.Document_name.Contains(cmbDocSearch.Text))
                   .Select(p => p.Document_name).FirstOrDefault()?.ToString();
                LoadScanData();
            }
        }

        private void cmbPhone_number_DropDownOpened(object sender, EventArgs e)
        {
            FillComboBox();
        }

        private void cmbDocSearch_DropDownOpened(object sender, EventArgs e)
        {
            FillScanBox();
        }

        private void Status_Check_Checked(object sender, RoutedEventArgs e)
        {
            if (status == true) { status = false; }
            else { status = true; }

            if (txtDate_start.Text == "" || txtDate_start.Text == "" || path == null || path == "")
            {
                if (txtDate_start.Text == "")
                {
                    MessageBox.Show("Поле даты выдачи документа не может быть пустым");
                }

                if (txtDate_end.Text == "")
                {
                    MessageBox.Show("Поле даты окончания действия документа не может быть пустым");
                }

                if (path == "" || path == null)
                {
                    MessageBox.Show("Поле скана документа не может быть пустым");
                }
            }
            else
            {
                MessageBoxResult dialogResult = MessageBox.Show("Данные были изменены. Желаете сохранить изменения",
                "Оповещение", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    DBChanges.Document_Change(DBChanges.Operations.Edit, 1, cmbDocSearch.Text,
                        path, txtDate_start.DisplayDate.Date, txtDate_end.DisplayDate.Date, status);
                    FillComboBox();
                }
                else if (dialogResult == MessageBoxResult.No)
                {

                }
            }
        }
    }
}
