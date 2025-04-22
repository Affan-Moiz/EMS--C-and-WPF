using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ProjectVersion2.Model;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.ViewModels
{
    public class AdminUserModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
       
    
        Dictionary<Guid, Users> users;
        Dictionary<Guid, Expenses> expenses;

        public ExpenseCategories[] Categories;

        public AdminUserModel()
        {
            UserDataService userDataService = new UserDataService();
            users = userDataService.LoadUsersAsDictionary();
            ExpenseDataService expenseDataService = new ExpenseDataService();
            expenses = expenseDataService.LoadExpensesAsDictionary();

            Categories = Enum.GetValues(typeof(ExpenseCategories)) as ExpenseCategories[];

        }

        public void AddUser(Users user)
        {
            users[user.Id] = user;
            SaveUsers();
        }

        public void RemoveUser(Guid userId)
        {
            if (users.Remove(userId)) 
            {
                SaveUsers();
            }
        }

        public Users? GetUserById(Guid id)
        {
            users.TryGetValue(id, out var user);
            return user;
        }

        public Users? GetUserByUsername(string username)
        {
            return users.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void UpdateUser(Users user)
        {
            if (users.ContainsKey(user.Id))
            {
                users[user.Id] = user;
                SaveUsers();
            }
        }

        public void ApproveUser(Guid userId)
        {
            if (users.TryGetValue(userId, out var user))
            {
                user.IsApproved = true;
                SaveUsers();
            }
        }

        public void RejectUser(Guid userId)
        {
            if (users.TryGetValue(userId, out var user))
            {
                user.IsApproved = false;
                SaveUsers();
            }
        }

        public List<Users> GetPendingUsers()
        {
            return users.Values.Where(u => !u.IsApproved).ToList();
        }

        public void AddExpense(Expenses expense)
        {
            expenses[expense.Id] = expense;
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

        public void ApproveExpense(Guid expenseId)
        {
            if (expenses.TryGetValue(expenseId, out var expense))
            {
                expense.Status = ExpenseStatus.Approved;

                foreach (var payeeId in expense.Payees ?? new List<Guid>())
                {
                    if (users.TryGetValue(payeeId, out var payee))
                    {
                        var newExpense = new Expenses
                        {
                            Id = Guid.NewGuid(),
                            UserId = payee.Id,
                            Amount = expense.Amount,
                            Description = expense.Description,
                            Category = expense.Category,
                            PMethod = expense.PMethod,
                            Status = ExpenseStatus.Approved,
                            Date = expense.Date,
                            IsRecurring = expense.IsRecurring
                        };
                        expenses[newExpense.Id] = newExpense;
                    }
                }

                SaveExpenses();
            }
        }

        public List<Expenses> GetPendingExpenses()
        {
            return expenses.Values.Where(e => e.Status == ExpenseStatus.Pending).ToList();
        }

        private void SaveUsers()
        {
            UserDataService userDataService = new UserDataService();
            userDataService.SaveUsersFromDictionary(users);
        }

        private void SaveExpenses()
        {
            ExpenseDataService expenseDataService = new ExpenseDataService();
            expenseDataService.SaveExpensesFromDictionary(expenses);
        }

        public List<Users> GetAllUsers()
        {
            return users.Values.ToList();
        }

        public void UpdateExpense (Expenses expense)
        {
            if (expenses.ContainsKey(expense.Id))
            {
                expenses[expense.Id] = expense;
                SaveExpenses();
            }
        }

        public List<Expenses> GetExpensesById (Guid UserId)
        {
            return expenses.Values.Where(e => e.UserId == UserId).ToList();
        }

        public List<Expenses> GetAllExpenses()
        {
            return expenses.Values.ToList();
        }

        public void ApproveExpenseById(Guid expenseId)
        {
            if(expenses.TryGetValue(expenseId, out var expense))
            {
                expense.Status = ExpenseStatus.Approved;
                SaveExpenses();
            }
        }

        public void RejectExpenseById(Guid expenseId)
        {
            if (expenses.TryGetValue(expenseId,out var expense))
            {
                expense.Status = ExpenseStatus.Rejected;
                SaveExpenses();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}