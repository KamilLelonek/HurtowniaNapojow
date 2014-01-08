using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class BulkPackageHelper
    {
        public int Id { get; set; }
        public String PackageName { get; set; }
        public int Capacity { get; set; }
        
        private HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow            _bulkPackageRow;
        private HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow     _bulkPackageNameRow;

        public BulkPackageHelper(HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow bulkPackageRow)
        {
            _bulkPackageRow = bulkPackageRow;
            _bulkPackageNameRow = DataBaseBulkPackageNameHelper.GetBulkPackageNameByID(_bulkPackageRow.id_rodzaju_opakowania_zbiorczego);
           

            Id = _bulkPackageRow.Identyfikator;
            PackageName = _bulkPackageNameRow.NazwaOpakowania;
            Capacity = _bulkPackageRow.Pojemność;
          
        }

        public static IEnumerable<BulkPackageHelper> GetBulkPackage()
        {
            var bulkPackages = DataBaseBulkPackageHelper.GetBulkPackageData();
            return bulkPackages.Select(bulkPackage => new BulkPackageHelper(bulkPackage));
        }
    }
}
