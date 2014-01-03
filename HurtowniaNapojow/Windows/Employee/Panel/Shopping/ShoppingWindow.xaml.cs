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
            (new CustomerShoppingDetailsWindow(this, null)).ShowDialog();
        }

        private void DeleteShopping_Clicked(object sender, RoutedEventArgs e)
        {
            var shoppings = ShoppingsDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            shoppings.ForEach(shopping => DataBaseShoppingHelper.DeleteShoppingRow(shopping.Row));
        }
        #endregion
    }
}