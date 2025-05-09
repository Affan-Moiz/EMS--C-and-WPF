using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Core.ConditionalFormatting.Native;

namespace ProjectVersion2.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        Dictionary<Guid, Users>? users; // Dictionary to hold users
        Dictionary<Guid, Expenses> expenses;
        Dictionary<Guid, Salary> salaries;
        Dictionary<Guid, Notification> notifications;


        public ObservableCollection<string>? expensesCategory;
        public ObservableCollection<string>? paymentMethod;
        public ObservableCollection<string>? salaryType;
        private ObservableCollection<String> _availableUsernames;


        private UserDataService userDataService = new();
        private ExpenseDataService expenseDataService = new();
        private SalaryService salaryService = new();
        private ExpenseCategoriesService expenseCategoriesService = new();
        private NotificationService notificationService = new();

        public ObservableCollection<string> PaymentMethods { get { return paymentMethod; } }
        public ObservableCollection<string> Salaries { get { return salaryType; } }


        public ObservableCollection<Expenses> ExpensesList { get; set; }
        public ObservableCollection<Salary> SalariesList { get; set; }




        public ObservableCollection<string> AvailableUsernames { get { return _availableUsernames; } }


        private double _remainingBalance;
        private double _totalExpenses;
        private decimal _percentSpent;
        private Users? _activeUser;
        private ObservableCollection<Expenses> _upcomingExpenses;
        private int _upcomingExpensesCount;
        private ExpenseCategory? _category;
        private ObservableCollection<Notification> _notificationList;
        private double _weeklyBudget=0;
        private double _monthlyBudget=0;
        private ExportExpensesService exportExpenseService;
        private ExportSalaryService exportSalaryService;

        public Users? ActiveUser
        {
            get => _activeUser;
            set
            {
                if (_activeUser != value)
                {
                    _activeUser = value;
                    OnPropertyChanged(nameof(ActiveUser));
                }
            }
        }

        public ObservableCollection<Expenses> UpcomingExpenses { get { return _upcomingExpenses; } set { _upcomingExpenses = value; OnPropertyChanged(nameof(UpcomingExpenses)); } }
        public ObservableCollection<String> Categories { get { return expensesCategory; } set { expensesCategory = value; OnPropertyChanged(nameof(Categories)); } }

        public int UpcomingExpensesCount
        {
            get { return _upcomingExpensesCount; }
            set
            {
                if (_upcomingExpensesCount != value)
                {
                    _upcomingExpensesCount = value;
                    OnPropertyChanged(nameof(UpcomingExpensesCount));
                }
            }
        }

        public double WeeklyBudget
        {
            get { return _weeklyBudget; }
            set
            {
                if (_weeklyBudget != value)
                {
                    _weeklyBudget = value;
                    OnPropertyChanged(nameof(WeeklyBudget));
                }
            }
        }

        public double MonthlyBudget
        {
            get { return _monthlyBudget; }
            set
            {
                if (_monthlyBudget != value)
                {
                    _monthlyBudget = value;
                    OnPropertyChanged(nameof(MonthlyBudget));
                }
            }
        }


        public ObservableCollection<Notification> NotificationList
        {
            get { return _notificationList; }
            set
            {
                if (_notificationList != value)
                {
                    _notificationList = value;
                    OnPropertyChanged(nameof(NotificationList));
                }
            }
        }

        public double TotalExpenses
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

        public double RemainingBalance
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

        public ObservableDictionary<string, double> PieData { get; set; }


        public UserViewModel()
        {

        }
        public UserViewModel(Guid UserID)
        {

            users = userDataService.LoadUsersAsDictionary(); // Load users as a dictionary
            expenses = expenseDataService.LoadExpensesAsDictionary(); // Load expenses as a dictionary
            salaries = salaryService.LoadSalariesAsDictionary(); // Load salaries as a dictionary
            notifications = notificationService.LoadNotificationsAsDictionary(); // Load notifications as a dictionary

            _category = new();

            _activeUser = users.TryGetValue(UserID, out var user) ? user : null;
            expenses = GetExpensesByUserId(UserID).ToDictionary(e => e.Id); // Filter expenses for the current user
            salaries = GetSalariesByUserId(UserID).ToDictionary(s => s.Id); // Filter salaries for the current user
            notifications=GetNotificationsByID(UserID).ToDictionary(n => n.Id); // Filter notifications for the current user
            expensesCategory = new();
            InitExpenseCategories();
           

            paymentMethod = [.. Enum.GetNames(typeof(PaymentMethod))];
            salaryType = [.. Enum.GetNames(typeof(SalaryType))];

            ExpensesList = [.. expenses.Values]; // Initialize the expenses list for the current user
            SalariesList = [.. salaries.Values]; // Initialize the salaries list for the current user

            _totalExpenses = (double)GetTotalExpensesByUserId(UserID); // Initialize total expenses for the current user
            _remainingBalance = (double)GetTotalIncomeByUserId(UserID); // Initialize remaining balance for the current user
            _percentSpent = GetPercentSpent(UserID); // Initialize percent spent for the current user
            _availableUsernames = GetApprovedUsernames();
            _upcomingExpenses = new ObservableCollection<Expenses>(GetUpcomingExpensesByUserId(UserID));
            _upcomingExpensesCount = _upcomingExpenses.Count;
            _notificationList = new ObservableCollection<Notification>(GetNotificationsByID(UserID));
            _weeklyBudget = GetUserWeeklyBudget();
            _monthlyBudget = GetUserMonthlyBudget();

            exportExpenseService = new();
            exportSalaryService = new();


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
            foreach(var category in _category.Categories)
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
            }
            else
            {
                //If the category exists, move it to the bottom of the list
                expensesCategory.Remove("Add New");
                expensesCategory.Add("Add New");
            }
        }

        public double GetUserWeeklyBudget()
        {
            
            return ActiveUser.WeeklyBudget;
        }
        
        public double GetUserMonthlyBudget()
        {
            return ActiveUser.MonthlyBudget;
        }

        public void UpdateUserBudget(double weeklyBudget, double monthlyBudget)
        {
            ActiveUser.WeeklyBudget = weeklyBudget;
            ActiveUser.MonthlyBudget = monthlyBudget;
            SaveUsers();
        }

        public ObservableCollection<Notification> GetNotificationsByID(Guid userId)
        {
            //Get all the notifications for the user
            var notificationList = notifications.Values.Where(n => n.UserId == userId).ToList();
            //Convert the list to observable collection
            var observableCollection = new ObservableCollection<Notification>(notificationList);
            //Rearrange the list so that all the unread notifications are at the top while the read ones are at the bottom
            foreach (var notification in notificationList) {
                if (notification.IsRead)
                {
                    observableCollection.Remove(notification);
                    observableCollection.Add(notification);
                }
            }
            return observableCollection;
        }

        public void ReArrangeNotifications()
        {
            var readNotifications = NotificationList.Where(n => n.IsRead).ToList();
            foreach (var notification in readNotifications)
            {
                NotificationList.Remove(notification);
                NotificationList.Add(notification);
            }
        }

        public void AddNotification(Notification notification)
        {
            // If notification does not exist in the dictionary add it
            if (!notifications.ContainsKey(notification.Id))
            {
                notifications[notification.Id] = notification;
                NotificationList.Add(notification);
            }
            else
            {
                // If it exists, update the existing notification
                notifications[notification.Id] = notification;
            }
        }

        public void ExportExpensesToCSV(ObservableCollection<Expenses> expenses, bool flag, int option)
        {
            var expensesList = expenses.ToList();
            //If option is 0, then export the expenses for the past 30 days
            if (option == 0)
            {
                expensesList = expensesList.Where(e => e.Date >= DateTime.Now.AddDays(-30)).ToList();
            }
            else if (option == 1)
            {
                //Past year
                expensesList = expensesList.Where(e => e.Date >= DateTime.Now.AddYears(-1)).ToList();
            }
            exportExpenseService.ExportExpensesToCSV(expensesList, flag);
        }

        public void ExportSalariesToCSV(ObservableCollection<Salary> salaries, bool flag, int option)
        {
            var salariesList = salaries.ToList();
            //If option is 0, then export the expenses for the past 30 days
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

        public Users? GetUser()
        {
            return ActiveUser;
        }

        public void UpdateUser(Users updatedUser)
        {
            //Update the active users details other than his ID
            if (users.ContainsKey(updatedUser.Id))
            {
                users[updatedUser.Id].Username = updatedUser.Username;
                users[updatedUser.Id].Email = updatedUser.Email;
                users[updatedUser.Id].Role = updatedUser.Role;
                users[updatedUser.Id].IsApproved = updatedUser.IsApproved;
            }
            SaveUsers();

        }

        public void SaveUsers()
        {
            userDataService.SaveUsersFromDictionary(users); // Save users as a dictionary
        }

        public ObservableCollection<string> GetApprovedUsernames()
        {
            var approvedUsernames = new ObservableCollection<string>();
            foreach (var user in users.Values)
            {
                if (user.IsApproved)
                {
                    approvedUsernames.Add(user.Username);
                }
            }
            return approvedUsernames;
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
            //If the observable dictionary contains the key, update the value
            if (PieData.ContainsKey(expense.Category.ToString()))
            {
                PieData[expense.Category.ToString()] += (double)expense.Amount;
            }
            else
            {
                PieData.Add(expense.Category.ToString(), (double)expense.Amount);
            }

            //if the expense was recurring add it to the upcoming expenses list
            if (expense.IsRecurring)
            {
                UpcomingExpenses.Add(expense);
                UpcomingExpensesCount ++;
            }


            double temp = CheckWeeklyBudget();
            double temp2 = CheckMonthlyBudget();

            Notification weeklyNotification = new Notification()
            {
                Id = Guid.NewGuid(),
                UserId = ActiveUser.Id,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            if (temp == 0)
            {
                weeklyNotification.Message = ConstantInfo.BudgetArrived;
            }else if(temp > (ActiveUser.WeeklyBudget * 0.2) && temp > 0)
            {
                weeklyNotification.Message = ConstantInfo.BudgetApproaching;
            }else if (temp < 0 && temp != -1)
            {
                weeklyNotification.Message = ConstantInfo.BudgetConsumed;
            }

            Notification monthlyNotification = new Notification()
            {
                Id = Guid.NewGuid(),
                UserId = ActiveUser.Id,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            if (temp2 == 0)
            {
                monthlyNotification.Message = ConstantInfo.MonthlyBudgetArrived;
            }
            else if (temp2 > (ActiveUser.MonthlyBudget * 0.2) && temp2 > 0)
            {
                monthlyNotification.Message = ConstantInfo.MonthlyBudgetApproaching;
            }
            else if (temp2 < 0 && temp2 != -1)
            {
                monthlyNotification.Message = ConstantInfo.MonthlyBudgetConsumed;
            }

            //Add the notifications to the list
            if(temp2 != -1)
            {
                AddNotification(monthlyNotification);

            }

            if (temp != -1)
            {
                AddNotification(weeklyNotification);
            }


        }

        public double CheckWeeklyBudget()
        {
            if (ActiveUser.WeeklyBudget != 0)
            {
                //Find the total of the amount of the past 7 days of expenses
                var total = expenses.Values.Where(e => e.Date >= DateTime.Now.AddDays(-7)).Sum(e => e.Amount);
                //Check if the total is greater than the weekly budget
                if (total > 0)
                {
                    return ActiveUser.WeeklyBudget - total;

                }else
                {
                    return -1;
                }
            }else
            {
                return -1;
            }
        }

        public double CheckMonthlyBudget()
        {
            if (ActiveUser.MonthlyBudget != 0)
            {
                //Find the total of the amount of the past 30 days of expenses
                var total = expenses.Values.Where(e => e.Date >= DateTime.Now.AddDays(-30)).Sum(e => e.Amount);

                //Check if the total is greater than the monthly budget
                if (total > 0)
                {
                    return ActiveUser.MonthlyBudget - total;

                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public void RemoveExpense(Guid expenseId)
        {
            expenses.Remove(expenseId);
        }

        public Expenses? GetExpenseById(Guid id)
        {
            expenses.TryGetValue(id, out var expense);
            return expense;
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

        public List<Expenses> GetExpensesByUserId(Guid userId)
        {
            return expenses.Values.Where(e => e.UserId == userId).ToList();
        }

        public List<Expenses> GetUpcomingExpensesByUserId(Guid userId)
        {
            //Return all the expenses for the user that are recurring and have a date greater than today
            var expenseList = expenses.Values.Where(e => e.UserId == userId && e.IsRecurring).ToList();
            //Check each expense where the date is less than today and add 1 to the year of that date
            foreach (var expense in expenseList)
            {
                if (expense.Date <= DateTime.Now)
                {
                    expense.Date = expense.Date.AddYears(1);
                }
            }
            return expenseList;
        }

        private void SaveExpenses()
        {
            expenseDataService.SaveExpensesFromDictionary(expenses); 
        }

        private void SaveExpensesCategories()
        {
           ExpenseCategoriesService expenseCategoriesService = new ExpenseCategoriesService();
            expenseCategoriesService.SaveExpenseCategories(expensesCategory.ToList());
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
            
        }

        public void RemoveSalary(Guid salaryId)
        {
            salaries.Remove(salaryId);
        }

        public Salary? GetSalaryById(Guid id)
        {
            salaries.TryGetValue(id, out var salary);
            return salary;
        }

        public void SaveSalaries()
        {
            salaryService.SaveSalariesFromDictionary(salaries); // Save salaries as a dictionary
        }

        public void UpdateSalary(Guid salaryId, Salary updatedSalary)
        {
            if (salaries.ContainsKey(salaryId))
            {
                salaries[salaryId] = updatedSalary;
            }
        }

        public decimal GetTotalExpenses()
        {
            return (decimal)expenses.Values.Sum(e => e.Amount);
        }

        public decimal GetTotalExpensesByUserId(Guid userId)
        {
            return (decimal)expenses.Values.Where(e => e.UserId == userId).Sum(e => e.Amount);
        }

        public decimal GetTotalSalariesByUserId(Guid userId)
        {
            return (decimal)salaries.Values.Where(s => s.UserId == userId).Sum(s => s.Amount);
        }

        public decimal GetTotalSalaries()
        {
            return (decimal)salaries.Values.Sum(s => s.Amount);
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
            }
        }

        public void UpdateIncome(Guid userId)
        {
            RemainingBalance = (double)GetTotalIncomeByUserId(userId);
        }

        public void UpdatePercentSpent(Guid userId)
        {
            if (GetTotalSalariesByUserId(userId) == 0) // Avoid division by zero
                PercentSpent = 0;
            else
                PercentSpent = (GetTotalExpensesByUserId(userId) / GetTotalSalariesByUserId(userId));
        }

        public int GetUnreadNotificationCount()
        {
            //Get all the unread notifications for the user
            var unreadNotifications = NotificationList.Where(n => n.UserId == ActiveUser.Id && !n.IsRead).ToList();
            return unreadNotifications.Count;
        }

        public void SaveNotifications()
        {
            notificationService.SaveNotificationsFromDictionary(notifications); // Save notifications as a dictionary
        }

        public void Save()
        {
            SaveExpenses();
            SaveSalaries();
            SaveExpensesCategories();
            SaveNotifications();
            

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
