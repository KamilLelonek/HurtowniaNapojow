using System;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.Producer
{
    /// <summary>
    /// Interaction logic for ProducerEditWindow.xaml
    /// </summary>
    public partial class ProducerEditWindow
    {
        private readonly DataGrid _producerDataGrid;
        private HurtowniaNapojowDataSet.ProducenciRow _editProducer;

        public ProducerEditWindow(ref DataGrid producerDataGrid, ref HurtowniaNapojowDataSet.ProducenciRow editProducer)
        {
           
            _producerDataGrid = producerDataGrid;
            _editProducer = editProducer;
            InitializeComponent();

            OldNameTextBox.Text = _editProducer.NazwaProducenta;
         }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var newProducerName= NewNameTextBox.Text;

            var result = DataBaseProducerHelper.EditProducer(_editProducer,newProducerName);
            if (!result) return;

            _producerDataGrid.RebindContext(DataBaseProducerHelper.GetProducersData());
            CloseButton.PerformClick();
        }
    }
}