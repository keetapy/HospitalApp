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
    /// Логика взаимодействия для CategoryInfo.xaml
    /// </summary>
    public partial class CategoryInfo : Window
    {
        Category category;
        public CategoryInfo(Category category)
        {
            InitializeComponent();
            this.category = category;
            gridMain.DataContext = category;
            Doctors.Text = category.Doctors.Count.ToString();
            findDisease.ItemsSource = category.Diseases.ToList();
            lvInfo.ItemsSource = category.Doctors.ToList();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void detailsDoctor_click(object sender, MouseButtonEventArgs e)
        {
            DoctorInfo doctorInfo = new DoctorInfo(lvInfo.SelectedItem as Doctor,"user");
            doctorInfo.gridMain.DataContext = lvInfo.SelectedItem as Doctor;
            doctorInfo.BirthDate.Text = (lvInfo.SelectedItem as Doctor).BirthDate.ToShortDateString();
            doctorInfo.lvInfo.ItemsSource = (lvInfo.SelectedItem as Doctor).Patients.ToList();
            try
            {
                using (MemoryStream stream = new MemoryStream((lvInfo.SelectedItem as Doctor).Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    doctorInfo.img.Source = bitmapImage;
                }
            }
            catch { }
            doctorInfo.ShowDialog();
        }

        private void Disease_click(object sender, SelectionChangedEventArgs e)
        {
            DiseaseInfo detailsDisease = new DiseaseInfo(findDisease.SelectedItem as Disease);
            detailsDisease.ShowDialog();
        }
    }
}
