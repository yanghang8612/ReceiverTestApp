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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReceiverTestApp.Dialog
{
    /// <summary>
    /// TestItemManagement.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateManagement : MetroWindow
    {
        public TemplateManagement()
        {
            InitializeComponent();
            this.DataContext = ConfigModel.Instance;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddOrModifyTemplate();
            await DialogHost.Show(dialog, "TemplateDialog", delegate (object s, DialogOpenedEventArgs args)
            {
                dialog.SetSession(args.Session);
            });
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConfirmDialog(((TestItemTemplate)TemplatesListView.SelectedItem).Name);
            await DialogHost.Show(dialog, "TemplateDialog", DeleteDialog_OnDialogClosing);
        }

        private void DeleteDialog_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;
            ConfigModel.Instance.Templates.Remove((TestItemTemplate)TemplatesListView.SelectedItem);
            ConfigModel.Instance.WriteBack();
        }

        private async void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddOrModifyTemplate((TestItemTemplate)TemplatesListView.SelectedItem);
            await DialogHost.Show(dialog, "TemplateDialog", delegate (object s, DialogOpenedEventArgs args)
            {
                dialog.SetSession(args.Session);
            });
        }
    }
}
