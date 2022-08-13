using kursADO.Models;
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

namespace kursADO.DetailsWin
{
    /// <summary>
    /// Логика взаимодействия для RoomInfo.xaml
    /// </summary>
    public partial class RoomInfo : Window
    {
        Room room;
        public RoomInfo(Room room)
        {
            InitializeComponent();
            this.room = room;
            lvInfo.ItemsSource = room.Patients.ToList();
            gridMain.DataContext = room;
        }

        private void mainDetails_click(object sender, MouseButtonEventArgs e)
        {
            PatientInfo patientInfo = new PatientInfo();
            Patient pat = lvInfo.SelectedItem as Patient;
            patientInfo.gridMain.DataContext = pat;
            patientInfo.BirthDate.Text = pat.BirthDate.ToShortDateString();
            patientInfo.StartDate.Text = pat.StartDate.ToShortDateString();
            patientInfo.EndtDate.Text = pat.EndDate.ToString();
            if (pat.Sex == "M")
            {
                patientInfo.m.IsChecked = true;
            }
            else
            {
                patientInfo.f.IsChecked = true;
            }
            using (MemoryStream stream = new MemoryStream(pat.Photo))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                patientInfo.img.Source = bitmapImage;
            }
            patientInfo.ShowDialog();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
