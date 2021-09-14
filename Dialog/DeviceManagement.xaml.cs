using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using ReceiverTestApp.Domain;
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

namespace ReceiverTestApp.Dialog
{
    /// <summary>
    /// DeviceManagement.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceManagement : MetroWindow
    {
        public DeviceManagement()
        {
            InitializeComponent();
            this.DataContext = ConfigModel.Instance;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddOrModifyDevice();
            await DialogHost.Show(dialog, "DeviceDialog");
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConfirmDialog(((Device)DevicesListView.SelectedItem).Name);
            await DialogHost.Show(dialog, "DeviceDialog", DeleteDialog_OnDialogClosing);
        }

        private void DeleteDialog_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;
            ConfigModel.Instance.Devices.Remove((Device)DevicesListView.SelectedItem);
            ConfigModel.Instance.WriteBack();
        }

        private async void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddOrModifyDevice((Device)DevicesListView.SelectedItem);
            await DialogHost.Show(dialog, "DeviceDialog");
        }
    }
}
