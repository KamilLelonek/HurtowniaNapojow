using System;
using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class EmployeeShopping
    {
        public HurtowniaNapojowDataSet.ZakupyKlientaRow _shoppingRow;
        public HurtowniaNapojowDataSet.KlienciRow _customerRow;

        public int Id { get; set; }
        public String CustomerName { get; set; }
        public String Date { get; set; }
        public float Price { get; set; }

        public EmployeeShopping(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow)
        {
            _shoppingRow = shoppingRow;
            Update();
        }

        public void Update()
        {
            _customerRow = DataBaseCustomerHelper.GetCustomerForShopping(_shoppingRow);
            InjectData();
        }

        private void InjectData()
        {
            Id = _shoppingRow.Identyfikator;
            CustomerName = _customerRow.NazwaKlienta;
            Price = DataBaseShoppingHelper.CalculateShoppingPrice(_shoppingRow);
            Date = _shoppingRow.DataZłożenia.ToShortDateString();
        }

        public static IEnumerable<EmployeeShopping> EmployeeShoppingCollectionBuilder(HurtowniaNapojowDataSet.PracownicyRow employee)
        {
            var customerShoppingTableData = DataBaseShoppingHelper.GetShoppingData();
            return
                from customerShopping
                in customerShoppingTableData
                where customerShopping.id_pracownika == employee.Identyfikator
                select new EmployeeShopping(customerShopping);
        }
    }
}