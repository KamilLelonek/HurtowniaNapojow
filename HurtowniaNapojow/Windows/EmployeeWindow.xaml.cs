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
        protected void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Instance.LogoutUser();
            new LoginWindow().Show();
            this.Close();
        }
    }
}
