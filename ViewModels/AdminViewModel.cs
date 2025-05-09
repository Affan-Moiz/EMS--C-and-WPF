using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using DevExpress.Mvvm.Native;
using ProjectVersion2.Model;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;
using ProjectVersion2.Views;

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
        Dictionary<Guid, Notification> notifcations;

        private ObservableCollection<Users> _pendingUserList;
        private ObservableCollection<Expenses> _pendingExpensesList;
        private ObservableCollection<Expenses> _expensesList;
        private ObservableCollection<Salary> _salariesList;
        private ObservableCollection<Users> _usersList;
        private ObservableCollection<String> _availableUsersList;
        private Users _activeUser;
        private EncryptorDecryptor _encryptorDecryptor;
        private ExpenseCategory _category = new();
        private int _userCount = 0;
        private int _pendingUserCount = 0;
        private int _totalExpensesCount = 0;
        private int _pendingExpensesCount = 0;


        private ObservableCollection<string>? rolesCategory;
        private ObservableCollection<string>? expensesCategory = new();
        private ObservableCollection<string>? paymentMethod;

        // Services
        private readonly UserDataService userDataService = new();
        private readonly ExpenseDataService expenseDataService = new();
        private readonly SalaryService salaryDataService = new();
        private ExportUserServices exportUserServices = new();
        private ExportSalaryService exportSalaryService = new();
        private ExportExpensesService exportExpenseService = new();
        private ExpenseCategoriesService expenseCategoriesService = new();
        private NotificationService notificationService = new();


        // Properties
        public ObservableCollection<string> Roles => rolesCategory;
        public ObservableCollection<string> PaymentMethods => paymentMethod;

        public ObservableDictionary<string, double> PieData { get; set; }


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

        public int UserCount
        {
            get => _userCount;
            set
            {
                if (_userCount != value)
                {
                    _userCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PendingUserCount
        {
            get => _pendingUserCount;
            set
            {
                if (_pendingUserCount != value)
                {
                    _pendingUserCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalExpensesCount
        {
            get => _totalExpensesCount;
            set
            {
                if (_totalExpensesCount != value)
                {
                    _totalExpensesCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PendingExpensesCount
        {
            get => _pendingExpensesCount;
            set
            {
                if (_pendingExpensesCount != value)
                {
                    _pendingExpensesCount = value;
                    OnPropertyChanged();
                }
            }
        }


        public ObservableCollection<string> Categories
        {
            get { return expensesCategory; }
            set
            {
                if (expensesCategory != value)
                { expensesCategory = value; OnPropertyChanged(); }
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
            notifcations = notificationService.LoadNotificationsAsDictionary(); // Load notifications as a dictionary


            _pendingUserList = new ObservableCollection<Users>(GetPendingUsers());
            _pendingExpensesList = new ObservableCollection<Expenses>(GetAllPendingExpenses());
            _usersList = new ObservableCollection<Users>(GetAllUsers());
            _expensesList = new ObservableCollection<Expenses>(GetAllExpenses());
            _salariesList = new ObservableCollection<Salary>(GetAllSalaries());
            _availableUsersList = new ObservableCollection<String>(GetApprovedUsernames());
            _activeUser = GetUserById(Id);
            _encryptorDecryptor = new();
            _userCount = users.Count;
            _pendingUserCount = PendingUserList.Count;
            _totalExpensesCount = expenses.Count;
            _pendingExpensesCount = PendingExpensesList.Count;

            rolesCategory = new ObservableCollection<string>(Enum.GetNames(typeof(Role)));
            InitExpenseCategories();
            paymentMethod = new ObservableCollection<string>(Enum.GetNames(typeof(PaymentMethod)));

            PieData = new ObservableDictionary<string, double>();
            var pieData = GetPieData();
            foreach (var data in pieData)
            {
                PieData.Add(data.Argument.ToString(), data.Value);
            }
        }


        public void InitExpenseCategories()
        {
            //Add all the categories inside the private variable _category to expensesCategories
            foreach (var category in _category.Categories)
            {
                expensesCategory.Add(category);
            }
            var temp = expenseCategoriesService.LoadExpenseCategories();
            //Convert all the categories to string in temp

            foreach (var category in temp)
            {
                if (!expensesCategory.Contains(category.ToString()))
                {
                    expensesCategory.Add(category.ToString());
                }
            }

            if (!expensesCategory.Contains("Add New"))
            {
                expensesCategory.Add("Add New");
            }else
            {
                //If the category "Add New" already exists, remove it and add it again
                expensesCategory.Remove("Add New");
                expensesCategory.Add("Add New");
            }
        }

        // User Management Methods
        public void AddUser(Users user)
        {
            user.HashedPassword = _encryptorDecryptor.MD5Hash(user.HashedPassword);
            users[user.Id] = user;
            UsersList.Add(user);
            UserCount++;
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

        public List<DataPoint> GetPieData()
        {
            var pieData = new List<DataPoint>();
            foreach (var category in expensesCategory)
            {
                var total = expenses.Values.Where(e => e.Category.ToString() == category).Sum(e => e.Amount);
                pieData.Add(new DataPoint(category, (double)total));
            }
            return pieData;
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
                    if (pendingUser != null)
                    {
                        PendingUserList.Remove(pendingUser);
                        PendingUserCount--;
                    } 
                }
                else
                {
                    if (pendingUser == null)
                    {
                        PendingUserList.Add(user);
                        PendingUserCount++;
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
                PendingUserCount--;
            }
        }

        public void RejectUser(Guid userId)
        {
            if (users.TryGetValue(userId, out var user))
            {
                PendingUserList.Remove(user);
                users.Remove(userId);
                UsersList.Remove(user);
                PendingUserCount--;
            }
        }

        public void RejectUser(Users user)
        {
            if (users.ContainsKey(user.Id))
            {
                users.Remove(user.Id);
                PendingUserList.Remove(user);
                UsersList.Remove(user);
                PendingUserCount--;
                UserCount--;
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
                PendingExpensesCount++;
            }
            else
            {
                ExpensesList.Add(expense);
                TotalExpensesCount++;
            }

            if (PieData.ContainsKey(expense.Category.ToString()))
            {
                PieData[expense.Category.ToString()] += (double)expense.Amount;
            }
            else
            {
                PieData.Add(expense.Category.ToString(), (double)expense.Amount);
            }
        }

        public void AddExpenseCategory(string category)
        {
            if (!expensesCategory.Contains(category))
            {
                expensesCategory.Add(category);
                // Save the new category to the file
                SaveExpensesCategories();
            }
        }

        public void EditExpenseCategory(string previousCategory, string updatedCategory)
        {
            if (expensesCategory.Contains(previousCategory))
            {
                int index = expensesCategory.IndexOf(previousCategory);
                if (index != -1)
                {
                    expensesCategory[index] = updatedCategory;
                    // Save the updated category to the file
                    SaveExpensesCategories();
                }

                //Convert all the expenses whose Category is about to be updated to the new category
                foreach (var expense in expenses.Values)
                {
                    if (expense.Category == previousCategory)
                    {
                        expense.Category = updatedCategory;
                        UpdateExpense(expense);
                    }
                }
            }
        }

        public void RemoveExpenseCategory(string category)
        {
            if (expensesCategory.Contains(category))
            {
                //Convert all the expenses whose Category is about to be deleted to "Other"
                foreach (var expense in expenses.Values)
                {
                    if (expense.Category == category)
                    {
                        expense.Category = "Other";
                        UpdateExpense(expense);
                    }
                }
                expensesCategory.Remove(category);
                // Save the updated categories to the file
                SaveExpensesCategories();
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
                PendingExpensesCount--;
                TotalExpensesCount--;
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
                    TotalExpensesCount++;
                }
                var pendingExpense = PendingExpensesList.FirstOrDefault(e => e.Id == expense.Id);
                if (expense.Status == ExpenseStatus.Pending)
                {
                    if (pendingExpense == null)
                    {
                        PendingExpensesList.Add(expense);
                        PendingExpensesCount++;
                    }
                }
                else
                {
                    if (pendingExpense != null)
                    {
                        PendingExpensesList.Remove(pendingExpense);
                        PendingExpensesCount--;
                    }
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
                TotalExpensesCount--;
                PendingExpensesCount--;
            }
        }

        public void ApproveExpense(Expenses expense)
        {
            if (expenses.ContainsKey(expense.Id))
            {
                expenses[expense.Id].Status = ExpenseStatus.Approved;
                Notification notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = expense.UserId,
                    Message = $"Your expense '{expense.Description}' has been approved.",
                    CreatedAt = DateTime.Now
                };

                AddNotification(notification);

                PendingExpensesList.Remove(expense);
                PendingExpensesCount--;
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
                        // Add a notification for the payee

                        Notification payeeNotification = new Notification
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            Message = $"You have received an expense '{expense.Description}' from the admin.",
                            CreatedAt = DateTime.Now
                        };
                        AddNotification(payeeNotification);
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
                PendingExpensesCount--;
            }
        }

        public void AddNotification(Notification notification)
        {
            if(notifcations.ContainsKey(notification.Id))
            {
                notifcations[notification.Id] = notification;
            }
            else
            {
                notifcations.Add(notification.Id, notification);
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
            SaveExpensesCategories();
            SaveNotifications();
        }

        private void SaveUsers() => userDataService.SaveUsersFromDictionary(users);

        private void SaveExpenses() => expenseDataService.SaveExpensesFromDictionary(expenses);

        private void SaveSalaries() => salaryDataService.SaveSalariesFromDictionary(salaries);

        private void SaveExpensesCategories() => expenseCategoriesService.SaveExpenseCategories(expensesCategory.ToList());

        private void SaveNotifications() => notificationService.SaveNotificationsFromDictionary(notifcations);



        public void ExportUsersToCSV(ObservableCollection<Users> users, bool flag)
        {
            var usersList = users.ToList();
            exportUserServices.ExportUsersToCSV(usersList, flag);
        }

        public void ExportSalariesToCSV(ObservableCollection<Salary> salaries, bool flag, int option)
        {
            var salariesList = salaries.ToList();

            if (option == 0)
            {
                salariesList = salariesList.Where(e => e.Date >= DateTime.Now.AddDays(-30)).ToList();
            }
            else if (option == 1)
            {
                //Past year
                salariesList = salariesList.Where(e => e.Date >= DateTime.Now.AddYears(-1)).ToList();
            }

            exportSalaryService.ExportSalaryToCSV(salariesList, flag);
        }

        public void ExportExpensesToCSV(ObservableCollection<Expenses> expenses, bool flag , int option)
        {
            var expensesList = expenses.ToList();
            //If option is 0, then export the expenses for the past 30 days
            if (option == 0)
            {
                expensesList = expensesList.Where(e => e.Date >= DateTime.Now.AddDays(-30)).ToList();
            }else if (option == 1)
            {
                //Past year
                expensesList = expensesList.Where(e => e.Date >= DateTime.Now.AddYears(-1)).ToList();
            }
                exportExpenseService.ExportExpensesToCSV(expensesList, flag);
        }

        // PropertyChanged Helper
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
