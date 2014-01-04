using System.Windows;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Taste;
using HurtowniaNapojow.Windows.Employee.Warehouse.Taste;
using HurtowniaNapojow.Windows.Employee.Warehouse.GasType;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.GasType;

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

        private void TasteButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new TasteDetailsWindow(), blockPrevious: true);
        }

        private void PlusTaste_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new TasteNewWindow(), blockPrevious: true);
        }

        private void GasTypeButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new GasTypeDetailsWindow(), blockPrevious: true);
        }
        private void PlusGasType_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new GasTypeNewWindow(), blockPrevious: true);
        }

    }

}