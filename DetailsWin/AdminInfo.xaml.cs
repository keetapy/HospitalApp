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

namespace kursADO.DetailsWin
{
    /// <summary>
    /// Логика взаимодействия для AdminInfo.xaml
    /// </summary>
    public partial class AdminInfo : Window
    {
        string login;
        Admin admin;
        public AdminInfo(Admin adm, string login)
        {
            InitializeComponent();
            this.login = login;
            admin = adm;
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void getPass_click(object sender, RoutedEventArgs e)
        {
            if (login == "root" || admin.Login == login)
            {
                MessageBox.Show("Пароль: " + admin.Password);
            }
            else
            {
                MessageBox.Show("Недостаточно прав для просмотра информации", "Недостаточно прав", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
