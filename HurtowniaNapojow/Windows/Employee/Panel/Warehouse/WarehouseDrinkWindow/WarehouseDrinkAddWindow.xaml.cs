using System;
using System.Data;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using System.Windows.Input;


namespace HurtowniaNapojow.Windows.Employee.Warehouse.WarehouseDrinkWindow
{
    /// <summary>
    /// Interaction logic for WarehouseDrinkAddWindow.xaml
    /// </summary>
    public partial class WarehouseDrinkAddWindow
    {
        private ProducerDrinkHelper _producerDrink;
        private WarehouseDrink _warehouseProduct;
        private readonly Validator _validator = Validator.Instance;


        public WarehouseDrinkAddWindow(ref HurtowniaNapojowDataSet.NapojeProducentaRow productRow)
        {
            InitializeComponent();
            _producerDrink = new ProducerDrinkHelper(productRow);
            _warehouseProduct = new WarehouseDrink();
            ExpiryDate.SelectedDate = DateTime.Now;
            SetBinding();
        }


        private void SetBinding()
        {
            _producerDrink.BasePrice = _producerDrink.BasePrice * 100;
            DrinkDetailsGrid.DataContext = _producerDrink;
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(ProductPrice, ProductAmount, Location )) return;
            int amount = 0;
            float price = _producerDrink.Price*(float)1.30;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            bool validPrice = float.TryParse(ProductPrice.Text, out price);
            if (!validAmount || !validPrice) { MessageBox.Show("Proszę podać poprawne Wartości", Globals.TITLE_ERROR); return; }
           
            var idProducerDrink = _producerDrink.Id;
            var quantity = amount;
            var warehousePrice = price;
            var location = Location.Text;
            var expirationDate = ExpiryDate.SelectedDate.Value;
            //var expirationDate = (DateTime)ExpiryDate.DataContext;
            DataBaseWarehouseDrinkHelper.AddNewWarehouseDrink(idProducerDrink,quantity,warehousePrice,location,expirationDate);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            

            var text = ProductPrice.Text;
            var notNumberPressed = (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && (e.Key < Key.D0 || e.Key > Key.D9);
            var separatorPressed = e.Key == Key.Decimal || e.Key == Key.OemPeriod || e.Key == Key.OemComma;
            var inputJustSeparator = String.IsNullOrEmpty(text) && separatorPressed;

            if (inputJustSeparator || !separatorPressed && notNumberPressed)
            {
                e.Handled = true;
            }
            if (text.Contains(","))
            {
                var floatingPartStartPosition = text.IndexOf(",") + 1;
                var floatingPart = text.Substring(floatingPartStartPosition);
                var cursorInFloatingPart = ProductPrice.SelectionStart >= floatingPartStartPosition;
                var precisionEqualTwo = floatingPart.Length > 1 && cursorInFloatingPart;
                if (precisionEqualTwo || separatorPressed) e.Handled = true;
            }
            if (text.Contains("."))
            {
                var floatingPartStartPosition = text.IndexOf(".") + 1;
                var floatingPart = text.Substring(floatingPartStartPosition);
                var cursorInFloatingPart = ProductPrice.SelectionStart >= floatingPartStartPosition;
                var precisionEqualTwo = floatingPart.Length > 1 && cursorInFloatingPart;
                if (precisionEqualTwo || separatorPressed) e.Handled = true;
            }
        }

        private void PriceProducerTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var text = ProductPrice.Text;
            if (!text.Contains(".")) return;

            ProductPrice.Text = text.Replace(".", ",");
            ProductPrice.SelectionStart = text.Length;
            ProductPrice.Focus();
        }

        private void PriceProducerTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProductAmountDecrease_OnClick(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            if (validAmount && amount > 0) amount -= 1;
            else amount = 0;

            ProductAmount.Text = amount + "";
           
        }

        private void ProductAmountIncrease_OnClick(object sender, RoutedEventArgs e)
        {
            int amount = 0;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            if (validAmount) amount += 1;
            else amount = 1;

            ProductAmount.Text = amount + "";
            
        }

       
    }
}