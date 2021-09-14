using ReceiverTestApp.Domain;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ReceiverTestApp.Dialog
{
    /// <summary>
    /// MessageDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MessageConfirm : UserControl
    {
        public class DataModel : PropertyNotifyObject
        {
            public bool IsShow
            {
                set { this.SetValue(x => x.IsShow, value); }
                get { return this.GetValue(x => x.IsShow); }
            }

            public bool ShowFolder
            {
                set { this.SetValue(x => x.ShowFolder, value); }
                get { return this.GetValue(x => x.ShowFolder); }
            }

            public string Message
            {
                set { this.SetValue(x => x.Message, value); }
                get { return this.GetValue(x => x.Message); }
            }
        }

        public DataModel Model;

        public List<string> FolderList { get; }

        public List<string> FileList { get; }

        public Timer Timer;

        public MessageConfirm()
        {
            InitializeComponent();
            Model = new DataModel();
            FolderList = new List<string>();
            FileList = new List<string>();
            Timer = new Timer(200);
            Timer.Elapsed += Check;
            DataContext = Model;
        }

        public void SetRinexMessage(string path, string name)
        {
            Model.ShowFolder = true;
            FolderList.Add(path);
            FileList.Add(name);
            if (!Timer.Enabled) Timer.Start();
        }

        public void Check(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < FileList.Count; i++)
            {
                if (!File.Exists(FolderList[i] + "\\" + FileList[i])) return;
            }
            Model.ShowFolder = false;
            FolderList.Clear();
            FileList.Clear();
            Timer.Stop();
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < FolderList.Count; i++)
            {
                Process.Start(@"explorer.exe", FolderList[i]);
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Model.IsShow = false;
            Model.Message = "";
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter) && Keyboard.Modifiers == ModifierKeys.Alt)
            {
                e.Handled = true;
                Model.IsShow = false;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += UserControl_KeyDown;
        }
    }
}
