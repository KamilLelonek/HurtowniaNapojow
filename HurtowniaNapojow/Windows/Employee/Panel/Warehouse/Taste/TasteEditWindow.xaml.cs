using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.Taste
{
    /// <summary>
    /// Interaction logic for TasteNewWindow.xaml
    /// </summary>
    public partial class TasteEditWindow
    {
        private readonly DataGrid _tasteDataGrid;
        private HurtowniaNapojowDataSet.SmakiRow _editTaste;

        public TasteEditWindow(ref DataGrid tasteDataGrid, ref HurtowniaNapojowDataSet.SmakiRow editTaste)
        {
           
            _tasteDataGrid = tasteDataGrid;
            _editTaste = editTaste;
            InitializeComponent();

            OldNameTextBox.Text = _editTaste.NazwaSmaku;
         }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var newNazwaSmaku = NewNameTextBox.Text;

            var result = DataBaseTasteHelper.EditTaste(_editTaste,newNazwaSmaku);
            if (!result) return;

            _tasteDataGrid.RebindContext(DataBaseTasteHelper.GetTastesData());
            CloseButton.PerformClick();
        }
    }
}