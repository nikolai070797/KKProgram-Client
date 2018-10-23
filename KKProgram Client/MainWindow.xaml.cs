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

namespace KKProgram_Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listItems.ItemsSource = DataDevice.devices;
        }

        private void listItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newsession(object sender, RoutedEventArgs e)
        {
            newsession newses = new newsession();
            newses.ShowDialog();

        }

        private void tariff(object sender, RoutedEventArgs e)
        {
            listtariff r = new listtariff();
            r.ShowDialog();
        }

        private void report(object sender, RoutedEventArgs e)
        {
            report rep = new report();
            rep.ShowDialog();
        }

        private void network(object sender, RoutedEventArgs e)
        {
            Network net = new Network();
            net.ShowDialog();
        }

        private void SaveListDevice(object sender, RoutedEventArgs e)
        {
            DataDevice.Save();
        }

        private void DeleteDevice(object sender, RoutedEventArgs e)
        {
            if (listItems.SelectedIndex == -1)
                return;

            if (listItems.SelectedItem is DataDevice)
            {





                DataDevice delete = (DataDevice)listItems.SelectedItem;
                if (MessageBox.Show("Данное действие безвозвратно удалит объект из базы данных. Вы уверены?", "Предупреждение", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    delete.Delete();
                    //delete.Delete();
                    //listItems.ItemsSource = DataDevice.SelectAll();
                }
            }
        }
    }
}
