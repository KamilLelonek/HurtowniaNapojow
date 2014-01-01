using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HurtowniaNapojow.Helpers
{
    public static class Globals
    {
        public static readonly String DEFAULT_PASSWORD = "password";
        public static readonly String TITLE_SUCCESS = "Sukces";
        public static readonly String TITLE_ERROR = "Błąd";
        public static readonly String ADMIN_NAME = "admin";
        public static readonly String COOKIE_FILE_NAME = @"\cookie.hn";

        public const String EMAIL_REGEX =
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public static readonly Char COOKIE_DELIMETER = '_';
    }
}