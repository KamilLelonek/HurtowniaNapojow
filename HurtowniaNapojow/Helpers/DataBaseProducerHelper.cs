using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using System;
using System.Windows;
using System.Data;
using System.Data.OleDb;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProducerHelper
    {
        private static readonly ProducenciTableAdapter ProducerTableAdapter = new ProducenciTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.ProducenciRow> _producersData = ProducerTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.ProducenciRow> GetProducersData()
        {
            return _producersData;
        }

        public static HurtowniaNapojowDataSet.ProducenciRow GetProducerByID(int producerId)
        {
            return GetProducersData().First(producer => producer.Identyfikator == producerId);
        }

        public static Boolean AddNewProducer(String newProducer)
        {
            var doesProducerExist = GetProducersData().Any(producer => producer.NazwaProducenta == newProducer);
            if (doesProducerExist)
            {
                MessageBox.Show("Wprowadzana nazwa producenta już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            try
            {
                ProducerTableAdapter.Insert(newProducer);
                RefreshData();
                MessageBox.Show("Pomyślnie dodano nowego producenta napojów", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Wprowadzane dane są nieprawidłowe.\nPole nazwa producenta nie może być puste!", "Błąd");
                return false;
            }
        }

        public static Boolean DeleteProducerRow(DataRow producerRow)
        {
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_procuenta == (producerRow as HurtowniaNapojowDataSet.ProducenciRow).Identyfikator);
            if (productExists)
            {
                MessageBox.Show("Do wybranego producenta '" + (producerRow as HurtowniaNapojowDataSet.ProducenciRow).NazwaProducenta + "' są przypisane napoje producenta. Wybrany Producent nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            producerRow.Delete();
            var result = ProducerTableAdapter.Update(producerRow) == 1;
            if (result) RefreshData();
            return result;
        }

        public static Boolean EditProducer(HurtowniaNapojowDataSet.ProducenciRow producer, String newProducerName)
        {
            producer.NazwaProducenta = newProducerName;
            return UpdateDB(producer, "Producent został zmieniony");
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.ProducenciRow producer, String messageIfSuccess)
        {
            try
            {
                ProducerTableAdapter.Update(producer);
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
            _producersData = ProducerTableAdapter.GetData();
        }
    }
}