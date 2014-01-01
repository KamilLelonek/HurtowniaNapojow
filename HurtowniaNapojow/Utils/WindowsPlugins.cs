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