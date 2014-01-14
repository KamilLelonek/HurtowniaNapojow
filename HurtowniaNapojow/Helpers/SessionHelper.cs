using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using HurtowniaNapojow.Database;

namespace HurtowniaNapojow.Helpers
{
    internal class SessionHelper
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

        private readonly String _pathForCookie = Environment.CurrentDirectory + Globals.COOKIE_FILE_NAME;

        public Boolean IsUserSet { get; private set; }
        public bool IsCurrentUserAdmin { get; private set; }
        public HurtowniaNapojowDataSet.PracownicyRow CurrentEmployee { get; private set; }

        # region Initialization

        private SessionHelper()
        {
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
                fileData = fileContent.Split(Globals.COOKIE_DELIMETER);
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
            if (!AreDataValid(email, password)) return false;

            ManageUserCookie(email, password, saveUserData);
            SetSessionState(email, password);
            return true;
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
            return DataBaseEmployeeHelper.IsUserAuthenticated(CurrentEmployee, password);
        }

        public Boolean ChangePassword(String newPassword)
        {
            return DataBaseEmployeeHelper.ChangePassword(CurrentEmployee, newPassword);
        }

        public Boolean ChangeEmail(String newEmail)
        {
            return DataBaseEmployeeHelper.ChangeEmail(CurrentEmployee, newEmail);
        }

        public Boolean ChangeName(String newFirstName, String newLastName)
        {
            return DataBaseEmployeeHelper.ChangeName(CurrentEmployee, newFirstName, newLastName);
        }

        #endregion Public methods

        #region Private methods

        private bool AreDataValid(string email, string password)
        {
            var employeesTable = DataBaseEmployeeHelper.GetEmployeesData();
            var employeesRows = from e in employeesTable where e.Email == email select e;
            var employeeList = employeesRows as IList<HurtowniaNapojowDataSet.PracownicyRow> ?? employeesRows.ToList();
            var areDataValid = employeeList.Count() != 0 && employeeList.First().Hasło.Equals(password);

            if (!areDataValid) return false;

            CurrentEmployee = employeeList.First();
            return true;
        }

        private void SetSessionState(String email, String password)
        {
            IsUserSet = !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password);
            IsCurrentUserAdmin = IsUserSet && CurrentEmployee.CzyAdministrator;
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
                    writer.Write(email + Globals.COOKIE_DELIMETER + password);
                }
            }
            catch (IOException)
            {
            }
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