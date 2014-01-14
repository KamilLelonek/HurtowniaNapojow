using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Reports.Admin;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping
{
    /// <summary>
    /// Interaction logic for EmployeeNewWindow.xaml
    /// </summary>
    public partial class CustomerShoppingNewWindow
    {
        private readonly ShoppingWindow _shoppingWindow;

        public CustomerShoppingNewWindow(ShoppingWindow shoppingWindow)
        {
            InitializeComponent();
            _shoppingWindow = shoppingWindow;
            CustomersDataGrid.FontSize = Globals.DATAGRID_FONT_SIZE;
            SetCustomersBinding();
            SetCustomersComponentsEvents();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CustomersChoose_Clicked(object sender, RoutedEventArgs e)
        {
            var customerRow = (((HurtowniaNapojowDataSet.KlienciRow)((Button)sender).DataContext));
            if (customerRow == null) return;
            
            HurtowniaNapojowDataSet.ZakupyKlientaRow shopingRow = DataBaseShoppingHelper.AddNewShopping(SessionHelper.Instance.CurrentEmployee, customerRow);
            (new CustomerShoppingDetailsWindow(_shoppingWindow, new EmployeeShopping(ref shopingRow))).ShowDialog();
            _shoppingWindow.SetShoppingBinding();
            Close();
        }

        private void SetCustomersComponentsEvents()
        {
            CustomersFilterTextBox.TextChanged += (sender, args) => CustomersFilterChanged();
            CustomersFilterComboBox.SelectionChanged += (sender, args) => CustomersFilterChanged();
        }
        private void CustomersResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            CustomersFilterComboBox.Text = Globals.FILTER_SELECT;
            CustomersFilterTextBox.Text = "";
            SetCustomersBinding();
        }
        private void CustomersFilterChanged()
        {
            var comboBoxItem = CustomersFilterComboBox.SelectedItem as ComboBoxItem;
            if (comboBoxItem == null) CustomersFilterComboBox.SelectedIndex = 0;
            comboBoxItem = CustomersFilterComboBox.SelectedItem as ComboBoxItem;

            var filterType = comboBoxItem.Name.ToString();
            var filterValue = CustomersFilterTextBox.Text.ToLower();

            if (filterType == "NazwaKlienta") CustomersDataGrid.DataContext = DataBaseCustomerHelper.GetCustomersData().Where(c => c.NazwaKlienta.ToLower().Contains(filterValue)).OrderBy(c => c.NazwaKlienta);
            else if (filterType == "NIP") CustomersDataGrid.DataContext = DataBaseCustomerHelper.GetCustomersData().Where(c => c.NIP.ToLower().Contains(filterValue)).OrderBy(c => c.NIP);
            else if (filterType == "NrTelefonu") CustomersDataGrid.DataContext = DataBaseCustomerHelper.GetCustomersData().Where(c => c.NrTelefonu.ToLower().Contains(filterValue)).OrderBy(c => c.NrTelefonu);
            else if (filterType == "Email") CustomersDataGrid.DataContext = DataBaseCustomerHelper.GetCustomersData().Where(c => c.Email.ToLower().Contains(filterValue)).OrderBy(c => c.Email);
            else if (filterType == "UlicaNumer") CustomersDataGrid.DataContext = DataBaseCustomerHelper.GetCustomersData().Where(c => c.UlicaNumer.ToLower().Contains(filterValue)).OrderBy(c => c.UlicaNumer);
            else if (filterType == "MiastoKod") CustomersDataGrid.DataContext = DataBaseCustomerHelper.GetCustomersData().Where(c => c.MiastoKod.ToLower().Contains(filterValue)).OrderBy(c => c.MiastoKod);
            else SetCustomersBinding();
        }

        public void SetCustomersBinding()
        {
            CustomersDataGrid.DataContext = DataBaseCustomerHelper.GetCustomersData().OrderBy(c => c.NazwaKlienta);
        }
    }
}