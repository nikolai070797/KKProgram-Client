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
    /// Логика взаимодействия для newsession.xaml
    /// </summary>
    public partial class newsession : Window
    {
        public newsession()
        {
            InitializeComponent();
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
