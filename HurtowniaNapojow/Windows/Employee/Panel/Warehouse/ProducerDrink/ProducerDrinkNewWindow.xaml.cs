using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Database.HurtowniaNapojowDataSetTableAdapters;
using HurtowniaNapojow.Utils;
using HurtowniaNapojow.Windows.Employee.Warehouse.BulkPackage;
using HurtowniaNapojow.Windows.Employee.Warehouse.Producer;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkName;
using HurtowniaNapojow.Windows.Employee.Warehouse.DrinkType;
using HurtowniaNapojow.Windows.Employee.Warehouse.Taste;
using HurtowniaNapojow.Windows.Employee.Warehouse.GasType;
using HurtowniaNapojow.Windows.Employee.Warehouse.PiecePackage;
using System.Linq;

namespace HurtowniaNapojow.Windows.Employee.Warehouse.ProducerDrink
{
    /// <summary>
    /// Interaction logic for ProducerDrinkNewWindow.xaml
    /// </summary>
    public partial class ProducerDrinkNewWindow
    {
        private readonly DataGrid _ProducerDrinkDataGrid;
        private readonly Validator _validator = Validator.Instance;

        public ProducerDrinkNewWindow()
        {
            _ProducerDrinkDataGrid = new DataGrid();
            InitializeComponent();
            SetDataBinding();
        }
        public ProducerDrinkNewWindow(ref DataGrid ProducerDrinkDataGrid)
        {
            _ProducerDrinkDataGrid = ProducerDrinkDataGrid;
            InitializeComponent();
            SetDataBinding();
        }

        private void SetDataBinding() 
        {
            DrinkNameDataBinding();
            ProducerDataBinding();
            DrinkTypeDataBinding();
            TasteDataBinding();
            GasTypeDataBinding();
            PiecePackageTypeDataBinding();
            BulkPackageTypeDataBinding();
        }

        private void DrinkNameDataBinding()
        {
            var drinkNames = DataBaseDrinkNameHelper.GetDrinkNamesData();
            var query = from item in drinkNames
                        orderby item.NazwaNapoju
                        select item;
            NameDrinkComboBox.ItemsSource = query;
        }

        private void ProducerDataBinding()
        {
            var producers = DataBaseProducerHelper.GetProducersData();
            var query = from item in producers
                        orderby item.NazwaProducenta
                        select item;
            ProducerComboBox.ItemsSource = query;
        }

        private void DrinkTypeDataBinding()
        {
            var drinkTypes = DataBaseDrinkTypeHelper.GetDrinkTypesData();
            var query = from item in drinkTypes
                        orderby item.NazwaRodzajuNapoju
                        select new { Identyfikator = item.Identyfikator, NazwaRodzajuNapoju = item.NazwaRodzajuNapoju+", Vat: "+item.StawkaPodatkowa*100+" %" };
            DrinkTypeComboBox.ItemsSource = query;
        }

        private void TasteDataBinding()
        {
            var tastes = DataBaseTasteHelper.GetTastesData();
            var query = from item in tastes
                        orderby item.NazwaSmaku
                        select item;
            TasteComboBox.ItemsSource = query;
        }

        private void GasTypeDataBinding()
        {
            var gasTypes = DataBaseGasTypeHelper.GetGasTypeData();
            var query = from item in gasTypes
                        orderby item.NazwaRodzaju
                        select item;
            GasTypeComboBox.ItemsSource = query;
        }
        private void PiecePackageTypeDataBinding()
        {
            var piecePackages = DataBasePiecePackageHelper.GetPiecePackageData();
            var piecePackageNames = DataBasePiecePackageNameHelper.GetPiecePackageNameData();
            var query = from item in piecePackages
                        join itemName in piecePackageNames on
                        item.id_rodzaju_opakowania_sztuki equals itemName.Identyfikator
                        orderby itemName.NazwaOpakowania, item.Pojemność
                        select new { Identyfikator = item.Identyfikator, NazwaOpakowania = itemName.NazwaOpakowania + ", Pojemnosć: " + item.Pojemność + "l" };
            PiecePackageComboBox.ItemsSource = query;
        }
        private void BulkPackageTypeDataBinding()
        {
            var bulkPackages = DataBaseBulkPackageHelper.GetBulkPackageData();
            var bulkPackageNames = DataBaseBulkPackageNameHelper.GetBulkPackageNameData();
            var query = from item in bulkPackages
                        join itemName in bulkPackageNames on
                        item.id_rodzaju_opakowania_zbiorczego equals itemName.Identyfikator
                        orderby itemName.NazwaOpakowania, item.Pojemność
                        select new { Identyfikator = item.Identyfikator, NazwaOpakowania = itemName.NazwaOpakowania + ", Pojemnosć sztuk:" + item.Pojemność + " " };
            BulkPackageComboBox.ItemsSource = query;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(PriceProducerTextBox)) return;
            if (_validator.IsFloatValid(PriceProducerTextBox)) return;

            if (_validator.AreComboBoxEmpty(NameDrinkComboBox)) return;
            if (_validator.AreComboBoxEmpty(ProducerComboBox)) return;
            if (_validator.AreComboBoxEmpty(DrinkTypeComboBox)) return;
            if (_validator.AreComboBoxEmpty(TasteComboBox)) return;
            if (_validator.AreComboBoxEmpty(GasTypeComboBox)) return;
            if (_validator.AreComboBoxEmpty(PiecePackageComboBox)) return;
            if (_validator.AreComboBoxEmpty(BulkPackageComboBox)) return;
            
            var newProducerDrinkName = (int)NameDrinkComboBox.SelectedValue;
            var newProducerDrinkTaste = (int)TasteComboBox.SelectedValue;
            var newProducerDrinkGasType = (int)GasTypeComboBox.SelectedValue;
            var newProducerDrinkProducer = (int)ProducerComboBox.SelectedValue;
            var newProducerDrinkType = (int)DrinkTypeComboBox.SelectedValue;
            var newProducerDrinkPiecePackageType = (int)PiecePackageComboBox.SelectedValue;
            var newProducerDrinkBulkPackageType = (int)BulkPackageComboBox.SelectedValue;
            var newProducerDrinkPrice = float.Parse(PriceProducerTextBox.Text);
            
            try
            {
               
               var result = DataBaseProducerDrinkHelper.AddNewProducerDrink(newProducerDrinkName, newProducerDrinkTaste, newProducerDrinkGasType,
                   newProducerDrinkProducer, newProducerDrinkType, newProducerDrinkPiecePackageType, newProducerDrinkBulkPackageType, newProducerDrinkPrice);
              
                if (!result)
               {
                   MessageBox.Show("Podane dane są nieprawidłowe", Globals.TITLE_ERROR);
                    return;
               }
           }
           catch (NullReferenceException)
            {
               MessageBox.Show("Błąd", Globals.TITLE_ERROR);
            }

            _ProducerDrinkDataGrid.RebindContext(DataBaseProducerDrinkHelper.GetProducerDrinkData());
            CloseButton.PerformClick();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                AddButton.PerformClick();
            }
        }

        private void PlusNewNameButton(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkNameNewWindow(), blockPrevious: true);
            DrinkNameDataBinding();
        }

        private void PlusNewProducerButton(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new ProducerNewWindow(), blockPrevious: true);
            ProducerDataBinding();
        }

        private void PlusNewDrinkTypeButton(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new DrinkTypeNewWindow(), blockPrevious: true);
            DrinkTypeDataBinding();
        }

        private void PlusNewTasteButton(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new TasteNewWindow(), blockPrevious: true);
            TasteDataBinding();
        }

        private void PlusNewGasTypeButton(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new GasTypeNewWindow(), blockPrevious: true);
            GasTypeDataBinding();
        }

        private void PlusNewPiecePackageButton(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new PiecePackageNewWindow(), blockPrevious: true);
            PiecePackageTypeDataBinding();
        }

        private void PlusNewBulkPackageButton(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new BulkPackageNewWindow(), blockPrevious: true);
            BulkPackageTypeDataBinding();
        }
    }
}