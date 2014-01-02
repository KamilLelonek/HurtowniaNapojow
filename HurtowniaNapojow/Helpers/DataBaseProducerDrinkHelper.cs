using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    public static class DataBaseProducerDrinkHelper
    {
        private static readonly NapojeProducentaTableAdapter ProducerDrinkTableAdapter = new NapojeProducentaTableAdapter();

        public static HurtowniaNapojowDataSet.NapojeProducentaDataTable GetProducerDrinkData()
        {
            return ProducerDrinkTableAdapter.GetData();
        }

        public static HurtowniaNapojowDataSet.NapojeProducentaRow GetDrinkByID(int drinkId)
        {
            return GetProducerDrinkData().FindByIdentyfikator(drinkId);
        }
    }
}