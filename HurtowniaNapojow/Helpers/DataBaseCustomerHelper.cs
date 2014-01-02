using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    class DataBaseCustomerHelper
    {
        private static readonly KlienciTableAdapter CustomerTableAdapter = new KlienciTableAdapter();

        public static HurtowniaNapojowDataSet.KlienciDataTable GetCustomersData()
        {
            return CustomerTableAdapter.GetData();
        }

        public static HurtowniaNapojowDataSet.KlienciRow GetCustomerForShopping(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow)
        {
            return GetCustomersData().FindByIdentyfikator(shoppingRow.id_klienta);
        }
    }
}