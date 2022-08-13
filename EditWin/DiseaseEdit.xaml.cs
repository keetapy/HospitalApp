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
    /// Логика взаимодействия для DiseaseEdit.xaml
    /// </summary>
    public partial class DiseaseEdit : Window
    {
        public DiseaseEdit()
        {
            InitializeComponent();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
