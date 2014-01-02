using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Admin
{
    class EmployeeShopping : INotifyPropertyChanged
    {
        private readonly HurtowniaNapojówDataSet.ZakupyKlientaRow _shoppingRow;
        private readonly HurtowniaNapojówDataSet.KlienciRow _customerRow;

        public int Id { get; set; }
        public String CustomerName { get; private set; }

        private String _date;
        public String Date
        {
            get
            {
                return _date;
            }
            private set
            {
                _date = value;
                OnPropertyChange();
            }
        }

        private float _profit;
        public float Profit
        {
            get
            {
                return _profit;
            }
            private set
            {
                _profit = value;
                OnPropertyChange();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public EmployeeShopping(HurtowniaNapojówDataSet.ZakupyKlientaRow shoppingRow)
        {
            _shoppingRow = shoppingRow;
            _customerRow = DataBaseCustomerHelper.GetCustomerForShopping(shoppingRow);
            InjectData();
        }

        private void InjectData()
        {
            Id = _shoppingRow.Identyfikator;
            CustomerName = _customerRow.NazwaKlienta;
            Profit = DataBaseShoppingHelper.CalculateShoppingProfit(_shoppingRow);
            Date = _shoppingRow.DataZłożenia.ToShortDateString();
        }

        private void OnPropertyChange([CallerMemberName] String caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

        public static IEnumerable<EmployeeShopping> EmployeeShoppingCollectionBuilder(HurtowniaNapojówDataSet.PracownicyRow employee)
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