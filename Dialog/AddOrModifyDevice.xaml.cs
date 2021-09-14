using ReceiverTestApp.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
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

namespace ReceiverTestApp.Dialog
{
    // 添加或修改设备对话框，有2个构造函数，分别对应添加或修改的操作
    public partial class AddOrModifyDevice : UserControl
    {
        // UI的DataContext类型，三个字段分别被对应的界面元素Binding
        private class DataModel : PropertyNotifyObject
        {
            public string DeviceName
            {
                set { this.SetValue(x => x.DeviceName, value); }
                get { return this.GetValue(x => x.DeviceName); }
            }

            public string SelectedPort
            {
                set { this.SetValue(x => x.SelectedPort, value); }
                get { return this.GetValue(x => x.SelectedPort); }
            }

            public string SelectedBaudRate
            {
                set { this.SetValue(x => x.SelectedBaudRate, value); }
                get { return this.GetValue(x => x.SelectedBaudRate); }
            }

            public ObservableCollection<string> Ports { get; }

            public ObservableCollection<string> Rates { get; }

            public DataModel()
            {
                Ports = new ObservableCollection<string>(SerialPort.GetPortNames());
                Rates = new ObservableCollection<string>(new string[] 
                { 
                    "600", "1200", "2400", "4800", "9600", "19200", "38400"
                });
            }
        }

        // 保存的要编辑的Device实例
        private readonly Device CurDevice;

        // 保存的DataContext实例
        private readonly DataModel Model;

        // 新增设备对话框的构造函数
        public AddOrModifyDevice()
        {
            InitializeComponent();
            DataModel data = new DataModel();
            this.DataContext = data;
            this.Model = data;
        }

        // 编辑设备对话框的构造函数
        public AddOrModifyDevice(Device device)
        {
            InitializeComponent();
            this.CurDevice = device;
            DataModel data = new DataModel
            {
                DeviceName = device.Name,
                SelectedPort = device.Port,
                SelectedBaudRate = device.BaudRate.ToString()
            };
            this.DataContext = data;
            this.Model = data;
        }

        // 触发保存事件，要写回配置文件
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurDevice == null)
            {
                ConfigModel.Instance.Devices.Add(new Device
                {
                    Name = Model.DeviceName,
                    Port = Model.SelectedPort,
                    BaudRate = int.Parse(Model.SelectedBaudRate)
                });
            }
            else
            {
                CurDevice.Name = Model.DeviceName;
                CurDevice.Port = Model.SelectedPort;
                CurDevice.BaudRate = int.Parse(Model.SelectedBaudRate);
            }
            // 写回配置文件
            ConfigModel.Instance.WriteBack();
        }
    }
}
