using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Database.HurtowniaNapojówDataSetTableAdapters;

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
        private readonly PracownicyTableAdapter _employeesTableAdapter;

        public Boolean IsUserSet { get; private set; }
        public bool IsCurrentUserAdmin { get; set; }
        public HurtowniaNapojówDataSet.PracownicyRow CurrentEmployee { get; private set; }

        # region Initialization
        private SessionHelper()
        {
            _employeesTableAdapter = new PracownicyTableAdapter();
            Initialize();
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
        #endregion Initialization

        #region Public methods
        public Boolean LoginUser(String email, String password, Boolean saveUserData)
        {
            if (AreDataValid(email, password))
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

        public Boolean IsUserAuthenticated(String password)
        {
            return CurrentEmployee.Field<String>("Hasło").Equals(password);
        }

        public String ChangePassword(String newPassword)
        {
            CurrentEmployee.SetField("Hasło", newPassword);
            return UpdateDB();
        }

        public String ChangeEmail(String newEmail)
        {
            CurrentEmployee.Email = newEmail;
            return UpdateDB();
        }

        public String ChangeFirstName(String newFirstName)
        {
            CurrentEmployee.Imię = newFirstName;
            return UpdateDB();
        }

        public String ChangeLasttName(String newLastName)
        {
            CurrentEmployee.Nazwisko = newLastName;
            return UpdateDB();
        }

        private String UpdateDB()
        {
            try
            {
                _employeesTableAdapter.Update(CurrentEmployee);
                return null;
            }
            catch (OleDbException e)
            {
                return e.Message;
            }
        }

        #endregion Public methods

        #region Private methods
        private bool AreDataValid(string email, string password)
        {
            var employeesTable = _employeesTableAdapter.GetData();
            var employeesRows = from e in employeesTable where e.Field<String>("Email") == email select e;
            var areDataValid = employeesRows.Count() != 0 && employeesRows.First().Field<String>("Hasło").Equals(password);

            if (!areDataValid) return false;

            CurrentEmployee = employeesRows.First();
            return true;
        }

        private void SetSessionState(String email, String password)
        {
            IsUserSet = !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password);
            IsCurrentUserAdmin = IsUserSet && CurrentEmployee.Field<Boolean>("CzyAdministrator");
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
            catch (IOException) { }
        }

        private void DeleteUserCookie()
        {
            if (File.Exists(_pathForCookie))
            {
                File.Delete(_pathForCookie);
            }
        }
        #endregion Private methods
        #endregion Instance properties
    }
}