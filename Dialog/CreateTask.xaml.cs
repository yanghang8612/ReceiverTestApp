using ReceiverTestApp.Domain;
using ReceiverTestApp.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ReceiverTestApp.Dialog
{
	// 创建测试任务对话框
    // 分步骤展示，共三个步骤
	public partial class CreateTask : UserControl
    {
        // UI的DataContext类型，采用ConfigModel，其中的Device和Template列表的元素均含Selected属性
        private class DataModel : PropertyNotifyObject
        {
            public string TaskName
            {
                set { this.SetValue(x => x.TaskName, value); }
                get { return this.GetValue(x => x.TaskName); }
            }

            public string WorkPath
            {
                set { this.SetValue(x => x.WorkPath, value); }
                get { return this.GetValue(x => x.WorkPath); }
            }

            public ConfigModel Config { get; }

            public DataModel()
            {
                TaskName = "";
                Config = ConfigModel.Instance;
            }
        }

        private readonly DataModel Model = new DataModel();

        private MainWindowModel MainModel;

		public CreateTask(MainWindowModel MainModel)
		{
			InitializeComponent();
            this.DataContext = Model;
            this.MainModel = MainModel;
            Model.TaskName = "hehe";
            Model.WorkPath = @"D:\WorkDir";
        }

        // 触发保存事件，
        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<TestItemTemplate> templates = new List<TestItemTemplate>();
            foreach (var template in Model.Config.Templates)
            {
                if (template.IsSelected) 
                {
                    templates.Add(template);
                    template.IsSelected = false;
                }
            }
            List<Device> devices = new List<Device>();
            foreach (var device in Model.Config.Devices)
            {
                if (device.IsSelected)
                {
                    devices.Add(device);
                    device.IsSelected = false;
                }
            }
            TestTask task = new TestTask(Model.TaskName, Model.WorkPath, templates, devices);
            MainModel.ShowContent = true;
            MainModel.ShowModal = false;
            MainModel.Task = task;
            MainModel.CurInsDevice = task.RunningDevices[0];
            MainModel.CurStepDevice = task.RunningDevices[0];
            MainModel.CurSysDevice = task.RunningDevices[0];
            MainModel.Tasks.Add(task);
            MainModel.Messages.Insert(0, new Message("任务规划已创建"));
        }

        private void ChooseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Model.WorkPath = CommonMethod.ChooseFolderDialog();
        }
    }
}
