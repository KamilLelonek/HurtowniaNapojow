using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Reports.Admin;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.Customer;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping.Product;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class CustomerShoppingDetailsWindow
    {
        private readonly EmployeeShopping _shopping;
        private readonly IRebindlable _shoppingWindow;

        public CustomerShoppingDetailsWindow(IRebindlable shoppingWindow, EmployeeShopping shopping)
        {
            InitializeComponent();
            _shopping = shopping;
            _shoppingWindow = shoppingWindow;
            ProductsDataGrid.FontSize = Globals.DATAGRID_FONT_SIZE;
            SetShoppingBinding();
        }

        public void SetShoppingBinding()
        {
            _shopping.Update();
            CustomerDetailsGrid.DataContext = null;
            ProductsDataGrid.DataContext = null;
            CustomerDetailsGrid.DataContext = _shopping;
            ProductsDataGrid.DataContext = ShoppingProduct.GetProductsForShopping(_shopping._shoppingRow).OrderBy(s => s.DrinkName);
        }


        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            RebindAndClose();
        }

        private void CustomerDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            (new CustomerDetailsWindow(_shoppingWindow, _shopping._customerRow)).ShowDialog();
            SetShoppingBinding();
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            var products = ProductsDataGrid.SelectedItems.OfType<ShoppingProduct>().ToList();
            int count = products.Count();
            if (count > 0)
            {
                if (MessageBox.Show("Czy na pewno chcesz usunąć zaznacze produkty?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    products.ForEach(product => DataBaseProductHelper.DeleteProductRow(product._productRow));
                    SetShoppingBinding();
                }
            }
        }

        private void ProductDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var shopping = ((ShoppingProduct)((Button)sender).DataContext);
            (new ProductDetailsWindow(this, ref shopping._productRow)).ShowDialog();
        }

        private void ProductNewButton_Click(object sender, RoutedEventArgs e)
        {
            (new ProductNewWindow(this, _shopping)).ShowDialog();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć te zakupy klienta?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (DataBaseShoppingHelper.DeleteShoppingRow(_shopping._shoppingRow))
                {
                    RebindAndClose();
                }
            }
        }

        private void RebindAndClose()
        {
            if (_shoppingWindow != null)
            {
                _shoppingWindow.RebindData();
            }
            Close();
        }
    }
}