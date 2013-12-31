using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            EmployeeDataForm.DataContext = SessionHelper.Instance.CurrentEmployee;
        }
        protected void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Instance.LogoutUser();
            new LoginWindow().Show();
            Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (SectionEmail != null & SectionPassword != null && SectionData != null)
            {
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
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            CurrentPasswordTextBox.Password = "";
            switch (_currentSection)
            {
                    case Section.Email:
                        EmailSection_Email.Text = "";
                        break;

                    case Section.Password:
                        PasswordSection_NewPassword.Password = "";
                        PasswordSection_PasswordConfirmation.Password = "";
                        break;

                    case Section.Data:
                        DataSection_FisrtName.Text = "";
                        DataSection_LastName.Text = "";
                        break;
            }
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

            ShowResult(result, "Email został zmieniony");
        }


        private void ChangePassword()
        {
            var isDataEmpty = IsDataEmpty(PasswordSection_NewPassword, PasswordSection_PasswordConfirmation);
            if (isDataEmpty) return;

            var newPassword = PasswordSection_NewPassword.Password;
            var result = SessionHelper.Instance.ChangePassword(newPassword);

            ShowResult(result, "Hasło zostało zmienione");
        }

        private void ChangeData()
        {
            var isDataEmpty = IsDataEmpty(DataSection_FisrtName, DataSection_LastName);
            if (isDataEmpty) return;

            var firstName = DataSection_FisrtName.Text;
            var lastName = DataSection_LastName.Text;
            var result = SessionHelper.Instance.ChangeName(firstName, lastName);

            ShowResult(result, "Dane osobowe zostały zmienione");
        }
        private void ShowResult(String messageIfError, String messageIfSuccess)
        {
            if (messageIfError == null)
            {
                ButtonReset.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                MessageBox.Show(messageIfSuccess, "Sukces");
            }
            else
            {
                MessageBox.Show(messageIfError, "Błąd");
            }
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
    }
}