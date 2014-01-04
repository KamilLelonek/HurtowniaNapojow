using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class ShoppingProduct
    {
        public int Id { get; set; }
        public String DrinkName { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }

        public HurtowniaNapojowDataSet.ZakupyKlientaRow     _shoppingRow;
        public HurtowniaNapojowDataSet.ProduktyKlientaRow   _productRow;
        public HurtowniaNapojowDataSet.NapojeProducentaRow    _drinkRow;

        public ShoppingProduct(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow, HurtowniaNapojowDataSet.ProduktyKlientaRow productRow)
        {
            _shoppingRow = shoppingRow;
            _productRow = productRow;
            _drinkRow = DataBaseProducerDrinkHelper.GetDrinkByID(DataBaseWarehouseDrinkHelper.GetDrinkById(productRow.id_napoju_hurtowni).id_napoju_producenta);

            Id = _productRow.Identyfikator;
            DrinkName = DataBaseDrinkNameHelper.GetDrinkNameByID(_drinkRow.id_nazwy_napoju).NazwaNapoju;
            Amount = _productRow.Liczba;
            Price = _productRow.Kwota;
        }


        public static IEnumerable<ShoppingProduct> GetProductsForShopping(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow)
        {
            var products = DataBaseProductHelper.GetProductsForShopping(shoppingRow);
            return products.Select(product => new ShoppingProduct(shoppingRow, product));
        }
    }
}
