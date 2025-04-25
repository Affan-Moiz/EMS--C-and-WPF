using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ProjectVersion2.Model;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
       
    
        Dictionary<Guid, Users> users;
        Dictionary<Guid, Expenses> expenses;
        Dictionary<Guid, Salary> salaries;

        public ObservableCollection<Users> PendingUserList
        {
            get { return _pendingUserList; }
            set
            {
                if (_pendingUserList != value)
                {
                    _pendingUserList = value;
                    OnPropertyChanged(nameof(_pendingUserList)); // Notify the UI about the change
                }
            }
        }
        public ObservableCollection<Expenses> PendingExpensesList { get; set; }
        public ObservableCollection<Users> UsersList { get; set; }


        UserDataService userDataService = new UserDataService();
        ExpenseDataService expenseDataService = new ExpenseDataService();
        SalaryService salaryDataService = new SalaryService();

        public ObservableCollection<string> Roles { get { return rolesCategory; } }

        public ObservableCollection<string>? rolesCategory;

        public ObservableCollection<Users> _pendingUserList;



        public AdminViewModel()
        {
            users = userDataService.LoadUsersAsDictionary();
            expenses = expenseDataService.LoadExpensesAsDictionary();
            salaries = salaryDataService.LoadSalariesAsDictionary();


            _pendingUserList = [.. GetPendingUsers()];
            PendingExpensesList = [.. GetPendingExpenses()];
            UsersList = [.. GetAllUsers()];
            rolesCategory = [.. Enum.GetNames(typeof(Role))];


            //Categories = Enum.GetValues(typeof(ExpenseCategories)) as ExpenseCategories[];

        }

        public void AddUser(Users user)
        {
            users[user.Id] = user;
            UsersList.Add(user);
        }


        public void RemoveUser(Users user)
        {
            if (users.Remove(user.Id))
            {
                UsersList.Remove(user);
                DeleteUserExpenses(user.Id);
                DeleteUserSalaries(user.Id);

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
                //Find the user in the UsersList and update it
                var index = UsersList.IndexOf(UsersList.FirstOrDefault(u => u.Id == user.Id));
                if (index != -1)
                {
                    UsersList[index] = user;
                }else {
                    UsersList.Add(user);
                }

                //if (user.IsApproved && PendingUserList.Contains(user))
                //{
                //    PendingUserList.Remove(user);
                //}
                //else if (!user.IsApproved && !PendingUserList.Contains(user) )
                //{
                //    PendingUserList.Add(user);
                //}

                var pendingUser = PendingUserList.FirstOrDefault(u => u.Id == user.Id);

                if (user.IsApproved)
                {
                    //If Pending users list contains a user with the same id 
                    if (pendingUser != null)
                    {
                        PendingUserList.Remove(pendingUser);
                    }
                }
                else
                {
                    if (pendingUser == null)
                    {
                        PendingUserList.Add(user);
                    }
                }

            }
        }

        public void ApproveUser(Users user)
        {
            if (users.ContainsKey(user.Id))
            {
                users[user.Id].IsApproved = true;
                PendingUserList.Remove(user);
                UsersList[UsersList.IndexOf(user)].IsApproved = true;
            }
        }

        public void RejectUser(Guid userId)
        {
            if (users.TryGetValue(userId, out var user))
            {
                PendingUserList.Remove(user);
                users.Remove(userId);
                UsersList.Remove(user);
            }
        }

        public void RejectUser(Users user)
        {
            if (users.ContainsKey(user.Id))
            {
                users.Remove(user.Id);
                PendingUserList.Remove(user);
                UsersList.Remove(user);
            }
        }

        public List<Users> GetPendingUsers()
        {
            return users.Values.Where(u => !u.IsApproved).ToList();
        }

        public void AddExpense(Expenses expense)
        {
            expenses[expense.Id] = expense;
        }

        public void RemoveExpense(Guid expenseId)
        {
            expenses.Remove(expenseId);
        }

        public void RejectExpense(Guid expenseId)
        {
            if (expenses.TryGetValue(expenseId, out var expense))
            {
                expenses.Remove(expenseId);
            }
        }

        public void RejectExpense(Expenses expense)
        {
            if (expenses.ContainsKey(expense.Id))
            {
                expenses.Remove(expense.Id);
                PendingExpensesList.Remove(expense);
            }
        }

        public Expenses? GetExpenseById(Guid id)
        {
            expenses.TryGetValue(id, out var expense);
            return expense;
        }

        public void ApproveExpense(Expenses expense)
        {
            if (expenses.ContainsKey(expense.Id))
            {
                expenses[expense.Id].Status = ExpenseStatus.Approved;
                PendingExpensesList.Remove(expense);
            }
        }

        public List<Expenses> GetPendingExpenses()
        {
            return expenses.Values.Where(e => e.Status == ExpenseStatus.Pending).ToList();
        }

        private void SaveUsers()
        {
            userDataService.SaveUsersFromDictionary(users);
        }

        private void SaveExpenses()
        {
            expenseDataService.SaveExpensesFromDictionary(expenses);
        }

        private void SaveSalaries()
        {
            salaryDataService.SaveSalariesFromDictionary(salaries);
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
            }
        }

        public List<Expenses> GetExpensesById (Guid UserId)
        {
            return expenses.Values.Where(e => e.UserId == UserId).ToList();
        }

        public bool HasPendingExpenses(Users user)
        {
            return expenses.Values.Any(e => e.UserId == user.Id && e.Status == ExpenseStatus.Pending);
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
            }
        }

        public void RejectExpenseById(Guid expenseId)
        {
            if (expenses.TryGetValue(expenseId,out var expense))
            {
                expense.Status = ExpenseStatus.Rejected;
            }
        }

       public void DeleteUserExpenses(Guid UserID)
        {
            var userExpenses = expenses.Values.Where(e => e.UserId == UserID).ToList();
            foreach (var expense in userExpenses)
            {
                expenses.Remove(expense.Id);
            }
        }

        public void DeleteUserSalaries(Guid UserID)
        {
            var userSalaries = salaries.Values.Where(s => s.UserId == UserID).ToList();
            foreach (var salary in userSalaries)
            {
                salaries.Remove(salary.Id);
            }
        }

        public void Save()
        {
            SaveExpenses();
            SaveUsers();
            SaveSalaries();
        }


        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}