using System;
using System.Collections.Generic;
using System.Linq;
using HurtowniaNapojow.Database;
using HurtowniaNapojow.Helpers;

namespace HurtowniaNapojow.Reports.Admin
{
    public class EmployeeIncome
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public float Income { get; set; }

        public EmployeeIncome(HurtowniaNapojowDataSet.PracownicyRow employee)
        {
            Id = employee.Identyfikator;
            FirstName = employee.Imię;
            LastName = employee.Nazwisko;
            Income = DataBaseEmployeeHelper.CalculateEmployeeIncome(employee);
        }

        public static IEnumerable<EmployeeIncome> GetEmployeesIncome()
        {
            var employees = DataBaseEmployeeHelper.GetEmployeesData();
            return employees.Select(employee => new EmployeeIncome(employee));
        }
    }
}