using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.GasType
{
    /// <summary>
    /// Interaction logic for GasTypeNewWindow.xaml
    /// </summary>
    public partial class GasTypeNewWindow
    {
        private readonly DataGrid _gasTypeDataGrid;

        public GasTypeNewWindow()
        {
            _gasTypeDataGrid = new DataGrid();
            InitializeComponent();
        }
        public GasTypeNewWindow(ref DataGrid gasTypeDataGrid)
        {
            _gasTypeDataGrid = gasTypeDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        
            var newNameGasType = NameTextBox.Text;

            var result = DataBaseGasTypeHelper.AddNewGasType(newNameGasType);
            if (!result) return;

            _gasTypeDataGrid.RebindContext(DataBaseGasTypeHelper.GetGasTypeData());
            CloseButton.PerformClick();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddButton.PerformClick();
            }
        }
    }
}