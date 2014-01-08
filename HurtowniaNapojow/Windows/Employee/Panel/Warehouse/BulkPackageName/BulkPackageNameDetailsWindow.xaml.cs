using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackageName;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.BulkPackageName
{
    /// <summary>
    /// Interaction logic for BulkPackageNameDetailsWindow.xaml
    /// </summary>
    public partial class BulkPackageNameDetailsWindow
    {
        private bool _bulkPackageNamesFilled = false;

        public BulkPackageNameDetailsWindow()
        {
            InitializeComponent();
            if (!_bulkPackageNamesFilled)
            {
                _bulkPackageNamesFilled = true;
                SetBulkPackageNameBinding();
            }
        }

        public void SetBulkPackageNameBinding()
        {
            BulkPackageNameDataGrid.RebindContext(DataBaseBulkPackageHelper.GetBulkPackageData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new BulkPackageNameNewWindow(ref BulkPackageNameDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var bulkPackageNames = BulkPackageNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            bulkPackageNames.ForEach(bulkPackageName =>
            {
                HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow editBulkPackageName = (HurtowniaNapojowDataSet.NazwyOpakowaniaZbiorczegoRow)bulkPackageName.Row;
                this.OpenWindow(new BulkPackageNameEditWindow(ref BulkPackageNameDataGrid, ref editBulkPackageName), blockPrevious: true);
            });
            SetBulkPackageNameBinding();
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
                var bulkPackageNames = BulkPackageNameDataGrid.SelectedItems.OfType<DataRowView>().ToList();
                bulkPackageNames.ForEach(bulkPackageName => DataBaseBulkPackageNameHelper.DeleteBulkPackageNameRow(bulkPackageName.Row));
              
            }
            
        }
    }
}
