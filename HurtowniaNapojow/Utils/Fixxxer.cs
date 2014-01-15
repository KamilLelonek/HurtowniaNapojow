using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Utils
{
    public class Fixxxer
    {
        public static void FixDatabase()
        {
            if (MessageBox.Show("Fixxxer starts. MAKE COPY OF ACCDB !!!!!!! Continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
            int fixxxed = 0;

            //fix zero products
            fixxxed = 0;
            var products = DataBaseProductHelper.GetProductsData().ToList();
            products.ForEach(p =>
            {
                if (p.Liczba < 1 || p.Kwota < 0.0001)
                {
                    p.Liczba = 2 + (new Random().Next()) % 10;
                    p.Kwota = p.Liczba * DataBaseWarehouseDrinkHelper.GetDrinkById(p.id_napoju_hurtowni).CenaHurtowni;
                    DataBaseProductHelper.UpdateDB(p, "fix zero product");
                    fixxxed++;
                }
            });
            if (fixxxed > 0)
                MessageBox.Show("fix zero products .... fixxer fixed " + fixxxed);

            fixxxed = 0;
            //fix drinks left quantity to min 50
            int minLeftQuantity = 50;
            IEnumerable<HurtowniaNapojowDataSet.NapojeHurtowniRow> drinks = DataBaseWarehouseDrinkHelper.GetWarehouseDrinkData();
            drinks.ToList().ForEach(d => {
                int l = DataBaseWarehouseDrinkHelper.CalculateLeftQuantity(d);
                if (l < minLeftQuantity)
                {
                    d.LiczbaSztuk += minLeftQuantity - l;
                    DataBaseWarehouseDrinkHelper.UpdateDB(d, "fix drinks left quantity to min 50", false);
                    fixxxed++;                    
                }
            });
            if(fixxxed > 0)
                MessageBox.Show("fix drinks left quantity to min 50 .... fixxer fixed " + fixxxed);

            //fix date of shopping
            fixxxed = 0;
            var shops = DataBaseShoppingHelper.GetShoppingData().OrderBy(s => s.Identyfikator).ToList();
            DateTime lastDate = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            int count = shops.Count();
            shops.ForEach( s =>
                {
                    s.DataZłożenia = DateTime.Now.Subtract(TimeSpan.FromDays(count--));
                    DataBaseShoppingHelper.UpdateDB(s);
                });
            if (fixxxed > 0)
                MessageBox.Show("fix date of shopping .... fixxer fixed " + fixxxed);

            MessageBox.Show("Fixxxer exit");
        }
    }
}
