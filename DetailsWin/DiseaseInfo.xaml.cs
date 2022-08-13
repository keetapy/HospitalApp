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
    /// Логика взаимодействия для DiseaseInfo.xaml
    /// </summary>
    public partial class DiseaseInfo : Window
    {
        Disease disease;

        public DiseaseInfo(Disease disease)
        {
            InitializeComponent();
            this.disease = disease;
            gridMain.DataContext = this.disease;
            Patients.Text = this.disease.Patients.Count().ToString();
            lvInfo.ItemsSource = this.disease.Patients.ToList<Patient>();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void thisMonth_click(object sender, RoutedEventArgs e)
        {
            List<Patient> sort = disease.Patients.Where(a => a.StartDate.Month == DateTime.Now.Month).ToList();
            Patients.Text = sort.Count.ToString();
            lvInfo.ItemsSource = sort;
        }

        private void thisDay_click(object sender, RoutedEventArgs e)
        {
            List<Patient> sort = disease.Patients.Where(a => a.StartDate.Day == DateTime.Now.Day).ToList();
            Patients.Text = sort.Count.ToString();
            lvInfo.ItemsSource = sort;
        }

        private void thisYear_click(object sender, RoutedEventArgs e)
        {
            List<Patient> sort = disease.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year).ToList();
            Patients.Text = sort.Count.ToString();
            lvInfo.ItemsSource = sort;
        }

        private void thisCurr_click(object sender, RoutedEventArgs e)
        {
            List<Patient> sort = disease.Patients.Where(a => a.EndDate == null).ToList();
            Patients.Text = sort.Count.ToString();
            lvInfo.ItemsSource = sort;
        }

        private void thisEnd_click(object sender, RoutedEventArgs e)
        {
            List<Patient> sort = disease.Patients.Where(a => a.EndDate != null).ToList();
            Patients.Text = sort.Count.ToString();
            lvInfo.ItemsSource = sort;
        }

        private void detailsPatient_click(object sender, MouseButtonEventArgs e)
        {
            PatientInfo detailsPatient = new PatientInfo();
            detailsPatient.gridMain.DataContext = lvInfo.SelectedItem as Patient;
            using (MemoryStream stream = new MemoryStream((lvInfo.SelectedItem as Patient).Photo))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                detailsPatient.img.Source = bitmapImage;
            }
            detailsPatient.ShowDialog();
        }

        private void clearChoose_click(object sender, RoutedEventArgs e)
        {

            lvInfo.ItemsSource = disease.Patients.ToList<Patient>();

        }
    }
}
