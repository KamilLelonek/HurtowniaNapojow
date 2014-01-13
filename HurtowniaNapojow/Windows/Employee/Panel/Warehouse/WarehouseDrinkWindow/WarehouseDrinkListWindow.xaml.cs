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
using HurtowniaNapojow.Windows.Employee.Warehouse.ProducerDrink;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.WarehouseDrinkWindow
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class WarehouseDrinkNewWindow
    {
        private readonly Validator _validator = Validator.Instance;

        public WarehouseDrinkNewWindow()
        {
           InitializeComponent();
           SetDataInDataGrid();
           SetDrinkComponentsEvents();
        }

        private void SetDataInDataGrid()
        {
            DrinkDataGrid.DataContext = ProducerDrinkHelper.GetProducerDrinksLinq().OrderBy(d => d.Name);

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
            var drinkRow = (((ProducerDrinkHelper)((Button)sender).DataContext));
            if (drinkRow == null) return;

            HurtowniaNapojowDataSet.NapojeProducentaRow productRow = DataBaseProducerDrinkHelper.GetDrinkByID(drinkRow.Id);
            (new WarehouseDrinkAddWindow( ref productRow)).ShowDialog();
            
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
            DrinkDataGrid.DataContext = ProducerDrinkHelper.GetProducerDrinks();
        }
        private void DrinkFilterChanged()
        {
            var comboBoxItem = DrinkFilterComboBox.SelectedItem as ComboBoxItem;
            if (comboBoxItem == null) DrinkFilterComboBox.SelectedIndex = 0;
            comboBoxItem = DrinkFilterComboBox.SelectedItem as ComboBoxItem;

            var filterType = comboBoxItem.Name.ToString();
            var filterValue = DrinkFilterTextBox.Text.ToLower();
            if (filterType == "Nazwa") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.Name.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Smak") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.TasteName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Gaz") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.GasName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Producent") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.ProducerName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "Typ") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.TypeName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "NazwaSztuki") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.PiecePackageName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "PojSztuki") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.PiecePackageVolume.ToString().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "NazwaZbiorczego") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.BulkPackageName.ToLower().Contains(filterValue)).OrderBy(d => d.Name);
            else if (filterType == "PojZbiorczego") DrinkDataGrid.DataContext = (ProducerDrinkHelper.GetProducerDrinks()).Where(d => d.BulkPackageVolume.ToString().Contains(filterValue)).OrderBy(d => d.Name);
            else DrinkDataGrid.DataContext = ProducerDrinkHelper.GetProducerDrinks().OrderBy(d => d.Name);
        }

        private void AddNewProducerDrink(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerDrinkNewWindow(), blockPrevious: true);
            SetDataInDataGrid();
        }
    }
}