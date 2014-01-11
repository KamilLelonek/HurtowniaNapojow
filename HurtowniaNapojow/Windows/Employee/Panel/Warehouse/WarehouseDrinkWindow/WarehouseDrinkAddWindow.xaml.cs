using System;
using System.Data;
using System.Windows;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;


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
            DrinkDetailsGrid.DataContext = _producerDrink;
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
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
            //var expirationDate = (DateTime)ExpiryDate.DataContext;
            DataBaseWarehouseDrinkHelper.AddNewWarehouseDrink(idProducerDrink,quantity,warehousePrice,location,expirationDate);
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