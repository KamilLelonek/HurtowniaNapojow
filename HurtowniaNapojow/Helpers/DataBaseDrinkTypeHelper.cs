using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using System;
using System.Windows;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;


namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseDrinkTypeHelper
    {
        private static readonly RodzajeNapojuTableAdapter DrinkTypeTableAdapter = new RodzajeNapojuTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.RodzajeNapojuRow> _drinkTypesData = DrinkTypeTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.RodzajeNapojuRow> GetDrinkTypesData()
        {
            return _drinkTypesData;
        }

        public static HurtowniaNapojowDataSet.RodzajeNapojuRow GetDrinkTypeByID(int drinkTypeId)
        {
            return GetDrinkTypesData().First(drinkType => drinkType.Identyfikator == drinkTypeId);
        }

        public static Boolean AddNewDrinkType(String newDrinkType, string newTaxRate)
        {
            var doesDrinkTypeExist = GetDrinkTypesData().Any(drinkType => drinkType.NazwaRodzajuNapoju == newDrinkType);
            if (doesDrinkTypeExist)
            {
                MessageBox.Show("Wprowadzany rodzaj gazu już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            
            float TaxRate = 0;
            var regex = new Regex(@"^[0-9]*(?:\,[0-9]*)?$");
            if (regex.IsMatch(newTaxRate) && newDrinkType.Length > 0 && float.TryParse(newTaxRate, out TaxRate))
            {
                try
                {
                    DrinkTypeTableAdapter.Insert(newDrinkType, TaxRate);
                    RefreshData();
                    MessageBox.Show("Pomyślnie dodano nowy rodzaj napoju", Globals.TITLE_SUCCESS);
                    return true;
                }
                catch (OleDbException)
                {
                    MessageBox.Show("Wprowadzane dane są nieprawidłowe.\nPole nazwa rodzaju napoju i stawka podatku nie mogą być puste!", "Błąd");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Wprowadzane dane są nieprawidłowe.\nPole nazwa rodzaju napoju i stawka podatku nie mogą być puste!", "Błąd");
                return false;
            }
        }

        public static Boolean DeleteDrinkTypeRow(DataRow drinkTypeRow)
        {
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_rodzaju_napoju == (drinkTypeRow as HurtowniaNapojowDataSet.RodzajeNapojuRow).Identyfikator);
            if (productExists)
            {
                MessageBox.Show("Do wybranego rodzaju napoju '" + (drinkTypeRow as HurtowniaNapojowDataSet.RodzajeNapojuRow).NazwaRodzajuNapoju + "' są przypisane napoje producenta. Rodzaj napoju nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            drinkTypeRow.Delete();
            return DrinkTypeTableAdapter.Update(drinkTypeRow) == 1;
        }

        public static Boolean EditDrinkType(HurtowniaNapojowDataSet.RodzajeNapojuRow drinkType, String newDrinkTypeName, String newTaxRate)
        {
            float TaxRate = 0;
            var regex = new Regex(@"^[0-9]*(?:\,[0-9]*)?$");
            if (regex.IsMatch(newTaxRate) && newDrinkTypeName.Length > 0 && float.TryParse(newTaxRate, out TaxRate))
            {
                drinkType.NazwaRodzajuNapoju = newDrinkTypeName;
                drinkType.StawkaPodatkowa = TaxRate;
                return UpdateDB(drinkType, "Rodzaj napoju został zmieniony");
            }
            else {
                MessageBox.Show("Edycja danych nie może być przeprowadzona ponieważ w bazie danych istnieje rekord zawierający wprowadzane dane lub wprowadzane dane są nieprawidłowe.\nPola nie może być puste!", "Błąd");
                return false;
            }
           
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.RodzajeNapojuRow drinkType, String messageIfSuccess)
        {
            try
            {
                DrinkTypeTableAdapter.Update(drinkType);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                RefreshData();
                return true;
            }
            catch (OleDbException )
            {
                MessageBox.Show("Edycja danych nie może być przeprowadzona ponieważ w bazie danych istnieje rekord zawierający wprowadzane dane lub wprowadzane dane są nieprawidłowe.\nPole nie może być puste!", "Błąd");
                return false;
            }
        }

        private static void RefreshData()
        {
            _drinkTypesData = DrinkTypeTableAdapter.GetData();
        }

    }
}