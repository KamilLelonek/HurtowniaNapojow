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
        private WarehouseDrink _drink;
        private int _lastAmount;

        public ProductDetailsWindow(CustomerShoppingDetailsWindow parentWindow, ref HurtowniaNapojowDataSet.ProduktyKlientaRow productRow)
        {
            InitializeComponent();
            _product = productRow;
            _lastAmount = _product.Liczba;
            _parentWindow = parentWindow;
            SetBinding();
        }

        private void SetDrinkBinding()
        {
            _drink = new WarehouseDrink(DataBaseWarehouseDrinkHelper.GetDrinkById(_product.id_napoju_hurtowni));
            DrinkDetailsGrid.DataContext = _drink;
        }

        private void SetBinding()
        {
            SetDrinkBinding();
            ProductDetailsGrid.DataContext = _product;
        }

        private void Save()
        {
            if (_validator.AreControlsEmpty(ProductPrice, ProductAmount)) return;
            int amount = _product.Liczba;
            float price = _product.Kwota;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            bool validPrice = float.TryParse(ProductPrice.Text, out price);
            if (!validPrice) validPrice = float.TryParse(ProductPrice.Text.Replace('.', ','), out price);
            if (!validAmount || !validPrice) return;
            int delta = amount - _lastAmount;
            if (DataBaseWarehouseDrinkHelper.ValidateAmount(_product, delta))
            {
                _product.Liczba = amount;
                _product.Kwota = price;
                _lastAmount = amount;
                DataBaseProductHelper.UpdateDB(_product, "Dane produktu zaktualizowane pomyślnie");
            }
            SetDrinkBinding();
            _parentWindow.SetShoppingBinding();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DataBaseProductHelper.DeleteProductRow(_product);
                _parentWindow.SetShoppingBinding();
                this.Close();
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Save();
            if(_product.Liczba == 0 || _product.Kwota == 0)
            {
                MessageBoxResult result = MessageBox.Show("Pozostawiono wyzerowane atrybuty produktu. Czy chcesz usunąć ten produkt?", "Potwierdzenie", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    DataBaseProductHelper.DeleteProductRow(_product);
                    _parentWindow.SetShoppingBinding();
                    this.Close();
                }
                if(result == MessageBoxResult.No)
                    this.Close();

            }
            else this.Close();
        }

        private void ProductAmountDecrease_OnClick(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            if (validAmount && amount > 0) amount -= 1;
            else amount = 0;

            ProductAmount.Text = amount + "";
            ProductPrice.Text = amount * _drink.Price + "";
        }

        private void ProductAmountIncrease_OnClick(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            if (validAmount) amount += 1;
            else amount = 1;

            ProductAmount.Text = amount + "";
            ProductPrice.Text = amount * _drink.Price + "";
        }

        private void ProductPriceCompute_OnClick(object sender, RoutedEventArgs e)
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