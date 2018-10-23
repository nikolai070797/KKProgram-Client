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

namespace KKProgram_Client
{
    /// <summary>
    /// Логика взаимодействия для listtariff.xaml
    /// </summary>
    public partial class listtariff : Window
    {
        public listtariff()
        {
            InitializeComponent();
        }

        private void Newtariff(object sender, RoutedEventArgs e)
        {
            tariff t = new tariff();
            t.ShowDialog();
        }

        private void Edittariff(object sender, RoutedEventArgs e)
        {

        }

        private void Deltariff(object sender, RoutedEventArgs e)
        {

        }

        private void listItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listItems.SelectedIndex == -1)
            {
                tariff t = new tariff();
                t.ShowDialog();
            }
        }
    }
}
