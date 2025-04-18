using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectVersion2.ViewModels;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for DashboardScreen.xaml
    /// </summary>
    public partial class DashboardScreen : Window
    {
        UserViewModel UserViewModel;
        public decimal _totalExpenses;
        public string TotalExpenses
        {
            get
            {
                return _totalExpenses.ToString("C2");
            }
        }
        public DashboardScreen(Guid UserId)
        {
            DataContext = this; // Set the DataContext for data binding
            InitializeComponent();
            UserViewModel = new UserViewModel(UserId);
            _totalExpenses = UserViewModel.TotalExpenses;
        }

        private void ViewExpenseList_Click(object sender, RoutedEventArgs e)
        {
            var expenseListScreen = new ExpenseList(UserViewModel.GetUserID());
            expenseListScreen.Show();
            this.Close();
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseScreen addExpenseScreen = new AddExpenseScreen(UserViewModel.GetUserID());
            addExpenseScreen.Show();
        }

        

        private void AddSalary_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the AddSalaryScreen
            AddSalaryScreen addSalaryScreen = new AddSalaryScreen(UserViewModel.GetUserID());
            addSalaryScreen.Show();

        }

        private void ViewSalary_Click(object sender, RoutedEventArgs e)
        {
            //Navigate to the SalaryList
            SalaryList salaryListScreen = new SalaryList(UserViewModel.GetUserID());
            salaryListScreen.Show();
        }

        private void ViewExpenses_Click(object sender, RoutedEventArgs e)
        {
            ExpenseList expenseListScreen = new ExpenseList(UserViewModel.GetUserID());
            expenseListScreen.Show();
        }

        
    }
}
