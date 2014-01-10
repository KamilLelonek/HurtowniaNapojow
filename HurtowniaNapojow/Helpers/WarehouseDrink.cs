using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class WarehouseDrink
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String TasteName { get; set; }
        public String GasName { get; set; }
        public String ProducerName { get; set; }
        public String TypeName { get; set; }

        public String PiecePackageName { get; set; }
        public float  PiecePackageVolume { get; set; }
        public String BulkPackageName { get; set; }

        public float  BulkPackageVolume { get; set; }
        public String PiecePackageFull { get; set; }
        public String BulkPackageFull { get; set; }
        public float BasePrice { get; set; }
        public float Price { get; set; }
        public String Date { get; set; }
        public int Quantity { get; set; }
        public String Location { get; set; }

        public HurtowniaNapojowDataSet.NapojeHurtowniRow            _warehouseDrinkRow;
        public HurtowniaNapojowDataSet.NapojeProducentaRow          _producerDrinkRow;
        public HurtowniaNapojowDataSet.NazwyNapojuRow               _drinkNameRow;
        public HurtowniaNapojowDataSet.SmakiRow                     _tasteRow;
        public HurtowniaNapojowDataSet.RodzajeGazuRow               _gasTypeRow;
        public HurtowniaNapojowDataSet.ProducenciRow                _producerRow;
        public HurtowniaNapojowDataSet.RodzajeNapojuRow             _drinkTypeRow;
        public HurtowniaNapojowDataSet.OpakowaniaSztukiRow          _piecePackageRow;
        public HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow     _piecePackageNameRow;
        public HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow        _bulkPackageRow;
        public HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow _bulkPackageNameRow;

        public WarehouseDrink() { }

        public WarehouseDrink(HurtowniaNapojowDataSet.NapojeHurtowniRow warehouseDrinkRow)
        {
            _warehouseDrinkRow = warehouseDrinkRow;
            _producerDrinkRow = DataBaseProducerDrinkHelper.GetDrinkByID(_warehouseDrinkRow.id_napoju_producenta);
            _drinkNameRow = DataBaseDrinkNameHelper.GetDrinkNameByID(_producerDrinkRow.id_nazwy_napoju);
            _tasteRow = DataBaseTasteHelper.GetTasteByID(_producerDrinkRow.id_smaku);
            _gasTypeRow = DataBaseGasTypeHelper.GetGasTypeByID(_producerDrinkRow.id_rodzaju_gazu);
            _producerRow = DataBaseProducerHelper.GetProducerByID(_producerDrinkRow.id_procuenta);
            _drinkTypeRow = DataBaseDrinkTypeHelper.GetDrinkTypeByID(_producerDrinkRow.id_rodzaju_napoju);
            _piecePackageRow = DataBasePiecePackageHelper.GetPiecePackageByID(_producerDrinkRow.id_opakowania_sztuki);
            _piecePackageNameRow = DataBasePiecePackageNameHelper.GetPiecePackageNameByID(_piecePackageRow.id_rodzaju_opakowania_sztuki);
            _bulkPackageRow = DataBaseBulkPackageHelper.GetBulkPackageByID(_producerDrinkRow.id_opakowania_zbiorczego);
            _bulkPackageNameRow = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID(_bulkPackageRow.id_rodzaju_opakowania_zbiorczego);

            Id = _warehouseDrinkRow.Identyfikator;
            Name = _drinkNameRow.NazwaNapoju;
            TasteName = _tasteRow.NazwaSmaku;
            GasName = _gasTypeRow.NazwaRodzaju;
            ProducerName = _producerRow.NazwaProducenta;
            TypeName = _drinkTypeRow.NazwaRodzajuNapoju;
            BasePrice = _drinkTypeRow.StawkaPodatkowa;
            Price = _warehouseDrinkRow.CenaHurtowni;
            Date = _warehouseDrinkRow.DataWażności.ToShortDateString();
            PiecePackageName = _piecePackageNameRow.NazwaOpakowania;
            PiecePackageVolume = _piecePackageRow.Pojemność;
            PiecePackageFull = PiecePackageName + " " + PiecePackageVolume;
            BulkPackageName = _bulkPackageNameRow.NazwaOpakowania;
            BulkPackageVolume = _bulkPackageRow.Pojemność;
            BulkPackageFull = BulkPackageName + " " + BulkPackageVolume;
            Quantity = warehouseDrinkRow.LiczbaSztuk;
            Location = warehouseDrinkRow.Położenie;
        }

        public static IEnumerable<WarehouseDrink> GetWarehouseDrinks()
        {
            var drinks = DataBaseWarehouseDrinkHelper.GetWarehouseDrinkData();
            return drinks.Select(drink => new WarehouseDrink(drink));
        }
    }
}
