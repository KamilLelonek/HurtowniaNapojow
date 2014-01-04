using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

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
            return GetDrinkTypesData().First(drinkTType => drinkTType.Identyfikator == drinkTypeId);
        }
    }
}