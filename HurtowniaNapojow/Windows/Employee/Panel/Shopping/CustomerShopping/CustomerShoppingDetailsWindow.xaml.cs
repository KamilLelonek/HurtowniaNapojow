using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
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
        private readonly ShoppingWindow _shoppingWindow;

        public CustomerShoppingDetailsWindow(ShoppingWindow shoppingWindow, EmployeeShopping shopping)
        {
            InitializeComponent();
            _shopping = shopping;
            _shoppingWindow = shoppingWindow;
            SetShoppingBinding();
        }

        public void SetShoppingBinding()
        {
            _shopping.Update();
            CustomerDetailsGrid.DataContext = null;
            ProductsDataGrid.DataContext = null;
            CustomerDetailsGrid.DataContext = _shopping;
            ProductsDataGrid.DataContext = ShoppingProduct.GetProductsForShopping(_shopping._shoppingRow);
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //if (_validator.AreControlsEmpty(NazwaKlientaTextBox, NrTelefonuTextBox, UlicaNrTextBox)) return;
            //if (!String.IsNullOrEmpty(EmailTextBox.Text) && !_validator.IsEmailValid(EmailTextBox)) return;
            //DataBaseCustomerHelper.UpdateDB(_customer, "Dane klienta zaktualizowane pomyślnie");
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            _shoppingWindow.SetShoppingBinding();
            Close();
        }

        private void CustomerDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            (new CustomerDetailsWindow(_shoppingWindow, ref _shopping._customerRow)).ShowDialog();
            SetShoppingBinding();
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            var products = ProductsDataGrid.SelectedItems.OfType<ShoppingProduct>().ToList();
            products.ForEach(product => DataBaseProductHelper.DeleteProductRow(product._productRow));
            SetShoppingBinding();
        }

        private void ProductDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var shopping = ((ShoppingProduct)((Button)sender).DataContext);
            (new ProductDetailsWindow(this, ref shopping._productRow)).ShowDialog();
        }
    }
}