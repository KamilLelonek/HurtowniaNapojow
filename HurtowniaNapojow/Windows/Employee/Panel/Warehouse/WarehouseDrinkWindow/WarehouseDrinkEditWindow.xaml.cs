using System;
using System.Data;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;


namespace HurtowniaNapojow.Windows.Employee.Warehouse.WarehouseDrinkWindow
{
    /// <summary>
    /// Interaction logic for WarehouseDrinkEditWindow.xaml
    /// </summary>
    public partial class WarehouseDrinkEditWindow
    {
        private ProducerDrinkHelper _producerDrink;
        private WarehouseDrink _warehouseProduct;
        HurtowniaNapojowDataSet.NapojeHurtowniRow _productWarehouseRow;
        private readonly Validator _validator = Validator.Instance;


        public WarehouseDrinkEditWindow(ref HurtowniaNapojowDataSet.NapojeHurtowniRow productWarehouseRow)
        {
            InitializeComponent();
            _productWarehouseRow = productWarehouseRow;
            _warehouseProduct = new WarehouseDrink(productWarehouseRow);
            _producerDrink = new ProducerDrinkHelper(DataBaseProducerDrinkHelper.GetDrinkByID(productWarehouseRow.id_napoju_producenta));
            SetBinding();
        }


        private void SetBinding()
        {
            DrinkDetailsGrid.DataContext = _producerDrink;
            ExpiryDate.SelectedDate = _productWarehouseRow.DataWażności;
            Location.Text = _warehouseProduct.Location;
            ProductPrice.Text = _warehouseProduct.Price.ToString();
            ProductAmount.Text = _warehouseProduct.Quantity.ToString();
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(ProductPrice, ProductAmount, Location )) return;
            int amount = 0;
            float price = _producerDrink.Price*(float)1.30;
            bool validAmount = int.TryParse(ProductAmount.Text, out amount);
            bool validPrice = float.TryParse(ProductPrice.Text, out price);
            if (!validAmount || !validPrice) return;
           
            var idProducerDrink = _producerDrink.Id;
            var quantity = amount;
            var warehousePrice = price;
            var location = Location.Text;
            var expirationDate = ExpiryDate.SelectedDate.Value;
           
            try
            {

                var result = DataBaseWarehouseDrinkHelper.EditWarehouseDrink(_productWarehouseRow, idProducerDrink, quantity, warehousePrice, location, expirationDate);
                if (!result)
                {
                    MessageBox.Show("Podane dane są nieprawidłowe", Globals.TITLE_ERROR);
                    this.Close();
                    return;

                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Błąd", Globals.TITLE_ERROR);
            }
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