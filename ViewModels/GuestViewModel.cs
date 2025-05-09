using ProjectVersion2.Model;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.ViewModels
{
    public class GuestViewModel : INotifyPropertyChanged
    {
        private Dictionary<Guid, Expenses> expenses;
        private Dictionary<Guid, Guest> guests;
        Dictionary<Guid, Notification> notifcations;
        Dictionary<Guid, Log> logs;


        private Guest _ActiveGuest;

        private ObservableCollection<Expenses> _pendingExpensesList;

        private ExpenseDataService expenseDataService = new();
        private GuestService guestService = new();
        private NotificationService notificationService = new();
        private LogsService logService = new();


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

        public event PropertyChangedEventHandler? PropertyChanged;

        public GuestViewModel(Guid guestId)
        {
            expenses = expenseDataService.LoadExpensesAsDictionary();
            guests = guestService.LoadGuestsAsDictionary();
            notifcations = notificationService.LoadNotificationsAsDictionary();
            logs = logService.LoadLogsAsDictionary();


            _pendingExpensesList = new ObservableCollection<Expenses>(GetAllPendingExpenses());
            _ActiveGuest = GetGuestById(guestId);
        }

        public ObservableCollection<Expenses> GetAllPendingExpenses()
        {
            return new ObservableCollection<Expenses>(expenses.Values.Where(e => e.Status == ExpenseStatus.Pending));
        }

        public Guest GetGuestByUsername(string username)
        {
            return guests.Values.FirstOrDefault(g => g.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public Guest GetGuestById(Guid id)
        {
            return guests.Values.FirstOrDefault(g => g.Id == id);
        }

        public Guest GetCurrentGuest()
        {
            return _ActiveGuest;
        }

        public void AddNotification(Notification notification)
        {
            if (notifcations.ContainsKey(notification.Id))
            {
                notifcations[notification.Id] = notification;
            }
            else
            {
                notifcations.Add(notification.Id, notification);
            }
        }

        public void UpdateExpense(Expenses expense)
        {
            //Update the existing pending expense
            if (expenses.ContainsKey(expense.Id))
            {
                expenses[expense.Id] = expense;
            }
            else
            {
                // If the expense doesn't exist, add it to the dictionary
                expenses.Add(expense.Id, expense);
            }
        }

        public void AddLog(Log log)
        {
            if (logs.ContainsKey(log.Id))
            {
                logs[log.Id] = log;
            }
            else
            {
                logs.Add(log.Id, log);
            }
        }

        public void SaveExpenses()
        {
            expenseDataService.SaveExpensesFromDictionary(expenses);
        }

        public void SaveNotification()
        {
            notificationService.SaveNotificationsFromDictionary(notifcations);
        }

        public void SaveLogs()
        {
            logService.SaveLogsFromDictionary(logs);
        }

        public void Save()
        {
            SaveExpenses();
            SaveNotification();
            SaveLogs();
        }

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
