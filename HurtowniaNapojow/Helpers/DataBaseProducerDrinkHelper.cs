using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using System;
using System.Windows;
using System.Data.OleDb;
using System.Data;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProducerDrinkHelper
    {
        private static readonly NapojeProducentaTableAdapter ProducerDrinkTableAdapter = new NapojeProducentaTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.NapojeProducentaRow> _producerDrinksData = ProducerDrinkTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.NapojeProducentaRow> GetProducerDrinkData()
        {
            return _producerDrinksData;
        }

        public static HurtowniaNapojowDataSet.NapojeProducentaRow GetDrinkByID(int drinkId)
        {
            return _producerDrinksData.First(producerDrink => producerDrink.Identyfikator == drinkId);
        }

        public static Boolean AddNewProducerDrink(int idDrinkName, int idTaste, int idGasType, int idProducer, int idDrinkType, int idPiecePackage, int idBulkPackage, double producerPrice)
        {
            var doesProducerDrinkExist = GetProducerDrinkData().Any(producerDrink => (producerDrink.id_nazwy_napoju == idDrinkName) && (producerDrink.id_smaku == idTaste) && (producerDrink.id_rodzaju_gazu == idGasType)
                                                                                            && (producerDrink.id_procuenta == idProducer) && (producerDrink.id_rodzaju_napoju == idDrinkType) 
                                                                                            && (producerDrink.id_opakowania_sztuki == idPiecePackage) && (producerDrink.id_opakowania_zbiorczego == idBulkPackage));
            if (doesProducerDrinkExist)
            {
                MessageBox.Show("Wprowadzany napój producenta już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            try
            {
                ProducerDrinkTableAdapter.Insert(idDrinkName, idTaste, idGasType, idProducer, idDrinkType, idPiecePackage, idBulkPackage, producerPrice);
                RefreshData();
                MessageBox.Show("Pomyślnie dodano nowy napój producenta", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Wprowadzane dane są nieprawidłowe.\n!", "Błąd");
                return false;
            }
        }

        public static Boolean DeleteProducerDrinkRow(DataRow producerDrinkRow)
        {
            var warehouseProductExists = DataBaseWarehouseDrinkHelper.GetWarehouseDrinkData().Any(product => product.id_napoju_producenta == (producerDrinkRow as HurtowniaNapojowDataSet.NapojeProducentaRow).Identyfikator);
            if (warehouseProductExists)
            {
                MessageBox.Show("Do wybranego napoju producenta są przypisane napoje hurtowni. Wybrany napój producenta nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            producerDrinkRow.Delete();
            var result = ProducerDrinkTableAdapter.Update(producerDrinkRow) == 1;
            if (result) RefreshData();
            return result;
        }

        public static Boolean EditProducerDrink(HurtowniaNapojowDataSet.NapojeProducentaRow producerDrink, int idDrinkName, int idTaste, int idGasType, int idProducer, int idDrinkType, int idPiecePackage, int idBulkPackage, double producerPrice)
        {
            producerDrink.id_nazwy_napoju = idDrinkName;
            producerDrink.id_smaku = idTaste;
            producerDrink.id_rodzaju_gazu = idGasType;
            producerDrink.id_procuenta = idProducer;
            producerDrink.id_rodzaju_napoju = idDrinkType;
            producerDrink.id_opakowania_sztuki = idPiecePackage;
            producerDrink.id_opakowania_zbiorczego = idBulkPackage;
            producerDrink.CenaProducenta = producerPrice;
                
            return UpdateDB(producerDrink, "Napój producenta został zmieniony");
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.NapojeProducentaRow producerDrink, String messageIfSuccess)
        {
            try
            {
                ProducerDrinkTableAdapter.Update(producerDrink);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                RefreshData();
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Edycja danych nie może być przeprowadzona ponieważ w bazie danych istnieje rekord zawierający wprowadzane dane lub wprowadzane dane są nieprawidłowe.\nPole nie może być puste!", "Błąd");
                return false;
            }
        }

        private static void RefreshData()
        {
            _producerDrinksData = ProducerDrinkTableAdapter.GetData();
        }
    }
}