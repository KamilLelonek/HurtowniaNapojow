using System;
using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class ShoppingDetailedProduct
    {
        public String DrinkName { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public String TasteName { get; set; }
        public String GasName { get; set; }
        public String ProducerName { get; set; }
        public String TypeName { get; set; }
        public String PiecePackageFull { get; set; }
        public String BulkPackageFull { get; set; }
        public String Location { get; set; }

        public ShoppingDetailedProduct(HurtowniaNapojowDataSet.ProduktyKlientaRow productRow)
        {
            var warehouseDrinkID = productRow.id_napoju_hurtowni;

            var warehouseDrink = DataBaseWarehouseDrinkHelper.GetDrinkById(warehouseDrinkID);
            var producerDrink = DataBaseProducerDrinkHelper.GetDrinkByID(warehouseDrink.id_napoju_producenta);
            var drinkNameRow = DataBaseDrinkNameHelper.GetDrinkNameByID(producerDrink.id_nazwy_napoju);
            var tasteRow = DataBaseTasteHelper.GetTasteByID(producerDrink.id_smaku);
            var gasTypeRow = DataBaseGasTypeHelper.GetGasTypeByID(producerDrink.id_rodzaju_gazu);
            var producerRow = DataBaseProducerHelper.GetProducerByID(producerDrink.id_procuenta);
            var drinkTypeRow = DataBaseDrinkTypeHelper.GetDrinkTypeByID(producerDrink.id_rodzaju_napoju);
            var piecePackageRow = DataBasePiecePackageHelper.GetPiecePackageByID(producerDrink.id_opakowania_sztuki);
            var piecePackageNameRow = DataBasePiecePackageNameHelper.GetPiecePackageNameByID(piecePackageRow.id_rodzaju_opakowania_sztuki);
            var bulkPackageRow = DataBaseBulkPackageHelper.GetBulkPackageByID(producerDrink.id_opakowania_zbiorczego);
            var bulkPackageNameRow = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID(bulkPackageRow.id_rodzaju_opakowania_zbiorczego);

            var piecePackageName = piecePackageNameRow.NazwaOpakowania;
            var piecePackageVolume = piecePackageRow.Pojemność;
            var bulkPackageName = bulkPackageNameRow.NazwaOpakowania;
            var bulkPackageVolume = bulkPackageRow.Pojemność;

            DrinkName = drinkNameRow.NazwaNapoju;
            Amount = productRow.Liczba;
            Price = productRow.Kwota;
            TasteName = tasteRow.NazwaSmaku;
            TypeName = drinkTypeRow.NazwaRodzajuNapoju;
            ProducerName = producerRow.NazwaProducenta;
            GasName = gasTypeRow.NazwaRodzaju;
            Location = warehouseDrink.Położenie;
            PiecePackageFull = piecePackageName + " " + piecePackageVolume;
            BulkPackageFull = bulkPackageName + " " + bulkPackageVolume;
        }
        public static IEnumerable<ShoppingDetailedProduct> GetDetailedProductsForShopping(HurtowniaNapojowDataSet.ZakupyKlientaRow shoppingRow)
        {
            var products = DataBaseProductHelper.GetProductsForShopping(shoppingRow);
            return products.Select(product => new ShoppingDetailedProduct(product));
        }
    }
}