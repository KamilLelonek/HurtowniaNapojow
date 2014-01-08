using System.Windows;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.BulkPackageName;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.DrinkName;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.DrinkType;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.PiecePackageName;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Producer;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Taste;
using HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackageName;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkName;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkType;
using HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackageName;
using HurtowniaNapojow.Windows.Employee.Warehouse.Producer;
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

        private void DrinkNameButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkNameDetailsWindow(), blockPrevious: true);
        }

        private void DrinkNameType_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkNameNewWindow(), blockPrevious: true);
        }

        private void BulkPackageNameButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new BulkPackageNameDetailsWindow(), blockPrevious: true);
        }

        private void PiecePackageNameButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageNameDetailsWindow(), blockPrevious: true);
        }

        private void PiecePackageNamePlus_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageNameNewWindow(), blockPrevious: true);
        }

        private void BulkPackageNamePlus_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new BulkPackageNameNewWindow(), blockPrevious: true);
        }

        private void ProducerButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerDetailsWindow(), blockPrevious: true);
        }

        private void ProducerPlus_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerNewWindow(), blockPrevious: true);
        }

        private void DrinkTypeButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkTypeDetailsWindow(), blockPrevious: true);
        }

        private void DrinkTypePlus_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkTypeNewWindow(), blockPrevious: true);
        }

    }

}