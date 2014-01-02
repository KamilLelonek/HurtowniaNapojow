namespace System.Windows
{
    public static class WindowsPlugins
    {
        public static void OpenWindow(this Window window, Window newWindow, bool closePreviousWindow = true)
        {
            newWindow.Show();
            if(closePreviousWindow)
                window.Close();
        }
    }
}