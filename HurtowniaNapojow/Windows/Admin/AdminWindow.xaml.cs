using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;
using HurtowniaNapojow.Helpers;

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
        public AdminWindow()
        {
            InitializeComponent();
            SetBinding();
            EmployeesDataGrid.DataContext = Enumerable.AsEnumerable(new PracownicyTableAdapter().GetData());
        }

        #region Employees TAB
        private void ShowDetails_Clicked(object sender, RoutedEventArgs e)
        {
            var employeeRow = (((DataRowView)((Button)sender).DataContext)).Row as HurtowniaNapojówDataSet.PracownicyRow;
            if (employeeRow == null) return;

            this.OpenWindow(new EmployeeDetails(ref employeeRow));
        }

        #endregion Employees TAB

        #region Settings TAB
        private void SetBinding()
        {
            EmployeeDataForm.DataContext = null;
            EmployeeDataForm.DataContext = SessionHelper.Instance.CurrentEmployee;
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
            SetBinding();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var currentPassword = CurrentPasswordTextBox.Password;
            var isPasswordEmpty = String.IsNullOrEmpty(currentPassword);
            var isUserAuthenticated = SessionHelper.Instance.IsUserAuthenticated(currentPassword);

            if (!isUserAuthenticated)
            {
                MessageBox.Show("Podane hasło różni się od obecnego", "Złe hasło");
                return;
            }
            if (isPasswordEmpty)
            {
                MessageBox.Show("Podaj proszę bieżące hasło", "Brak danych");
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
            var isDataEmpty = IsDataEmpty(EmailSection_Email);
            if (isDataEmpty) return;

            var newEmail = EmailSection_Email.Text;
            var result = SessionHelper.Instance.ChangeEmail(newEmail);

            if (result) ButtonReset.PerformClick();
        }

        private void ChangePassword()
        {
            var isDataEmpty = IsDataEmpty(PasswordSection_NewPassword, PasswordSection_PasswordConfirmation);
            if (isDataEmpty) return;

            var newPassword = PasswordSection_NewPassword.Password;
            var passwordConfirmation = PasswordSection_PasswordConfirmation.Password;
            var doPasswordsMatch = DoPasswordMatch(newPassword, passwordConfirmation);

            if (!doPasswordsMatch) return;

            var result = SessionHelper.Instance.ChangePassword(newPassword);
            if (result) ButtonReset.PerformClick();
        }

        private Boolean DoPasswordMatch(string newPassword, string passwordConfirmation)
        {
            if (newPassword.Equals(passwordConfirmation)) return true;

            MessageBox.Show("Podane hasła nie pasują do siebie", "Błąd");
            return false;
        }

        private void ChangeData()
        {
            var isDataEmpty = IsDataEmpty(DataSection_FisrtName, DataSection_LastName);
            if (isDataEmpty) return;

            var firstName = DataSection_FisrtName.Text;
            var lastName = DataSection_LastName.Text;
            var result = SessionHelper.Instance.ChangeName(firstName, lastName);

            if (result) ButtonReset.PerformClick();
        }

        private Boolean IsDataEmpty(params Control[] args)
        {
            var strings = args.Select(GetTextFromControl);
            var isDataEmpty = strings.Any(String.IsNullOrEmpty);
            if (!isDataEmpty) return false;

            MessageBox.Show("Proszę wypełnij wszystkie pola", "Błąd");
            return true;
        }

        private String GetTextFromControl(Control control)
        {
            var textBox = control as TextBox;
            if (textBox != null) return textBox.Text;

            var passwordBox = control as PasswordBox;
            return passwordBox != null ? passwordBox.Password : "";
        }
        #endregion Settings TAB
    }
}