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
    public static class DataBaseBulkPackageHelper
    {
        private static readonly OpakowaniaZbiorczeTableAdapter BulkPackageTableAdapter = new OpakowaniaZbiorczeTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow> _bulkPackageData = BulkPackageTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow> GetBulkPackageData()
        {
            return _bulkPackageData;
        }

        public static HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow GetBulkPackageByID(int bulkPackageId)
        {
            return GetBulkPackageData().First(bulkPackage => bulkPackage.Identyfikator == bulkPackageId);
        }

        //Zmienić int na float
        public static Boolean AddNewBulkPackage(HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow newBulkPackageName, int newBulkCapacity)
        {
            var doesBulkPackageExist = GetBulkPackageData().Any(bulkPackage => (bulkPackage.id_nazwy_opakowania_zbiorczego == newBulkPackageName.Identyfikator && bulkPackage.Pojemność == newBulkCapacity));
            if (doesBulkPackageExist)
            {
                MessageBox.Show("Wprowadzany rodzaj opakowania zbiorczego już istnieje", Globals.TITLE_ERROR);
                return false;
            }
           
            try
            {
                BulkPackageTableAdapter.Insert(newBulkCapacity, newBulkPackageName.Identyfikator);
                _bulkPackageData = BulkPackageTableAdapter.GetData();
                MessageBox.Show("Pomyślnie dodano nowy rodzaj opakowania zbiorczego", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                return false;
            }
        }

        public static Boolean DeleteBulkPackageRow(DataRow bulkPackageRow)
        {   
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_opakowania_zbiorczego == (bulkPackageRow as HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow).Identyfikator);
            if (productExists)
            {
                HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow tempBulkPackageName = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID((bulkPackageRow as HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow).id_nazwy_opakowania_zbiorczego);
                MessageBox.Show("Do wybranego rodzaju opakowania zbiorczego'" +tempBulkPackageName.NazwaOpakowania+ ", " +(bulkPackageRow as HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow).Pojemność + "' są przypisane napoje producenta. Rodzaj opakowania zbiorczego nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            bulkPackageRow.Delete();
            return BulkPackageTableAdapter.Update(bulkPackageRow) == 1;
        }

        public static Boolean EditBulkPackage(HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow bulkPackage, HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow newBulkPackageName, int newBulkPackageCapacity)
        {
            bulkPackage.Pojemność = newBulkPackageCapacity;
            return UpdateDB(bulkPackage, "Rodzaju opakowania zbiorczego został edytowany");

        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow bulkPackage, String messageIfSuccess)
        {
            try
            {
                BulkPackageTableAdapter.Update(bulkPackage);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                _bulkPackageData = BulkPackageTableAdapter.GetData();
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