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
    /// Логика взаимодействия для RoomEdit.xaml
    /// </summary>
    public partial class RoomEdit : Window
    {
        public Room room;
        bool res = true;
        public RoomEdit(Room room)
        {
            InitializeComponent();
            this.room = room;
            gridMain.DataContext = room;
            
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            if (res)
            {
                room.MaxPatients = Convert.ToInt32(Count.Text);
                room.Department = findDepartment.SelectedItem as Department;
                DialogResult = true;
            }
        }

        private void change_change(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(Count.Text) < room.Patients.Count && Count.Text != null)
                {
                    res = false;
                    MessageBox.Show("Невозможно установить значение меньше чем " + room.Patients.Count);

                }
                else if (Count.Text == null)
                {
                    res = false;
                }
                else
                    res = true;
            }
            catch
            {
                res = false;
                    MessageBox.Show("Невозможно установить значение");
            }

        }
    }
}
