﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using HurtowniaNapojow.Windows.Admin;

namespace HurtowniaNapojow.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        private readonly Validator _validator = Validator.Instance;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (_validator.AreControlsEmpty(PasswordBox, TextBoxEmail) || !_validator.IsEmailValid(TextBoxEmail))
                return;

            var sessionHelper = SessionHelper.Instance;
            var email = TextBoxEmail.Text;
            var password = PasswordBox.Password;
            var saveUserData = CheckBoxRememberData.IsChecked.GetValueOrDefault(false);
            var loginSuccess = sessionHelper.LoginUser(email, password, saveUserData);

            if (!loginSuccess)
            {
                MessageBox.Show("Podane dane logowanie są niepoprawne!", Globals.TITLE_ERROR);
            }
            else
            {
                if (sessionHelper.IsCurrentUserAdmin)
                {
                    this.OpenWindow(new AdminWindow());
                }
                else
                {
                    this.OpenWindow(new EmployeeWindow());
                }
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonLogin.PerformClick();
            }
        }
    }
}