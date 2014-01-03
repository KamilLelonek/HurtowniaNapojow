using System.Collections;
using System.Windows.Input;
using HurtowniaNapojow.Reports;

namespace System.Windows
{
    public static class WindowsPlugins
    {
        public static void OpenReport(this Window window, IEnumerable dataTable, String reportRelativePath)
        {
            window.OpenWindow(new ReportWindow(dataTable, reportRelativePath), blockPrevious: true);
        }

        public static void OpenWindow(this Window window, Window newWindow, bool closePrevious = true, bool blockPrevious = false)
        {
            if (blockPrevious)
            {
                newWindow.ShowDialog();
            }
            else
            {
                window.Cursor = Cursors.Wait;
                newWindow.Loaded += (sender, args) => newWindow.Cursor = Cursors.Arrow;

                newWindow.Show();
                if (closePrevious)
                {
                    window.Close();
                }
            }
        }
    }
}