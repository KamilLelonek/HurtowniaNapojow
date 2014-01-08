using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse.GasType;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.GasType
{
    /// <summary>
    /// Interaction logic for GasTypeDetailsWindow.xaml
    /// </summary>
    public partial class GasTypeDetailsWindow
    {
        private bool _gasesFilled = false;

        public GasTypeDetailsWindow()
        {
            InitializeComponent();
            if (!_gasesFilled)
            {
                _gasesFilled = true;
                SetGasTypeBinding();
            }
        }

        public void SetGasTypeBinding()
        {
           GasTypeDataGrid.RebindContext(DataBaseGasTypeHelper.GetGasTypeData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new GasTypeNewWindow(ref GasTypeDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tastes = GasTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            tastes.ForEach(gasType => {
             HurtowniaNapojowDataSet.RodzajeGazuRow editGasType = (HurtowniaNapojowDataSet.RodzajeGazuRow)gasType.Row;
             this.OpenWindow(new GasTypeEditWindow(ref GasTypeDataGrid, ref editGasType), blockPrevious: true);
            });
            SetGasTypeBinding();
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
                var gases = GasTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
                gases.ForEach(gas => DataBaseGasTypeHelper.DeleteGasTypeRow(gas.Row));
            }
            
        }
    }
}
