using kursADO.Models;
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

namespace kursADO.EditWin
{
    /// <summary>
    /// Логика взаимодействия для PatientEdit.xaml
    /// </summary>
    public partial class PatientEdit : Window
    {
        public byte[] imageBytes;
        public string sex;
        LibContext context;

        public PatientEdit()
        {
            InitializeComponent();
        }
        public PatientEdit(LibContext context, Patient patient)
        {
            InitializeComponent();
            this.context = context;
            findDisease.ItemsSource = context.Diseases.ToList();
            findDoctor.ItemsSource = context.Doctors.ToList();
            findRoom.ItemsSource = context.Rooms.Where(a=>a.Department.Name==patient.Department.Name).ToList();
            findDisease.SelectedItem = patient.Disease;
            findDoctor.SelectedItem = patient.Doctor;
            findRoom.SelectedItem = patient.Room;
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
        private void Disease_Closed(object sender, EventArgs e)
        {
            if (findDisease.IsDropDownOpen == false)
            {
                if (findDisease.SelectedItem != null)
                {
                    Category tmp = (findDisease.SelectedItem as Disease).Category;
                    Department forRoom = context.Departments.First(a => a.Name == tmp.Department.Name);
                    department.Text = tmp.Department.Name;
                    findDoctor.ItemsSource = tmp.Doctors;
                    findRoom.ItemsSource = forRoom.Rooms.Where(a => a.CurrPatient < a.MaxPatients);
                }
            }
        }
        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void m_checked(object sender, RoutedEventArgs e)
        {
            sex = "М";
            //MessageBox.Show(""+sex);
        }

        private void f_checked(object sender, RoutedEventArgs e)
        {
            sex = "Ж";

        }

        
    }
}
