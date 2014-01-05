using System;
using System.Data;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping;

namespace HurtowniaNapojow.Windows.Employee.Panel.Shopping.CustomerShopping.Product
{
    /// <summary>
    /// Interaction logic for EmployeeDetail.xaml
    /// </summary>
    public partial class ProductDetailsWindow
    {
        private readonly HurtowniaNapojowDataSet.ProduktyKlientaRow _product;
        private readonly Validator _validator = Validator.Instance;
        private readonly CustomerShoppingDetailsWindow _parentWindow;
        private readonly WarehouseDrink _drink;

        public ProductDetailsWindow(CustomerShoppingDetailsWindow parentWindow, ref HurtowniaNapojowDataSet.ProduktyKlientaRow productRow)
        {
            InitializeComponent();
            _product = productRow;
            _parentWindow = parentWindow;
            _drink = new WarehouseDrink(DataBaseWarehouseDrinkHelper.GetDrinkById(productRow.id_napoju_hurtowni));
            SetBinding();
        }


        private void SetBinding()
        {
            DrinkDetailsGrid.DataContext = _drink;
            ProductDetailsGrid.DataContext = _product;
        }

      

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.SetShoppingBinding();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataBaseProductHelper.DeleteProductRow(_product);
            _parentWindow.SetShoppingBinding();
            this.Close();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(ProductAmount, ProductPrice)) return;
            DataBaseProductHelper.UpdateDB(_product, "Dane produktu pomyślnie zaktualizowane");
            _parentWindow.SetShoppingBinding();
            this.Close();
        }

        private void ProductAmountCompute_OnClick(object sender, RoutedEventArgs e)
        {
            _product.Kwota = _product.Liczba * _drink.Price;
            ProductPrice.Text = _product.Kwota + "";
        }
    }
}