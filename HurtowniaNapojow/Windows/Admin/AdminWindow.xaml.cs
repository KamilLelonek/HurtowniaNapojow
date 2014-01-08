using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Reports.Admin;
using HurtowniaNapojow.Utils;

namespace HurtowniaNapojow.Windows.Admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow
    {
        private enum Section
        {
            Email,
            Password,
            Data
        };

        private Section _currentSection;
        private readonly Validator _validator = Validator.Instance;

        public AdminWindow()
        {
            InitializeComponent();
            SetSettingsBinding();
            SetEmployeesBinding();
            SetComponentsEvents();
        }

        private void SetEmployeesBinding()
        {
            EmployeesDataGrid.RebindContext(DataBaseEmployeeHelper.GetEmployeesData());
            ((DataTable)EmployeesDataGrid.DataContext).DefaultView.Sort = EmployeesDataGrid.Columns[1].Header.ToString();
        }

        #region Employees TAB
        #region Filter
        private void SetComponentsEvents()
        {
            FilterTextBox.TextChanged += (sender, args) => FilterChanged();
            FilterComboBox.SelectionChanged += (sender, args) => FilterChanged();
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            FilterComboBox.Text = Globals.FILTER_SELECT;
            FilterTextBox.Text = "";
            ((DataTable)EmployeesDataGrid.DataContext).DefaultView.RowFilter = "";
        }
        private void FilterChanged()
        {
            var comboBoxItem = FilterComboBox.SelectedItem as ComboBoxItem;
            if (comboBoxItem == null) FilterComboBox.SelectedIndex = 0;
            comboBoxItem = FilterComboBox.SelectedItem as ComboBoxItem;

            var filterType = comboBoxItem.Content.ToString();
            var filterValue = FilterTextBox.Text;
            var filter = String.Format("{0} LIKE '%{1}%'", filterType, filterValue);
            ((DataTable)EmployeesDataGrid.DataContext).DefaultView.RowFilter = filter;
        }

        #endregion Filter

        private void ShowDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var dataContext = button.DataContext;
            var dataRowView = (DataRowView)dataContext;
            var dataRow = dataRowView.Row;
            var employeeRow = dataRow as HurtowniaNapojowDataSet.PracownicyRow;

            this.OpenWindow(new EmployeeDetailsWindow(ref employeeRow));
        }

        private void AddNewEmployee_Clicked(object sender, RoutedEventArgs e)
        {
            this.OpenWindow(new EmployeeNewWindow(ref EmployeesDataGrid), blockPrevious: true);
        }

        private void DeleteEmployees_Clicked(object sender, RoutedEventArgs e)
        {
            var employees = EmployeesDataGrid.SelectedItems.OfType<DataRowView>().ToList();
            employees.ForEach(employee => DataBaseEmployeeHelper.DeleteEmployeeRow(employee.Row));
        }

        #endregion Employees TAB

        #region Settings TAB

        private void SetSettingsBinding()
        {
            EmployeeDataForm.RebindContext(SessionHelper.Instance.CurrentEmployee);
        }

        private void MenuLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Instance.LogoutUser();
            this.OpenWindow(new LoginWindow());
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (SectionEmail == null || SectionPassword == null || SectionData == null) return;

            var sectionEmail = sender.Equals(RadioButtonEmailSection);
            var sectionPassword = sender.Equals(RadioButtonPasswordSection);
            var sectionData = sender.Equals(RadioButtonDataSection);

            SectionEmail.IsEnabled = sectionEmail;
            SectionPassword.IsEnabled = sectionPassword;
            SectionData.IsEnabled = sectionData;

            if (sectionEmail)
            {
                _currentSection = Section.Email;
            }
            else if (sectionPassword)
            {
                _currentSection = Section.Password;
            }
            else if (sectionData)
            {
                _currentSection = Section.Data;
            }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            CurrentPasswordTextBox.Password = "";
            SetSettingsBinding();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var currentPassword = CurrentPasswordTextBox.Password;
            var isPasswordEmpty = String.IsNullOrEmpty(currentPassword);
            var isUserAuthenticated = SessionHelper.Instance.IsUserAuthenticated(currentPassword);

            if (!isUserAuthenticated)
            {
                MessageBox.Show("Podane hasło rożni się od obecnego", Globals.TITLE_ERROR);
                return;
            }
            if (isPasswordEmpty)
            {
                MessageBox.Show("Podaj proszę bieżące hasło", Globals.TITLE_ERROR);
                return;
            }

            switch (_currentSection)
            {
                case Section.Email:
                    ChangeEmail();
                    break;

                case Section.Password:
                    ChangePassword();
                    break;

                case Section.Data:
                    ChangeData();
                    break;
            }
        }

        private void ChangeEmail()
        {
            var isDataEmpty = _validator.AreControlsEmpty(EmailSection_Email);
            if (isDataEmpty) return;

            var newEmail = EmailSection_Email.Text;
            var result = SessionHelper.Instance.ChangeEmail(newEmail);

            if (result) ButtonReset.PerformClick();
        }

        private void ChangePassword()
        {
            var isDataEmpty = _validator.AreControlsEmpty(PasswordSection_NewPassword,
                PasswordSection_PasswordConfirmation);
            if (isDataEmpty) return;

            var newPassword = PasswordSection_NewPassword.Password;
            var passwordConfirmation = PasswordSection_PasswordConfirmation.Password;
            var doPasswordsMatch = _validator.DoPasswordMatch(newPassword, passwordConfirmation);

            if (!doPasswordsMatch) return;

            var result = SessionHelper.Instance.ChangePassword(newPassword);
            if (result) ButtonReset.PerformClick();
        }

        private void ChangeData()
        {
            var isDataEmpty = _validator.AreControlsEmpty(DataSection_FisrtName, DataSection_LastName);
            if (isDataEmpty) return;

            var firstName = DataSection_FisrtName.Text;
            var lastName = DataSection_LastName.Text;
            var result = SessionHelper.Instance.ChangeName(firstName, lastName);

            if (result) ButtonReset.PerformClick();
        }

        #endregion Settings TAB

        #region Reports
        private void EmployeesSummaryReport_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = DataBaseEmployeeHelper.GetEmployeesData();
            this.OpenReport(dataTable, @"Admin/EmployeesSummary.rdlc");
        }

        private void EmployeesIncomeByIncomeReport_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = EmployeeIncome.GetEmployeesIncome();
            this.OpenReport(dataTable, @"Admin/EmployeesIncomeByIncome.rdlc");
        }

        private void EmployeesIncomeByLastNameReport_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = EmployeeIncome.GetEmployeesIncome();
            this.OpenReport(dataTable, @"Admin/EmployeesIncomeByLastName.rdlc");
        }
        #endregion Reports
    }
}