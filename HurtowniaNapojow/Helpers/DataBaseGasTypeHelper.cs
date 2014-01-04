using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseGasTypeHelper
    {
        private static readonly RodzajeGazuTableAdapter GasTypeTableAdapter = new RodzajeGazuTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.RodzajeGazuRow> _gasTypesData = GasTypeTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.RodzajeGazuRow> GetGasTypeByID()
        {
            return _gasTypesData;
        }

        public static HurtowniaNapojowDataSet.RodzajeGazuRow GetGasTypeByID(int gasTypeId)
        {
            return GetGasTypeByID().First(gasType => gasType.Identyfikator == gasTypeId);
        }
    }
}