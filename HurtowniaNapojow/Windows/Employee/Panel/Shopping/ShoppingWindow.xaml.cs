using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Reports.Admin;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.Customer;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class ShoppingWindow
    {
        private readonly Validator _validator = Validator.Instance;
        private bool _customersFilled = false;
        private bool _shoppingsFilled = false;

        public ShoppingWindow()
        {
            InitializeComponent();
            SetCustomersComponentsEvents();
            SetShoppingsComponentsEvents();
            CustomersDataGrid.FontSize = 18;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomersTab.IsSelected && !_customersFilled)
            {
                _customersFilled = true;
                SetCustomersBinding();
            }
            if (ShoppingsTab.IsSelected && !_shoppingsFilled)
            {
                _shoppingsFilled = true;
                SetShoppingBinding();
            }
        }

        private void MenuShoppingPanel_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ShoppingWindow());
        }

        private void MenuWarehousePanel_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new WarehouseWindow());
        }

        private void MenuBack_Click(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new EmployeeWindow());
        }

        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Instance.LogoutUser();
            this.OpenWindow(new LoginWindow());
        }

        #region TAB_Customers
        #region TAB_CustomersFilter
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
        #endregion
        public void SetCustomersBinding()
        {
            CustomersFilterComboBox.Text = Globals.FILTER_SELECT;
            CustomersFilterTextBox.Text = "";
            CustomersDataGrid.RebindContext(DataBaseCustomerHelper.GetCustomersData().OrderBy(c => c.NazwaKlienta));
        }

        private void Customers_ShowDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var customerRow = (((HurtowniaNapojowDataSet.KlienciRow)((Button)sender).DataContext));
            if (customerRow == null) return;
            (new CustomerDetailsWindow(this, customerRow)).ShowDialog();
        }

        private void AddNewCustomer_Clicked(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new CustomerNewWindow(this), blockPrevious: true);
        }
        
        private void DeleteCustomer_Clicked(object sender, RoutedEventArgs e)
        {
            var customers = CustomersDataGrid.SelectedItems.OfType<HurtowniaNapojowDataSet.KlienciRow>().ToList();
            int count = customers.Count();
            customers.ForEach(customer => DataBaseCustomerHelper.DeleteCustomerRow(customer));
            if(count > 0)
                SetCustomersBinding();
        }
        private void Customers_AddNewShopping_Clicked(object sender, RoutedEventArgs e)
        {
            var customerRow = (((HurtowniaNapojowDataSet.KlienciRow)((Button)sender).DataContext));
            if (customerRow == null) return;
            HurtowniaNapojowDataSet.ZakupyKlientaRow shopingRow = DataBaseShoppingHelper.AddNewShopping(SessionHelper.Instance.CurrentEmployee, customerRow);
            (new CustomerShoppingDetailsWindow(this, new EmployeeShopping(ref shopingRow))).ShowDialog();
            SetShoppingBinding();
        }
        #endregion

        #region TAB_Shopping
        #region TAB_CustomersFilter
        private void SetShoppingsComponentsEvents()
        {
            ShoppingsFilterTextBox.TextChanged += (sender, args) => ShoppingsFilterChanged();
        }
        private void ShoppingsResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ShoppingsFilterTextBox.Text = "";
            SetShoppingBinding();
        }
        private void ShoppingsFilterChanged()
        {
            var filterValue = ShoppingsFilterTextBox.Text.ToLower();
            ShoppingsDataGrid.RebindContext(EmployeeShopping.EmployeeShoppingCollectionBuilder(SessionHelper.Instance.CurrentEmployee).Where(e => e.CustomerName.ToLower().Contains(filterValue)));
        }
        #endregion
        public void SetShoppingBinding()
        {
            ShoppingsDataGrid.RebindContext(EmployeeShopping.EmployeeShoppingCollectionBuilder(SessionHelper.Instance.CurrentEmployee).OrderBy(c => c.CustomerName));
        }

        private void Shopping_ShowDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var shopping = ((EmployeeShopping)((Button)sender).DataContext);
            (new CustomerShoppingDetailsWindow(this, shopping)).ShowDialog();
        }

        private void AddNewCustomerShopping_Clicked(object sender, RoutedEventArgs e)
        {
            (new CustomerShoppingNewWindow(this)).ShowDialog();
        }

        private void DeleteShopping_Clicked(object sender, RoutedEventArgs e)
        {
            var shoppings = ShoppingsDataGrid.SelectedItems.OfType<EmployeeShopping>().ToList();
            int count = shoppings.Count();
            shoppings.ForEach(shopping => DataBaseShoppingHelper.DeleteShoppingRow(shopping._shoppingRow));
            if(count > 0)
                SetShoppingBinding();
        }
        #endregion

        
    }
}