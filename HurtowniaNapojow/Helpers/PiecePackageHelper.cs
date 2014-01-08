using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    public class PiecePackageHelper
    {
        public int Id { get; set; }
        public String PackageName { get; set; }
        public float Capacity { get; set; }
        
        private HurtowniaNapojowDataSet.OpakowaniaSztukiRow          _piecePackageRow;
        private HurtowniaNapojowDataSet.NazwyOpakowaniaSztukiRow     _piecePackageNameRow;

        public PiecePackageHelper(HurtowniaNapojowDataSet.OpakowaniaSztukiRow piecePackageRow)
        {
            _piecePackageRow = piecePackageRow;
            _piecePackageNameRow = DataBasePiecePackageNameHelper.GetPiecePackageNameByID(_piecePackageRow.id_rodzaju_opakowania_sztuki);
           

            Id = _piecePackageRow.Identyfikator;
            PackageName = _piecePackageNameRow.NazwaOpakowania;
            Capacity = (float)_piecePackageRow.Pojemność;
          
        }

        public static IEnumerable<PiecePackageHelper> GetPiecePackage()
        {
            var piecePackages = DataBasePiecePackageHelper.GetPiecePackageData();
            return piecePackages.Select(piecePackage => new PiecePackageHelper(piecePackage));
        }
    }
}
