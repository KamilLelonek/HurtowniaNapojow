using System;

namespace HurtowniaNapojow.Helpers
{
    public static class Globals
    {
        public static readonly String DEFAULT_PASSWORD = "password";
        public static readonly String TITLE_SUCCESS = "Sukces";
        public static readonly String TITLE_ERROR = "Błąd";
        public static readonly String ADMIN_NAME = "admin";
        public static readonly String COOKIE_FILE_NAME = @"\cookie.hn";
        public static readonly String FILTER_SELECT = "Wybierz filtr";

        public const String DATE_FORMAT = "{0:dd/MM/yyyy}";

        public const String CODE_CITY_REGEX = @"[0-9]{2}-[0-9]{3} [ a-zA-Z]{1,92}";
        public const String EMAIL_REGEX = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public const String NIP_REGEX = @"^((\d{3}[- ]\d{3}[- ]\d{2}[- ]\d{2})|(\d{3}[- ]\d{2}[- ]\d{2}[- ]\d{3}))$";

        public static readonly Char COOKIE_DELIMETER = '_';
    }
}