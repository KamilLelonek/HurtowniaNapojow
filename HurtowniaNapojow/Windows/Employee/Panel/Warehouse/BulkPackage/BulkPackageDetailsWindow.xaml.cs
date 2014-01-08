using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Utils;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse;
using HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackage;



namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.BulkPackage
{
    /// <summary>
    /// Interaction logic for BulkPackageDetailsWindow.xaml
    /// </summary>
    public partial class BulkPackageDetailsWindow
    {
        private bool _BulkPackagesFilled = false;

        public BulkPackageDetailsWindow()
        {
            InitializeComponent();
            if (!_BulkPackagesFilled)
            {
                _BulkPackagesFilled = true;
                SetBulkPackageBinding();
            }
        }

        public void SetBulkPackageBinding()
        {
            var BulkPackageTable = Helpers.BulkPackageHelper.GetBulkPackage();
            BulkPackageDataGrid.DataContext = BulkPackageTable;
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new BulkPackageNewWindow(ref BulkPackageDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var BulkPackages = BulkPackageDataGrid.SelectedItems.Cast<BulkPackageHelper>().ToList();
            BulkPackages.ForEach(BulkPackage =>
            {
                var BulkPackageToEdit = DataBaseBulkPackageHelper.GetBulkPackageByID(BulkPackage.Id);
                HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow editBulkPackage = (HurtowniaNapojowDataSet.OpakowaniaZbiorczeRow)BulkPackageToEdit;
                this.OpenWindow(new BulkPackageEditWindow(ref BulkPackageDataGrid, ref editBulkPackage), blockPrevious: true);
            });
            SetBulkPackageBinding();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                //do yes stuff
                var BulkPackages = BulkPackageDataGrid.SelectedItems.Cast<BulkPackageHelper>().ToList();
                BulkPackages.ForEach(BulkPackage =>
                                    {
                                        var BulkPackageToDelete = DataBaseBulkPackageHelper.GetBulkPackageByID(BulkPackage.Id);
                                        DataBaseBulkPackageHelper.DeleteBulkPackageRow(BulkPackageToDelete);
                                    } );
                SetBulkPackageBinding();
            }
            
        }
    }
}
