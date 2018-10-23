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
    public partial class AddGroup : Window
    {

        public static string name { get; set; }

        public AddGroup()
        {
            InitializeComponent();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void Button_Save(object sender, RoutedEventArgs e)
        {
            name = textbox.Text;
            ListGroup.name = name;
            


            Close();
        }
    }
}
