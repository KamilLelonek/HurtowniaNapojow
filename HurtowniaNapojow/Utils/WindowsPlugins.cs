using System.Windows.Input;

namespace System.Windows
{
    public static class WindowsPlugins
    {
        public static void OpenWindow(this Window window, Window newWindow, bool closePrevious = true, bool blockPrevious = false)
        {
            window.Cursor = Cursors.Wait;
            newWindow.Loaded += (sender, args) => newWindow.Cursor = Cursors.Arrow;

            if (blockPrevious)
            {
                newWindow.ShowDialog();
            }
            else
            {
                newWindow.Show();
                if (closePrevious)
                {
                    window.Close();
                }
            }
        }
    }
}