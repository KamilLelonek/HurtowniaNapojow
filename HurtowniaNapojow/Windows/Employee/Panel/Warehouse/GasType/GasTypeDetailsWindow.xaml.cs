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
           GasTypeDataGrid.RebindContext(new RodzajeGazuTableAdapter().GetData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new GasTypeNewWindow(ref GasTypeDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tastes = GasTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            if (tastes.Count > 0)
            {
                tastes.ForEach(gasType =>
                {
                    HurtowniaNapojowDataSet.RodzajeGazuRow editGasType = (HurtowniaNapojowDataSet.RodzajeGazuRow)gasType.Row;
                    this.OpenWindow(new GasTypeEditWindow(ref GasTypeDataGrid, ref editGasType), blockPrevious: true);
                });
                SetGasTypeBinding();
            }
            else
            {
                MessageBox.Show("Nie wybrano danych do edycji, zaznacz rekord(y) przeznaczone do edycji.", "Uwaga");
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
             var gases = GasTypeDataGrid.SelectedItems.OfType<DataRowView>().ToList();
             if (gases.Count > 0)
             {
                 if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                 {
                     //do no stuff
                 }
                 else
                 {
                     //do yes stuff
                     gases.ForEach(gas => DataBaseGasTypeHelper.DeleteGasTypeRow(gas.Row));
                 }
             }
             else
             {
                 MessageBox.Show("Nie wybrano danych do usunięcia, zaznacz rekord(y) przeznaczone do usunięcia.", "Uwaga");
             }
            
        }
    }
}
