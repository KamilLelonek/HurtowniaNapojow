using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.DrinkName
{
    /// <summary>
    /// Interaction logic for DrinkNameNewWindow.xaml
    /// </summary>
    public partial class DrinkNameEditWindow
    {
        private readonly DataGrid _drinkNameDataGrid;
        private HurtowniaNapojowDataSet.NazwyNapojuRow _editDrinkName;

        public DrinkNameEditWindow(ref DataGrid drinkNameDataGrid, ref HurtowniaNapojowDataSet.NazwyNapojuRow editDrinkName)
        {
           
            _drinkNameDataGrid = drinkNameDataGrid;
            _editDrinkName = editDrinkName;
            InitializeComponent();

            OldNameTextBox.Text = _editDrinkName.NazwaNapoju;
         }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var newDrinkName = NewNameTextBox.Text;

            var result = DataBaseDrinkNameHelper.EditDrinkName(_editDrinkName,newDrinkName);
            if (!result) return;

            _drinkNameDataGrid.RebindContext(DataBaseDrinkNameHelper.GetDrinkNamesData());
            CloseButton.PerformClick();
        }
    }
}