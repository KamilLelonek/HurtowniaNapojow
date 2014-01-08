using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkName;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.DrinkName
{
    /// <summary>
    /// Interaction logic for DrinkNameDetailsWindow.xaml
    /// </summary>
    public partial class DrinkNameDetailsWindow
    {
        private bool _drinkNamesFilled = false;

        public DrinkNameDetailsWindow()
        {
            InitializeComponent();
            if (! _drinkNamesFilled)
            {
                _drinkNamesFilled = true;
                SetDrinkNameBinding();
            }
        }

        public void SetDrinkNameBinding()
        {
           DrinkNameDataGrid.RebindContext(DataBaseDrinkNameHelper.GetDrinkNamesData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkNameNewWindow(ref DrinkNameDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var drinkNames = DrinkNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            drinkNames.ForEach(drinkName => {
             HurtowniaNapojowDataSet.NazwyNapojuRow editDrinkName = (HurtowniaNapojowDataSet.NazwyNapojuRow)drinkName.Row;
             this.OpenWindow(new DrinkNameEditWindow(ref DrinkNameDataGrid, ref editDrinkName), blockPrevious: true);
            });
            SetDrinkNameBinding();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                //do yes stuff
                var drinkNames = DrinkNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
                drinkNames.ForEach(drinkName => DataBaseDrinkNameHelper.DeleteDrinkNameRow(drinkName.Row));
            }
            
        }
    }
}
