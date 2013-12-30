using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Windows.Admin;

namespace HurtowniaNapojow.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private const String EmailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper sessionHelper = SessionHelper.Instance;
            String email = TextBoxEmail.Text;
            String password = PasswordBox.Password;

            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password) || !(email.Equals("admin") || Regex.IsMatch(email, EmailRegex)))
            {
                MessageBox.Show("Uzupełnij poprawnie wszystkie dane!", "Błąd logowania");
                return;
            }

            Boolean saveUserData = CheckBoxRememberData.IsChecked.GetValueOrDefault(false);
            Boolean loginSuccess = sessionHelper.LoginUser(email, password, saveUserData);

            if (!loginSuccess)
            {
                MessageBox.Show("Podane dane logowanie są niepoprawne!", "Błąd logowania");
            }
            else
            {
                if (sessionHelper.IsCurrentUserAdmin)
                {
                    new AdminWindow().Show();
                }
                else
                {
                    new EmployeeWindow().Show();
                }
                Close();
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonLogin.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }
}