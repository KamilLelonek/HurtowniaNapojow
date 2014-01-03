using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseWarehouseDrinkHelper
    {
        private static readonly NapojeHurtowniTableAdapter WarehouseDrinkTableAdapter = new NapojeHurtowniTableAdapter();
        private static IEnumerable<HurtowniaNapojowDataSet.NapojeHurtowniRow> _warehouseDrinksData = WarehouseDrinkTableAdapter.GetData();

        public static IEnumerable<HurtowniaNapojowDataSet.NapojeHurtowniRow> GetWarehouseDrinkData()
        {
            return _warehouseDrinksData;
        }

        public static HurtowniaNapojowDataSet.NapojeHurtowniRow GetDrinkById(int drinkId)
        {
            return GetWarehouseDrinkData().First(drink => drink.Identyfikator == drinkId);
        }
    }
}