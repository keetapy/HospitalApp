using kursADO.AddWin;
using kursADO.DetailsWin;
using kursADO.EditWin;
using kursADO.Models;
using kursADO.Statistick;
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

namespace kursADO
{
    /// <summary>
    /// Логика взаимодействия для MainWindowForDoctor.xaml
    /// </summary>
    public partial class MainWindowForDoctor : Window
    {
        public LibContext context;
        Doctor CurrUser;
        string currentType = "patient";
        CollectionView view;

        public MainWindowForDoctor(LibContext context, string login)
        {
            InitializeComponent();
            this.context = context;
            CurrUser = context.Doctors.First(x => x.Login == login);
        }

        private void doctor_Click(object sender, RoutedEventArgs e)
        {
            DoctorInfo doctorInfo = new DoctorInfo(CurrUser, CurrUser.Login);
            try
            {
                using (MemoryStream stream = new MemoryStream(CurrUser.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    doctorInfo.img.Source = bitmapImage;
                }
            }
            catch
            {

            }
            doctorInfo.gridMain.DataContext = CurrUser;
            doctorInfo.lvInfo.ItemsSource = CurrUser.Patients.ToList();
            doctorInfo.ShowDialog();
        }
        private bool FindFilterName(object item)
        {
            string tmp = "";
            if (String.IsNullOrEmpty(txtFind.Text))
                return true;
            else
            {
                switch (currentType)
                {
                    case "patient":
                        tmp = (item as Patient).FirstName + " " + (item as Patient).LastName + " " + (item as Patient).Disease.Name;
                        return (tmp.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "disease":
                        return ((item as Disease).Name.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "rooms":
                        tmp = (item as Room).Id + " " + (item as Room).Department.Name;
                        return (tmp.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    default:
                        return true;
                }
            }
        }
        private void patient_Click(object sender, RoutedEventArgs e)
        {
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = CurrUser.Patients.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void txtFind_txtchanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvInfo.ItemsSource).Refresh();

        }

        private void disease_Click(object sender, RoutedEventArgs e)
        {
            currentType = "disease";
            DataTemplate template = (DataTemplate)this.Resources["DiseaseTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = CurrUser.Category.Diseases.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void mainDetails_click(object sender, MouseButtonEventArgs e)
        {
            switch (currentType)
            {
                case "patient":
                    Patient pat = lvInfo.SelectedItem as Patient;
                    detailsPatient(pat);
                    break;
                case "disease":
                    Disease dis = lvInfo.SelectedItem as Disease;
                    detailsDisease(dis);
                    break;
                default:
                    break;
            }
        }

        private void detailsDisease(Disease dis)
        {
            DiseaseInfo detailsDisease = new DiseaseInfo(dis);
            detailsDisease.ShowDialog();
        }

        private void detailsPatient(Patient pat)
        {
            PatientInfo patientInfo = new PatientInfo();
            patientInfo.gridMain.DataContext = pat;
            patientInfo.BirthDate.Text = pat.BirthDate.ToShortDateString();
            patientInfo.StartDate.Text = pat.StartDate.ToShortDateString();
            patientInfo.EndtDate.Text = pat.EndDate.ToString();
            if (pat.Sex == "М")
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

        private void stat_click(object sender, RoutedEventArgs e)
        {
            CategStatist statist = new CategStatist(context.Diseases.ToList(), CurrUser.Category);
            statist.ShowDialog();
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            switch (currentType)
            {
                case "patient":
                    Patient tmp = lvInfo.SelectedItem as Patient;
                    deletePatient(tmp); break;


                case "disease":
                    Disease dis = lvInfo.SelectedItem as Disease;
                    deleteDisease(dis);
                    break;

                default:
                    break;
            }
        }

        private void deletePatient(Patient tmp)
        {
            MessageBoxResult message = MessageBox.Show($"Удалить пациента {tmp.FirstName} {tmp.LastName}?", "Удаление", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                context.Patients.Remove(tmp);
                foreach (Room item in context.Rooms.ToList())
                {
                    item.CurrPatient = Convert.ToInt32(item.Patients.Where(a => a.EndDate == null).Count());

                }
                context.SaveChanges();
            }
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = CurrUser.Patients.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void deleteDisease(Disease dis)
        {
            MessageBoxResult message = MessageBox.Show($"Удалить заболевание {dis.Name}?", "Удаление", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                context.Diseases.Remove(dis);

                context.SaveChanges();
            }
            currentType = "disease";
            DataTemplate template = (DataTemplate)this.Resources["DiseaseTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = CurrUser.Category.Diseases.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void edit_click(object sender, RoutedEventArgs e)
        {
            DataTemplate template;
            switch (currentType)
            {
                case "patient":
                    Patient tmp = lvInfo.SelectedItem as Patient;
                    editPatient(tmp);
                    currentType = "patient";
                    template = (DataTemplate)this.Resources["PatientsTemplate"];
                    lvInfo.ItemTemplate = template;
                    lvInfo.ItemsSource = CurrUser.Patients.ToList();
                    view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
                    view.Filter = FindFilterName; break;
                case "disease":
                    editDisease(lvInfo.SelectedItem as Disease);
                    currentType = "disease";
                    template = (DataTemplate)this.Resources["DiseaseTemplate"];
                    lvInfo.ItemTemplate = template;
                    lvInfo.ItemsSource = CurrUser.Category.Diseases.ToList();
                    view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
                    view.Filter = FindFilterName;
                    break;
            }
        }

        private void editDisease(Disease disease)
        {
            var edit = context.Diseases.First(a => a.Name == disease.Name);
            DiseaseEdit editDisease = new DiseaseEdit();
            editDisease.findCategory.ItemsSource = context.Categories.ToList<Category>();
            editDisease.gridMain.DataContext = disease;
            if (editDisease.ShowDialog() == true)
            {
                edit.Name = editDisease.Name.Text;
                edit.Category = editDisease.findCategory.SelectedItem as Category;
            }
            context.SaveChanges();
        }
        private void editPatient(Patient tmp)
        {
            PatientEdit patientEdit = new PatientEdit(context, tmp);
            var edit = context.Patients.First(a => a.Phone == tmp.Phone);
            if (tmp.EndDate != null)
                patientEdit.endCheck.IsChecked = true;
            if (tmp.Sex == "М")
            {
                patientEdit.m.IsChecked = true;
            }
            else
            {
                patientEdit.f.IsChecked = true;
            }
            using (MemoryStream stream = new MemoryStream(tmp.Photo))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                patientEdit.img.Source = bitmapImage;
            }
            patientEdit.imageBytes = tmp.Photo;
            patientEdit.gridMain.DataContext = tmp;
            // patientEdit.findDisease.ItemsSource = context.Diseases.ToList<Disease>();
            //patientEdit.findDoctor.ItemsSource = context.Doctors.ToList<Doctor>();
            // patientEdit.findRoom.ItemsSource = context.Rooms.Where(a => a.CurrPatient < a.MaxPatients).ToList();
            patientEdit.department.Text = tmp.Department.ToString();

            ////////////////////////////////////////////////
            if (patientEdit.ShowDialog() == true)
            {
                Doctor temp = (patientEdit.findDoctor.SelectedItem as Doctor);
                Disease disease = (patientEdit.findDisease.SelectedItem as Disease);
                Department department = context.Departments.First(a => a.Name == patientEdit.department.Text);
                Room room = (patientEdit.findRoom.SelectedItem as Room);
                edit.FirstName = patientEdit.FirstName.Text;
                edit.LastName = patientEdit.LastName.Text;
                edit.Phone = patientEdit.Phone.Text;
                edit.BirthDate = patientEdit.BirthDate.SelectedDate.GetValueOrDefault();
                edit.StartDate = patientEdit.StartDate.SelectedDate.GetValueOrDefault();
                edit.Doctor = context.Doctors.First(a => a.FirstName == temp.FirstName && a.Phone == temp.Phone);
                edit.Department = context.Departments.First(a => a.Name == department.Name);
                edit.Disease = context.Diseases.First(a => a.Name == disease.Name);
                edit.Sex = patientEdit.sex;
                edit.Photo = patientEdit.imageBytes;
                edit.Room = context.Rooms.First(a => a.Id == room.Id);
                if (patientEdit.endCheck.IsChecked == true)
                {
                    edit.EndDate = DateTime.Now;
                    //edit.Room = null;
                }
                else
                {
                    edit.EndDate = null;
                }
                context.SaveChanges();
                foreach (Room item in context.Rooms.ToList())
                {
                    item.CurrPatient = Convert.ToInt32(item.Patients.Where(a => a.EndDate == null).Count());

                }
                context.SaveChanges();
            }
        }
        private void addPatient_click(object sender, MouseButtonEventArgs e)
        {
            AddPatient addPatient = new AddPatient(context);
            addPatient.findDisease.ItemsSource = context.Diseases.ToList<Disease>();
            addPatient.findDoctor.ItemsSource = context.Doctors.ToList<Doctor>();
            addPatient.findDoctor.SelectedItem = CurrUser;
            addPatient.findDoctor.IsEnabled = false;
            if (addPatient.ShowDialog() == true)
            {
                Doctor tmp = (addPatient.findDoctor.SelectedItem as Doctor);
                Disease disease = (addPatient.findDisease.SelectedItem as Disease);
                Room room = (addPatient.findRoom.SelectedItem as Room);
                Department department = context.Departments.First(a => a.Name == addPatient.department.Text);
                Patient newPatient = new Patient
                {
                    Sex = addPatient.sex,
                    FirstName = addPatient.FirstName.Text,
                    LastName = addPatient.LastName.Text,
                    Phone = addPatient.Phone.Text,
                    BirthDate = addPatient.BirthDate.SelectedDate.GetValueOrDefault(),
                    StartDate = addPatient.StartDate.SelectedDate.GetValueOrDefault(),
                    Doctor = context.Doctors.First(a => a.FirstName == tmp.FirstName && a.Phone == tmp.Phone),
                    Department = context.Departments.First(a => a.Name == department.Name),
                    Disease = context.Diseases.First(a => a.Name == disease.Name),
                    Photo = addPatient.imageBytes,
                    Room = context.Rooms.First(a => a.Id == room.Id),
                };
                context.Patients.Add(newPatient);
                context.Rooms.First(a => a.Id == room.Id).CurrPatient += 1;
                context.SaveChanges();
            }
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = CurrUser.Patients.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void addDisease_click(object sender, MouseButtonEventArgs e)
        {
            AddDisease addDisease = new AddDisease();
            addDisease.findCategory.ItemsSource = context.Categories.ToList();
            addDisease.findCategory.SelectedItem = CurrUser.Category;
            if (addDisease.ShowDialog() == true)
            {
                Category tmp = addDisease.findCategory.SelectedItem as Category;
                Disease disease = new Disease
                {
                    Name = addDisease.Name.Text,
                    Category = tmp
                };
                context.Diseases.Add(disease);
                context.SaveChanges();
            }
            currentType = "disease";
            DataTemplate template = (DataTemplate)this.Resources["DiseaseTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = CurrUser.Category.Diseases.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void editDoctor_click(object sender, MouseButtonEventArgs e)
        {
            DoctorEdit doctorEdit = new DoctorEdit();
            doctorEdit.gridMain.DataContext = CurrUser;
            doctorEdit.findCateg.ItemsSource = context.Categories.ToList();
            doctorEdit.findCateg.SelectedItem = CurrUser.Category;
            if (CurrUser.Patients.Count > 0)
            {
                doctorEdit.findCateg.IsEnabled = false;
            }
            try
            {
                using (MemoryStream stream = new MemoryStream(CurrUser.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    doctorEdit.img.Source = bitmapImage;
                    doctorEdit.imageBytes = CurrUser.Photo;

                }
            }
            catch { }
            var edit = context.Doctors.First(a => a.Phone == CurrUser.Phone);
            if (doctorEdit.ShowDialog() == true)
            {
                edit.FirstName = doctorEdit.FirstName.Text;
                edit.LastName = doctorEdit.LastName.Text;
                edit.Phone = doctorEdit.Phone.Text;
                edit.Photo = doctorEdit.imageBytes;
                edit.Category = doctorEdit.findCateg.SelectedItem as Category;
                edit.BirthDate = doctorEdit.BirthDate.SelectedDate.GetValueOrDefault();
                edit.Login = doctorEdit.txtLog.Text;
                edit.Password = doctorEdit.txtPas.Text;
                context.SaveChanges();
            }
            currentType = "doctor";
            DataTemplate template = (DataTemplate)this.Resources["DoctorsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Doctors.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
    }
}
