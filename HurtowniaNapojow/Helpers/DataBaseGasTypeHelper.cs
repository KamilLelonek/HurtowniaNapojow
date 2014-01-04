using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseGasTypeHelper
    {
        private static readonly RodzajeGazuTableAdapter GasTypeTableAdapter = new RodzajeGazuTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.RodzajeGazuRow> _gasTypesData = GasTypeTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.RodzajeGazuRow> GetGasTypeData()
        {
            return _gasTypesData;
        }

        public static HurtowniaNapojowDataSet.RodzajeGazuRow GetGasTypeByID(int gasTypeId)
        {
            return GetGasTypeData().First(gasType => gasType.Identyfikator == gasTypeId);
        }

        public static Boolean AddNewGasType(String newGasType)
        {
            var doesTasteExist = GetGasTypeData().Any(taste => taste.NazwaRodzaju == newGasType);
            if (doesTasteExist)
            {
                MessageBox.Show("Wprowadzany rodzaj gazu już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            GasTypeTableAdapter.Insert(newGasType);
            _gasTypesData = GasTypeTableAdapter.GetData();
            MessageBox.Show("Pomyślnie dodano nowy rodzaj gazu", Globals.TITLE_SUCCESS);
            return true;
        }

        public static Boolean DeleteGasTypeRow(DataRow gasTypeRow)
        {
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_rodzaju_gazu == (gasTypeRow as HurtowniaNapojowDataSet.RodzajeGazuRow).Identyfikator);
            if (productExists)
            {
                MessageBox.Show("Do wybranego rodzaju gazu '" + (gasTypeRow as HurtowniaNapojowDataSet.RodzajeGazuRow).NazwaRodzaju + "' są przypisane napoje producenta. Rodzaj Gazu nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            gasTypeRow.Delete();
            return GasTypeTableAdapter.Update(gasTypeRow) == 1;
        }

        public static Boolean EditTaste(HurtowniaNapojowDataSet.RodzajeGazuRow taste, String newGasTypeName)
        {
            taste.NazwaRodzaju = newGasTypeName;
            return UpdateDB(taste, "Rodzaj Gazu został zmieniony");

        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.RodzajeGazuRow taste, String messageIfSuccess)
        {
            try
            {
                GasTypeTableAdapter.Update(taste);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                _gasTypesData = GasTypeTableAdapter.GetData();
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                return false;
            }
        }

    }
}