using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class ProducerDrinkHelper
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

        public ProducerDrinkHelper() { }

        public ProducerDrinkHelper(HurtowniaNapojowDataSet.NapojeProducentaRow producerDrinkRow)
        {
            _producerDrinkRow = producerDrinkRow;
            _drinkNameRow = DataBaseDrinkNameHelper.GetDrinkNameByID(_producerDrinkRow.id_nazwy_napoju);
            _tasteRow = DataBaseTasteHelper.GetTasteByID(_producerDrinkRow.id_smaku);
            _gasTypeRow = DataBaseGasTypeHelper.GetGasTypeByID(_producerDrinkRow.id_rodzaju_gazu);
            _producerRow = DataBaseProducerHelper.GetProducerByID(_producerDrinkRow.id_procuenta);
            _drinkTypeRow = DataBaseDrinkTypeHelper.GetDrinkTypeByID(_producerDrinkRow.id_rodzaju_napoju);
            _piecePackageRow = DataBasePiecePackageHelper.GetPiecePackageByID(_producerDrinkRow.id_opakowania_sztuki);
            _piecePackageNameRow = DataBasePiecePackageNameHelper.GetPiecePackageNameByID(_piecePackageRow.id_rodzaju_opakowania_sztuki);
            _bulkPackageRow = DataBaseBulkPackageHelper.GetBulkPackageByID(_producerDrinkRow.id_opakowania_zbiorczego);
            _bulkPackageNameRow = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID(_bulkPackageRow.id_rodzaju_opakowania_zbiorczego);

            Id = _producerDrinkRow.Identyfikator;
            Name = _drinkNameRow.NazwaNapoju;
            TasteName = _tasteRow.NazwaSmaku;
            GasName = _gasTypeRow.NazwaRodzaju;
            ProducerName = _producerRow.NazwaProducenta;
            TypeName = _drinkTypeRow.NazwaRodzajuNapoju;
            BasePrice = _drinkTypeRow.StawkaPodatkowa;
            Price = (float)_producerDrinkRow.CenaProducenta;
            PiecePackageName = _piecePackageNameRow.NazwaOpakowania;
            PiecePackageVolume = _piecePackageRow.Pojemność;
            BulkPackageName = _bulkPackageNameRow.NazwaOpakowania;
            BulkPackageVolume = _bulkPackageRow.Pojemność;
            PiecePackageFull = PiecePackageName + " " + PiecePackageVolume;
            BulkPackageFull = BulkPackageName + " " + BulkPackageVolume;
           }

        public static IEnumerable<ProducerDrinkHelper> GetProducerDrinks()
        {
            var drinks = DataBaseProducerDrinkHelper.GetProducerDrinkData();
            return drinks.Select(drink => new ProducerDrinkHelper(drink));
        }

        public static IEnumerable<ProducerDrinkHelper> GetProducerDrinksLinq()
        {
            var producerDrinks = DataBaseProducerDrinkHelper.GetProducerDrinkData();
            var drinkNames = DataBaseDrinkNameHelper.GetDrinkNamesData();
            var drinkProducers = DataBaseProducerHelper.GetProducersData();
            var drinkTastes = DataBaseTasteHelper.GetTastesData();
            var drinkTypes = DataBaseDrinkTypeHelper.GetDrinkTypesData();
            var drinkGases = DataBaseGasTypeHelper.GetGasTypeData();
            var drinkPiecePackages = DataBasePiecePackageHelper.GetPiecePackageData();
            var drinkPiecePackageNames = DataBasePiecePackageNameHelper.GetPiecePackageNameData();
            var drinkBulkPackages = DataBaseBulkPackageHelper.GetBulkPackageData();
            var drinkBulkPackageNames = DataBaseBulkPackageNameHelper.GetBulkPackageNameData();

            var query = from item in producerDrinks
                        join drinkName in drinkNames on
                        item.id_nazwy_napoju equals drinkName.Identyfikator
                        join drinkProducer in drinkProducers on
                        item.id_procuenta equals drinkProducer.Identyfikator
                        join drinkTaste in drinkTastes on
                        item.id_smaku equals drinkTaste.Identyfikator
                        join drinkType in drinkTypes on
                        item.id_rodzaju_napoju equals drinkType.Identyfikator
                        join drinkGase in drinkGases on
                        item.id_rodzaju_gazu equals drinkGase.Identyfikator
                        join drinkPiecePackage in drinkPiecePackages on
                        item.id_opakowania_sztuki equals drinkPiecePackage.Identyfikator
                        join drinkPiecePackageName in drinkPiecePackageNames on
                        drinkPiecePackage.id_rodzaju_opakowania_sztuki equals drinkPiecePackageName.Identyfikator
                        join drinkBulkPackage in drinkBulkPackages on
                        item.id_opakowania_zbiorczego equals drinkBulkPackage.Identyfikator
                        join drinkBulkPackageName in drinkBulkPackageNames on
                        drinkBulkPackage.id_rodzaju_opakowania_zbiorczego equals drinkBulkPackageName.Identyfikator
                        select new ProducerDrinkHelper
                        {
                            Id = item.Identyfikator,
                            Name = drinkName.NazwaNapoju,
                            ProducerName = drinkProducer.NazwaProducenta,
                            Price = (float)item.CenaProducenta,
                            BasePrice = drinkType.StawkaPodatkowa*100,
                            TasteName = drinkTaste.NazwaSmaku,
                            TypeName = drinkType.NazwaRodzajuNapoju,
                            GasName = drinkGase.NazwaRodzaju,
                            PiecePackageName = drinkPiecePackageName.NazwaOpakowania,
                            PiecePackageVolume = drinkPiecePackage.Pojemność,
                            BulkPackageName = drinkBulkPackageName.NazwaOpakowania,
                            BulkPackageVolume = drinkBulkPackage.Pojemność
                        };
            return query;
        }
    }
}
