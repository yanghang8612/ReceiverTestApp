using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using ReceiverTestApp.Dialog;
using ReceiverTestApp.Domain;
using ReceiverTestApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ReceiverTestApp
{
    public class MainWindowModel : PropertyNotifyObject
	{
        public bool ShowContent
        {
            set { this.SetValue(x => x.ShowContent, value); }
            get { return this.GetValue(x => x.ShowContent); }
        }

        public bool ShowModal
        {
            set { this.SetValue(x => x.ShowModal, value); }
            get { return this.GetValue(x => x.ShowModal); }
        }

        public bool IsRunning
        {
            set { this.SetValue(x => x.IsRunning, value); }
            get { return this.GetValue(x => x.IsRunning); }
        }

        public TestTask Task
        {
            set { this.SetValue(x => x.Task, value); }
            get { return this.GetValue(x => x.Task); }
        }

        public RunningDevice CurStepDevice
        {
            set { this.SetValue(x => x.CurStepDevice, value); }
            get { return this.GetValue(x => x.CurStepDevice); }
        }

        public RunningDevice CurInsDevice
        {
            set { this.SetValue(x => x.CurInsDevice, value); }
            get { return this.GetValue(x => x.CurInsDevice); }
        }

        public RunningDevice CurSysDevice
        {
            set { this.SetValue(x => x.CurSysDevice, value); }
            get { return this.GetValue(x => x.CurSysDevice); }
        }

        public ObservableCollection<TestTask> Tasks { get; }

        public ObservableCollection<Message> Messages { get; }

        public MainWindowModel()
        {
            ShowContent = false;
            ShowModal = true;
            IsRunning = false;
            Tasks = new ObservableCollection<TestTask>();
            Messages = new ObservableCollection<Message>();
        }
	}

	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : MetroWindow
    {
		private MainWindowModel DataModel = new MainWindowModel();

        private readonly MessageConfirm ConfirmDialog = new MessageConfirm();

        private readonly object Lock = new object();

        public MainWindow()
        {
            InitializeComponent();
			this.DataContext = DataModel;
            //string s = "${LIBRES}>=100";
            //Dictionary<string, double> vars = new Dictionary<string, double>();
            //vars["LIBRES"] = 100;
            //Console.WriteLine(Check(s, vars));
            //vars["LIBRES"] = 10;
            //Console.WriteLine(Check(s, vars));
            //vars["LIBRES"] = 101;
            //Console.WriteLine(Check(s, vars));
            //signal_type_input_para_s[] p = new signal_type_input_para_s[1];
            //p[0].recv_nmea_file = "C:\\1.txt";
            //p[0].sim_nmea_file = "C:\\2.txt";
            //p[0].signal_type = 1;
            //signal_type_output_para_s r = SolveMethod.hcl_signal_type(p);
            //SolveMethod.RecordToFile(r.ndata, r.time, r.data3D, @"C:\shabi.dat");
            //Console.WriteLine();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DataModel != null)
            {
                foreach (var d in DataModel.Task.RunningDevices)
                {
                    if (d.Port.IsOpen)
                    {
                        d.Port.Close();
                    }
                }
            }
        }

        private async void Add_MenuItem_Click(object sender, RoutedEventArgs e)
		{
            var dialog = new CreateTask(DataModel);
            await DialogHost.Show(dialog);
        }

        private void Close_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            DataModel = new MainWindowModel();
            DataContext = DataModel;
        }

        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			//_ = TestTaskTree.SelectedItem as TreeViewItem;
			//TestTaskTree.ContextMenu = TestTaskTree.Resources["TaskContext"] as ContextMenu;
			//switch (SelectedItem.Tag.ToString())
			//{
			//	case "Solution":
					
			//		break;
			//	case "Folder":
			//		TestTaskTree.ContextMenu = TestTaskTree.Resources["FolderContext"] as System.Windows.Controls.ContextMenu;
			//		break;
			//}
		}

		private void Add_TestItem(object sender, RoutedEventArgs e)
		{
		}

		private void Remove_TestItem(object sender, RoutedEventArgs e)
		{
		}

        private void TestItemManagement_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new TemplateManagement
            {
                Owner = this,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.ShowDialog();
        }

        private void TestDeviceManagement_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new DeviceManagement
            {
                Owner = this,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.ShowDialog();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DataModel.IsRunning = true;
            foreach (var d in DataModel.Task.RunningDevices)
            {
                ThreadPool.QueueUserWorkItem(o => ExecuteTestItem(d));
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            DataModel.IsRunning = false;
        }

        private void ExecuteTestItem(RunningDevice d)
        {
            string rootPath = DataModel.Task.WorkPath + "\\" + d.Config.Name;
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            for (int i = 0; i < d.Items.Count; i++)
            {
                TestItem item = d.Items[i];
                int idx = int.Parse(item.Template.Name.Split('.')[0]);
                d.Standard = item.Template.GetStandard();
                d.Steps = d.Items[i].Steps;
                string itemPath = rootPath + "\\" + item.Template.Name;
                if (Directory.Exists(itemPath))
                {
                    CommonMethod.DelectDir(itemPath);
                }
                else
                {
                    Directory.CreateDirectory(itemPath);
                }
                for (int j = 0; j < item.Signals.Count; j++, item.SignalIdx++)
                {
                    Dictionary<string, double> vars = new Dictionary<string, double>();
                    Dictionary<string, int[]> arrays = new Dictionary<string, int[]>();
                    long start = 0;
                    ShowConfirmMessage(d.Config.Name + ":请切换到频点 " + item.Signals[j].Name);
                    while (ConfirmDialog.Model.IsShow) Thread.Sleep(100);
                    for (int k = 0; k < item.Steps.Count; k++, item.StepIdx++)
                    {
                        if (k != 0) item.Steps[k - 1].IsHighlight = false;
                        item.Steps[k].IsHighlight = true;
                        string[] ins = item.Steps[k].Ins.Split(new char[] { ' ' }, 3);
                        if (ins[0] == "SYSTEM")
                        {
                            if (ins[1] == "END")
                            {
                                if (idx >= 2 && idx <= 4)
                                {
                                    double[] res = new double[1];
                                    res[0] = vars["power"];
                                    item.Signals[j].Result = res;
                                }
                                item.Steps[k].IsHighlight = false;
                                break;
                            }
                            switch (ins[1])
                            {
                                case "WAIT":
                                    int countdown = int.Parse(ins[2]);
                                    RecordMessage(string.Format("{0} 等待中 0/{1}秒……", d.Config.Name, countdown));
                                    for (int l = 1; l <= countdown; l++)
                                    {
                                        Thread.Sleep(1000);
                                        IncreaseTime(d.Config.Name, l);
                                        item.Signals[j].Ticks += 1000;
                                        item.Ticks += 1000;
                                        DataModel.Task.Ticks += 1000;
                                    }
                                    RemoveMessage();
                                    break;
                                case "DIALOG":
                                    string msg = ins[2].Split(' ')[1].Replace('\'', ' ');
                                    if (msg.Contains("$"))
                                    {
                                        msg = Replace(msg, vars, arrays);
                                    }
                                    if (msg.Contains("RINEX"))
                                    {
                                        lock (Lock)
                                        {
                                            ConfirmDialog.SetRinexMessage(itemPath, item.Signals[j].Name + ".20O");
                                        }
                                    }
                                    ShowConfirmMessage(d.Config.Name + ":" + msg);
                                    while (ConfirmDialog.Model.IsShow) Thread.Sleep(100);
                                    break;
                                case "STARTREC":
                                    start = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                                    RecordMessage(string.Format("开始记录 {0} 数据。", d.Config.Name));
                                    string dataFile = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                    string recordFile = string.Format("{0}\\{1}.bin", itemPath, item.Signals[j].Name);
                                    d.LogFile = new BinaryWriter(File.Create(dataFile));
                                    d.IsLogging = true;
                                    d.Recorder = new TimeFileRecorder(recordFile, dataFile);
                                    d.Recorder.Start();
                                    break;
                                case "STOPREC":
                                    RecordMessage(string.Format("停止记录 {0} 数据。", d.Config.Name));
                                    d.IsLogging = false;
                                    d.LogFile.Flush();
                                    d.LogFile.Close();
                                    d.Recorder.Stop();
                                    break;
                                case "DEFVAR":
                                    if (ins[2].Contains("{"))
                                    {
                                        string name = ins[2].Split('=')[0];
                                        string[] array = ins[2].Split('=')[1].Replace("{", "").Replace("}", "").Split(',');
                                        int[] T = new int[array.Length];
                                        for (int l = 0; l < array.Length; l++)
                                        {
                                            T[l] = int.Parse(array[l]);
                                        }
                                        arrays[name] = T;
                                    }
                                    else
                                    {
                                        vars[ins[2].Split('=')[0]] = int.Parse(ins[2].Split('=')[1]);
                                    }
                                    break;
                                case "SETVAR":
                                    vars[ins[2].Split('=')[0]] += ins[2].Contains("+") ? 1 : -1;
                                    break;
                                case "GOTO":
                                case "GOTOREL":
                                    int step = int.Parse(ins[2].Split(' ')[0]);
                                    if (!ins[2].Contains("IF") || Check(ins[2].Split(' ')[2], vars))
                                    {
                                        item.Steps[k].IsHighlight = false;
                                        if (ins[1] == "GOTO")
                                        {
                                            k = step - 2;
                                        }
                                        else
                                        {
                                            k += step - 1;
                                        }
                                    }
                                    break;
                                case "CALLLIB":
                                    RecordMessage(string.Format(" {0} 调用动态库。", d.Config.Name));
                                    if (item.Template.Name.StartsWith("1."))
                                    {
                                        string recv = string.Format("{0}\\{1}.20O", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.TrackSatelliteNumberByRINEX(recv, item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("2."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.AcquisitionSensitivityByNMEA(recv, item.Template.Location, vars["power"], item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("3."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.ReacquisitionSensitivityByNMEA(recv, item.Template.Location, vars["power"], item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("4."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.ReacquisitionSensitivityByNMEA(recv, item.Template.Location, vars["power"], item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("5."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        string time = string.Format("{0}\\{1}.bin", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.ColdStartTimeByNMEA(recv, item.Template.Location, time, (double)start / 1000,item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("6."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        string time = string.Format("{0}\\{1}.bin", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.WarmStartTimeByNMEA(recv, item.Template.Location, time, (double)start / 1000, item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("7."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        string time = string.Format("{0}\\{1}.bin", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.HotStartTimeByNMEA(recv, item.Template.Location, time, (double)start / 1000, item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("8."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        string time = string.Format("{0}\\{1}.bin", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.SignalReacquisitionTimeByNMEA(recv, item.Template.Location, time, (double)start / 1000, item.Signals[j].Value);
                                    }
                                    else if (item.Template.Name.StartsWith("9."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        string time = string.Format("{0}\\{1}.bin", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.RtkInitializeTimeByNMEA(recv, item.Template.Location, time, (double)start / 1000, item.Signals[j].Value);
                                        
                                    }
                                    else if (item.Template.Name.StartsWith("10."))
                                    {
                                        string recv = string.Format("{0}\\{1}.20O", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.InternalNoiseLevelByRINEX(recv, item.Template.Measure, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("11."))
                                    {
                                        string recv = string.Format("{0}\\{1}.20O", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.PseudorangeAccuracyByRINEX(recv, item.Template.Measure, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("12."))
                                    {
                                        string recv = string.Format("{0}\\{1}.20O", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.CarrierPhaseAccuracyByRINEX(recv, item.Template.Measure, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("13."))
                                    {
                                        string recv = string.Format("{0}\\{1}.20O", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.DopplerAccuracyByRINEX(recv, item.Template.Measure, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("14."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.StaticSinglePointByNMEA(recv, item.Template.Location, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("15."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.DynamicSinglePointByNMEA(recv, item.Template.Location, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("16."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.StaticBaselineAccuracyByNMEA(recv, item.Template.Location, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("17."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.RtkAccuracyByNMEA(recv, item.Template.Location, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("18."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        if (item.Signals[j].Result == null)
                                        {
                                            item.Signals[j].Result = new double[3];
                                        }
                                        int sceneIdx = (int)vars["i"];
                                        item.Signals[j].Result[sceneIdx - 1] = SolveMethod.SpeedAccuracyByNMEA(recv, item.Template.Location, item.Signals[j], itemPath, sceneIdx);
                                    }
                                    else if (item.Template.Name.StartsWith("19."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.InteroperatePerformanceByNMEA(recv, item.Template.Location, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("20."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.DynamicPerformanceByNMEA(recv, item.Template.Location, item.Signals[j], itemPath);
                                    }
                                    else if (item.Template.Name.StartsWith("21."))
                                    {
                                        string recv = string.Format("{0}\\{1}.txt", itemPath, item.Signals[j].Name);
                                        item.Signals[j].Result = SolveMethod.DataUpdatingRateByNMEA(recv, item.Template.Location, item.Signals[j].Value);
                                    }
                                    if (item.Signals[j].Result.Length < 5)
                                    {
                                        vars["LIBRES"] = item.Signals[j].Result[0];
                                        RecordMessage(string.Format("动态库计算结果：{0}", item.Signals[j].Result[0]));
                                    } 
                                    else
                                    {
                                        vars["LIBRES"] = item.Signals[j].Result[4];
                                        RecordMessage(string.Format("动态库计算结果：{0}", item.Signals[j].Result[4]));
                                    }
                                    break;
                                case "LIBPARA":
                                    break;
                            }
                        }
                        if (ins[0] == "RCVDEV")
                        {
                            switch (ins[1])
                            {
                                // TODO 处理端口异常，图片加载后关闭项目释放
                                case "OPEN":
                                    d.CreatePort(int.Parse(ins[2]));
                                    while (!d.Port.IsOpen)
                                    {
                                        try
                                        {
                                            d.Port.Open();
                                            d.RecvThread.Start();
                                        }
                                        catch (UnauthorizedAccessException e)
                                        {
                                            ShowConfirmMessage("请关闭占用" + d.Config.Port + "的程序");
                                        }
                                    }
                                    RecordMessage(string.Format("打开 {0} 串口。", d.Config.Name));
                                    break;
                                case "CLOSE":
                                    d.Port.Close();
                                    d.CanStop = true;
                                    RecordMessage(string.Format("关闭 {0} 串口。", d.Config.Name));
                                    break;
                                case "SET":
                                    if (d.Port.IsOpen)
                                    {
                                        d.Port.Close();
                                        d.CreatePort(int.Parse(ins[2].Split(' ')[1]));
                                        d.Port.Open();
                                    }
                                    else
                                    {
                                        d.CreatePort(int.Parse(ins[2].Split(' ')[1]));
                                    }
                                    RecordMessage(string.Format("设置 {0} 波特率为{1}", d.Config.Name, ins[2].Split(' ')[1]));
                                    break;
                                case "MSG":
                                    string msg = ins[2].Replace("'", "");
                                    if (!msg.Contains("gga"))
                                    {
                                        d.Port.WriteLine("log gpgga ontime 1");
                                        Thread.Sleep(1000);
                                    }
                                    if (msg.Contains("$"))
                                    {
                                        msg = Replace(msg, vars, arrays);
                                    }
                                    d.Port.WriteLine(msg);
                                    break;
                            }
                        }
                        if (ins[0] == "POWDEV")
                        {
                            switch (ins[1])
                            {
                                case "OPEN":
                                    RecordMessage(string.Format("打开程控电源串口。"));
                                    break;
                                case "CLOSE":
                                    RecordMessage(string.Format("关闭程控电源串口。"));
                                    break;
                                case "SET":
                                    RecordMessage(string.Format("设置程控电源波特率为{0}", ins[2]));
                                    break;
                                case "MSG":
                                    //string msg = ins[2].Replace("'", "");
                                    //if (msg.Contains("$"))
                                    //{
                                    //    msg = Replace(msg, vars, arrays);
                                    //}
                                    //d.Port.WriteLine(msg);
                                    break;
                            }
                        }
                        if (ins[0] == "TIMEDEV")
                        {
                            switch (ins[1])
                            {
                                case "OPEN":
                                    RecordMessage(string.Format("打开时间间隔计数器串口。"));
                                    break;
                                case "CLOSE":
                                    RecordMessage(string.Format("关闭时间间隔计数器串口。"));
                                    break;
                                case "SET":
                                    RecordMessage(string.Format("设置时间间隔计数器波特率为{0}", ins[2]));
                                    break;
                                case "MSG":
                                    //string msg = ins[2].Replace("'", "");
                                    //if (msg.Contains("$"))
                                    //{
                                    //    msg = Replace(msg, vars, arrays);
                                    //}
                                    //d.Port.WriteLine(msg);
                                    break;
                            }
                        }
                        if (ins[1] != "WATI" || ins[1] != "STOPREC")
                        {
                            Thread.Sleep(200);
                            item.Signals[j].Ticks += 200;
                            item.Ticks += 200;
                            DataModel.Task.Ticks += 200;
                        }
                        //InsListView.ScrollToBottom();
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => InsListView.ScrollToBottom()));
                    }
                    item.Steps[item.Steps.Count - 1].IsHighlight = false;
                    item.Signals[j].Progress = 100;
                }
                item.Progress = 100;
                string header = "";
                string title = "";
                string label = "";
                switch (idx)
                {
                    case 1:
                        header = "通道数";
                        break;
                    case 2:
                    case 3:
                    case 4:
                        header = "灵敏度";
                        label = "dBm";
                        break;
                    case 5:
                        header = "冷启动首次定位时间";
                        title = "冷启动首次定位时间";
                        label = "s";
                        break;
                    case 6:
                        header = "温启动首次定位时间";
                        title = "温启动首次定位时间";
                        label = "s";
                        break;
                    case 7:
                        header = "热启动首次定位时间";
                        title = "热启动首次定位时间";
                        label = "s";
                        break;
                    case 8:
                        header = "重捕获时间";
                        title = "重捕获时间";
                        label = "s";
                        break;
                    case 9:
                        header = "RTK初始化时间";
                        title = "RTK初始化时间";
                        label = "s";
                        break;
                    case 10:
                        header = "内部噪声水平";
                        title = "内部噪声水平";
                        label = "m";
                        break;
                    case 11:
                        header = "伪距测量精度";
                        title = "伪距测量误差";
                        label = "m";
                        break;
                    case 12:
                        header = "载波相位测量精度";
                        title = "载波相位测量误差";
                        label = "m";
                        break;
                    case 13:
                        header = "多普勒测量精度";
                        title = "多普勒测量误差";
                        label = "Hz";
                        break;
                    case 18:
                        header = "测速精度";
                        title = "测速误差";
                        label = "m/s";
                        break;
                }
                if (idx < 14)
                {
                    Add(item.Result, new ThreeColumns
                    {
                        Col1 = "频点",
                        Col2 = header,
                        Col3 = "是否达标"
                    });
                    for (int k = 0; k < item.Signals.Count; k++)
                    {
                        Freq f = item.Signals[k];
                        Add(item.Result, new ThreeColumns
                        {
                            Col1 = f.Name,
                            Col2 = f.Result[0].ToString("G3") + label,
                            Col3 = Check(f.Result[0], item.Template.Target) ? "是" : "否"
                        });
                    }
                }
                else if (idx == 18)
                {
                    Add(item.Result, new FiveColumns
                    {
                        Col1 = "频点",
                        Col2 = "第1场景",
                        Col3 = "第2场景",
                        Col4 = "第3场景",
                        Col5 = "是否达标"
                    });
                    for (int k = 0; k < item.Signals.Count; k++)
                    {
                        Freq f = item.Signals[k];
                        Add(item.Result, new FiveColumns
                        {
                            Col1 = f.Name,
                            Col2 = f.Result[0].ToString("G3") + label,
                            Col3 = f.Result[1].ToString("G3") + label,
                            Col4 = f.Result[2].ToString("G3") + label,
                            Col5 = (Check(f.Result[0], item.Template.Target) 
                                && Check(f.Result[1], item.Template.Target) 
                                && Check(f.Result[2], item.Template.Target)) ? "是" : "否"
                        }); ;
                    }
                }
                else if (idx == 21)
                {
                    Add(item.Result, new FourColumns
                    {
                        Col1 = "频点",
                        Col2 = "数据更新率",
                        Col3 = "三维定位误差",
                        Col4 = "是否达标"
                    });
                    for (int k = 0; k < item.Signals.Count; k++)
                    {
                        Freq f = item.Signals[k];
                        Add(item.Result, new FourColumns
                        {
                            Col1 = f.Name,
                            Col2 = f.Result[0].ToString("G3") + "Hz",
                            Col3 = f.Result[1].ToString("G3") + "m",
                            Col4 = Check(f.Result[0], item.Template.Target) ? "是" : "否"
                        });
                    }
                }
                else
                {
                    Add(item.Result, new SevenColumns
                    {
                        Col1 = "频点",
                        Col2 = "东向",
                        Col3 = "北向",
                        Col4 = "垂直",
                        Col5 = "水平",
                        Col6 = "三维",
                        Col7 = "是否达标",
                    });
                    for (int k = 0; k < item.Signals.Count; k++)
                    {
                        Freq f = item.Signals[k];
                        Add(item.Result, new SevenColumns
                        {
                            Col1 = f.Name,
                            Col2 = f.Result[0].ToString("G3") + "m",
                            Col3 = f.Result[1].ToString("G3") + "m",
                            Col4 = f.Result[2].ToString("G3") + "m",
                            Col5 = f.Result[3].ToString("G3") + "m",
                            Col6 = f.Result[4].ToString("G3") + "m",
                            Col7 = Check(f.Result[0], item.Template.Target) ? "是" : "否"
                        });
                    }
                }
                switch (idx)
                {
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        using (StreamWriter writer = new StreamWriter(itemPath + "\\" + "dat.txt"))
                        {
                            for (int k = 0; k < item.Signals.Count; k++)
                            {
                                Freq f = item.Signals[k];
                                writer.WriteLine(f.Name.Split('_')[1] + " " + f.Result[0]);
                            }
                        }
                        Add(item.Images, TestItem.WriteHistogramPlotScript(itemPath, "dat", title, "信号类型", label));
                        break;
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                        Add(item.Images, TestItem.WriteLinePlotScript(item.Signals, itemPath, "dat", title, label));
                        break;
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 19:
                    case 20:
                        Add(item.Images, TestItem.WriteLinePlotScript(item.Signals, itemPath, "E", "东向定位误差", "m"));
                        Add(item.Images, TestItem.WriteLinePlotScript(item.Signals, itemPath, "N", "北向定位误差", "m"));
                        Add(item.Images, TestItem.WriteLinePlotScript(item.Signals, itemPath, "U", "垂直定位误差", "m"));
                        Add(item.Images, TestItem.WriteLinePlotScript(item.Signals, itemPath, "2D", "水平定位误差", "m"));
                        Add(item.Images, TestItem.WriteLinePlotScript(item.Signals, itemPath, "3D", "三维定位误差", "m"));
                        Add(item.Images, TestItem.WriteCountLinePlotScript(item.Signals, itemPath, "NS", "定位卫星数量", "个"));
                        break;
                    case 18:
                        for (int k = 0; k < item.Signals.Count; k++)
                        {
                            Add(item.Images, TestItem.WriteLinePlotScript(item.Signals[k].Name, itemPath));
                        }
                        break;
                }
            }
            DataModel.Task.Progress = 100;
        }

        private void Add(ObservableCollection<object> collection, object data)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => collection.Add(data)));
        }

        private void Add(ObservableCollection<string> collection, string data)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => collection.Add(data)));
        }

        private bool Check(double value, string target)
        {
            target = Regex.Replace(target, "[a-z]", "", RegexOptions.IgnoreCase).Replace("/", "");
            if (target.StartsWith(">="))
            {
                return value >= double.Parse(target.Substring(2));
            }
            else if (target.StartsWith("<="))
            {
                return value <= double.Parse(target.Substring(2));
            }
            else if (target.StartsWith(">"))
            {
                return value > double.Parse(target.Substring(1));
            }
            else
            {
                return value < double.Parse(target.Substring(1));
            }
        }

        private bool Check(string s, Dictionary<string, double> vars)
        {
            string[] ins = s.Split(new char[] { ' ', '>', '<', '=' });
            string v = ins[0];
            string t = ins[ins.Length - 1];
            string op = s.Replace(v, "").Replace(t, "");
            if (v.Contains("$"))
            {
                v = v.Substring(2, v.Length - 3);
            }
            switch (op)
            {
                case ">":
                    return vars[v] > double.Parse(t);
                case ">=":
                    return vars[v] >= double.Parse(t);
                case "<":
                    return vars[v] < double.Parse(t);
                case "<=":
                    return vars[v] <= double.Parse(t);
                case "==":
                    return vars[v] == double.Parse(t);
            }
            return false;
        }

        private string Replace(string s, Dictionary<string, double> vars, Dictionary<string, int[]> arrays)
        {
            int sIdx = s.IndexOf('$');
            int eIdx = s.IndexOf('}');
            string t = s.Substring(sIdx, eIdx - sIdx + 1);
            string exp = t.Substring(2, t.Length - 3);
            if (exp.Contains("["))
            {
                int mIdx = exp.IndexOf('[');
                string i = exp.Substring(mIdx + 1, exp.Length - mIdx - 2);
                string name = exp.Substring(0, mIdx);
                return s.Replace(t, arrays[name][(int)vars[i]].ToString());
            }
            else
            {
                return s.Replace(t, vars[exp].ToString());
            }
        }

        private void ShowConfirmMessage(string msg)
        {
            lock (Lock)
            {
                if (!ConfirmDialog.Model.IsShow)
                {
                    ConfirmDialog.Model.IsShow = true;
                    ConfirmDialog.Model.Message = msg;
                    Application.Current.Dispatcher.BeginInvoke(new Action(ShowDialog));
                }
                else
                {
                    ConfirmDialog.Model.Message += "\r\n" + msg;
                }
            }
        }

        private new async void ShowDialog()
        {
            await DialogHost.Show(ConfirmDialog);
        }

        private void RecordMessage(string m)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => DataModel.Messages.Insert(0, new Message(m))));
        }

        private void IncreaseTime(string name, int time)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            { 
                for (int i = 0; i < DataModel.Messages.Count; i++)
                {
                    if (DataModel.Messages[i].Content.Contains(name))
                    {
                        DataModel.Messages[i].Content = DataModel.Messages[i].Content.Replace((time - 1) + "/", time + "/");
                        break;
                    }
                }
            }));
        }

        private void RemoveMessage()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => DataModel.Messages.RemoveAt(0)));
        }

        private void RemoveAndRecordMessage(string m)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                DataModel.Messages.RemoveAt(0);
                DataModel.Messages.Insert(0, new Message(m));
            }));
        }
    }
}
