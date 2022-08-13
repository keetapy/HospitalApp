using kursADO.DetailsWin;
using kursADO.Models;
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

namespace kursADO.Statistick
{
    public class testList
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int Length { get; set; }
        public testList(string Name, int Count)
        {
            this.Name = Name;
            this.Count = Count;

            Length =Count * 10;
        }
    }
    /// <summary>
    /// Логика взаимодействия для DiseaseStatist.xaml
    /// </summary>
        public class DiseaseStat
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public int Length { get; set; }
            public int Height { get; set; }
            public int Size { get; set; }
        }
    public partial class DiseaseStatist : Window
    {
        /*public List<testList> test;
        public int mainH;*/

        public int defCount=0;
        public List<DiseaseStat> diseaseStats=new List<DiseaseStat>();
        List<Disease> diseases;
        List<Disease> diseasesDef;
        public DiseaseStatist()
        {
            InitializeComponent();           
        }
        public DiseaseStatist(List<Disease> diseases, List<Category> categories)
        {
            InitializeComponent();
            List<Category> menu = new List<Category>
            {
                new Category{Name="Все категории"}
            };
            menu.AddRange(categories);
            chooseCateg.ItemsSource = menu;
            this.diseases=diseasesDef= diseases;
            chooseCateg.SelectedItem = menu.Where(a => a.Name == "Все категории");
            lvInfo.ItemsSource = allTime(diseases);
        }
        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public List<Disease> ChooseCateg(List<Disease> diseases, Category category)
        {
            if(category.Name!= "Все категории")
            return diseases.Where(a => a.Category.Name == category.Name).ToList();
            else
            {
                return diseases;
            }
        }
        
        public List<DiseaseStat> thisYear(List<Disease> diseases)
        {
            diseaseStats = new List<DiseaseStat>();
            try
            {
                defCount = diseases.Count();
                int maxCount = diseases.Max(x => x.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year).Count());
                double k = 1;
                int font = 0;
                if (maxCount < 10)
                {
                    k = 50;
                    font = 20;
                }
                else if (maxCount < 100)
                {
                    k = 5;
                    font = 15;
                }
                else if (maxCount < 1000)
                {
                    k = 0.1;
                    font = 10;
                }
                else if (maxCount < 10000)
                {
                    k = 0.01;
                    font = 7;
                }

                foreach (Disease item in diseases)
                {
                    DiseaseStat disease = new DiseaseStat
                    {
                        Name = item.Name,
                        Count = item.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year).Count(),
                        Length = Convert.ToInt32(item.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year).Count() * k),
                        Height = Convert.ToInt32(230 / defCount),
                        Size = font
                    };
                    diseaseStats.Add(disease);
                }
            }
            catch
            {
                MessageBox.Show("Нет данных");
            }
            
            return diseaseStats;
        }
        public List<DiseaseStat> thisMonth(List<Disease> diseases)
        {
            diseaseStats = new List<DiseaseStat>();
            defCount = diseases.Count();
            int maxCount = diseases.Max(x => x.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year && a.StartDate.Month == DateTime.Now.Month).Count());
            double k = 1;
            int font = 0;
            if (maxCount < 10)
            {
                k = 50;
                font = 20;
            }
            else if (maxCount < 100)
            {
                k = 5;
                font = 15;
            }
            else if (maxCount < 1000)
            {
                k = 0.1;
                font = 10;
            }
            else if (maxCount < 10000)
            {
                k = 0.01;
                font = 7;
            }

            foreach (Disease item in diseases)
            {
                DiseaseStat disease = new DiseaseStat
                {
                    Name = item.Name,
                    Count = item.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year && a.StartDate.Month == DateTime.Now.Month).Count(),
                    Length = Convert.ToInt32(item.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year && a.StartDate.Month == DateTime.Now.Month).Count() * k),
                    Height = Convert.ToInt32(230 / defCount),
                    Size = font
                };
                diseaseStats.Add(disease);
            }
            return diseaseStats;
        }
        public List<DiseaseStat> thisWeek(List<Disease> diseases)
        {
            diseaseStats = new List<DiseaseStat>();
            defCount = diseases.Count();
            int maxCount = diseases.Max(x => x.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year && a.StartDate.Month == DateTime.Now.Month && a.StartDate.Day <= DateTime.Now.Day && a.StartDate.Day >= DateTime.Now.Day - 6).Count());
            double k = 1;
            int font = 0;
            if (maxCount < 10)
            {
                k = 50;
                font = 20;
            }
            else if (maxCount < 100)
            {
                k = 5;
                font = 15;
            }
            else if (maxCount < 1000)
            {
                k = 0.1;
                font = 10;
            }
            else if (maxCount < 10000)
            {
                k = 0.01;
                font = 7;
            }

            foreach (Disease item in diseases)
            {
                DiseaseStat disease = new DiseaseStat
                {
                    Name = item.Name,
                    Count = item.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year && a.StartDate.Month == DateTime.Now.Month).Count(),
                    Length = Convert.ToInt32(item.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year && a.StartDate.Month == DateTime.Now.Month).Count() * k),
                    Height = Convert.ToInt32(230 / defCount),
                    Size = font
                };
                diseaseStats.Add(disease);
            }
            return diseaseStats;
        }
        public List<DiseaseStat> allTime(List<Disease> diseases)
        {
            diseaseStats = new List<DiseaseStat>();
            defCount = diseases.Count();
            int maxCount = diseases.Max(x => x.Patients.Count());
            double k = 1;
            int font = 0;
            if (maxCount < 10)
            {
                k = 50;
                font = 20;
            }
            else if (maxCount < 100)
            {
                k = 5;
                font = 15;
            }
            else if (maxCount < 1000)
            {
                k = 0.1;
                font = 10;
            }
            else if (maxCount < 10000)
            {
                k = 0.01;
                font = 7;
            }

            foreach (Disease item in diseases)
            {
                DiseaseStat disease = new DiseaseStat
                {
                    Name = item.Name,
                    Count = item.Patients.Count(),
                    Length = Convert.ToInt32(item.Patients.Count() * k),
                    Height = Convert.ToInt32(230 / defCount),
                    Size = font
                };
                diseaseStats.Add(disease);
            }
            return diseaseStats;
        }
        private void thisYear_click(object sender, RoutedEventArgs e)
        {

            if (diseases.Count > 0)
                lvInfo.ItemsSource = thisYear(diseases);
            else
                lvInfo.ItemsSource = null;
        }

        private void thisMonth_click(object sender, RoutedEventArgs e)
        {
            if(diseases.Count>0)
                lvInfo.ItemsSource = thisMonth(diseases);
            else
                lvInfo.ItemsSource = null;
        }

        private void thisWeek_click(object sender, RoutedEventArgs e)
        {
            
            if(diseases.Count>0)
                lvInfo.ItemsSource = thisWeek(diseases);
            else
                lvInfo.ItemsSource = null;
        }

        private void allTime_click(object sender, RoutedEventArgs e)
        {
            
            if(diseases.Count>0)
                lvInfo.ItemsSource = allTime(diseases);
            else
                lvInfo.ItemsSource = null;
        }

        private void moreInfo_click(object sender, MouseButtonEventArgs e)
        {
            DiseaseInfo detailsDisease = new DiseaseInfo(diseases.First(a=>a.Name==(lvInfo.SelectedItem as DiseaseStat).Name));
            detailsDisease.ShowDialog();
        }

        private void chooseItem_click(object sender, RoutedEventArgs e)
        {
            diseases = ChooseCateg(diseasesDef, chooseCateg.SelectedItem as Category);
            if (diseases.Count > 0)
                lvInfo.ItemsSource = allTime(diseases);
            else
                lvInfo.ItemsSource = null;
        }
    }
}
