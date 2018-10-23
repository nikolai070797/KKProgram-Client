using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KKProgram_Client
{
    [Serializable]
    public class DataDevice 
    {
        public static ObservableCollection<DataDevice> devices = new ObservableCollection<DataDevice>();
        static uint autoincrement = 1;

        public uint IDTableHost { get; set; }
        public uint ID {get; set;}
        public string Status { get; set; }
        public uint Left { get; set; }
        public uint Past { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public uint Price { get; set; }
        public uint Last { get; set; }
        public string Group { get; set; }

        public DataDevice(TableHost host)
        {
            IDTableHost = (uint)host.idDevice;
            ID = autoincrement++;
            devices.Add(this);
        }
        public DataDevice(AddGroup  name)
        {
            Status = AddGroup.name;
        }

        public static void Save() // Сохранение списка
        {
            BinaryFormatter bin = new BinaryFormatter();
            using (FileStream fs = new FileStream("LIST_Device", FileMode.Create, FileAccess.Write))
            {
                fs.Write(BitConverter.GetBytes(autoincrement), 0, 4); // первые 4 байта занимает индекс
                bin.Serialize(fs, devices); // Сохраняем остальные данные
            }
        }

        static DataDevice() // Загрузка списка
        {
            byte[] temp = new byte[4]; // Место для индекса
            BinaryFormatter bin = new BinaryFormatter();
            using (FileStream fs = new FileStream("LIST_Device", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (fs.Length > 0)
                {
                    fs.Read(temp, 0, 4); // помещаем сюда индекс
                    devices = (ObservableCollection<DataDevice>)bin.Deserialize(fs); // Заменяем данные из файла
                    autoincrement = BitConverter.ToUInt32(temp, 0); // Заменяем индекс
                }
            }

        }

        public void Delete()
        {
            devices.Remove(this);
            System.Windows.MessageBox.Show("Удаление прошло успешно. \n Не забудьте сохранить список", "Внимание");
        }


        public async void StartTimer(uint minutesCount)
        {
            Start = DateTime.Now;
            End = Start.AddMinutes(minutesCount);
            Left = minutesCount;
            Past = 0;
            /*await Task.Run(()=> {

            }).Wait();*/
        }

        
    }
}
