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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kursADO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LibContext context;
        Admin currAdmin =null;
        Doctor currDoc = null;
        CollectionView view;
        string currentType= "patient";
        public MainWindow()
        {
           
            InitializeComponent();
            context = new LibContext();
            if (!context.Database.Exists())
            {
                Department terap = new Department { Name = "Терапия" };
                Department kard = new Department { Name = "Кардиология" };
                Department nevr = new Department { Name = "Неврология" };
                Department hirurg = new Department { Name = "Хирургия" };
                Department travm = new Department { Name = "Травматология" };
                context.Departments.AddRange(new List<Department> { terap, kard, nevr, hirurg, travm });
                context.SaveChanges();

                Category infect = new Category { Name = "Инфекционное", Department = terap };
                Category ped = new Category { Name = "Педиатрическое", Department = terap };
                Category geron = new Category { Name = "Геронтологическое", Department = terap };
                Category infarct = new Category { Name = "Инфарктное", Department = kard };
                context.Categories.AddRange(new List<Category> { infarct, infect, ped, geron });
                context.SaveChanges();

                Disease kor = new Disease { Name = "Корь", Category = infect };
                Disease gek = new Disease { Name = "ГЭК", Category = infect };
                Disease ang = new Disease { Name = "Ангина", Category = infect };
                Disease cov = new Disease { Name = "COVID-19", Category = infect };
                context.Diseases.AddRange(new List<Disease> { kor, gek, ang, cov });
                context.SaveChanges();

                Room r1 = new Room { Department = terap, MaxPatients = 4, CurrPatient = 0 };
                Room r2 = new Room { Department = terap, MaxPatients = 4, CurrPatient = 0 };
                Room r3 = new Room { Department = terap, MaxPatients = 4, CurrPatient = 0 };
                Room r4 = new Room { Department = terap, MaxPatients = 4, CurrPatient = 0 };
                Room r5 = new Room { Department = kard, MaxPatients = 2, CurrPatient = 0 };
                Room r6 = new Room { Department = kard, MaxPatients = 2, CurrPatient = 0 };
                context.Rooms.AddRange(new List<Room> { r1, r2, r3, r4, r5, r6 });
                context.SaveChanges();
                byte[] imageBytes;
                FileInfo fileInfo = new FileInfo("C:\\я у мамы программист\\ADO\\kursADO\\img\\eye.png");
                using (FileStream stream = fileInfo.Open(FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        imageBytes = reader.ReadBytes((int)fileInfo.Length);
                    }
                }
                Admin admin = new Admin { FirstName = "Main", LastName = "Admin", BirthDate = DateTime.Now, Phone = "+000000000000", Login = "root", Password = "111", Photo=imageBytes };
                context.Admins.Add(admin);
                context.SaveChanges();

                Doctor doctor1 = new Doctor
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    BirthDate = new DateTime(1980, 10, 10),
                    Phone = "+380666660000",
                    Category = infect,
                    Photo=imageBytes,
                    Login="Ivanov_1980",
                    Password="10101980"
                    
                };
                Doctor doctor2 = new Doctor
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    BirthDate = new DateTime(1985, 5, 10),
                    Phone = "+380660860000",
                    Category = ped,
                    Photo = imageBytes,
                    Login = "Petrov_1985",
                    Password = "10051985"
                };
                context.Doctors.AddRange(new List<Doctor> { doctor1, doctor2 });
                context.SaveChanges();
                MessageBox.Show("DB created");
            }
            if (context.Database.Exists())
            {
               // MessageBox.Show("DB exists");
                /*lvInfo.ItemsSource = context.Patients.ToList<Patient>();
                lvInfo.ItemsSource = context.Doctors.ToList<Doctor>();
                lvInfo.ItemsSource = context.Departments.ToList<Department>();
                lvInfo.ItemsSource = context.Categories.ToList<Category>();
                lvInfo.ItemsSource = context.Diseases.ToList<Disease>();*/
            }
            Login login = new Login();

            if (login.ShowDialog() == true)
            {
                foreach (Admin item in context.Admins)
                {
                    if (item.Login == login.log.Text && item.Password == login.pas.Password)
                    {
                        currAdmin = item;
                        break;
                    }
                }
                //if (currAdmin != null)
                //{
                //    //MessageBox.Show("Вы вошли как администратор " + currAdmin.ToString());
                //    //lvInfo.ItemsSource = context.Patients.ToList();
                //}
                if (currAdmin == null)
                {
                    foreach (Doctor item in context.Doctors)
                    {
                        if (item.Login == login.log.Text && item.Password == login.pas.Password)
                        {
                            currDoc = item;
                            break;
                        }
                    }
                    if (currDoc!=null)
                    {
                        //MessageBox.Show("Вы вошли как доктор " + currDoc.ToString());
                        MainWindowForDoctor mainWindowForDoctor = new MainWindowForDoctor(context,currDoc.Login);
                        this.Visibility = Visibility.Hidden;

                        if (mainWindowForDoctor.ShowDialog() == false)
                        {
                            context = mainWindowForDoctor.context;
                            context.SaveChanges();
                            this.Close();
                        }
                        
                    }
                    
                    else
                    {
                        MessageBox.Show("Пользователь не найден");
                        this.Close();
                    }

                }
                lvInfo.ItemsSource = context.Patients.ToList();
                view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
                view.Filter = FindFilterName;
            }
            else
                this.Close();
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
                        tmp = (item as Patient).FirstName + " " +(item as Patient).LastName+ " " + (item as Patient).Disease.Name;
                        return (tmp.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "doctor":
                        tmp = (item as Doctor).FirstName + " " + (item as Doctor).LastName + " " + (item as Doctor).Category.Name;
                        return (tmp.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "department":
                        return ((item as Department).Name.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "category":
                        return ((item as Category).Name.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "disease":
                        return ((item as Disease).Name.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "rooms":
                        tmp = (item as Room).Id + " "  + (item as Room).Department.Name;
                        return (tmp.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    case "admins":
                        tmp = (item as Admin).FirstName + " " + (item as Admin).LastName;
                        return (tmp.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                    default:
                        return true;
                }
            }
        }
        #region add
        private void addPatient_click(object sender, RoutedEventArgs e)
        {
            AddPatient addPatient = new AddPatient(context);
            addPatient.findDisease.ItemsSource = context.Diseases.ToList<Disease>();
            addPatient.findDoctor.ItemsSource = context.Doctors.ToList<Doctor>();
            if (addPatient.ShowDialog() == true)
            {
                Doctor tmp = (addPatient.findDoctor.SelectedItem as Doctor);
                Disease disease = (addPatient.findDisease.SelectedItem as Disease);
                Room room = (addPatient.findRoom.SelectedItem as Room);
                Department department = context.Departments.First(a => a.Name == addPatient.department.Text);
                Patient newPatient = new Patient {
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
                    Room= context.Rooms.First(a => a.Id == room.Id),
                };
                context.Patients.Add(newPatient);
                context.Rooms.First(a => a.Id == room.Id).CurrPatient+=1;
                context.SaveChanges();
            }
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Patients.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void addDoctor_click(object sender, RoutedEventArgs e)
        {
            AddDoctor addDoctor = new AddDoctor(context);
            addDoctor.findCateg.ItemsSource = context.Categories.ToList();
            if (addDoctor.ShowDialog() == true)
            {
                Category tmp = addDoctor.findCateg.SelectedItem as Category;
                Doctor newDoc = new Doctor
                {
                    FirstName = addDoctor.FirstName.Text,
                    LastName=addDoctor.LastName.Text,
                    Phone=addDoctor.Phone.Text,
                    Category=tmp,
                    Photo=addDoctor.imageBytes,
                    BirthDate=addDoctor.BirthDate.SelectedDate.GetValueOrDefault(),
                    Login=addDoctor.txtLog.Text,
                    Password=addDoctor.txtPas.Text
                };
                context.Doctors.Add(newDoc);
                context.SaveChanges();
            }
            currentType = "doctor";
            DataTemplate template = (DataTemplate)this.Resources["DoctorsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Doctors.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void addDepartment_click(object sender, RoutedEventArgs e)
        {
            AddDepartment addDepartment = new AddDepartment();
            if (addDepartment.ShowDialog() == true)
            {
                Department newDepartment = new Department
                {
                    Name = addDepartment.Name.Text,

                };
                context.Departments.Add(newDepartment);
                

            }
            context.SaveChanges();
            currentType = "department";
            DataTemplate template = (DataTemplate)this.Resources["DepartmensTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Departments.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void addCategory_click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategory = new AddCategory();
            addCategory.findDepartment.ItemsSource = context.Departments.ToList<Department>();
            if (addCategory.ShowDialog() == true)
            {
                Category newCategory = new Category
                {
                    Name = addCategory.Name.Text,
                    Department = addCategory.findDepartment.SelectedItem as Department
                };
                context.Categories.Add(newCategory);
                context.SaveChanges();

            }
            currentType = "category";
            DataTemplate template = (DataTemplate)this.Resources["CategoryTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Categories.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void addDisease_click(object sender, RoutedEventArgs e)
        {
            AddDisease addDisease = new AddDisease();
            addDisease.findCategory.ItemsSource = context.Categories.ToList();
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
            lvInfo.ItemsSource = context.Diseases.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
        private void addAdmin_click(object sender, RoutedEventArgs e)
        {
            AddAdmin addAdmin = new AddAdmin(context);
            if (addAdmin.ShowDialog() == true)
            {
                Admin newAdmin = new Admin
                {
                    FirstName = addAdmin.FirstName.Text,
                    LastName = addAdmin.LastName.Text,
                    Phone = addAdmin.Phone.Text,
                    Photo = addAdmin.imageBytes,
                    BirthDate = addAdmin.BirthDate.SelectedDate.GetValueOrDefault(),
                    Login = addAdmin.txtLog.Text,
                    Password = addAdmin.txtPas.Text
                };
                context.Admins.Add(newAdmin);
                context.SaveChanges();
            }
            currentType = "admins";
            DataTemplate template = (DataTemplate)this.Resources["AdminTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Admins.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
        #endregion
        private void sortNamePatient_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Patients.OrderBy(a => a.LastName).ToList();
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void sortStartDatePatient_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Patients.OrderByDescending(a => a.StartDate).ToList();
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void sortNameDoctor_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Doctors.OrderBy(a => a.LastName).ToList<Doctor>();
            currentType = "doctor";
            DataTemplate template = (DataTemplate)this.Resources["DoctorsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void sortNameDepartment_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Departments.OrderBy(a => a.Name).ToList<Department>();
            currentType = "department";
            DataTemplate template = (DataTemplate)this.Resources["DepartmensTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;

        }

        private void sortNameCategory_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Categories.OrderBy(a => a.Name).ToList<Category>();
            currentType = "category";
            DataTemplate template = (DataTemplate)this.Resources["CategoryTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;

        }

        private void sortNameDisease_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Diseases.OrderBy(a => a.Name).ToList<Disease>();
            currentType = "disease";
            DataTemplate template = (DataTemplate)this.Resources["DiseaseTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;

        }
        #region show
        private void patient_Click(object sender, RoutedEventArgs e)
        {
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Patients.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void doctor_Click(object sender, RoutedEventArgs e)
        {
            currentType = "doctor";
            DataTemplate template = (DataTemplate)this.Resources["DoctorsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Doctors.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void department_Click(object sender, RoutedEventArgs e)
        {
            currentType = "department";
            DataTemplate template = (DataTemplate)this.Resources["DepartmensTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Departments.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void category_Click(object sender, RoutedEventArgs e)
        {
            currentType = "category";
            DataTemplate template = (DataTemplate)this.Resources["CategoryTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Categories.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void disease_Click(object sender, RoutedEventArgs e)
        {
            currentType = "disease";
            DataTemplate template = (DataTemplate)this.Resources["DiseaseTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Diseases.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
        private void Room_Click(object sender, RoutedEventArgs e)
        {
            currentType = "rooms";
            DataTemplate template = (DataTemplate)this.Resources["RoomsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Rooms.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void AdminClick_Click(object sender, RoutedEventArgs e)
        {
            
            if (currAdmin.Login == "root")
            {
                currentType = "admins";
                DataTemplate template = (DataTemplate)this.Resources["AdminTemplate"];
                lvInfo.ItemTemplate = template;
                lvInfo.ItemsSource = context.Admins.ToList();
                view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
                view.Filter = FindFilterName;
            }
            else
            {
                detailsAdmin(context.Admins.First(a => a.Login == currAdmin.Login));               
            }
        }
        #endregion
        #region delete
        private void delete_click(object sender, RoutedEventArgs e)
        {
            switch (currentType)
            {
                case "patient":
                    Patient tmp = lvInfo.SelectedItem as Patient;
                    deletePatient(tmp); break;
                case "doctor":
                    Doctor d = lvInfo.SelectedItem as Doctor;
                    deleteDoctor(d); 
                    break;
                case "department":
                    Department dep = lvInfo.SelectedItem as Department;
                    deleteDepartment(dep); 
                    break;
                case "category":
                    Category cat = lvInfo.SelectedItem as Category;
                    deleteCategoey(cat); 
                    break;
                case "disease":
                    Disease dis = lvInfo.SelectedItem as Disease;
                    deleteDisease(dis); 
                    break;
                case "admins":
                    Admin adm = lvInfo.SelectedItem as Admin;
                    if (adm.Login == "root")
                    {
                        MessageBox.Show("Недостаточно прав для удаления", "Недостаточно прав", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    deleteAdmin(adm);
                    break;
                default:
                    break;
            }
        }

        private void deleteAdmin(Admin adm)
        {
            MessageBoxResult message = MessageBox.Show($"Удалить администратора {adm.FirstName} {adm.LastName}?", "Удаление", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                context.Admins.Remove(adm);
                context.SaveChanges();
            }
            currentType = "admins";
            DataTemplate template = (DataTemplate)this.Resources["AdminTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Admins.ToList();
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
            lvInfo.ItemsSource = context.Diseases.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void deleteCategoey(Category cat)
        {
            MessageBoxResult message = MessageBox.Show($"Удалить категорию {cat.Name}?", "Удаление", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                context.Categories.Remove(cat);
                context.SaveChanges();
            }
            lvInfo.ItemsSource = context.Categories.ToList();
        }

        private void deleteDepartment(Department dep)
        {
            MessageBoxResult message = MessageBox.Show($"Удалить отделение {dep.Name}?", "Удаление", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                context.Departments.Remove(dep);
                context.SaveChanges();
            }
            lvInfo.ItemsSource = context.Departments.ToList();
        }

        private void deleteDoctor(Doctor d)
        {
            MessageBoxResult message = MessageBox.Show($"Удалить доктора {d.FirstName} {d.LastName}?", "Удаление", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                context.Doctors.Remove(d);
                context.SaveChanges();
            }
            lvInfo.ItemsSource = context.Diseases.ToList();
            currentType = "doctor";
            DataTemplate template = (DataTemplate)this.Resources["DoctorsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Doctors.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
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
            lvInfo.ItemsSource = context.Patients.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
        #endregion
        #region edit
        private void edit_click(object sender, RoutedEventArgs e)
        {
            DataTemplate template;
            switch (currentType)
            {
                case "patient":
                    Patient tmp = lvInfo.SelectedItem as Patient;
                    editPatient(tmp);
                    lvInfo.ItemsSource = context.Patients.ToList<Patient>(); break;
                case "doctor":
                    Doctor doc = lvInfo.SelectedItem as Doctor;
                    editDoctor(doc);
                    lvInfo.ItemsSource = context.Doctors.ToList<Doctor>();
                    break;
                case "department":
                    Department dep = lvInfo.SelectedItem as Department;
                    editDepartment(dep);
                    lvInfo.ItemsSource = context.Departments.ToList<Department>();
                    break;
                case "category":
                    editCategory(lvInfo.SelectedItem as Category);
                    lvInfo.ItemsSource = context.Categories.ToList<Category>();
                    break;
                case "disease":
                    editDisease(lvInfo.SelectedItem as Disease);
                    currentType = "disease";
                     template = (DataTemplate)this.Resources["DiseaseTemplate"];
                    lvInfo.ItemTemplate = template;
                    lvInfo.ItemsSource = context.Diseases.ToList();
                    view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
                    view.Filter = FindFilterName;
                    break;
                case "admins":
                    editAdmin(lvInfo.SelectedItem as Admin);
                    currentType = "admins";
                     template = (DataTemplate)this.Resources["AdminTemplate"];
                    lvInfo.ItemTemplate = template;
                    lvInfo.ItemsSource = context.Admins.ToList();
                    view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
                    view.Filter = FindFilterName;
                    break;
                case "rooms":
                    Room room = lvInfo.SelectedItem as Room;
                    editRoom(room);
                    break;
                default:
                    break;
            }
        }

        private void editRoom(Room room)
        {
            RoomEdit roomEdit = new RoomEdit(room);
            var edit = context.Rooms.First(a => a.Id == room.Id);
            roomEdit.findDepartment.ItemsSource = context.Departments.ToList();
            roomEdit.findDepartment.SelectedItem = room.Department;
            if (room.Patients.Count > 0)
            {
                roomEdit.findDepartment.IsEnabled = false;
            }
            if(roomEdit.ShowDialog()==true)
            edit = roomEdit.room;
            context.SaveChanges();
            currentType = "rooms";
            DataTemplate template = (DataTemplate)this.Resources["RoomsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Rooms.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void editCategory(Category category)
        {
            CategoryEdit categoryEdit = new CategoryEdit(category);
            var edit = context.Categories.First(a => a.Id == category.Id);
            if (categoryEdit.ShowDialog() == true)
                edit = categoryEdit.category;
            context.SaveChanges();
            currentType = "category";
            DataTemplate template = (DataTemplate)this.Resources["CategoryTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Categories.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void editDepartment(Department dep)
        {
            DepartmentEdit departmentEdit = new DepartmentEdit();
            var edit = context.Departments.First(a => a.Id == dep.Id);
            if (departmentEdit.ShowDialog() == true)
            {
                edit.Name = departmentEdit.Name.Text;
            }
            context.SaveChanges();
            currentType = "department";
            DataTemplate template = (DataTemplate)this.Resources["DepartmensTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Departments.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void editDisease(Disease disease)
        {
            var edit = context.Diseases.First(a => a.Name == disease.Name);
            DiseaseEdit editDisease = new DiseaseEdit();
            editDisease.findCategory.ItemsSource = context.Categories.ToList<Category>();
            if (disease.Patients.Count > 0)
                editDisease.findCategory.IsEnabled = false;
            editDisease.gridMain.DataContext = disease;
            if (editDisease.ShowDialog() == true)
            {
                edit.Name = editDisease.Name.Text;
                edit.Category = editDisease.findCategory.SelectedItem as Category;
            }
            context.SaveChanges();
            currentType = "disease";
            DataTemplate template = (DataTemplate)this.Resources["DiseaseTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Diseases.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
        private void editAdmin(Admin adm)
        {
            AdminEdit adminEdit = new AdminEdit();
            adminEdit.gridMain.DataContext = adm;
            try
            {
                using (MemoryStream stream = new MemoryStream(adm.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    adminEdit.img.Source = bitmapImage;
                    adminEdit.imageBytes = adm.Photo;
                }
            }
            catch
            {

            }
            var edit = context.Admins.First(a => a.Login == adm.Login);
            if (adminEdit.ShowDialog() == true)
            {
                edit.FirstName = adminEdit.FirstName.Text;
                edit.LastName = adminEdit.LastName.Text;
                edit.Phone = adminEdit.Phone.Text;
                edit.Photo = adminEdit.imageBytes;
                edit.BirthDate = adminEdit.BirthDate.SelectedDate.GetValueOrDefault();
                edit.Login = adminEdit.txtLog.Text;
                edit.Password = adminEdit.txtPas.Text;
                context.SaveChanges();
            }
            currentType = "admins";
            DataTemplate template = (DataTemplate)this.Resources["AdminTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Admins.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
        private void editDoctor(Doctor doc)
        {
            if (currAdmin.Login == "root")
            {
                DoctorEdit doctorEdit = new DoctorEdit();
                doctorEdit.gridMain.DataContext = doc;
                doctorEdit.findCateg.ItemsSource = context.Categories.ToList();
                doctorEdit.findCateg.SelectedItem = doc.Category;
                if (doc.Patients.Count>0)
                {
                    doctorEdit.findCateg.IsEnabled = false;
                }
                try
                {
                    using (MemoryStream stream = new MemoryStream(doc.Photo))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        doctorEdit.img.Source = bitmapImage;
                        doctorEdit.imageBytes = doc.Photo;

                    }
                }
                catch { }
                var edit = context.Doctors.First(a => a.Phone == doc.Phone);
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
            else
                MessageBox.Show("Недостаточно прав для изменения информации", "Недостаточно прав",MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void editPatient(Patient tmp)
        {
            PatientEdit patientEdit = new PatientEdit(context,tmp);
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
            try
            {
                using (MemoryStream stream = new MemoryStream(tmp.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    patientEdit.img.Source = bitmapImage;
                }
            }
            catch { }
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
                    //context.Rooms.First(a => a.Id == room.Id).Patients.Remove(edit);
                    //edit.Room = null;
                }
                else
                {
                    edit.EndDate = null;
                } 
                context.SaveChanges();
                foreach (Room item in context.Rooms.ToList())
                {
                    item.CurrPatient =Convert.ToInt32(item.Patients.Where(a=>a.EndDate==null).Count());
                    
                }
                context.SaveChanges();
            }
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Patients.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }
        #endregion
        #region details
        private void mainDetails_click(object sender, MouseButtonEventArgs e)
        {
            switch (currentType)
            {
                case "patient":
                    Patient pat = lvInfo.SelectedItem as Patient;
                    detailsPatient(pat);
                    break;
                case "doctor":
                    Doctor doc = lvInfo.SelectedItem as Doctor;
                    detailsDoctor(doc);
                    break;
                case "department":
                    Department dep = lvInfo.SelectedItem as Department;
                    detailsDepartment(dep);
                    break;
                case "category":
                    Category cat = lvInfo.SelectedItem as Category;
                    detailsCategory(cat);
                    break;
                case "disease":
                    Disease dis = lvInfo.SelectedItem as Disease;
                    detailsDisease(dis);
                    break;
                case "admins":
                    Admin adm = lvInfo.SelectedItem as Admin;
                    detailsAdmin(adm);
                    break;
                case "rooms":
                    Room room = lvInfo.SelectedItem as Room;
                    detailsRoom(room);
                    break;
                default:
                    break;
            }
        }
        private void detailsRoom(Room room)
        {
            RoomInfo roomInfo = new RoomInfo(room);
            roomInfo.ShowDialog();
        }
        private void detailsDepartment(Department dep)
        {
            DepartmentInfo departmentInfo = new DepartmentInfo(dep);
            departmentInfo.ShowDialog();
        }
        private void detailsCategory(Category cat)
        {
            CategoryInfo categoryInfo = new CategoryInfo(cat);
            categoryInfo.ShowDialog();
        }
        private void detailsAdmin(Admin adm)
        {
            AdminInfo adminInfo = new AdminInfo(adm, currAdmin.Login);
            adminInfo.gridMain.DataContext =adm;
            try
            {
                using (MemoryStream stream = new MemoryStream(adm.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    adminInfo.img.Source = bitmapImage;
                }
            }
            catch
            {

            }
            
            adminInfo.ShowDialog();
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
            else if(pat.Sex=="Ж")
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
        private void details_click(object sender, RoutedEventArgs e)
        {
            switch (currentType)
            {
                case "patient":
                    Patient pat = lvInfo.SelectedItem as Patient;
                    detailsPatient(pat);
                    break;
                case "doctor":
                    Doctor doc = lvInfo.SelectedItem as Doctor;
                    detailsDoctor(doc);
                    break;
                case "department":
                    Department dep = lvInfo.SelectedItem as Department;
                    //detailsDepartment(dep);
                    break;
                case "category":
                    Category cat = lvInfo.SelectedItem as Category;
                    //detailsCategory(cat);
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
        private void detailsDoctor(Doctor doc)
        {
            DoctorInfo doctorInfo = new DoctorInfo(doc,currAdmin.Login);
            doctorInfo.gridMain.DataContext = doc;
            doctorInfo.BirthDate.Text = doc.BirthDate.ToShortDateString();
            doctorInfo.lvInfo.ItemsSource = doc.Patients.ToList();
            try
            {
                using (MemoryStream stream = new MemoryStream(doc.Photo))
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
        #endregion
        #region FiltersDate
        private void thisCurr_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Patients.Where(a => a.EndDate == null).ToList();
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void thisEnd_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Patients.Where(a => a.EndDate != null).ToList();
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void thisDay_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Patients.Where(a => a.StartDate.Day == DateTime.Now.Day).ToList();
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void thisMonth_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Patients.Where(a => a.StartDate.Month == DateTime.Now.Month).ToList();
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void thisYear_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = context.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year).ToList();
            currentType = "patient";
            DataTemplate template = (DataTemplate)this.Resources["PatientsTemplate"];
            lvInfo.ItemTemplate = template;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;
        }

        private void clearChoose_click(object sender, RoutedEventArgs e)
        {
            lvInfo.ItemsSource = null;
        }
        #endregion
        private void txtFind_txtchanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvInfo.ItemsSource).Refresh();
        }

        private void testStat_click(object sender, RoutedEventArgs e)
        {
            DiseaseStatist statist = new DiseaseStatist(context.Diseases.ToList(),context.Categories.ToList());
            statist.ShowDialog();
        }

        private void clear_click(object sender, RoutedEventArgs e)
        {
            txtFind.Text = "";
        }

        private void addRoom_click(object sender, RoutedEventArgs e)
        {
            AddRoom room = new AddRoom();
            room.findDepartment.ItemsSource = context.Departments.ToList();
            if (room.ShowDialog() == true)
            {
                context.Rooms.Add(room.room);
                context.SaveChanges();
            }
            currentType = "rooms";
            DataTemplate template = (DataTemplate)this.Resources["RoomsTemplate"];
            lvInfo.ItemTemplate = template;
            lvInfo.ItemsSource = context.Rooms.ToList();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvInfo.ItemsSource);
            view.Filter = FindFilterName;

        }
    }
}
