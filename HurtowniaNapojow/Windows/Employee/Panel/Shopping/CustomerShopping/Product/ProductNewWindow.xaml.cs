using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HurtowniaNapojow.Database;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Reports.Admin;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping.Product
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class ProductNewWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private readonly EmployeeShopping _shopping;
        private readonly CustomerShoppingDetailsWindow _parentWindow;

        public ProductNewWindow(CustomerShoppingDetailsWindow parentWindow, EmployeeShopping shopping)
        {
            _parentWindow = parentWindow;
            _shopping = shopping;
            InitializeComponent();
            DrinkDataGrid.DataContext = WarehouseDrink.GetWarehouseDrinks();
            SetDrinkComponentsEvents();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CloseButton.PerformClick();
        }

        private void DrinkChoose_Clicked(object sender, RoutedEventArgs e)
        {
            var drinkRow = (((WarehouseDrink)((Button)sender).DataContext));
            if (drinkRow == null) return;

            HurtowniaNapojowDataSet.ProduktyKlientaRow productRow = DataBaseProductHelper.AddNewProduct(_shopping._shoppingRow, drinkRow._warehouseDrinkRow);
            (new ProductDetailsWindow(_parentWindow, ref productRow)).ShowDialog();
            _parentWindow.SetShoppingBinding();
            Close();
        }

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

            var filterType = comboBoxItem.Content.ToString();
            var filterValue = DrinkFilterTextBox.Text.ToLower();
            if (filterType == "Nazwa") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.Name.ToLower().Contains(filterValue));
            else if (filterType == "Smak") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.TasteName.ToLower().Contains(filterValue));
            else if (filterType == "Rodzaj Gazu") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.GasName.ToLower().Contains(filterValue));
            else if (filterType == "Producent") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.ProducerName.ToLower().Contains(filterValue));
            else if (filterType == "Type") DrinkDataGrid.DataContext = (WarehouseDrink.GetWarehouseDrinks()).Where(d => d.TypeName.ToLower().Contains(filterValue));
            else DrinkDataGrid.DataContext = WarehouseDrink.GetWarehouseDrinks();
        }
    }
}