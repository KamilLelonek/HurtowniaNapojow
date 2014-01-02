using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseWarehouseDrinkHelper
    {
        private static readonly NapojeHurtowniTableAdapter WarehouseDrinkTableAdapter = new NapojeHurtowniTableAdapter();

        public static HurtowniaNapojowDataSet.NapojeHurtowniDataTable GetWarehouseDrinkData()
        {
            return WarehouseDrinkTableAdapter.GetData();
        }

        public static HurtowniaNapojowDataSet.NapojeHurtowniRow GetDrinkById(int drinkId)
        {
            return GetWarehouseDrinkData().FindByIdentyfikator(drinkId);
        }

        public static float CalculateDrinkProfit(HurtowniaNapojowDataSet.NapojeHurtowniRow warehouseDrink)
        {
            var producerDrink = DataBaseProducerDrinkHelper.GetDrinkByID(warehouseDrink.id_napoju_producenta);
            return warehouseDrink.CenaHurtowni - producerDrink.CenaProducenta;
        }
    }
}