using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.DrinkName
{
    /// <summary>
    /// Interaction logic for DrinkNameNewWindow.xaml
    /// </summary>
    public partial class DrinkNameNewWindow
    {
        private readonly DataGrid _drinkNameDataGrid;

        public DrinkNameNewWindow()
        {
            _drinkNameDataGrid = new DataGrid();
            InitializeComponent();
        }
        public DrinkNameNewWindow(ref DataGrid drinkNameDataGrid)
        {
            _drinkNameDataGrid = drinkNameDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        
            var newNameDrinkName = NameTextBox.Text;

                var result = DataBaseDrinkNameHelper.AddNewDrinkName(newNameDrinkName);
                if (!result)
                {
                    //MessageBox.Show("Podane dane są nieprawidłowe", Globals.TITLE_ERROR);
                    return;
                }
   
            _drinkNameDataGrid.RebindContext(DataBaseDrinkNameHelper.GetDrinkNamesData());
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