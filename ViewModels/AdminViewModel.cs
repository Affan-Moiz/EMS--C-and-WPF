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
        // Event
        public event PropertyChangedEventHandler? PropertyChanged;

        // Fields
        private Dictionary<Guid, Users> users;
        private Dictionary<Guid, Expenses> expenses;
        private Dictionary<Guid, Salary> salaries;

        private ObservableCollection<Users> _pendingUserList;
        private ObservableCollection<Expenses> _pendingExpensesList;
        private ObservableCollection<Expenses> _expensesList;
        private ObservableCollection<Salary> _salariesList;
        private ObservableCollection<Users> _usersList;
        private ObservableCollection<String> _availableUsersList;
        private Users _activeUser;
        private EncryptorDecryptor _encryptorDecryptor;

        private ObservableCollection<string>? rolesCategory;
        private ObservableCollection<string>? expensesCategory;
        private ObservableCollection<string>? paymentMethod;

        // Services
        private readonly UserDataService userDataService = new UserDataService();
        private readonly ExpenseDataService expenseDataService = new ExpenseDataService();
        private readonly SalaryService salaryDataService = new SalaryService();

        // Properties
        public ObservableCollection<string> Roles => rolesCategory;
        public ObservableCollection<string> Categories => expensesCategory;
        public ObservableCollection<string> PaymentMethods => paymentMethod;

        public Users ActiveUser
        {
            get => _activeUser;
            set
            {
                if (_activeUser != value)
                {
                    _activeUser = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Users> PendingUserList
        {
            get => _pendingUserList;
            set
            {
                if (_pendingUserList != value)
                {
                    _pendingUserList = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<String> AvailableUsersList
        {
            get => _availableUsersList;
            set
            {
                if (_availableUsersList != value)
                {
                    _availableUsersList = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Expenses> PendingExpensesList
        {
            get => _pendingExpensesList;
            set
            {
                if (_pendingExpensesList != value)
                {
                    _pendingExpensesList = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Expenses> ExpensesList
        {
            get => _expensesList;
            set
            {
                if (_expensesList != value)
                {
                    _expensesList = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Users> UsersList
        {
            get => _usersList;
            set
            {
                if (_usersList != value)
                {
                    _usersList = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Salary> SalariesList
        {
            get => _salariesList;
            set
            {
                if (_salariesList != value)
                {
                    _salariesList = value;
                    OnPropertyChanged();
                }
            }
        }

        // Constructor
        public AdminViewModel(Guid Id)
        {
            users = userDataService.LoadUsersAsDictionary();
            expenses = expenseDataService.LoadExpensesAsDictionary();
            salaries = salaryDataService.LoadSalariesAsDictionary();

            _pendingUserList = new ObservableCollection<Users>(GetPendingUsers());
            _pendingExpensesList = new ObservableCollection<Expenses>(GetAllPendingExpenses());
            _usersList = new ObservableCollection<Users>(GetAllUsers());
            _expensesList = new ObservableCollection<Expenses>(GetAllExpenses());
            _salariesList = new ObservableCollection<Salary>(GetAllSalaries());
            _availableUsersList = new ObservableCollection<String>(GetApprovedUsernames());
            _activeUser = GetUserById(Id);
            _encryptorDecryptor = new();

            rolesCategory = new ObservableCollection<string>(Enum.GetNames(typeof(Role)));
            expensesCategory = new ObservableCollection<string>(Enum.GetNames(typeof(ExpenseCategories)));
            paymentMethod = new ObservableCollection<string>(Enum.GetNames(typeof(PaymentMethod)));
        }

        // User Management Methods
        public void AddUser(Users user)
        {
            user.HashedPassword = _encryptorDecryptor.MD5Hash(user.HashedPassword);
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

        public Users? GetUserById(Guid id) => users.TryGetValue(id, out var user) ? user : null;

        //Get a list of all the usernames of the users that are approved
        public List<string> GetApprovedUsernames() => users.Values.Where(u => u.IsApproved).Select(u => u.Username).ToList();

        public Users? GetUserByUsername(string username) =>
            users.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        public void UpdateUser(Users user)
        {
            if (users.ContainsKey(user.Id))
            {
                users[user.Id] = user;
                var index = UsersList.IndexOf(UsersList.FirstOrDefault(u => u.Id == user.Id));
                if (index != -1)
                {
                    UsersList[index] = user;
                }
                else
                {
                    UsersList.Add(user);
                }

                var pendingUser = PendingUserList.FirstOrDefault(u => u.Id == user.Id);
                if (user.IsApproved)
                {
                    if (pendingUser != null) PendingUserList.Remove(pendingUser);
                }
                else
                {
                    if (pendingUser == null) PendingUserList.Add(user);
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

        public List<Users> GetPendingUsers() => users.Values.Where(u => !u.IsApproved).ToList();

        public List<Users> GetAllUsers() => users.Values.ToList();

        // Expense Management Methods
        public void AddExpense(Expenses expense)
        {
            expenses[expense.Id] = expense;
            if (expense.Status == ExpenseStatus.Pending)
            {
                PendingExpensesList.Add(expense);
            }
            else
            {
                ExpensesList.Add(expense);
            }
        }

        public void RemoveExpense(Guid expenseId)
        {
            expenses.Remove(expenseId);
            var expense = ExpensesList.FirstOrDefault(e => e.Id == expenseId) ??
                          PendingExpensesList.FirstOrDefault(e => e.Id == expenseId);
            if (expense != null)
            {
                ExpensesList.Remove(expense);
                PendingExpensesList.Remove(expense);
            }
        }

        public void UpdateExpense(Expenses expense)
        {
            if(expenses.ContainsKey(expense.Id))
            {
                expenses[expense.Id] = expense;
                var index = ExpensesList.IndexOf(ExpensesList.FirstOrDefault(e => e.Id == expense.Id));
                if (index != -1)
                {
                    ExpensesList[index] = expense;
                }
                else
                {
                    ExpensesList.Add(expense);
                }
                var pendingExpense = PendingExpensesList.FirstOrDefault(e => e.Id == expense.Id);
                if (expense.Status == ExpenseStatus.Pending)
                {
                    if (pendingExpense == null) PendingExpensesList.Add(expense);
                }
                else
                {
                    if (pendingExpense != null) PendingExpensesList.Remove(pendingExpense);
                }
            }
        }

        public void RemoveExpense(Expenses expense)
        {
            if (expenses.ContainsKey(expense.Id))
            {
                expenses.Remove(expense.Id);
                ExpensesList.Remove(expense);
                PendingExpensesList.Remove(expense);
            }
        }

        public void ApproveExpense(Expenses expense)
        {
            if (expenses.ContainsKey(expense.Id))
            {
                expenses[expense.Id].Status = ExpenseStatus.Approved;
                PendingExpensesList.Remove(expense);
                //All the users with the usernames stored inside the "Payees" list in this expense should have an expense added to their list
                foreach (var payee in expense.Payees)
                {
                    var user = GetUserByUsername(payee);
                    if (user != null)
                    {
                        var newExpense = new Expenses
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            Amount = expense.Amount,
                            Date = expense.Date,
                            Description = expense.Description,
                            Category = expense.Category,
                            PMethod = expense.PMethod,
                            Status = ExpenseStatus.Approved
                        };
                        AddExpense(newExpense);
                    }
                }
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

        public ObservableCollection<Expenses> GetAllPendingExpenses()
        {
            return new ObservableCollection<Expenses>(expenses.Values.Where(e => e.Status == ExpenseStatus.Pending));
        }
        

        public bool HasPendingExpenses(Users user)
        {
            return expenses.Values.Any(e => e.UserId == user.Id && e.Status == ExpenseStatus.Pending);
        }

        public List<Expenses> GetAllExpenses() => expenses.Values.ToList();

        public void DeleteUserExpenses(Guid userId)
        {
            var userExpenses = expenses.Values.Where(e => e.UserId == userId).ToList();
            foreach (var expense in userExpenses)
            {
                expenses.Remove(expense.Id);
            }
        }

        // Salary Management Methods
        public List<Salary> GetAllSalaries()
        {
            return salaries.Values.ToList();
        }

        public void DeleteUserSalaries(Guid userId)
        {
            var userSalaries = salaries.Values.Where(s => s.UserId == userId).ToList();
            foreach (var salary in userSalaries)
            {
                salaries.Remove(salary.Id);
            }
        }

        // Save Methods
        public void Save()
        {
            SaveExpenses();
            SaveUsers();
            SaveSalaries();
        }

        private void SaveUsers() => userDataService.SaveUsersFromDictionary(users);

        private void SaveExpenses() => expenseDataService.SaveExpensesFromDictionary(expenses);

        private void SaveSalaries() => salaryDataService.SaveSalariesFromDictionary(salaries);

        // PropertyChanged Helper
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
