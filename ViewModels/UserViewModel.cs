using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ProjectVersion2.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        Users? ActiveUser;
        Dictionary<Guid, Expenses> expenses;
        Dictionary<Guid, Salary> salaries;
        public ObservableCollection<string>? expensesCategory;
        public ObservableCollection<string>? paymentMethod;
        public ObservableCollection<string>? salaryType;

        public ObservableCollection<string> Categories { get { return expensesCategory; } }
        public ObservableCollection<string> PaymentMethods { get { return paymentMethod; } }
        public ObservableCollection<string> Salaries { get { return salaryType; } }

        public ObservableCollection<Expenses> ExpensesList { get; set; }
        public ObservableCollection<Salary> SalariesList { get; set; }


        private decimal _remainingBalance;

        private decimal _totalExpenses;

        private decimal _percentSpent;

        public decimal TotalExpenses
        {
            get { return _totalExpenses; }
            set
            {
                if (_totalExpenses != value)
                {
                    _totalExpenses = value;
                    OnPropertyChanged(nameof(TotalExpenses));
                }
            }
        }

        public decimal RemainingBalance
        {
            get { return _remainingBalance; }
            set
            {
                if (_remainingBalance != value)
                {
                    _remainingBalance = value;
                    OnPropertyChanged(nameof(RemainingBalance));
                }
            }
        }

        public decimal PercentSpent
        {
            get { return _percentSpent; }
            set
            {
                if (_percentSpent != value)
                {
                    _percentSpent = value;
                    OnPropertyChanged(nameof(PercentSpent));
                }
            }
        }



        public UserViewModel(Guid UserID)
        {
            UserDataService userDataService = new();
            var users = userDataService.LoadUsersAsDictionary(); // Load users as a dictionary
            ExpenseDataService expenseDataService = new();
            expenses = expenseDataService.LoadExpensesAsDictionary(); // Load expenses as a dictionary
            SalaryService salaryService = new();
            salaries = salaryService.LoadSalariesAsDictionary(); // Load salaries as a dictionary

            ActiveUser = users.TryGetValue(UserID, out var user) ? user : null;
            expenses = GetExpensesByUserId(UserID).ToDictionary(e => e.Id); // Filter expenses for the current user
            salaries = GetSalariesByUserId(UserID).ToDictionary(s => s.Id); // Filter salaries for the current user

            expensesCategory = [.. Enum.GetNames(typeof(ExpenseCategories))];
            paymentMethod = [.. Enum.GetNames(typeof(PaymentMethod))];
            salaryType = [.. Enum.GetNames(typeof(SalaryType))];

            ExpensesList = [.. expenses.Values]; // Initialize the expenses list for the current user
            SalariesList = [.. salaries.Values]; // Initialize the salaries list for the current user

            _totalExpenses = GetTotalExpensesByUserId(UserID); // Initialize total expenses for the current user
            _remainingBalance = GetTotalIncomeByUserId(UserID); // Initialize remaining balance for the current user
            _percentSpent = GetPercentSpent(UserID); // Initialize percent spent for the current user

        }

        public decimal GetPercentSpent(Guid userId)
        {
            if (GetTotalSalariesByUserId(userId) == 0) // Avoid division by zero
                return 0;
            
            if(GetTotalExpensesByUserId(userId) == 0)
                return 0;

            PercentSpent = (GetTotalExpensesByUserId(userId) / GetTotalSalariesByUserId(userId));

            if (PercentSpent > 1)
            {
                return 1;
            }

            return (GetTotalExpensesByUserId(userId) / GetTotalSalariesByUserId(userId));
        }

        public Users? GetUserByID(Guid id)
        {
            return ActiveUser;
        }

        public List<Expenses> GetExpenses()
        {
            return expenses.Values.ToList();
        }

        public void AddExpense(Expenses expense)
        {
            expenses[expense.Id] = expense;
            ExpensesList.Add(expense); 
            RemainingBalance -= expense.Amount; 
            TotalExpenses += expense.Amount;
            PercentSpent=GetPercentSpent(ActiveUser.Id);
            SaveExpenses();

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
            expenseDataService.SaveExpensesFromDictionary(expenses); 
        }

        public Guid GetUserID()
        {
            return ActiveUser?.Id ?? Guid.Empty;
        }

        public string GetUserName()
        {
            return ActiveUser?.Username ?? string.Empty;
        }

        public string GetUserEmail()
        {
            return ActiveUser?.Email ?? string.Empty;
        }

        public string GetUserRole()
        {
            return ActiveUser?.Role.ToString() ?? string.Empty;
        }

        //Get Salaries by UserId
        public List<Salary> GetSalariesByUserId(Guid userId)
        {
            return salaries.Values.Where(s => s.UserId == userId).ToList();
        }

        public void AddSalary(Salary salary)
        {
            salaries[salary.Id] = salary;
            SalariesList.Add(salary); 
            RemainingBalance += salary.Amount;
            PercentSpent=GetPercentSpent(ActiveUser.Id);
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

        public decimal GetTotalSalariesByUserId(Guid userId)
        {
            return salaries.Values.Where(s => s.UserId == userId).Sum(s => s.Amount);
        }

        public decimal GetTotalSalaries()
        {
            return salaries.Values.Sum(s => s.Amount);
        }

        public decimal GetTotalIncome()
        {
            return GetTotalSalaries() - GetTotalExpenses();
        }

        public decimal GetTotalIncomeByUserId(Guid UserID)
        {
            return GetTotalSalariesByUserId(UserID) - GetTotalExpensesByUserId(UserID);
        }

        public void UpdateExpense(Guid expenseId, Expenses updatedExpense)
        {
            if (expenses.ContainsKey(expenseId))
            {
                expenses[expenseId] = updatedExpense;
                SaveExpenses();
            }
        }

        public void UpdateIncome(Guid userId)
        {
            RemainingBalance = GetTotalIncomeByUserId(userId);
        }

        public void UpdatePercentSpent(Guid userId)
        {
            if (GetTotalSalariesByUserId(userId) == 0) // Avoid division by zero
                PercentSpent = 0;
            else
                PercentSpent = (GetTotalExpensesByUserId(userId) / GetTotalSalariesByUserId(userId));
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
