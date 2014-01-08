﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkType;


namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.DrinkType
{
    /// <summary>
    /// Interaction logic for DrinkTypeDetailsWindow.xaml
    /// </summary>
    public partial class DrinkTypeDetailsWindow
    {
        private bool _DrinkesFilled = false;

        public DrinkTypeDetailsWindow()
        {
            InitializeComponent();
            if (!_DrinkesFilled)
            {
                _DrinkesFilled = true;
                SetDrinkTypeBinding();
            }
        }

        public void SetDrinkTypeBinding()
        {
           DrinkTypeDataGrid.RebindContext(DataBaseDrinkTypeHelper.GetDrinkTypesData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkTypeNewWindow(ref DrinkTypeDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tastes = DrinkTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            tastes.ForEach(DrinkType => {
             HurtowniaNapojowDataSet.RodzajeNapojuRow editDrinkType = (HurtowniaNapojowDataSet.RodzajeNapojuRow)DrinkType.Row;
             this.OpenWindow(new DrinkTypeEditWindow(ref DrinkTypeDataGrid, ref editDrinkType), blockPrevious: true);
            });
            SetDrinkTypeBinding();
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
                var Drinkes = DrinkTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
                Drinkes.ForEach(Drink => DataBaseDrinkTypeHelper.DeleteDrinkTypeRow(Drink.Row));
            }
            
        }
    }
}
