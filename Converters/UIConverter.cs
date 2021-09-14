using MahApps.Metro.Controls;
using ReceiverTestApp.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ReceiverTestApp.Converters
{
    // 测试项目、频点、设备是否有某一项被选中转bool值转换器
    class ObjListToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<TestItemTemplate> templates)
            {
                foreach (var item in templates)
                {
                    if (item.IsSelected) return true;
                }
            }
            if (value is ObservableCollection<Signal> signals)
            {
                foreach (var item in signals)
                {
                    if (item.IsSelected) return true;
                }
            }
            if (value is ObservableCollection<Device> devices)
            {
                foreach (var item in devices)
                {
                    if (item.IsSelected) return true;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // 多对象列表转bool值转换器
    class MultiObjToEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values) 
            {
                if (value is null || value is string && string.IsNullOrWhiteSpace((value ?? "").ToString()))
                {
                    return false;
                }
                if (value is bool)
                {
                    return value;
                }
                if (value is ObservableCollection<Signal> signals)
                {
                    foreach (var s in signals)
                    {
                        if (s.IsSelected) return true;
                    }
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // bool转控件是否可见属性转换器
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // 进度是否满100%转CheckBox是否勾选转换器
    class ItemStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) >= 100 ? "CheckBoxOutline" : "CheckboxBlankOutline";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // 当前测试步骤高亮转条目颜色转换器
    class HighlightToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Green" : "White";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
