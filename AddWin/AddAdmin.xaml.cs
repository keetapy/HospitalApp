using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace kursADO.AddWin
{
    /// <summary>
    /// Логика взаимодействия для AddAdmin.xaml
    /// </summary>
    public partial class AddAdmin : Window
    {
        public byte[] imageBytes;
        LibContext context;
        bool CheckPassRes = false;
        public AddAdmin()
        {
            InitializeComponent();
        }
        public AddAdmin(LibContext context)
        {
            InitializeComponent();
            this.context = context;
        }
        private void addPhoto_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                using (FileStream stream = fileInfo.Open(FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        imageBytes = reader.ReadBytes((int)fileInfo.Length);
                    }
                }
                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    img.Source = bitmapImage;
                }
            }
        }

        private void checkPass_chahge(object sender, TextChangedEventArgs e)
        {
            if (txtRepPas.Text == txtPas.Text)
            {
                CheckPassRes = true;
                txtPas.BorderBrush = Brushes.GreenYellow;
                txtPas.BorderThickness = new Thickness(3);
            }
            else
            {
                CheckPassRes = false;
                txtPas.BorderBrush = Brushes.Red;
                txtPas.BorderThickness = new Thickness(3);
            }
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            if (CheckPassRes)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        private void checkLog_focus(object sender, RoutedEventArgs e)
        {
            if (context.Doctors.FirstOrDefault(a => a.Login == txtLog.Text) != null)
            {
                MessageBox.Show("Такой пользователь уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                txtLog.Text = "";
            }
        }
    }
}
