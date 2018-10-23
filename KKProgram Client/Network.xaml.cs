using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace KKProgram_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Network : Window
    {
        
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);

        public static List<TableHost> _host = new List<TableHost>();
        string hostname = "";
        IPHostEntry entry;
        string[] ipToString = new string[4];


        public Network()
        {
            InitializeComponent();

            listView1.ItemsSource = null;
            listView1.ItemsSource = _host;

            // Получение имени компьютера.
            String host = System.Net.Dns.GetHostName();
            // Получение ip-адреса.
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];
            // Показ адреса в label'е.
            label3.Content = ip.ToString();
            ipToString = ip.ToString().Split('.');
            Add();
        }

        string[] ipadressText;
        string[] hostnameText;
        string[] macaddressText;

        private void Add()
        {
            
            string[] str = File.ReadAllLines("IPMAC.txt");
            ipadressText = new string[str.Length];
            hostnameText = new string[str.Length];
            macaddressText = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                string[] s = str[i].Split('#');
                ipadressText[i] = s[0];
                hostnameText[i] = s[1];
                macaddressText[i] = s[2];
                comboBox1.Items.Add(s[1]);
            }

        }

        private void WakeFunction(string MAC_ADDRESS)
        {
            WOLClass client = new WOLClass();
            client.Connect(new IPAddress(0xffffffff), 0x2fff);
            client.SetClientToBrodcastMode();
            int counter = 0;
            //буффер для отправки
            byte[] bytes = new byte[1024];
            //Первые 6 бит 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //Повторим MAC адрес 16 раз
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] = byte.Parse(MAC_ADDRESS.Substring(i, 2), NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //Отправляем полученый магический пакет
            int reterned_value = client.Send(bytes, 1024);
        }

        static int id = 0;
        private void GetInform(string textName)
        {
            string IP_Address = "";
            string HostName = "";
            string MacAddress = "";


            try
            {
                //Проверяем существует ли IP
                entry = Dns.GetHostEntry(textName);
                foreach (IPAddress a in entry.AddressList)
                {
                    IP_Address = a.ToString();
                    break;
                }

                //Получаем HostName
                HostName = entry.HostName;

                try
                {

                    //Получаем Mac-address
                    IPAddress dst = IPAddress.Parse(IP_Address);



                    byte[] macAddr = new byte[6];
                    uint macAddrLen = (uint)macAddr.Length;

                    if (SendARP(BitConverter.ToInt32(dst.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
                        throw new InvalidOperationException("SendARP failed.");

                    string[] str = new string[(int)macAddrLen];
                    for (int i = 0; i < macAddrLen; i++)
                        str[i] = macAddr[i].ToString("x2");

                    MacAddress = string.Join(":", str);

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                //Далее, если всё успешно, добавляем все данные в список, после чего выводим всё в ListView
                Dispatcher.Invoke(new Action(() =>
                    {
                        /*for (int j = 0; j <= id; j++, id = j)
                        {
                            MessageBox.Show("j = " + j);
                           
                        }*/

                        _host.Add(new TableHost() { idDevice = id++, ipAdress = IP_Address, nameComputer = HostName, MacAdress = MacAddress });
                        listView1.ItemsSource = null;
                        listView1.ItemsSource = _host;

                    }));

            }
           catch { }
          //  catch  (Exception ex) { MessageBox.Show(ex.Message); }
            
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {

            //string message = "";
            //Print(GetIP(textBox1.Text), GetHostName(textBox1.Text), GetMac(message));
        }

        private /*async*/ void StartScan(object sender, RoutedEventArgs e)
        {
           // List<Task> tasks = new List<Task>();
            int i = int.Parse(ipToString[0]);
            int j = int.Parse(ipToString[1]);
            {
                for (int k = 0; k < 6; k++)
                {
                    for (int m = 0; m < 254; m++)
                    {
                        /*tasks.Add(new Task(
                            () => GetInform(string.Format("{0}.{1}.{2}.{3}", i.ToString(), j.ToString(), k.ToString(), m.ToString()))
                            ));*/
                        Thread _thread = new Thread(() => GetInform(string.Format("{0}.{1}.{2}.{3}", i.ToString(), j.ToString(), k.ToString(), m.ToString())));
                        _thread.Start();
                    }
                }
                
            }

            /*var array = tasks.ToArray();
            for (int i1 = 0; i1 < array.Length; i1++)
                array[i1].Start();
            Task.WaitAll(array);*/
        }

        private void PowerOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WakeFunction(_host[listView1.SelectedIndex].MacAdress.ToString().Replace(":", ""));
                MessageBox.Show("Операция выполнена успешно!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch { MessageBox.Show("Запрос некорретный!", "Внимание!Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void ClearList_Click(object sender, RoutedEventArgs e)
        {
            _host.Clear();
            listView1.Items.Refresh();
        }

        private void copyIP_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_host[listView1.SelectedIndex].ipAdress.ToString());
        }

        private void copyName_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_host[listView1.SelectedIndex].nameComputer.ToString());
        }

        private void copyMacAddress_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_host[listView1.SelectedIndex].MacAdress.ToString());
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            GetInform(textBox1.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (var write = new StreamWriter(Environment.CurrentDirectory + "/IPMAC.txt", true))
            {


                for (int index = 0; index < _host.Count; index++)
                {

                    if (!macaddressText.Contains(_host[index].MacAdress))
                        write.WriteLine(_host[index].idDevice + "#" + _host[index].ipAdress + "#" + _host[index].nameComputer + "#" + _host[index].MacAdress);
                }
                write.Close();
            }

        }

        void P()
        {
            for (double i = -192; i < 10; i += 0.004)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    rectangle1.Margin = new Thickness(0, 101, i, 10);
                }));

            }//12,0,12,1


        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ii = rectangle1.Width;
            textBox2.Text = ipadressText[comboBox1.SelectedIndex];
            textBox3.Text = comboBox1.SelectedItem.ToString();
            textBox4.Text = macaddressText[comboBox1.SelectedIndex];
            Thread thread = new Thread(P);
            thread.Start();
        }

        private void PowerOn1(object sender, RoutedEventArgs e)
        {
            try
            {
                WakeFunction(textBox4.Text.Replace(":", ""));
                MessageBox.Show("Операция выполнена успешно!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch { MessageBox.Show("Запрос некорретный!", "Внимание!Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Close_grid(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(CLose);
            thread.Start();
        }

        void CLose()
        {
            for (double i = 9; i > -193; i -= 0.004)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    rectangle1.Margin = new Thickness(0, 101, i, 10);
                }));

            }//12,0,12,1
        }

        private void Add_Device(object sender, RoutedEventArgs e)
        {
            new DataDevice((TableHost)listView1.SelectedItem);
        }

    }

    public class TableHost
    {
        public int idDevice { get; set; }
        public string ipAdress { get; set; }
        public string nameComputer { get; set; }
        public string MacAdress { get; set; }
    }

    public class WOLClass : UdpClient
    {
        public WOLClass()
            : base()
        { }
        //Установим broadcast для отправки сообщений
        public void SetClientToBrodcastMode()
        {
            if (this.Active)
                this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
        }
    }
}
