﻿using System;
using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class CustomerShopping
    {
        public int Id { get; set; }
        public String CustomerName { get; set; }
        public String EmployeeFirstName { get; set; }
        public String EmployeeLastName { get; set; }
        public int ProductsCount { get; set; }
        public int ProductsTotalAmount { get; set; }
        public float Price { get; set; }
        public String Date { get; set; }
        public DateTime DateRaw { get; set; }

        public HurtowniaNapojowDataSet.KlienciRow       _customerRow;
        public HurtowniaNapojowDataSet.PracownicyRow    _employeeRow;
        public HurtowniaNapojowDataSet.ZakupyKlientaRow _shoppingRow;

        public CustomerShopping(HurtowniaNapojowDataSet.KlienciRow customerRow, HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow)
        {
            _customerRow = customerRow;
            _shoppingRow = shoppingRow;
            _employeeRow = DataBaseEmployeeHelper.GetEmployeesData().First(e => e.Identyfikator == shoppingRow.id_pracownika);

            Id = _shoppingRow.Identyfikator;
            CustomerName = _customerRow.NazwaKlienta;
            EmployeeFirstName = _employeeRow.Imię;
            EmployeeLastName = _employeeRow.Nazwisko;
            Date = String.Format(Globals.DATE_FORMAT, _shoppingRow.DataZłożenia);
            DateRaw = _shoppingRow.DataZłożenia;
            ProductsCount = DataBaseShoppingHelper.CalculateShoppingProductsCount(_shoppingRow);
            ProductsTotalAmount = DataBaseShoppingHelper.CalculateShoppingProductsTotalAmount(_shoppingRow);
            Price = DataBaseShoppingHelper.CalculateShoppingPrice(_shoppingRow);
        }

        public static IEnumerable<CustomerShopping> GetCustomerShoppings(HurtowniaNapojowDataSet.KlienciRow customerRow)
        {
            var shoppings = DataBaseShoppingHelper.GetShoppingForCustomer(customerRow);
            return shoppings.Select(shopping => new CustomerShopping(customerRow, shopping));
        }
    }
}