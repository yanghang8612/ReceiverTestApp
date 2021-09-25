using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReceiverTestApp.Domain
{
    class DupTaskNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            MainWindowModel model = MainWindow.DataModel;
            string name = (value ?? "").ToString();
            foreach (TestTask task in model.Tasks) 
            {
                if (task.Name.Equals(name) && !task.Name.Equals(model.SelectedName))
                {
                    return new ValidationResult(false, "规划名称已存在。");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
