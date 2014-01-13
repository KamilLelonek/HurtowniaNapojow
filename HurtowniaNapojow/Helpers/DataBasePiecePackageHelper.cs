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
    public static class DataBasePiecePackageHelper
    {
        private static readonly OpakowaniaSztukiTableAdapter PiecePackageTableAdapter = new OpakowaniaSztukiTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.OpakowaniaSztukiRow> _piecePackageData = PiecePackageTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.OpakowaniaSztukiRow> GetPiecePackageData()
        {
            return _piecePackageData;
        }

        public static HurtowniaNapojowDataSet.OpakowaniaSztukiRow GetPiecePackageByID(int piecePackageId)
        {
            return GetPiecePackageData().First(piecePackage => piecePackage.Identyfikator == piecePackageId);
        }

        public static Boolean AddNewPiecePackage(HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow piecePackageName, float newPieceCapacity)
        {
            var doesPiecePackageExist = GetPiecePackageData().Any(piecePackage => (piecePackage.id_rodzaju_opakowania_sztuki == piecePackageName.Identyfikator && piecePackage.Pojemność == newPieceCapacity));
            if (doesPiecePackageExist)
            {
                MessageBox.Show("Wprowadzany rodzaj opakowania sztuki już istnieje", Globals.TITLE_ERROR);
                return false;
            }
            try
            {
                PiecePackageTableAdapter.Insert(newPieceCapacity, piecePackageName.Identyfikator);
                RefreshData();
                MessageBox.Show("Pomyślnie dodano nowy rodzaj opakowania sztuki", Globals.TITLE_SUCCESS);
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                return false;
            }
        }

        public static Boolean DeletePiecePackageRow(DataRow piecePackageRow)
        {   
            var productExists = DataBaseProducerDrinkHelper.GetProducerDrinkData().Any(product => product.id_opakowania_sztuki == (piecePackageRow as HurtowniaNapojowDataSet.OpakowaniaSztukiRow).Identyfikator);
            if (productExists)
            {
                HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow tempPiecePackageName = DataBasePiecePackageNameHelper.GetPiecePackageNameByID((piecePackageRow as HurtowniaNapojowDataSet.OpakowaniaSztukiRow).id_rodzaju_opakowania_sztuki);
                MessageBox.Show("Do wybranego rodzaju opakowania sztuki '"+tempPiecePackageName.NazwaOpakowania+", "+ (piecePackageRow as HurtowniaNapojowDataSet.OpakowaniaSztukiRow).Pojemność + "' są przypisane napoje producenta. Rodzaj opakowania sztuki nie zostanie usunięty.", Globals.TITLE_ERROR);
                return false;
            }
            piecePackageRow.Delete();
            return PiecePackageTableAdapter.Update(piecePackageRow) == 1;
        }

        public static Boolean EditPiecePackage(HurtowniaNapojowDataSet.OpakowaniaSztukiRow piecePackage, HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow newPiecePackageName, float newPieceCapacity)
        {
            piecePackage.id_rodzaju_opakowania_sztuki = newPiecePackageName.Identyfikator;
            piecePackage.Pojemność = newPieceCapacity;
            return UpdateDB(piecePackage, "Rodzaju opakowania sztuki został zmieniony");
        }

        public static Boolean UpdateDB(HurtowniaNapojowDataSet.OpakowaniaSztukiRow piecePackage, String messageIfSuccess)
        {
            try
            {
                PiecePackageTableAdapter.Update(piecePackage);
                MessageBox.Show(messageIfSuccess, Globals.TITLE_SUCCESS);
                RefreshData();
                return true;
            }
            catch (OleDbException e)
            {
                MessageBox.Show(e.Message, Globals.TITLE_ERROR);
                return false;
            }
        }

        private static void RefreshData()
        {
            _piecePackageData = PiecePackageTableAdapter.GetData();
        }

    }
}