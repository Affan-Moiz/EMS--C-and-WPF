using ProjectVersion2.Model;
using System.Windows;
using ProjectVersion2.ViewModels;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for DashboardScreen.xaml
    /// </summary>
    public partial class DashboardScreen : Window
    {
        UserViewModel UserViewModel;

        

       
        public DashboardScreen(Guid UserId)
        {
            InitializeComponent();
            UserViewModel = new UserViewModel(UserId);
            DataContext = UserViewModel;
        }

        private void ViewExpenseList_Click(object sender, RoutedEventArgs e)
        {
            var expenseListScreen = new ExpenseList(UserViewModel.GetUserID(), ref UserViewModel);
            expenseListScreen.Show();
            this.Close();
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseScreen addExpenseScreen = new AddExpenseScreen(UserViewModel.GetUserID(), ref UserViewModel);
            addExpenseScreen.Show();
        }

        

        private void AddSalary_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the AddSalaryScreen
            AddSalaryScreen addSalaryScreen = new AddSalaryScreen(UserViewModel.GetUserID(),ref UserViewModel);
            addSalaryScreen.Show();

        }

        private void ViewSalary_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the SalaryList
            SalaryList salaryListScreen = new SalaryList(UserViewModel.GetUserID(), ref UserViewModel);
            salaryListScreen.Show();
        }

        private void ViewExpenses_Click(object sender, RoutedEventArgs e)
        {
            ExpenseList expenseListScreen = new ExpenseList(UserViewModel.GetUserID(), ref UserViewModel);
            expenseListScreen.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }
    }
}
