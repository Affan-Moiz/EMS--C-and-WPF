using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.ViewModels
{
    public class UserViewModel
    {
        Users? CurrUser;
        Dictionary<Guid, Expenses> expenses;
        Dictionary<Guid, Salary> salaries;
        public string[]? expensesCategory;
        public string[]? paymentMethod;
        public string[]? salaryType;
        public decimal TotalExpenses { get; set; }

        

        public UserViewModel(Guid UserID)
        {
            UserDataService userDataService = new UserDataService();
            var users = userDataService.LoadUsersAsDictionary(); // Load users as a dictionary
            ExpenseDataService expenseDataService = new ExpenseDataService();
            expenses = expenseDataService.LoadExpensesAsDictionary(); // Load expenses as a dictionary
            SalaryService salaryService = new SalaryService();
            salaries = salaryService.LoadSalariesAsDictionary(); // Load salaries as a dictionary

            CurrUser = users.TryGetValue(UserID, out var user) ? user : null;
            expenses = GetExpensesByUserId(UserID).ToDictionary(e => e.Id); // Filter expenses for the current user
            salaries = GetSalariesByUserId(UserID).ToDictionary(s => s.Id); // Filter salaries for the current user

            expensesCategory = Enum.GetNames(typeof(ExpenseCategories));
            paymentMethod = Enum.GetNames(typeof(PaymentMethod));
            salaryType = Enum.GetNames(typeof(SalaryType));

            TotalExpenses = GetTotalExpensesByUserId(UserID);
        }

        public Users? GetUserByID(Guid id)
        {
            return CurrUser;
        }

        public List<Expenses> GetExpenses()
        {
            return expenses.Values.ToList();
        }

        public void AddExpense(Expenses expense)
        {
            expenses[expense.Id] = expense;
            SaveExpenses();
            TotalExpenses += expense.Amount;
        }

        public void RemoveExpense(Guid expenseId)
        {
            if (expenses.Remove(expenseId))
            {
                SaveExpenses();
            }
        }

        public Expenses? GetExpenseById(Guid id)
        {
            expenses.TryGetValue(id, out var expense);
            return expense;
        }

        public List<Expenses> GetExpensesByUserId(Guid userId)
        {
            return expenses.Values.Where(e => e.UserId == userId).ToList();
        }

        private void SaveExpenses()
        {
            ExpenseDataService expenseDataService = new ExpenseDataService();
            expenseDataService.SaveExpensesFromDictionary(expenses); // Save expenses as a dictionary
        }

        public Guid GetUserID ()
        {
            return CurrUser?.Id ?? Guid.Empty;
        }

        public string GetUserName()
        {
            return CurrUser?.Username ?? string.Empty;
        }

        public string GetUserEmail()
        {
            return CurrUser?.Email ?? string.Empty;
        }

        public string GetUserRole()
        {
            return CurrUser?.Role.ToString() ?? string.Empty;
        }

        //Get Salaries by UserId
        public List<Salary> GetSalariesByUserId(Guid userId)
        {
            return salaries.Values.Where(s => s.UserId == userId).ToList();
        }

        public void AddSalary(Salary salary)
        {
            salaries[salary.Id] = salary;
            SaveSalaries();
        }

        public void RemoveSalary(Guid salaryId)
        {
            if (salaries.Remove(salaryId))
            {
                SaveSalaries();
            }
        }

        public Salary? GetSalaryById(Guid id)
        {
            salaries.TryGetValue(id, out var salary);
            return salary;
        }

        public void SaveSalaries()
        {
            SalaryService salaryService = new SalaryService();

            salaryService.SaveSalariesFromDictionary(salaries); // Save salaries as a dictionary
        }

        public void UpdateSalary(Guid salaryId, Salary updatedSalary)
        {
            if (salaries.ContainsKey(salaryId))
            {
                salaries[salaryId] = updatedSalary;
                SaveSalaries();
            }
        }

        public decimal GetTotalExpenses()
        {
            return expenses.Values.Sum(e => e.Amount);
        }

        public decimal GetTotalExpensesByUserId(Guid userId)
        {
            return expenses.Values.Where(e => e.UserId == userId).Sum(e => e.Amount);
        }


    }
}
