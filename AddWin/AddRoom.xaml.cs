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

namespace kursADO.AddWin
{
    /// <summary>
    /// Логика взаимодействия для AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Window
    {
        public Room room;
        public AddRoom()
        {
            InitializeComponent();
            room = new Room();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            if (findDepartment.SelectedItem != null && Count.Text != null)
            {
                room.MaxPatients = Convert.ToInt32(Count.Text);
                room.Department = findDepartment.SelectedItem as Department;
                DialogResult = true;

            }
            else
                MessageBox.Show("Не все поля заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
    }
}
