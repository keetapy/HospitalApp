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
    /// Логика взаимодействия для DepartmentInfo.xaml
    /// </summary>
    
    public partial class DepartmentInfo : Window
    {
        Department department;
        public DepartmentInfo(Department department)
        {
            InitializeComponent();
            this.department = department;
            gridMain.DataContext = department;
            findCategory.ItemsSource = department.Categories.ToList<Category>();
            lvInfo.ItemsSource = department.Patients.ToList<Patient>();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void detailsPatients_click(object sender, MouseButtonEventArgs e)
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

        private void Category_click(object sender, SelectionChangedEventArgs e)
        {
            CategoryInfo detailsCategory = new CategoryInfo(findCategory.SelectedItem as Category);
            detailsCategory.ShowDialog();
        }
    }
}
