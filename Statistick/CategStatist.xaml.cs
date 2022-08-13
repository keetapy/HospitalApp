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
    /// <summary>
    /// Логика взаимодействия для CategStatist.xaml
    /// </summary>
    public class CategStat
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
    }
    public partial class CategStatist : Window
    {
        /*public List<testList> test;
        public int mainH;*/

        public int defCount = 0;
        public List<DiseaseStat> diseaseStats = new List<DiseaseStat>();
        List<Disease> diseases;
        public CategStatist()
        {
            InitializeComponent();
        }
        public CategStatist(List<Disease> diseases, Category categories)
        {
            InitializeComponent();
           
            this.diseases = ChooseCateg(diseases,categories);
           // lvInfo.ItemsSource = allTime(diseases);
        }
        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public List<Disease> ChooseCateg(List<Disease> diseases, Category category)
        {
           
                return diseases.Where(a => a.Category.Name == category.Name).ToList();
           
        }

        public List<DiseaseStat> thisYear(List<Disease> diseases)
        {
            diseaseStats = new List<DiseaseStat>();
            defCount = diseases.Count();
            int maxCount = diseases.Max(x => x.Patients.Where(a => a.StartDate.Year == DateTime.Now.Year).Count());
            double k = 1;
            int font = 0;
            if (maxCount < 10)
            {
                k = 100;
                font = 20;
            }
            else if (maxCount < 100)
            {
                k = 10;
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
                k = 100;
                font = 20;
            }
            else if (maxCount < 100)
            {
                k = 10;
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
                k = 100;
                font = 20;
            }
            else if (maxCount < 100)
            {
                k = 10;
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
                k = 100;
                font = 20;
            }
            else if (maxCount < 100)
            {
                k = 10;
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

            lvInfo.ItemsSource = thisYear(diseases);
        }

        private void thisMonth_click(object sender, RoutedEventArgs e)
        {

            lvInfo.ItemsSource = thisMonth(diseases);
        }

        private void thisWeek_click(object sender, RoutedEventArgs e)
        {


            lvInfo.ItemsSource = thisWeek(diseases);
        }

        private void allTime_click(object sender, RoutedEventArgs e)
        {

            lvInfo.ItemsSource = allTime(diseases);
        }

        private void moreInfo_click(object sender, MouseButtonEventArgs e)
        {
            DiseaseInfo detailsDisease = new DiseaseInfo(diseases.First(a => a.Name == (lvInfo.SelectedItem as DiseaseStat).Name));
            detailsDisease.ShowDialog();
        }
       
    }
}
