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

        private Validator() { }

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
            var email = emailTextBox.Text;

            if (String.IsNullOrEmpty(email)) return false;
            if (email.Equals(Globals.ADMIN_NAME)) return true;

            var match = Regex.IsMatch(email, Globals.EMAIL_REGEX);
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

        public bool IsCityCodeValid(TextBox MiastoKodTextBox)
        {
            var kodMiasto = MiastoKodTextBox.Text;

            if (String.IsNullOrEmpty(kodMiasto)) return false;

            var match = Regex.IsMatch(kodMiasto, Globals.CODE_CITY_REGEX);
            if (match) return true;

            MessageBox.Show("Podaj poprawnie Kod Miasto (00-000 Miasto)", Globals.TITLE_ERROR);
            return false;
        }

        public bool IsNIPValid(TextBox NIPTextBox)
        {
            var nip = NIPTextBox.Text;

            if (String.IsNullOrEmpty(nip)) return false;

            var match = Regex.IsMatch(nip, Globals.NIP_REGEX);
            if (match) return true;

            MessageBox.Show("Podaj poprawny NIP", Globals.TITLE_ERROR);
            return false;
        }
    }
}