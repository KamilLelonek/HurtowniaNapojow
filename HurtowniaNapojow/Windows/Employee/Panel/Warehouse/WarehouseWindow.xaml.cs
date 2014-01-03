using System.Windows;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class WarehouseWindow
    {
        private readonly Validator _validator = Validator.Instance;

        public WarehouseWindow()
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

        private void MenuBack_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new EmployeeWindow());
        }

        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Instance.LogoutUser();
            this.OpenWindow(new LoginWindow());
        }
    }
}