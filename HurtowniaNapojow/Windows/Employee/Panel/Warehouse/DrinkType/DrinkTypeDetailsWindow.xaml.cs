﻿using System;
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
           DrinkTypeDataGrid.RebindContext(new RodzajeNapojuTableAdapter().GetData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkTypeNewWindow(ref DrinkTypeDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tastes = DrinkTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (tastes.Count > 0)
            {
                tastes.ForEach(DrinkType =>
                {
                    HurtowniaNapojowDataSet.RodzajeNapojuRow editDrinkType = (HurtowniaNapojowDataSet.RodzajeNapojuRow)DrinkType.Row;
                    this.OpenWindow(new DrinkTypeEditWindow(ref DrinkTypeDataGrid, ref editDrinkType), blockPrevious: true);
                });
                SetDrinkTypeBinding();
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
            var Drinkes = DrinkTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (Drinkes.Count > 0)
            {
                if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                Drinkes.ForEach(Drink => DataBaseDrinkTypeHelper.DeleteDrinkTypeRow(Drink.Row));
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do usunięcia, zaznacz rekord(y) przeznaczone do usunięcia.", "Uwaga");
            }
            
        }
    }
}
