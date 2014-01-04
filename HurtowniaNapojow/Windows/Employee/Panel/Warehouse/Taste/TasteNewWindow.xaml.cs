using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.Taste
{
    /// <summary>
    /// Interaction logic for TasteNewWindow.xaml
    /// </summary>
    public partial class TasteNewWindow
    {
        private readonly DataGrid _tasteDataGrid;

        public TasteNewWindow()
        {
            _tasteDataGrid = new DataGrid();
            InitializeComponent();
        }
        public TasteNewWindow(ref DataGrid tasteDataGrid)
        {
            _tasteDataGrid = tasteDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        
            var newNazwaSmaku = NameTextBox.Text;

            var result = DataBaseTasteHelper.AddNewTaste(newNazwaSmaku);
            if (!result) return;

            _tasteDataGrid.RebindContext(DataBaseTasteHelper.GetTastesData());
            CloseButton.PerformClick();
        }
    }
}