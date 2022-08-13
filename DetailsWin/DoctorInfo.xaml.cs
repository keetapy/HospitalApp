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
    /// Логика взаимодействия для DoctorInfo.xaml
    /// </summary>
    public partial class DoctorInfo : Window
    {
        string login;
        Doctor CurrDoctor;
        public DoctorInfo()
        {
            InitializeComponent();
        }
        public DoctorInfo(Doctor tmp,string login)
        {
            InitializeComponent();
            this.CurrDoctor = tmp;
            this.login = login;          

        }
       
        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void getPass_click(object sender, RoutedEventArgs e)
        {
           
                if(login == "root"||CurrDoctor.Login==login)
                {
                    MessageBox.Show("Пароль: " + CurrDoctor.Password);
                }
                else
                {
                MessageBox.Show("Недостаточно прав для просмотра информации", "Недостаточно прав", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void diseaseChoose_click(object sender, RoutedEventArgs e)
        {
            Patient curr = lvInfo.SelectedItem as Patient;
            List<Patient> sort = CurrDoctor.Patients.Where(a => a.Disease.Name == curr.Disease.Name).ToList();
            lvInfo.ItemsSource = sort;
        }

        private void clearChoose_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = CurrDoctor.Patients.ToList<Patient>();
        }

        private void detailsPatient_click(object sender, MouseButtonEventArgs e)
        {
            Patient pat = lvInfo.SelectedItem as Patient;
            PatientInfo patientInfo = new PatientInfo();
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

        private void NowChoose_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = CurrDoctor.Patients.Where(a => a.EndDate != null).ToList();
        }
    }
}
