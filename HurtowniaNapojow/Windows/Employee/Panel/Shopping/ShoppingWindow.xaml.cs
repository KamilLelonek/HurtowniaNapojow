using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.Customer;
using HurtowniaNapojow.Windows.Employee.Panel.Warehouse;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class ShoppingWindow
    {
        private readonly Validator _validator = Validator.Instance;

        public ShoppingWindow()
        {
            InitializeComponent();
            SetCustomersBinding();
            SetShoppingBinding();
            SetProductBinding();
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
<<<<<<< HEAD
            ShoppingsDataGrid.RebindContext(EmployeeShopping.EmployeeShoppingCollectionBuilder(SessionHelper.Instance.CurrentEmployee));
=======
            ShoppingsDataGrid.RebindContext(EmployeeShopping.EmployeeShoppingCollectionBuilder(SessionHelper.Instance.CurrentEmployee, false, false));
>>>>>>> 9ca2e99e84fb3b2964ff7978e8f61f51dc58eeef
        }

        private void Shopping_ShowDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var employeeRow =
                (((DataRowView)((Button)sender).DataContext)).Row as HurtowniaNapojowDataSet.ZakupyKlientaRow;
            if (employeeRow == null) return;

            //this.OpenWindow(new EmployeeDetailsWindow(ref employeeRow));
        }

        private void DeleteShopping_Clicked(object sender, RoutedEventArgs e)
        {
            var shoppings = ShoppingsDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            shoppings.ForEach(shopping => DataBaseShoppingHelper.DeleteShoppingRow(shopping.Row));
        }
        #endregion

        #region TAB_Product
        public void SetProductBinding()
        {
            ProductsDataGrid.RebindContext(new ProduktyKlientaTableAdapter().GetData());
        }

        private void Product_ShowDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var employeeRow =
                (((DataRowView)((Button)sender).DataContext)).Row as HurtowniaNapojowDataSet.ProduktyKlientaRow;
            if (employeeRow == null) return;

            //this.OpenWindow(new EmployeeDetailsWindow(ref employeeRow));
        }

        private void DeleteProduct_Clicked(object sender, RoutedEventArgs e)
        {
            var products = ProductsDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            products.ForEach(product => DataBaseProductHelper.DeleteProductRow(product.Row));
        }
        #endregion
    }
}