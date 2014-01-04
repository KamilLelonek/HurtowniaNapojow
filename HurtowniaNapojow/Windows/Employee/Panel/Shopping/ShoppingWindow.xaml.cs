using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Admin;
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
            ((DataTable)CustomersDataGrid.DataContext).DefaultView.RowFilter = "";
        }
        private void CustomersFilterChanged()
        {
            var comboBoxItem = CustomersFilterComboBox.SelectedItem as ComboBoxItem;
            if (comboBoxItem == null) CustomersFilterComboBox.SelectedIndex = 0;
            comboBoxItem = CustomersFilterComboBox.SelectedItem as ComboBoxItem;

            var filterType = comboBoxItem.Content.ToString();
            var filterValue = CustomersFilterTextBox.Text;
            var filter = String.Format("{0} LIKE '%{1}%'", filterType, filterValue);
            ((DataTable)CustomersDataGrid.DataContext).DefaultView.RowFilter = filter;
        }
        #endregion
        public void SetCustomersBinding()
        {
            CustomersDataGrid.RebindContext(new KlienciTableAdapter().GetData());
        }

        private void Customers_ShowDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var customerRow =
                (((DataRowView)((Button)sender).DataContext)).Row as HurtowniaNapojowDataSet.KlienciRow;
            if (customerRow == null) return;

            (new CustomerDetailsWindow(this, ref customerRow)).ShowDialog();
        }

        private void AddNewCustomer_Clicked(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new CustomerNewWindow(ref CustomersDataGrid), blockPrevious: true);
        }
        
        private void DeleteCustomer_Clicked(object sender, RoutedEventArgs e)
        {
            var customers = CustomersDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            customers.ForEach(customer => DataBaseCustomerHelper.DeleteCustomerRow(customer.Row));
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
            ShoppingsDataGrid.RebindContext(EmployeeShopping.EmployeeShoppingCollectionBuilder(SessionHelper.Instance.CurrentEmployee));
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