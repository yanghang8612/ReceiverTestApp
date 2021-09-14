using MaterialDesignThemes.Wpf;
using ReceiverTestApp.Domain;
using ReceiverTestApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    // 添加或修改测试项目对话框，有2个构造函数，分别对应添加或修改的操作
    public partial class AddOrModifyTemplate : UserControl
    {
        // UI的DataContext类型，七个字段分别被对应的界面元素Binding
        private class DataModel : PropertyNotifyObject
        {
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

            public ConfigModel Config { get; }

            public DataModel()
            {
                Config = ConfigModel.Instance;
                Loop = 1;
                foreach (var s in Config.Signals)
                {
                    s.IsSelected = false;
                }
            }
        }

        // 保存的要编辑的Template实例
        private readonly TestItemTemplate CurTemplate;

        // 保存的DataContext实例
        private readonly DataModel Model;

        // 对话框会话，便于在处理流程中自主关闭本身
        private DialogSession Session;

        // 新增测试项目对话框的构造函数
        public AddOrModifyTemplate()
        {
            InitializeComponent();
            DataModel data = new DataModel();
            this.DataContext = data;
            this.Model = data;
        }

        // 编辑测试项目对话框的构造函数
        public AddOrModifyTemplate(TestItemTemplate t)
        {
            InitializeComponent();
            DataModel data = new DataModel
            {
                Name = t.Name,
                Source = t.Source,
                Location = t.Location,
                Measure = t.Measure,
                Target = t.Target,
                Loop = t.Loop,
                Script = t.Script
            };
            data.Script = data.Script.Replace(";", "\r\n");
            foreach (var i in Regex.Split(t.Signals, ",", RegexOptions.IgnorePatternWhitespace))
            {
                data.Config.Signals[int.Parse(i)].IsSelected = true;
            }
            this.CurTemplate = t;
            this.Model = data;
            this.DataContext = data;
        }

        public void SetSession(DialogSession s)
        {
            Session = s;
        }

        // 触发保存事件，要写回配置文件，并主动关闭对话框
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string signal = "";
            foreach (var s in Model.Config.Signals)
            {
                if (s.IsSelected) signal += s.Value.ToString() + ",";
            }
            signal = signal.Remove(signal.Length - 1);
            Model.Script = Model.Script.Replace("\r\n", ";");
            if (CurTemplate == null)
            {
                ConfigModel.Instance.Templates.Add(new TestItemTemplate
                {
                    Name = Model.Name,
                    Source = Model.Source,
                    Location = Model.Location,
                    Measure = Model.Measure,
                    Signals = signal,
                    Target = Model.Target,
                    Loop = Model.Loop,
                    Script = Model.Script
                });
            }
            else
            {
                CurTemplate.Name = Model.Name;
                CurTemplate.Source = Model.Source;
                CurTemplate.Location = Model.Location;
                CurTemplate.Measure = Model.Measure;
                CurTemplate.Signals = signal;
                CurTemplate.Target = Model.Target;
                CurTemplate.Loop = Model.Loop;
                CurTemplate.Script = Model.Script;
            }
            // 写回配置文件
            ConfigModel.Instance.WriteBack();
            Session.Close();
        }

        // 选择定位文件，打开文件选择对话框
        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            Model.Location = CommonMethod.ChooseFileDialog();
        }

        // 选择测量文件，打开文件选择对话框
        private void MeasureButton_Click(object sender, RoutedEventArgs e)
        {
            Model.Measure = CommonMethod.ChooseFileDialog();
        }
    }
}
