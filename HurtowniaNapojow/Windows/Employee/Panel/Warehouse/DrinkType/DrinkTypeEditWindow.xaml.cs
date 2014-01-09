using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;
using System.Text.RegularExpressions;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.DrinkType
{
    /// <summary>
    /// Interaction logic for DrinkTypeNewWindow.xaml
    /// </summary>
    public partial class DrinkTypeEditWindow
    {
        private readonly DataGrid _DrinkTypeDataGrid;
        private HurtowniaNapojowDataSet.RodzajeNapojuRow _editDrinkType;
        private readonly Validator _validator = Validator.Instance;

        public DrinkTypeEditWindow(ref DataGrid DrinkTypeDataGrid, ref HurtowniaNapojowDataSet.RodzajeNapojuRow editDrinkType)
        {

            _DrinkTypeDataGrid = DrinkTypeDataGrid;
            _editDrinkType = editDrinkType;
            InitializeComponent();

            OldNameTextBox.Text = _editDrinkType.NazwaRodzajuNapoju;
            OldTaxRateTextBox.Text = _editDrinkType.StawkaPodatkowa.ToString();
            NewNameTextBox.Text = OldNameTextBox.Text;
            NewTaxRateTextBox.Text = OldTaxRateTextBox.Text;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(NewNameTextBox, NewTaxRateTextBox)) return;
            if (_validator.IsFloatValid(NewTaxRateTextBox)) return;
            var newNameDrinkType = NewNameTextBox.Text;
            var newTaxRate = float.Parse(NewTaxRateTextBox.Text);
            
            var result = DataBaseDrinkTypeHelper.EditDrinkType(_editDrinkType, newNameDrinkType, newTaxRate);
            if (!result) return;

            _DrinkTypeDataGrid.RebindContext(DataBaseDrinkTypeHelper.GetDrinkTypesData());
            CloseButton.PerformClick();
        }
    }
}