using ReceiverTestApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverTestApp.Domain
{
    public class NotifyPropertyBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    public static class NotifyPropertyBaseEx
    {
        public static void OnPropertyChanged<T, U>(this T npb, Expression<Func<T, U>> exp) where T : NotifyPropertyBase, new()
        {
            string _PropertyName = CommonMethod.GetPropertyName(exp);
            npb.OnPropertyChanged(_PropertyName);
        }
    }

    public class PropertyNotifyObject : NotifyPropertyBase, IDisposable
    {
        public PropertyNotifyObject() { }

        Dictionary<object, object> _ValueDictionary = new Dictionary<object, object>();

        #region 根据属性名得到属性值
        public T GetPropertyValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException("invalid " + propertyName);
            object _propertyValue;
            if (!_ValueDictionary.TryGetValue(propertyName, out _propertyValue))
            {
                _propertyValue = default(T);
                _ValueDictionary.Add(propertyName, _propertyValue);
            }
            return (T)_propertyValue;
        }
        #endregion

        public void SetPropertyValue<T>(string propertyName, T value)
        {
            if (!_ValueDictionary.ContainsKey(propertyName) || _ValueDictionary[propertyName] != (object)value)
            {
                _ValueDictionary[propertyName] = value;
                OnPropertyChanged(propertyName);
            }
        }

        #region Dispose
        public void Dispose()
        {
            DoDispose();
        }

        ~PropertyNotifyObject()
        {
            DoDispose();
        }

        void DoDispose()
        {
            if (_ValueDictionary != null)
                _ValueDictionary.Clear();
        }
        #endregion
    }

    public static class PropertyNotifyObjectEx
    {
        public static U GetValue<T, U>(this T t, Expression<Func<T, U>> exp) where T : PropertyNotifyObject
        {
            string _pN = CommonMethod.GetPropertyName(exp);
            return t.GetPropertyValue<U>(_pN);
        }

        public static void SetValue<T, U>(this T t, Expression<Func<T, U>> exp, U value) where T : PropertyNotifyObject
        {
            string _pN = CommonMethod.GetPropertyName(exp);
            t.SetPropertyValue<U>(_pN, value);
        }
    }
}
