using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
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
           DrinkNameDataGrid.RebindContext(new NazwyNapojuTableAdapter().GetData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkNameNewWindow(ref DrinkNameDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var drinkNames = DrinkNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (drinkNames.Count > 0)
            {
                drinkNames.ForEach(drinkName =>
                {
                    HurtowniaNapojowDataSet.NazwyNapojuRow editDrinkName = (HurtowniaNapojowDataSet.NazwyNapojuRow)drinkName.Row;
                    this.OpenWindow(new DrinkNameEditWindow(ref DrinkNameDataGrid, ref editDrinkName), blockPrevious: true);
                });
                SetDrinkNameBinding();
            } 
            else
            {
                MessageBox.Show("Nie wybrano danych do edycji, zaznacz rekord(y) przeznaczone do edycji.", "Uwaga");
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var drinkNames = DrinkNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();

            if (drinkNames.Count > 0)
            {
                if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    //do yes stuff
                    drinkNames.ForEach(drinkName => DataBaseDrinkNameHelper.DeleteDrinkNameRow(drinkName.Row));
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do usunięcia, zaznacz rekord(y) przeznaczone do usunięcia.", "Uwaga");
            }
            
        }
    }
}
