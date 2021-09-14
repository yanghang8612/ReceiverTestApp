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
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverTestApp.Domain
{
    public class TestItemTemplate : PropertyNotifyObject
    {
        [JsonIgnore]
        public bool IsSelected
        {
            set 
            { 
                this.SetValue(x => x.IsSelected, value);
                ConfigModel.Instance.SetValue(x => x.TemplateSelected, value);
            }
            get { return this.GetValue(x => x.IsSelected); }
        }

        public string Name
        {
            set { this.SetValue(x => x.Name, value); }
            get { return this.GetValue(x => x.Name); }
        }

        public int Source
        {
            set { this.SetValue(x => x.Source, value); }
            get { return this.GetValue(x => x.Source); }
        }

        public string Location
        {
            set { this.SetValue(x => x.Location, value); }
            get { return this.GetValue(x => x.Location); }
        }

        public string Measure
        {
            set { this.SetValue(x => x.Measure, value); }
            get { return this.GetValue(x => x.Measure); }
        }

        public string Signals
        {
            set { this.SetValue(x => x.Signals, value); }
            get { return this.GetValue(x => x.Signals); }
        }

        public string Target
        {
            set { this.SetValue(x => x.Target, value); }
            get { return this.GetValue(x => x.Target); }
        }

        public int Loop
        {
            set { this.SetValue(x => x.Loop, value); }
            get { return this.GetValue(x => x.Loop); }
        }

        public string Script
        {
            set { this.SetValue(x => x.Script, value); }
            get { return this.GetValue(x => x.Script); }
        }

        public static Dictionary<long, ecef_position_s> Standard = new Dictionary<long, ecef_position_s>();

        public Dictionary<long, ecef_position_s> GetStandard()
        {
            lock (Standard)
            {
                if (Standard.Count == 0)
                {
                    using (StreamReader reader = File.OpenText(Location))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains("GGA"))
                            {
                                ecef_position_s p = SolveMethod.SolveGGA(line);
                                Standard[p.time.time] = p;
                            }
                        }
                    }
                }
                return Standard;
            }
        }
    }

    public class Signal : PropertyNotifyObject
    {
        [JsonIgnore]
        public bool IsSelected
        {
            set 
            { 
                this.SetValue(x => x.IsSelected, value);
                ConfigModel.Instance.SetValue(x => x.SignalSelected, value);
            }
            get { return this.GetValue(x => x.IsSelected); }
        }

        public string Name { set; get; }

        public int Value { set; get; }
    }

    public class Device : PropertyNotifyObject
    {
        [JsonIgnore]
        public bool IsSelected
        {
            set 
            { 
                this.SetValue(x => x.IsSelected, value);
                ConfigModel.Instance.SetValue(x => x.DeviceSelected, value);
            }
            get { return this.GetValue(x => x.IsSelected); }
        }

        public string Name
        {
            set { this.SetValue(x => x.Name, value); }
            get { return this.GetValue(x => x.Name); }
        }

        public string Port
        {
            set { this.SetValue(x => x.Port, value); }
            get { return this.GetValue(x => x.Port); }
        }

        public int BaudRate
        {
            set { this.SetValue(x => x.BaudRate, value); }
            get { return this.GetValue(x => x.BaudRate); }
        }
    }

    public class ConfigModel : PropertyNotifyObject
    {
        public static ConfigModel Instance = Init();

        private static ConfigModel Init()
        {
            using (StreamReader reader = File.OpenText(@"config.json"))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ConfigModel>(json);
            }
        }

        public void WriteBack()
        {
            using (StreamWriter writer = new StreamWriter(File.Open(@"config.json", FileMode.Create)))
            {
                string json = JsonConvert.SerializeObject(Instance);
                writer.Write(json);
            }
        }

        public bool TemplateSelected
        {
            set { this.SetValue(x => x.TemplateSelected, value); }
            get 
            {
                foreach (var t in Templates)
                {
                    if (t.IsSelected) return true;
                }
                return false;
            }
        }

        public bool SignalSelected
        {
            set { this.SetValue(x => x.SignalSelected, value); }
            get
            {
                foreach (var t in Signals)
                {
                    if (t.IsSelected) return true;
                }
                return false;
            }
        }

        public bool DeviceSelected
        {
            set { this.SetValue(x => x.DeviceSelected, value); }
            get
            {
                foreach (var t in Devices)
                {
                    if (t.IsSelected) return true;
                }
                return false;
            }
        }

        public ObservableCollection<TestItemTemplate> Templates { set; get; }

        public ObservableCollection<Signal> Signals { set; get; }

        public ObservableCollection<Device> Devices { set; get; }
    }
}
