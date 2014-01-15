using System.Windows;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Windows;
using HurtowniaNapojow.Windows.Admin;
using HurtowniaNapojow.Windows.Employee;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            Globals.LoadConfig();
            if(Globals.RUN_FIXXXER)
            {
                Fixxxer.FixDatabase();
                Globals.RUN_FIXXXER = false;
            }       

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
            Globals.SaveConfig();
            base.OnExit(e);
        }
    }
}