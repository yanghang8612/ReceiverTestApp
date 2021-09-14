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
    // 确认信息对话框交互逻辑，传入参数为确认消息内容，单击界面的确认关闭该对话框
    public partial class ConfirmDialog : UserControl
    {
        public ConfirmDialog(string name)
        {
            InitializeComponent();
            this.DataContext = name;
        }

        
    }
}
