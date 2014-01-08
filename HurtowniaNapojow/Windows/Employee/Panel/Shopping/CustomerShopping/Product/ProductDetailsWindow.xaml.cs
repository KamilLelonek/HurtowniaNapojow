using System;
using System.Data;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
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

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(ProductPrice, ProductAmount)) return;
            int amount = _product.Liczba;
            float price = _product.Kwota;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            bool validPrice = float.TryParse(ProductPrice.Text, out price);
            if (!validAmount || !validPrice) return;
            _product.Liczba = amount;
            _product.Kwota = price;
            DataBaseProductHelper.UpdateDB(_product, "Dane produktu zaktualizowane pomyślnie");
            _parentWindow.SetShoppingBinding();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataBaseProductHelper.DeleteProductRow(_product);
            _parentWindow.SetShoppingBinding();
            this.Close();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_product.Liczba == 0 && _product.Kwota == 0)
                DeleteButton_OnClick(sender, e);
            else
                this.Close();
        }

        private void ProductAmountCompute_OnClick(object sender, RoutedEventArgs e)
        {
            int amount = _product.Liczba;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            if (validAmount)
            {
                ProductPrice.Text = amount * _drink.Price + "";
            }            
        }
    }
}