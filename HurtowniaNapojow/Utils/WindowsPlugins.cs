using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System.Windows
{
    public static class WindowsPlugins
    {
        public static void OpenWindow(this Window window, Window newWindow)
        {
            newWindow.Show();
            window.Close();
        }
    }
}