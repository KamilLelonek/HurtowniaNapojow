using System.Windows;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse;

namespace HurtowniaNapojow.Windows.Employee
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class EmployeeWindow
    {
        private readonly Validator _validator = Validator.Instance;

        public EmployeeWindow()
        {
            InitializeComponent();
        }

        private void MenuShoppingPanel_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ShoppingWindow());
        }

        private void MenuWarehousePanel_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new WarehouseWindow());
        }

        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Instance.LogoutUser();
            this.OpenWindow(new LoginWindow());
        }
    }
}