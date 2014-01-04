using System.Windows;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Windows;
using HurtowniaNapojow.Windows.Admin;
using HurtowniaNapojow.Windows.Employee;

namespace HurtowniaNapojow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            var sessionHelper = SessionHelper.Instance;
            if (!sessionHelper.IsUserSet)
            {
                new LoginWindow().Show();
            }
            else
            {
                if (sessionHelper.IsCurrentUserAdmin)
                {
                    new AdminWindow().Show();
                }
                else
                {
                    new EmployeeWindow().Show();
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // add code here to cleanup database
            base.OnExit(e);
        }
    }
}