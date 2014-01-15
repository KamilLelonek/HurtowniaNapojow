using System;
using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Reports.Admin
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

            var idNapojuHurtowni = productRow.id_napoju_hurtowni;
            var napojHurtowni = DataBaseWarehouseDrinkHelper.GetDrinkById(idNapojuHurtowni);
            var idNapojuProducenta = napojHurtowni.id_napoju_producenta;
            var napojProducenta = DataBaseProducerDrinkHelper.GetDrinkByID(idNapojuProducenta);

            _drinkRow = napojProducenta;

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
