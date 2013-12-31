using System.Windows;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow
    {
        public EmployeeWindow()
        {
            InitializeComponent();
        }
        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Instance.LogoutUser();
            this.OpenWindow(new LoginWindow());
        }
    }
}
