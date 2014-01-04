using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

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
    }
}