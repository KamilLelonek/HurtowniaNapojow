using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using System.Text.RegularExpressions;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.DrinkType
{
    /// <summary>
    /// Interaction logic for DrinkTypeNewWindow.xaml
    /// </summary>
    public partial class DrinkTypeNewWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private readonly DataGrid _DrinkTypeDataGrid;

        public DrinkTypeNewWindow()
        {
            _DrinkTypeDataGrid = new DataGrid();
            InitializeComponent();
        }
        public DrinkTypeNewWindow(ref DataGrid DrinkTypeDataGrid)
        {
            _DrinkTypeDataGrid = DrinkTypeDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(NameTextBox, TaxRateTextBox)) return;
            if (_validator.IsFloatValid(TaxRateTextBox)) return;

            var newNameDrinkType = NameTextBox.Text;
            var newTaxRate = float.Parse(TaxRateTextBox.Text);
            var result = DataBaseDrinkTypeHelper.AddNewDrinkType(newNameDrinkType, newTaxRate);
            if (!result) return;

            _DrinkTypeDataGrid.RebindContext(DataBaseDrinkTypeHelper.GetDrinkTypesData());
            CloseButton.PerformClick();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddButton.PerformClick();
            }
        }
    }
}