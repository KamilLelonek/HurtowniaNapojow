using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using HurtowniaNapojow.Database.HurtowniaNapojowTableAdapters;

namespace HurtowniaNapojow.Helpers
{
    class SessionHelper
    {
        #region Class properties
        private static volatile SessionHelper _instance;
        private static readonly Object Lock = new object();

        public static SessionHelper Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SessionHelper();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Instance properties
        private readonly String _pathForCookie = Environment.CurrentDirectory + @"\cookie.hn";
        private readonly IEnumerable<Database.HurtowniaNapojow.PracownicyRow> _employeesDataTable;

        public Boolean IsUserSet { get; private set; }
        public bool IsCurrentUserAdmin { get; set; }
        public Database.HurtowniaNapojow.PracownicyRow CurrentEmployee { get; private set; }

        private SessionHelper()
        {
            Initialize();
            _employeesDataTable = Enumerable.AsEnumerable(new PracownicyTableAdapter().GetData());
        }
        private void Initialize()
        {
            if (!File.Exists(_pathForCookie)) return;

            String login;
            String password;

            using (var reader = new StreamReader(_pathForCookie))
            {
                var fileContent = reader.ReadLine();
                if (fileContent == null) return;

                ReadUserData(fileContent, out login, out password);
            }

            LoginUser(login, password, true);
        }
        private void ReadUserData(string fileContent, out String login, out String password)
        {
            String[] fileData;
            try
            {
                fileData = fileContent.Split('_');
            }
            catch (Exception)
            {
                login = "";
                password = "";
                return;
            }

            login = fileData[0];
            password = fileData[1];
        }

        public Boolean LoginUser(String email, String password, Boolean saveUserData)
        {
            if (email.Equals("admin") && password.Equals("password") || AreDataValid(email, password))
            {
                ManageUserCookie(email, password, saveUserData);
                SetSessionState(email, password);
                return true;
            }
            return false;
        }

        public void LogoutUser()
        {
            CurrentEmployee = null;
            IsUserSet = false;
            IsCurrentUserAdmin = false;
            DeleteUserCookie();
        }

        private bool AreDataValid(string email, string password)
        {
            var employee = from e in _employeesDataTable where e.Field<String>("Email") == email select e;
            var employeesRows = employee as IList<Database.HurtowniaNapojow.PracownicyRow> ?? employee.ToList();
            Boolean areDataValid = employeesRows.Count() != 0 && employeesRows.First().Field<String>("Hasło").Equals(password);

            if (!areDataValid) return false;

            CurrentEmployee = employeesRows.First();
            return true;
        }

        private void SetSessionState(String email, String password)
        {
            IsUserSet = !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password);
            IsCurrentUserAdmin = IsUserSet && email.Equals("admin") && password.Equals("password");
        }

        private void ManageUserCookie(String email, String password, Boolean saveData)
        {
            if (!saveData)
            {
                DeleteUserCookie();
            }
            else
            {
                SaveUserCookie(email, password);
            }
        }

        private void SaveUserCookie(String email, String password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password)) return;
            try
            {
                using (var writer = new StreamWriter(_pathForCookie))
                {
                    writer.Write(email + "_" + password);
                }
            }
            catch (IOException) { return; }
        }

        private void DeleteUserCookie()
        {
            if (File.Exists(_pathForCookie))
            {
                File.Delete(_pathForCookie);
            }
        }
        #endregion
    }
}