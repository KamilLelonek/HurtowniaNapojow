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
using HurtowniaNapojow.Windows.Employee.Warehouse;
using HurtowniaNapojow.Windows.Employee.Warehouse.ProducerDrink;


namespace HurtowniaNapojow.Windows.Employee.Panel.Warehouse.ProducerDrink
{
    /// <summary>
    /// Interaction logic for ProducerDrinkDetailsWindow.xaml
    /// </summary>
    public partial class ProducerDrinkDetailsWindow
    {
        private bool _ProducerDrinksFilled = false;

        public ProducerDrinkDetailsWindow()
        {
            InitializeComponent();
            if (!_ProducerDrinksFilled)
            {
                _ProducerDrinksFilled = true;
                SetProducerDrinkBinding();
            }
        }

        public void SetProducerDrinkBinding()
        {
            //var ProducerDrinkTable = Helpers.ProducerDrinkHelper.GetProducerDrink();
            //ProducerDrinkDataGrid.DataContext = ProducerDrinkTable;
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerDrinkNewWindow(ref ProducerDrinkDataGrid), blockPrevious: true);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var ProducerDrinks = ProducerDrinkDataGrid.SelectedItems.Cast<ProducerDrinkHelper>().ToList();
            //if (ProducerDrinks.Count > 0)
            //{
            //    ProducerDrinks.ForEach(ProducerDrink =>
            //    {
            //        var ProducerDrinkToEdit = DataBaseProducerDrinkHelper.GetProducerDrinkByID(ProducerDrink.Id);
            //        HurtowniaNapojowDataSet.OpakowaniaSztukiRow editProducerDrink = (HurtowniaNapojowDataSet.OpakowaniaSztukiRow)ProducerDrinkToEdit;
            //        this.OpenWindow(new ProducerDrinkEditWindow(ref ProducerDrinkDataGrid, ref editProducerDrink), blockPrevious: true);
            //    });
            //    SetProducerDrinkBinding();
            //}
            //else
            //{
            //    MessageBox.Show("Nie wybrano danych do edycji, zaznacz rekord(y) przeznaczone do edycji.", "Uwaga");
            //}
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
        //    var ProducerDrinks = ProducerDrinkDataGrid.SelectedItems.Cast<ProducerDrinkHelper>().ToList();
        //    if (ProducerDrinks.Count > 0)
        //    {
        //        if (MessageBox.Show("Czy na pewno chcesz trwale usunąć zaznaczone dane z bazy danych?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
        //        {
        //            ProducerDrinks.ForEach(ProducerDrink =>
        //                                {
        //                                    var ProducerDrinkToDelete = DataBaseProducerDrinkHelper.GetProducerDrinkByID(ProducerDrink.Id);
        //                                    DataBaseProducerDrinkHelper.DeleteProducerDrinkRow(ProducerDrinkToDelete);
        //                                });
        //            SetProducerDrinkBinding();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Nie wybrano danych do usunięcia, zaznacz rekord(y) przeznaczone do usunięcia.", "Uwaga");
        //    }
        }
    }
}
