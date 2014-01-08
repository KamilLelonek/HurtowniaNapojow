using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using System.Data;
using HurtowniaNapojow.Windows.Employee.Warehouse.Producer;

namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.Producer
{
    /// <summary>
    /// Interaction logic for ProducerDetailsWindow.xaml
    /// </summary>
    public partial class ProducerDetailsWindow
    {
        private bool _producersFilled = false;

        public ProducerDetailsWindow()
        {
            InitializeComponent();
            if (!_producersFilled)
            {
                _producersFilled = true;
                SetProducerBinding();
            }
        }

        public void SetProducerBinding()
        {
           ProducerDataGrid.RebindContext(DataBaseProducerHelper.GetProducersData());
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerNewWindow(ref ProducerDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var producers = ProducerDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            producers.ForEach(producer => {
             HurtowniaNapojowDataSet.ProducenciRow editProducer = (HurtowniaNapojowDataSet.ProducenciRow)producer.Row;
             this.OpenWindow(new ProducerEditWindow(ref ProducerDataGrid, ref editProducer), blockPrevious: true);
            });
            SetProducerBinding();
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
                var producers = ProducerDataGrid.SelectedItems.OfType<DataRowView>().ToList();
                producers.ForEach(producer => DataBaseProducerHelper.DeleteProducerRow(producer.Row));
            }
            
        }
    }
}
