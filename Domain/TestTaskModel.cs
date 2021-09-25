using ControlzEx.Standard;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReceiverTestApp.Dialog;
using ReceiverTestApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ReceiverTestApp.Domain
{
    public class Step : PropertyNotifyObject
    {
        public bool IsHighlight
        {
            set { this.SetValue(x => x.IsHighlight, value); }
            get { return this.GetValue(x => x.IsHighlight); }
        }

        public string Ins
        {
            set { this.SetValue(x => x.Ins, value); }
            get { return this.GetValue(x => x.Ins); }
        }

        public Step(string ins)
        {
            this.IsHighlight = false;
            this.Ins = ins;
        }
    }

    public class Message : PropertyNotifyObject
    {
        public string Time
        {
            set { this.SetValue(x => x.Time, value); }
            get { return this.GetValue(x => x.Time); }
        }

        public string Content
        {
            set { this.SetValue(x => x.Content, value); }
            get { return this.GetValue(x => x.Content); }
        }

        public Message(string content)
        {
            Time = DateTime.Now.ToString("HH:mm:ss");
            Content = content;
        }
    }

    public class Freq : PropertyNotifyObject
    {
        public string Name { get; }

        public int Value { get; }

        public int Progress
        {
            set { this.SetValue(x => x.Progress, value); }
            get { return this.GetValue(x => x.Progress); }
        }

        private int _ticks;
        public int Ticks
        {
            set
            {
                _ticks = value;
                if (Total != 0)
                {
                    Progress = (int)((double)_ticks / Total * 100);
                }
            }
            get { return _ticks; }
        }

        public int Total { set; get; }

        public double[] Result { set; get; }

        public Freq(Signal s, int t)
        {
            Name = s.Name;
            Value = s.Value;
            Total = t;
        }
    }

    public class ThreeColumns : PropertyNotifyObject
    {
        public string Col1
        {
            set { this.SetValue(x => x.Col1, value); }
            get { return this.GetValue(x => x.Col1); }
        }

        public string Col2
        {
            set { this.SetValue(x => x.Col2, value); }
            get { return this.GetValue(x => x.Col2); }
        }

        public string Col3
        {
            set { this.SetValue(x => x.Col3, value); }
            get { return this.GetValue(x => x.Col3); }
        }
    }

    public class FourColumns : PropertyNotifyObject
    {
        public string Col1
        {
            set { this.SetValue(x => x.Col1, value); }
            get { return this.GetValue(x => x.Col1); }
        }

        public string Col2
        {
            set { this.SetValue(x => x.Col2, value); }
            get { return this.GetValue(x => x.Col2); }
        }

        public string Col3
        {
            set { this.SetValue(x => x.Col3, value); }
            get { return this.GetValue(x => x.Col3); }
        }

        public string Col4
        {
            set { this.SetValue(x => x.Col4, value); }
            get { return this.GetValue(x => x.Col4); }
        }
    }

    public class FiveColumns : PropertyNotifyObject
    {
        public string Col1
        {
            set { this.SetValue(x => x.Col1, value); }
            get { return this.GetValue(x => x.Col1); }
        }

        public string Col2
        {
            set { this.SetValue(x => x.Col2, value); }
            get { return this.GetValue(x => x.Col2); }
        }

        public string Col3
        {
            set { this.SetValue(x => x.Col3, value); }
            get { return this.GetValue(x => x.Col3); }
        }

        public string Col4
        {
            set { this.SetValue(x => x.Col4, value); }
            get { return this.GetValue(x => x.Col4); }
        }

        public string Col5
        {
            set { this.SetValue(x => x.Col5, value); }
            get { return this.GetValue(x => x.Col5); }
        }
    }

    public class SevenColumns : PropertyNotifyObject
    {
        public string Col1
        {
            set { this.SetValue(x => x.Col1, value); }
            get { return this.GetValue(x => x.Col1); }
        }

        public string Col2
        {
            set { this.SetValue(x => x.Col2, value); }
            get { return this.GetValue(x => x.Col2); }
        }

        public string Col3
        {
            set { this.SetValue(x => x.Col3, value); }
            get { return this.GetValue(x => x.Col3); }
        }

        public string Col4
        {
            set { this.SetValue(x => x.Col4, value); }
            get { return this.GetValue(x => x.Col4); }
        }

        public string Col5
        {
            set { this.SetValue(x => x.Col5, value); }
            get { return this.GetValue(x => x.Col5); }
        }

        public string Col6
        {
            set { this.SetValue(x => x.Col6, value); }
            get { return this.GetValue(x => x.Col6); }
        }

        public string Col7
        {
            set { this.SetValue(x => x.Col7, value); }
            get { return this.GetValue(x => x.Col7); }
        }
    }

    public class TestItem : PropertyNotifyObject
    {
        public TestItemTemplate Template { set; get; }

        public int StepIdx
        {
            set { this.SetValue(x => x.StepIdx, value); }
            get { return this.GetValue(x => x.StepIdx); }
        }

        public ObservableCollection<Step> Steps
        {
            set { this.SetValue(x => x.Steps, value); }
            get { return this.GetValue(x => x.Steps); }
        }

        public int SignalIdx
        {
            set { this.SetValue(x => x.SignalIdx, value); }
            get { return this.GetValue(x => x.SignalIdx); }
        }

        public ObservableCollection<Freq> Signals { set; get; }

        public int Progress
        {
            set { this.SetValue(x => x.Progress, value); }
            get { return this.GetValue(x => x.Progress); }
        }

        private int _ticks;
        public int Ticks
        {
            set
            {
                _ticks = value;
                if (Total != 0)
                {
                    Progress = (int)((double)_ticks / Total * 100);
                }
            }
            get { return _ticks; }
        }

        public int Total { set; get; }

        public ObservableCollection<string> Images { set; get; }

        public ObservableCollection<object> Result { set; get; }

        public static TestItem FromTemplate(TestItemTemplate t)
        {
            TestItem item = new TestItem
            {
                Template = t,
                Steps = new ObservableCollection<Step>(),
                Signals = new ObservableCollection<Freq>(),
                Images = new ObservableCollection<string>(),
                Result = new ObservableCollection<object>()
            };

            foreach (var ins in t.Script.Split(';'))
            {
                item.Steps.Add(new Step(ins));
                if (ins.Contains("WAIT"))
                {
                    item.Total += int.Parse(ins.Split(' ')[2]) * 1000;
                }
                else
                {
                    item.Total += 200;
                }
            }

            foreach (var s in t.Signals.Split(','))
            {
                item.Signals.Add(new Freq(ConfigModel.Instance.Signals[int.Parse(s)], item.Total * item.Template.Loop));
            }

            item.Total *= item.Signals.Count * item.Template.Loop;
            return item;
        }

        public static string WriteLinePlotScript(ObservableCollection<Freq> signals, string path, string suffix, string title, string ylabel)
        {
            using (StreamWriter writer = new StreamWriter(string.Format("{0}\\{1}.plt", path, suffix)))
            {
                writer.WriteLine("reset");
                writer.WriteLine("set terminal png font \"Microsoft YaHei, 12\"");
                writer.WriteLine(string.Format("set output \"{0}.png\"", suffix));
                writer.WriteLine("set encoding utf8");
                writer.WriteLine(string.Format("set title \"{0}\"", title));
                writer.WriteLine(string.Format("set ylabel \" ({0}) \"", ylabel));
                writer.WriteLine("set timefmt \"%y/%m/%d\\t%H:%M:%S\"");
                writer.WriteLine("set xdata time");
                writer.WriteLine("set format x \"%m/%d\\n%H:%M\"");
                writer.WriteLine("set grid");
                string line = "plot ";
                for (int i = 0; i < signals.Count; i++)
                {
                    line += string.Format("'{0}.{1}' using 1:3 with lines title '{2}',", signals[i].Name, suffix, signals[i].Name.Replace('_', ' '));
                }
                line = line.Remove(line.Length - 1);
                writer.WriteLine(line);
            }
            ExecuteScript(path, suffix + ".plt");
            return string.Format("{0}\\{1}.png", path, suffix);
        }

        public static string WriteLinePlotScript(string signal, string path)
        {
            using (StreamWriter writer = new StreamWriter(string.Format("{0}\\{1}.plt", path, signal)))
            {
                writer.WriteLine("reset");
                writer.WriteLine("set terminal png font \"Microsoft YaHei, 12\"");
                writer.WriteLine(string.Format("set output \"{0}.png\"", signal));
                writer.WriteLine("set encoding utf8");
                writer.WriteLine(string.Format("set title \"{0}\"", "频点" + signal.Replace('_', ' ') + "测速误差"));
                writer.WriteLine(string.Format("set ylabel \" ({0}) \"", "m/s"));
                writer.WriteLine("set timefmt \"%y/%m/%d\\t%H:%M:%S\"");
                writer.WriteLine("set xdata time");
                writer.WriteLine("set format x \"%m/%d\\n%H:%M\"");
                writer.WriteLine("set grid");
                string line = "plot ";
                for (int i = 1; i < 4; i++)
                {
                    line += string.Format("'{0}.{1}' using 1:3 with lines title '第{1}场景',", signal, i);
                }
                line = line.Remove(line.Length - 1);
                writer.WriteLine(line);
            }
            ExecuteScript(path, signal + ".plt");
            return string.Format("{0}\\{1}.png", path, signal);
        }

        public static string WriteCountLinePlotScript(ObservableCollection<Freq> signals, string path, string suffix, string title, string ylabel)
        {
            using (StreamWriter writer = new StreamWriter(string.Format("{0}\\{1}.plt", path, suffix)))
            {
                writer.WriteLine("reset");
                writer.WriteLine("set terminal png font \"Microsoft YaHei, 12\"");
                writer.WriteLine(string.Format("set output \"{0}.png\"", suffix));
                writer.WriteLine("set encoding utf8");
                writer.WriteLine(string.Format("set title \"{0}\"", title));
                writer.WriteLine(string.Format("set ylabel \" ({0}) \"", ylabel));
                writer.WriteLine("set timefmt \"%y/%m/%d\\t%H:%M:%S\"");
                writer.WriteLine("set xdata time");
                writer.WriteLine("set ytics 1");
                writer.WriteLine("set format x \"%m/%d\\n%H:%M\"");
                writer.WriteLine("set grid");
                string line = "plot ";
                for (int i = 0; i < signals.Count; i++)
                {
                    line += string.Format("'{0}.{1}' using 1:3 with lines title '{2}',", signals[i].Name, suffix, signals[i].Name.Replace('_', ' '));
                }
                line = line.Remove(line.Length - 1);
                writer.WriteLine(line);
            }
            ExecuteScript(path, suffix + ".plt");
            return string.Format("{0}\\{1}.png", path, suffix);
        }

        public static string WriteHistogramPlotScript(string path, string suffix, string title, string xlabel, string ylabel)
        {
            using (StreamWriter writer = new StreamWriter(string.Format("{0}\\{1}.plt", path, suffix)))
            {
                writer.WriteLine("reset");
                writer.WriteLine("set terminal png font \"Microsoft YaHei, 12\"");
                writer.WriteLine(string.Format("set output \"{0}.png\"", suffix));
                writer.WriteLine("set encoding utf8");
                writer.WriteLine(string.Format("set title \"{0}\"", title));
                writer.WriteLine(string.Format("set xlabel \"{0}\"", xlabel));
                writer.WriteLine(string.Format("set ylabel \" ({0}) \"", ylabel));
                writer.WriteLine("set yrange [0:]");
                writer.WriteLine("set style data histogram");
                writer.WriteLine("set style histogram cluster gap 1");
                writer.WriteLine("set style fill solid border -1");
                writer.WriteLine("set grid");
                writer.WriteLine("unset key");
                writer.WriteLine(string.Format("plot '{0}.txt' using 2:xtic(1) with histogram", suffix));
            }
            ExecuteScript(path, suffix + ".plt");
            return string.Format("{0}\\{1}.png", path, suffix);
        }

        private static void ExecuteScript(string path, string script)
        {
            Process pr = new Process();
            pr.StartInfo.WorkingDirectory = path;
            pr.StartInfo.FileName = path + "\\" + script;
            pr.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pr.Start();
            pr.WaitForExit();
        }
    }

    public class Location : PropertyNotifyObject
    {
        public string Time
        {
            set { this.SetValue(x => x.Time, value); }
            get { return this.GetValue(x => x.Time); }
        }

        public double HDOP
        {
            set { this.SetValue(x => x.HDOP, value); }
            get { return this.GetValue(x => x.HDOP); }
        }

        public int Ns
        {
            set { this.SetValue(x => x.Ns, value); }
            get { return this.GetValue(x => x.Ns); }
        }

        public string Stat
        {
            set { this.SetValue(x => x.Stat, value); }
            get { return this.GetValue(x => x.Stat); }
        }

        public double X
        {
            set { this.SetValue(x => x.X, value); }
            get { return this.GetValue(x => x.X); }
        }

        public double Y
        {
            set { this.SetValue(x => x.Y, value); }
            get { return this.GetValue(x => x.Y); }
        }

        public double Z
        {
            set { this.SetValue(x => x.Z, value); }
            get { return this.GetValue(x => x.Z); }
        }

        public string Lat
        {
            set { this.SetValue(x => x.Lat, value); }
            get { return this.GetValue(x => x.Lat); }
        }

        public string Lon
        {
            set { this.SetValue(x => x.Lon, value); }
            get { return this.GetValue(x => x.Lon); }
        }

        public string Height
        {
            set { this.SetValue(x => x.Height, value); }
            get { return this.GetValue(x => x.Height); }
        }

        public void Set(ecef_position_s p)
        {
            Time = new DateTime(1970, 1, 1).AddSeconds(p.time.time).ToString("T");
            HDOP = p.hdop;
            Ns = p.ns;
            X = p.x;
            Y = p.y;
            Z = p.z;
            Lat = p.lat.ToString("f10");
            Lon = p.lon.ToString("f10");
            Height = p.height.ToString("f10");
            switch (p.stat)
            {
                case 0:
                    Stat = "定位不可用";
                    break;
                case 1:
                    Stat = "单点定位";
                    break;
                case 2:
                    Stat = "差分定位";
                    break;
                case 4:
                    Stat = "RTK固定解";
                    break;
                case 5:
                    Stat = "RTK浮点解";
                    break;
            }
        }
    }

    public class RunningDevice : PropertyNotifyObject
    {
        private class Point
        {
            public DateTime Time { get; set; }

            public double Value { get; set; }
        }

        public Device Config { set; get; }

        public SerialPort Port { set; get; }

        public Thread RecvThread { set; get; }

        public ObservableCollection<string> ReturnIns { get; }

        public Location Loc { set; get; }

        public SeriesCollection EastSeries { get; set; }

        public SeriesCollection NorthSeries { get; set; }

        public SeriesCollection HeiSeries { get; set; }

        public SeriesCollection SatSeries { get; set; }

        public Func<double, string> Formatter
        {
            set { this.SetValue(x => x.Formatter, value); }
            get { return this.GetValue(x => x.Formatter); }
        }

        public int Total { set; get; }

        public int ItemIdx
        {
            set 
            { 
                this.SetValue(x => x.ItemIdx, value); 
                if (Items != null && Items.Count != 0)
                {
                    CurItem = Items[ItemIdx];
                }
            }
            get { return this.GetValue(x => x.ItemIdx); }
        }

        public TestItem CurItem
        {
            set { this.SetValue(x => x.CurItem, value); }
            get { return CurItem = Items[ItemIdx]; }
        }

        public ObservableCollection<TestItem> Items { get; }

        public ObservableCollection<Step> Steps 
        {
            set { this.SetValue(x => x.Steps, value); }
            get { return this.GetValue(x => x.Steps); }
        }

        public Dictionary<long, ecef_position_s> Standard { set; get; }

        public BinaryWriter LogFile { set; get; }

        public TimeFileRecorder Recorder { set; get; }

        public bool IsLogging { set; get; }

        public bool CanStop { set; get; }

        public RunningDevice(Device d, List<TestItemTemplate> templates)
        {
            Config = d;
            ReturnIns = new ObservableCollection<string>();
            Loc = new Location();
            EastSeries = new SeriesCollection { };
            NorthSeries = new SeriesCollection { };
            HeiSeries = new SeriesCollection { };
            SatSeries = new SeriesCollection { };
            Formatter = Covert;
            ItemIdx = 0;
            Items = new ObservableCollection<TestItem>();
            foreach (var t in templates)
            {
                Items.Add(TestItem.FromTemplate(t));
            }
            Steps = Items[ItemIdx].Steps;
            Total = Items.Count * Items[0].Total;
        }

        public void CreatePort(int rate)
        {
            Port = new SerialPort(Config.Port, rate);
            //Port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
            RecvThread = new Thread(ReadPort);
            CanStop = false;
        }

        private void ReadPort()
        {
            List<int> ret = new List<int>();
            bool foundGGA = false;
            // TODO CPU使用率
            while (!CanStop)
            {
                try
                {
                    if (Port.IsOpen)
                    {
                        int b = Port.ReadByte();
                        ret.Add(b);
                        if (ret.Count == 6)
                        {
                            if (ret[0] == 0x24 && ret[3] == 0x47 
                                && ret[4] == 0x47 && ret[5] == 0x41)
                            {
                                foundGGA = true;
                            }
                            else
                            {
                                ret.RemoveAt(0);
                            }
                        }
                        if (foundGGA)
                        {
                            if (ret[ret.Count - 2] == 0x0D && ret[ret.Count - 1] == 0x0A)
                            {
                                string gga = "";
                                for (int i = 0; i < ret.Count; i++)
                                {
                                    gga += (char)ret[i];
                                }
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    if (ReturnIns.Count > 50)
                                    {
                                        ReturnIns.RemoveAt(0);
                                    }
                                    ReturnIns.Add(gga);
                                }));
                                ecef_position_s rev = SolveMethod.SolveGGA(gga);
                                Loc.Set(rev);
                                if (Standard.ContainsKey(rev.time.time))
                                {
                                    ecef_position_s sim = Standard[rev.time.time];
                                    enu_position_s d = SolveMethod.hcl_ecef2enu(rev, sim);
                                    AddPoint(EastSeries, d.time.time, d.e);
                                    AddPoint(NorthSeries, d.time.time, d.n);
                                    AddPoint(HeiSeries, d.time.time, d.u);
                                    AddPoint(SatSeries, rev.time.time, rev.ns);
                                }
                                ret.Clear();
                                foundGGA = false;
                            }
                        }
                        if (b != -1 && IsLogging)
                        {
                            LogFile.Write((byte)b);
                            LogFile.Flush();
                        }
                    }
                    Thread.Yield();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //string msg = Port.ReadLine() + "\n";
                //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    if (ReturnIns.Count > 50)
                //    {
                //        ReturnIns.RemoveAt(0);
                //    }
                //    ReturnIns.Add(msg);
                //}));
                //if (IsLogging && (!msg.Contains("gga") || CurItem.Template.Script.Contains("gga")))
                //{
                //    LogFile.Write(System.Text.Encoding.UTF8.GetBytes(msg));
                //    LogFile.Flush();
                //}
                //if (msg.Contains("GGA"))
                //{
                //    ecef_position_s rev = SolveMethod.SolveGGA(msg);
                //    Loc.Set(rev);
                //    if (rev.time.time != 0)
                //    {
                //        ecef_position_s sim = Standard[rev.time.time];
                //        enu_position_s d = SolveMethod.hcl_ecef2enu(rev, sim);
                //        AddPoint(EastSeries, d.time.time, d.e);
                //        AddPoint(NorthSeries, d.time.time, d.n);
                //        AddPoint(HeiSeries, d.time.time, d.u);
                //        AddPoint(SatSeries, rev.time.time, rev.ns);
                //    }
                //}
                while (true)
                {
                    int b = Port.ReadByte();
                    if (b == -1) break;
                    if (IsLogging)
                    {
                        LogFile.Write((byte)b);
                        LogFile.Flush();
                    }
                }
            } 
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void AddPoint(SeriesCollection series, long time, double value)
        {
            if (series.Count == 0)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                {
                    var config = Mappers.Xy<Point>()
                                .X(p => (double)p.Time.Ticks / TimeSpan.FromSeconds(10).Ticks)
                                .Y(p => p.Value);
                    LineSeries line = new LineSeries(config)
                    {
                        Title = "",
                        StrokeThickness = 0.5,
                        Fill = Brushes.Transparent,
                        LineSmoothness = 0,
                        PointGeometry = null,
                        Values = new ChartValues<Point>(),
                    };
                    series.Add(line);
                }));
            }
            else
            {
                DateTime dt = new DateTime(1970, 1, 1).AddSeconds(time);
                series[0].Values.Add(new Point()
                {
                    Time = dt,
                    Value = value
                });
                if (series[0].Values.Count > 60)
                {
                    series[0].Values.RemoveAt(0);
                }
            }
        }

        private string Covert(double d)
        {
            if (d > 0) 
                return new DateTime((long)(d * TimeSpan.FromSeconds(10).Ticks)).ToString("t");
            return "";
        }
    }

    public class TestTask : PropertyNotifyObject
	{
        public bool IsSelected
        {
            set { this.SetValue(x => x.IsSelected, value); }
            get { return this.GetValue(x => x.IsSelected); }
        }

        public string Name
        {
            set { this.SetValue(x => x.Name, value); }
            get { return this.GetValue(x => x.Name); }
        }

        public string WorkPath
        {
            set { this.SetValue(x => x.WorkPath, value); }
            get { return this.GetValue(x => x.WorkPath); }
        }

        public int Progress
        {
            set { this.SetValue(x => x.Progress, value); }
            get { return this.GetValue(x => x.Progress); }
        }

        private int _ticks;
        public int Ticks
        {
            set
            {
                _ticks = value;
                if (Total != 0)
                {
                    Progress = (int)((double)_ticks / Total * 100);
                }
            }
            get { return _ticks; }
        }

        public int Total { set; get; }

        public ObservableCollection<TestItemTemplate> Templates { get; }

        public ObservableCollection<Device> Devices { get; }

        public ObservableCollection<RunningDevice> RunningDevices { get; }

        public TestTask(string name, string path, List<TestItemTemplate> templates, List<Device> devices)
		{
            Name = name;
            WorkPath = path;
            Templates = new ObservableCollection<TestItemTemplate>(templates);
            Devices = new ObservableCollection<Device>(devices);
            RunningDevices = new ObservableCollection<RunningDevice>();
            foreach (var device in devices)
            {
                RunningDevices.Add(new RunningDevice(device, templates));
            }
            Total = RunningDevices.Count * RunningDevices[0].Total;
		}
	}

    public class TimeFileRecorder
    {
        private bool IsRuning = false;
        private BinaryWriter TimeFile;
        private string Target;

        public TimeFileRecorder(string file, string target)
        {
            TimeFile = new BinaryWriter(new FileStream(file, FileMode.Create));
            Target = target;
        }

        public void Start()
        {
            IsRuning = true;
            new Thread(Run).Start();
        }

        public void Stop()
        {
            IsRuning = false;
            TimeFile.Flush();
            TimeFile.Close();
        }

        private void Run()
        {
            while (IsRuning)
            {
                TimeFile.Write((double)new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() / 1000);
                TimeFile.Write((int)new FileInfo(Target).Length);
                Thread.Sleep(10);
            }
        }
    }
}
