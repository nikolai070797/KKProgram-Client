using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
    [Serializable]
    public partial class ListGroup : Window
    {
        public static ObservableCollection<ListGroup> group = new ObservableCollection<ListGroup>();
        
        public static string name { get; set; }
        static uint autoincrement = 1;

        public ListGroup()
        {
            
            InitializeComponent();
            listItems.ItemsSource = group;

        }



        static ListGroup()
        {

            byte[] temp = new byte[4]; // Место для индекса
            BinaryFormatter bin = new BinaryFormatter();
            using (FileStream fs = new FileStream("LIST_Group", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (fs.Length > 0)
                {
                    fs.Read(temp, 0, 4); // помещаем сюда индекс
                    autoincrement = BitConverter.ToUInt32(temp, 0); // Заменяем индекс
                }
            }
        }

        private void NewGroup(object sender, RoutedEventArgs e)
        {
            AddGroup gr = new AddGroup();
            gr.ShowDialog();
        }


        private void EditGroup(object sender, RoutedEventArgs e)
        {

        }

        private void DelGroup(object sender, RoutedEventArgs e)
        {

        }

        public void Update(object sender, RoutedEventArgs e)
        {
            listItems.ItemsSource = null;
            listItems.ItemsSource = group;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            BinaryFormatter bin = new BinaryFormatter();
            using (FileStream fs = new FileStream("LIST_Group", FileMode.Create, FileAccess.Write))
            {
                fs.Write(BitConverter.GetBytes(autoincrement), 0, 4); // первые 4 байта занимает индекс
                bin.Serialize(fs, group); // Сохраняем остальные данные
            }
        }
    }
}
