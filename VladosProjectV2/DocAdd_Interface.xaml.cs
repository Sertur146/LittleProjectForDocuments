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
using System.IO;
using System.Reflection;
using FilePath = System.IO.Path;
using Microsoft.Win32;
using System.Security;
using static System.Net.Mime.MediaTypeNames;

namespace VladosProjectV2
{
    /// <summary>
    /// Логика взаимодействия для DocAdd_Interface.xaml
    /// </summary>
    public partial class DocAdd_Interface : Window
    {
        public DocAdd_Interface()
        {
            InitializeComponent();
        }

        private static ProjectDB dbContext;
        int clientID;

        private void DocAdd_Interface1_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext = new ProjectDB();
            FillCombo();
            cmbClientSearch_foDocs_toAdd.SelectedIndex = -1;
            cmbDocSearch_toAdd.SelectedIndex = -1;
        }

        private void FillCombo()
        {
            var cli = from Client in dbContext.Clients select Client.Phone_number;
            cmbClientSearch_foDocs_toAdd.ItemsSource = cli.ToList();
        }

        private void FillSubDocs(string filename)
        {
            cmbDocSearch_toAdd.Items.Clear();
            string[] ArrayOfDocs = File.ReadAllLines(filename);
            for (int i = 0; i < ArrayOfDocs.Length; i++)
            {
                cmbDocSearch_toAdd.Items.Add(ArrayOfDocs[i]);
            }

            cmbDocSearch_toAdd.SelectedIndex = 0;
        }

        private void ComboReFill()
        {
            cmbClientSearch_foDocs_toAdd.Text = dbContext.Clients.Where(p => p.Phone_number.Contains
            (cmbClientSearch_foDocs_toAdd.Text)).Select(p => p.Phone_number).FirstOrDefault()?.ToString();
        }

        private void LoadData()
        {
            clientID = dbContext.Clients.Where(p => p.Phone_number == cmbClientSearch_foDocs_toAdd.Text)
                .Select(p => p.Client_ID).FirstOrDefault();
            txtClient_FIO.Text = dbContext.Clients.Where(p => p.Phone_number == cmbClientSearch_foDocs_toAdd.Text)
                .Select(p => p.First_name + p.Middle_name + p.Surname).FirstOrDefault()?.ToString();
            string maindoc = dbContext.Documents.AsEnumerable().Where(p => p.Client_ID == clientID)
                .Select(p => p.Document_name).FirstOrDefault()?.ToString();
            if (maindoc == rdbTP.Content.ToString())
            {
                rdbTP.IsChecked = true;
                cmbDocSearch_toAdd.IsEnabled = true;
            }
            else if (maindoc == rdbRVP.Content.ToString())
            {
                rdbRVP.IsChecked = true;
                cmbDocSearch_toAdd.IsEnabled = true;
            }
            else if (maindoc == Content.ToString())
            {
                rdbVNJ.IsChecked = true;
                cmbDocSearch_toAdd.IsEnabled = true;
            }
            else
            {
                rdbTP.IsChecked = false;
                rdbRVP.IsChecked = false;
                rdbVNJ.IsChecked = false;
                cmbDocSearch_toAdd.Items.Clear();
                cmbDocSearch_toAdd.Text = null;
                cmbDocSearch_toAdd.IsEnabled = false;
            }
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

        private void OpenFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            if (fileDialog.ShowDialog() == true)
            {
                //fileDialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
                string fileName = fileDialog.FileName;
                pbDoc_Scan.Source = new BitmapImage(new Uri(fileName));
            }
        }


        private void btnDoc_Find_Click(object sender, RoutedEventArgs e)
        {
            if (rdbTP.IsChecked == false && rdbRVP.IsChecked == false && rdbVNJ.IsChecked == false)
            {
                MessageBox.Show("Не выбран основной документ!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (cmbDocSearch_toAdd.Text == null || cmbDocSearch_toAdd.Text == "")
            {
                MessageBox.Show("Не выбрано название документа!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                //cmbDocSearch_toAdd.Background = new SolidColorBrush(Colors.IndianRed);
            }
            else
            {
                OpenFile();
                //OcrImage();
            }
        }

        private void btnDoc_Add_Click(object sender, RoutedEventArgs e)
        {
            if (dbContext.Documents.AsEnumerable().Where(p => p.Client_ID == clientID
                   && p.Document_name == cmbDocSearch_toAdd.Text).Select(p => p.Status).FirstOrDefault() == true)
            {
                MessageBox.Show("Невозможно добавить, скан этого документа уже добавлен", "Ошибка",
            MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (cmbDocSearch_toAdd.Text == null || cmbDocSearch_toAdd.Text == "")
            {
                MessageBox.Show("Не выбрано название документа!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (cmbClientSearch_foDocs_toAdd.Text == null || cmbClientSearch_foDocs_toAdd.Text == "")
            {
                MessageBox.Show("Не выбран клиент!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (pbDoc_Scan.Source != null) //если в pictureBox есть изображение
            {
                //string scans_path = Path.Combine(Directory.GetCurrentDirectory(), "Clients_Scans");
                string client_path = FilePath.Combine("Clients_Scans", txtClient_FIO.Text);
                if (!Directory.Exists(client_path))
                    Directory.CreateDirectory(client_path);
                string doc_path = FilePath.Combine(client_path, cmbDocSearch_toAdd.Text);
                //pbDoc_Scan.Image.Save(doc_path + ".png");
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)pbDoc_Scan.Source));
                using (FileStream stream = new FileStream(doc_path + ".png", FileMode.Create))
                    encoder.Save(stream);
                DBChanges.Document_Change(DBChanges.Operations.Add, clientID, cmbDocSearch_toAdd.Text,
                    doc_path, DateTime.Parse(txtDoc_dateStart.Text), DateTime.Parse(txtDoc_dateEnd.Text), true);
                MessageBox.Show("Скан документа: '" + cmbDocSearch_toAdd.Text + "' добавлен", "Сообщение",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                cmbDocSearch_toAdd.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Не добавлен скан документа!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rdbTP_Checked(object sender, RoutedEventArgs e)
        {
            FillSubDocs(@"DocNames\TPsubDocs.txt");
        }

        private void rdbRVP_Checked(object sender, RoutedEventArgs e)
        {
            FillSubDocs(@"DocNames\RVPsubDocs.txt");
        }

        private void rdbVNJ_Checked(object sender, RoutedEventArgs e)
        {
            FillSubDocs(@"DocNames\VNJsubDocs.txt");
        }

        private void cmbClientSearch_foDocs_toAdd_DropDownClosed(object sender, EventArgs e)
        {
            LoadData();
            txtDoc_dateEnd.Text = null;
            txtDoc_dateStart.Text = null;
        }

        private void cmbDocSearch_toAdd_DropDownClosed(object sender, EventArgs e)
        {
            cmbDocSearch_toAdd.Background = new SolidColorBrush(Colors.White);
            txtDoc_dateEnd.Text = null;
            txtDoc_dateStart.Text = null;
        }

        private void cmbClientSearch_foDocs_toAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ComboReFill();
                txtDoc_dateEnd.Text = null;
                txtDoc_dateStart.Text = null;
            }
        }
    }
}
