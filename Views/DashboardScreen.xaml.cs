using ProjectVersion2.Model;
using System.Windows;
using ProjectVersion2.ViewModels;
using System.Windows.Data;
using ProjectVersion2.Services;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for DashboardScreen.xaml
    /// </summary>
    public partial class DashboardScreen : Window
    {
        UserViewModel UserViewModel;
        ExportSalaryService exportSalaryService;
        ExportExpensesService exportExpensesService;



        public DashboardScreen(Guid UserId)
        {
            InitializeComponent();
            UserViewModel = new UserViewModel(UserId);
            DataContext = UserViewModel;
        }

        

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseScreen addExpenseScreen = new AddExpenseScreen(UserViewModel.GetUserID(), ref UserViewModel, false);
            addExpenseScreen.ShowDialog();
        }

        

        private void AddSalary_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the AddSalaryScreen
            AddSalaryScreen addSalaryScreen = new AddSalaryScreen(UserViewModel.GetUserID(),ref UserViewModel);
            addSalaryScreen.ShowDialog();

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            UserViewModel.Save();
            var loginScreen = new LoginScreen();
            loginScreen.Show();
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var gridControl = sender as DevExpress.Xpf.Grid.GridControl;

            var IdColumn = gridControl?.Columns["Id"];
            var UserIdColumn = gridControl?.Columns["UserId"];
            var IsRecurringColumn = gridControl?.Columns["IsRecurring"];

            if(IdColumn != null)
            {
                IdColumn.Visible = false;
            }

            if (UserIdColumn != null)
            {
                UserIdColumn.Visible = true;
            }

            if (IsRecurringColumn != null)
            {
                IsRecurringColumn.Visible = false;
            }

        }

        private void RequestExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseScreen addExpenseScreen = new AddExpenseScreen(UserViewModel.GetUserID(), ref UserViewModel, true);
            addExpenseScreen.ShowDialog();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            UserSettingsScreen userSettingsScreen = new UserSettingsScreen(ref UserViewModel);
            userSettingsScreen.ShowDialog();
        }
        private void ExportExpenses_Click(object sender, RoutedEventArgs e)
        {
            exportExpensesService = new();
            exportExpensesService.ExportExpensesToCSV(UserViewModel.ExpensesList.ToList(),true);
            MessageBox.Show("Operation Successful", "Export to CSV");

        }

        private void ExportSalary_Click(object sender, RoutedEventArgs e)
        {
            exportSalaryService = new ExportSalaryService();
            exportSalaryService.ExportSalaryToCSV(UserViewModel.SalariesList.ToList(),true);
            MessageBox.Show("Operation Successful", "Export to CSV");
        }

        private void UpcomingExpensesGridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var gridControl = sender as DevExpress.Xpf.Grid.GridControl;

            var IdColumn = gridControl?.Columns["Id"];
            var UserIdColumn = gridControl?.Columns["UserId"];
            var IsRecurringColumn = gridControl?.Columns["IsRecurring"];
            var DateColumn = gridControl?.Columns["Date"];

            if (DateColumn != null)
            {
                DateColumn.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                DateColumn.SortIndex = 0;
            }

            if (IdColumn != null)
            {
                IdColumn.Visible = false;

            }

            if (UserIdColumn != null)
            {
                UserIdColumn.Visible = false;
            }

            if(IsRecurringColumn != null)
            {
                IsRecurringColumn.Visible = false;
            }



        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationWindow notificationWindow = new NotificationWindow(ref UserViewModel);
            notificationWindow.Show();
            checkForUnread();
        }

        public void checkForUnread()
        {
            if (UserViewModel.GetUnreadNotificationCount() > 0)
            {
                NotificationButton.BorderBrush = System.Windows.Media.Brushes.Red;
                NotificationButton.BorderThickness = new Thickness(2);
            }
            else
            {
                NotificationButton.BorderBrush = System.Windows.Media.Brushes.Transparent;
                NotificationButton.BorderThickness = new Thickness(0);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If there are any un read notifications inside userViewModel.NotificationsList Make the border of the Notification button red and thick
            checkForUnread();

        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            checkForUnread();
        }
    }
}
