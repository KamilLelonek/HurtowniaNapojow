using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.Producer
{
    /// <summary>
    /// Interaction logic for ProducerNewWindow.xaml
    /// </summary>
    public partial class ProducerNewWindow
    {
        private readonly DataGrid _producerDataGrid;

        public ProducerNewWindow()
        {
            _producerDataGrid = new DataGrid();
            InitializeComponent();
        }
        public ProducerNewWindow(ref DataGrid producerDataGrid)
        {
            _producerDataGrid = producerDataGrid;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        
            var newProducer = NameTextBox.Text;

            var result = DataBaseProducerHelper.AddNewProducer(newProducer);
            if (!result) return;

            _producerDataGrid.RebindContext(DataBaseProducerHelper.GetProducersData());
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