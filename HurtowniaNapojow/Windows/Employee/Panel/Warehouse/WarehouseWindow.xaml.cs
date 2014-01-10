using System.Linq;
using System.Windows;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Taste;
using HurtowniaNapojow.Windows.Employee.Warehouse.Taste;
using HurtowniaNapojow.Windows.Employee.Warehouse.GasType;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.GasType;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.DrinkName;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkName;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.BulkPackageName;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.PiecePackageName;
using HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackageName;
using HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackageName;
using HurtowniaNapojow.Windows.Employee.Warehouse.Producer;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Producer;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.DrinkType;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkType;
using HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackage;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.BulkPackage;
using HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackage;
using HurtowniaNapojow.Windows.Employee.Warehouse.ProducerDrink;
using System.Windows.Controls;
using System.Collections.Generic;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse.PiecePackage;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class WarehouseWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private bool _producerDrinkFilled = false;
        private bool _warehouseDrinkFilled = false;
        private bool _crudDrinkFilled = false;
        private IEnumerable<ProducerDrinkHelper> producerDrinks;

        public WarehouseWindow()
        {
            InitializeComponent();
            SetProducerDrinkComponentsEvents();
            
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProducerDrinkTab.IsSelected && !_producerDrinkFilled)
            {
                _producerDrinkFilled = true;
                SetProducerDrinkBinding();
            }
            if (WarehouseDrinkTab.IsSelected && !_warehouseDrinkFilled)
            {
                _warehouseDrinkFilled = true;
                DrinkDataGrid.DataContext = WarehouseDrink.GetWarehouseDrinks().OrderBy(d => d.Name);
                SetDrinkComponentsEvents();

            }
            if (CrudDrinkTab.IsSelected && !_crudDrinkFilled)
            {
                _crudDrinkFilled = true;

            }
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


        #region TAB ProducerDrink

        public void SetProducerDrinkBinding()
        {
            ProducerDrinkFilterComboBox.Text = Globals.FILTER_SELECT;
            ProducerDrinkFilterTextBox.Text = "";
            producerDrinks = ProducerDrinkHelper.GetProducerDrinksLinq();
            ProducerDrinkDataGrid.RebindContext(producerDrinks.OrderBy(c => c.Name));
        }
        private void SetProducerDrinkComponentsEvents()
        {
            ProducerDrinkFilterTextBox.TextChanged += (sender, args) => ProducerDrinkFilterChanged();
            ProducerDrinkFilterComboBox.SelectionChanged += (sender, args) => ProducerDrinkFilterChanged();
        }

        private void ProducerDrinkFilterChanged()
        {
            var comboBoxItem = ProducerDrinkFilterComboBox.SelectedItem as ComboBoxItem;
            if (comboBoxItem == null) ProducerDrinkFilterComboBox.SelectedIndex = 0;
            comboBoxItem = ProducerDrinkFilterComboBox.SelectedItem as ComboBoxItem;

            var filterType = comboBoxItem.Name.ToString();
            var filterValue = ProducerDrinkFilterTextBox.Text.ToLower();

            if (filterType == "Name") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(c => c.Name.ToLower().Contains(filterValue)).OrderBy(c => c.Name);
            else if (filterType == "ProducerName") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(c => c.ProducerName.ToLower().Contains(filterValue)).OrderBy(c => c.ProducerName);
            else if (filterType == "TasteName") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(c => c.TasteName.ToLower().Contains(filterValue)).OrderBy(c => c.TasteName);
            else if (filterType == "TypeName") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(c => c.TypeName.ToLower().Contains(filterValue)).OrderBy(c => c.TypeName);
            else if (filterType == "GasName") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(c => c.GasName.ToLower().Contains(filterValue)).OrderBy(c => c.GasName);
            else if (filterType == "PiecePackageName") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(c => c.PiecePackageName.ToLower().Contains(filterValue)).OrderBy(c => c.PiecePackageName);
            else if (filterType == "BulkPackageName") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(c => c.BulkPackageName.ToLower().Contains(filterValue)).OrderBy(c => c.BulkPackageName);
            else if (filterType == "PiecePackageVolume") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(d => d.PiecePackageVolume.ToString().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "BulkPackageVolume") ProducerDrinkDataGrid.DataContext = producerDrinks.Where(d => d.BulkPackageVolume.ToString().Contains(filterValue)).OrderBy(d => d.Name);
            else SetProducerDrinkBinding();
        }
        private void AddNewProducerDrink_Clicked(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerDrinkNewWindow(), blockPrevious: true);
            SetProducerDrinkBinding();
        }

        private void EditProducerDrink_Clicked(object sender, RoutedEventArgs e)
        {
            var ProducerDrinks = ProducerDrinkDataGrid.SelectedItems.Cast<ProducerDrinkHelper>().ToList();
            if (ProducerDrinks.Count > 0)
            {
                ProducerDrinks.ForEach(ProducerDrink =>
                {
                    var ProducerDrinkToEdit = DataBaseProducerDrinkHelper.GetDrinkByID(ProducerDrink.Id);
                    HurtowniaNapojowDataSet.NapojeProducentaRow editProducerDrink = (HurtowniaNapojowDataSet.NapojeProducentaRow)ProducerDrinkToEdit;
                    this.OpenWindow(new ProducerDrinkEditWindow(ref editProducerDrink), blockPrevious: true);
                });
                SetProducerDrinkBinding();
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do edycji, zaznacz rekord(y) przeznaczone do edycji.", "Uwaga");
            }
        }

        private void DeleteProducerDrink_Clicked(object sender, RoutedEventArgs e)
        {
            var producerDrinks = ProducerDrinkDataGrid.SelectedItems.OfType<ProducerDrinkHelper>().ToList();
            if (producerDrinks.Count > 0)
            {
                if (MessageBox.Show("Czy na pewno chcesz trwale usun¹æ zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                {
                    producerDrinks.ForEach(producerDrink =>
                    {
                        var producerDrinkRow = DataBaseProducerDrinkHelper.GetDrinkByID(producerDrink.Id);
                        DataBaseProducerDrinkHelper.DeleteProducerDrinkRow(producerDrinkRow);
                    });
                    SetProducerDrinkBinding();
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do usuniêcia, zaznacz rekord(y) przeznaczone do usuniêcia.", "Uwaga");
            }
        }

        private void ProducerDrinkResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ProducerDrinkFilterComboBox.Text = Globals.FILTER_SELECT;
            ProducerDrinkFilterTextBox.Text = "";
            SetProducerDrinkBinding();
        }
        #endregion

        #region TAB CRUD Drink
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

        private void PiecePackageButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageDetailsWindow(), blockPrevious: true);
        }

        private void PiecePackagePlus_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageNewWindow(), blockPrevious: true);
        }

        private void BulkPackageButton_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new BulkPackageDetailsWindow(), blockPrevious: true);
        }

        private void BulkPackagePlus_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new BulkPackageNewWindow(), blockPrevious: true);
        }

        private void ProducerDrinkPlus_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerDrinkNewWindow(), blockPrevious: true);
        }

        #endregion

        #region TAB WarehouseDrink
        private void SetDrinkComponentsEvents()
        {
            DrinkFilterTextBox.TextChanged += (sender, args) => DrinkFilterChanged();
            DrinkFilterComboBox.SelectionChanged += (sender, args) => DrinkFilterChanged();
        }
        private void DrinkResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            DrinkFilterComboBox.Text = Globals.FILTER_SELECT;
            DrinkFilterTextBox.Text = "";
            DrinkDataGrid.DataContext = WarehouseDrink.GetWarehouseDrinks();
        }
        private void DrinkFilterChanged()
        {
            var comboBoxItem = DrinkFilterComboBox.SelectedItem as ComboBoxItem;
            if (comboBoxItem == null) DrinkFilterComboBox.SelectedIndex = 0;
            comboBoxItem = DrinkFilterComboBox.SelectedItem as ComboBoxItem;

            var filterType = comboBoxItem.Name.ToString();
            var filterValue = DrinkFilterTextBox.Text.ToLower();
            if (filterType == "Nazwa") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.Name.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Smak") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.TasteName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Gaz") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.GasName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Producent") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.ProducerName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Typ") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.TypeName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "NazwaSztuki") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.PiecePackageName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "PojSztuki") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.PiecePackageVolume.ToString().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "NazwaZbiorczego") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.BulkPackageName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "PojZbiorczego") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.BulkPackageVolume.ToString().Contains(filterValue)).OrderBy(d => d.Name);
            else DrinkDataGrid.DataContext = WarehouseDrink.GetWarehouseDrinks().OrderBy(d => d.Name);
        }
        #endregion
    }

}