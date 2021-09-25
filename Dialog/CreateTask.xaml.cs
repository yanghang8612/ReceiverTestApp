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

        private readonly MainWindowModel MainModel;

        private TestTask TestTask;

        public CreateTask(MainWindowModel mainModel)
        {
            InitializeComponent();
            DataContext = Model;
            MainModel = mainModel;
            MainModel.SelectedName = null;
            Model.TaskName = "hehe";
            Model.WorkPath = @"D:\WorkDir";
        }

        public CreateTask(MainWindowModel mainModel, TestTask task)
        {
            InitializeComponent();
            DataContext = Model;
            MainModel = mainModel;
            MainModel.SelectedName = task.Name;
            TestTask = task;
            Model.TaskName = task.Name;
            Model.WorkPath = task.WorkPath;
            foreach (TestItemTemplate t1 in task.Templates)
            {
                foreach (TestItemTemplate t2 in Model.Config.Templates)
                {
                    if (t2.Name.Equals(t1.Name))
                    {
                        t2.IsSelected = true;
                    }
                }
            }
            foreach (Device d1 in task.Devices)
            {
                foreach (Device d2 in Model.Config.Devices)
                {
                    if (d2.Name.Equals(d1.Name))
                    {
                        d2.IsSelected = true;
                    }
                }
            }
        }

        // 触发保存事件，
        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestTask != null)
            {
                TestTask.Name = Model.TaskName;
                TestTask.WorkPath = Model.WorkPath;
                TestTask.Templates.Clear();
                TestTask.Devices.Clear();
            }
            List<TestItemTemplate> templates = new List<TestItemTemplate>();
            foreach (TestItemTemplate template in Model.Config.Templates)
            {
                if (template.IsSelected)
                {
                    templates.Add(template);
                    template.IsSelected = false;
                    if (TestTask != null)
                    {
                        TestTask.Templates.Add(template);
                    }
                }
            }
            List<Device> devices = new List<Device>();
            foreach (Device device in Model.Config.Devices)
            {
                if (device.IsSelected)
                {
                    devices.Add(device);
                    device.IsSelected = false;
                    if (TestTask != null)
                    {
                        TestTask.Devices.Add(device);
                    }
                }
            }
            if (TestTask == null)
            {
                TestTask = new TestTask(Model.TaskName, Model.WorkPath, templates, devices);
                MainModel.Tasks.Add(TestTask);
                MainModel.Messages.Insert(0, new Message("任务规划已创建"));
            }
            else
            {
                MainModel.Messages.Insert(0, new Message("任务规划已修改"));
            }
            MainModel.ShowContent = true;
            MainModel.ShowModal = false;
            if (MainModel.Task == null)
            {
                MainModel.Task = TestTask;
                MainModel.CurInsDevice = TestTask.RunningDevices[0];
                MainModel.CurStepDevice = TestTask.RunningDevices[0];
                MainModel.CurSysDevice = TestTask.RunningDevices[0];
            }
        }

        private void ChooseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Model.WorkPath = CommonMethod.ChooseFolderDialog();
        }
    }
}
