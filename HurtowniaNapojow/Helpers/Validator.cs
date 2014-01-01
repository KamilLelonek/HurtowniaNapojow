using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HurtowniaNapojow.Helpers
{
    public sealed class Validator
    {
        private static volatile Validator _instance;
        private static readonly object Lock = new Object();

        private Validator()
        {
        }

        public static Validator Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new Validator();
                }

                return _instance;
            }
        }

        public Boolean IsEmailValid(TextBox emailTextBox)
        {
            if (emailTextBox.Text.Equals(Globals.ADMIN_NAME)) return true;

            var match = Regex.IsMatch(emailTextBox.Text, Globals.EMAIL_REGEX);
            if (match) return true;

            MessageBox.Show("Podaj poprawny adres email", Globals.TITLE_ERROR);
            return false;
        }

        public Boolean DoPasswordMatch(String newPassword, String passwordConfirmation)
        {
            if (newPassword.Equals(passwordConfirmation)) return true;

            MessageBox.Show("Podane hasła nie pasują do siebie", Globals.TITLE_ERROR);
            return false;
        }

        public Boolean AreControlsEmpty(params Control[] args)
        {
            var strings = args.Select(GetTextFromControl);
            var isDataEmpty = strings.Any(String.IsNullOrEmpty);
            if (!isDataEmpty) return false;

            MessageBox.Show("Proszę wypełnij wszystkie pola", Globals.TITLE_ERROR);
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