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

namespace kursADO.EditWin
{
    /// <summary>
    /// Логика взаимодействия для CategoryEdit.xaml
    /// </summary>
    public partial class CategoryEdit : Window
    {
        public Category category;
        public CategoryEdit(Category category)
        {
            InitializeComponent();
            this.category = category;
            gridMain.DataContext = category;
            if (category.Diseases.Count > 0)
                findDepartment.IsEnabled = false;
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            category.Name = Name.Text;
            category.Department = findDepartment.SelectedItem as Department;
            DialogResult = true;
        }
    }
}
