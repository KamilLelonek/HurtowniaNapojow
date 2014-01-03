using System;
using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Helpers
{
    class EmployeeShopping
    {
        private readonly HurtowniaNapojowDataSet.ZakupyKlientaRow _shoppingRow;
        private readonly HurtowniaNapojowDataSet.KlienciRow _customerRow;

        public int Id { get; set; }
        public String CustomerName { get; private set; }
        public String Date { get; set; }
        public float Price { get; set; }
        public float Profit { get; set; }

        public EmployeeShopping(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow, bool computePrice = true, bool computeProfit = true)
        {
            _shoppingRow = shoppingRow;
            _customerRow = DataBaseCustomerHelper.GetCustomerForShopping(shoppingRow);
            InjectData(computeProfit);
        }

        private void InjectData(bool computePrice = true, bool computeProfit = true)
        {
            Id = _shoppingRow.Identyfikator;
            CustomerName = _customerRow.NazwaKlienta;
            Price = computePrice ? DataBaseShoppingHelper.CalculateShoppingPrice(_shoppingRow) : 0;
            Profit = computeProfit ? DataBaseShoppingHelper.CalculateShoppingProfit(_shoppingRow) : 0;
            Date = _shoppingRow.DataZłożenia.ToShortDateString();
        }

        public static IEnumerable<EmployeeShopping> EmployeeShoppingCollectionBuilder(HurtowniaNapojowDataSet.PracownicyRow employee, bool computePrice = false, bool computeProfit = true)
        {
            var customerShoppingTableData = DataBaseShoppingHelper.GetShoppingData();
            return
                from customerShopping
                in customerShoppingTableData
                where customerShopping.id_pracownika == employee.Identyfikator
                select new EmployeeShopping(customerShopping, computePrice, computeProfit);
        }
    }
}