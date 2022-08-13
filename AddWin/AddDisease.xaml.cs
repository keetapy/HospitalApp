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
    /// Логика взаимодействия для AddDisease.xaml
    /// </summary>
    public partial class AddDisease : Window
    {
        public AddDisease()
        {
            InitializeComponent();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            if(Name.Text!=null&&findCategory!=null)
            this.DialogResult = true;
            else
                MessageBox.Show("Не все поля заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
