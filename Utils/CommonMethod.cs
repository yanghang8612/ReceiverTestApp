using System;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace ReceiverTestApp.Utils
{
    class CommonMethod
    {
        public static void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static string GetPropertyName<T, U>(Expression<Func<T, U>> exp)
        {
            string _pName = "";
            if (exp.Body is MemberExpression)
            {
                _pName = (exp.Body as MemberExpression).Member.Name;
            }
            else if (exp.Body is UnaryExpression)
            {
                _pName = ((exp.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            }
            return _pName;
        }

        public static string ChooseFolderDialog()
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                return m_Dialog.SelectedPath.Trim();
            }
            return null;
        }

        public static string ChooseFileDialog()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;

            }
            return null;
        }
    }
}
