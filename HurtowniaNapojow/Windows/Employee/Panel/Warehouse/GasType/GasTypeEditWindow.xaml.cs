using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.GasType
{
    /// <summary>
    /// Interaction logic for GasTypeNewWindow.xaml
    /// </summary>
    public partial class GasTypeEditWindow
    {
        private readonly DataGrid _gasTypeDataGrid;
        private HurtowniaNapojowDataSet.RodzajeGazuRow _editGasType;

        public GasTypeEditWindow(ref DataGrid gasTypeDataGrid, ref HurtowniaNapojowDataSet.RodzajeGazuRow editGasType)
        {
           
            _gasTypeDataGrid = gasTypeDataGrid;
            _editGasType = editGasType;
            InitializeComponent();

            OldNameTextBox.Text = _editGasType.NazwaRodzaju;
         }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var newNameGasType = NewNameTextBox.Text;

            var result = DataBaseGasTypeHelper.EditGasType(_editGasType,newNameGasType);
            if (!result) return;

            _gasTypeDataGrid.RebindContext(DataBaseGasTypeHelper.GetGasTypeData());
            CloseButton.PerformClick();
        }
    }
}