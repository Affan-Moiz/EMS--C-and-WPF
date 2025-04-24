using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;
using System.Windows;

namespace ProjectVersion2.Views
{
    public partial class ManagerDashboardScreen : Window
    {
        AdminUserModel adminUserModel = new();
        public ManagerDashboardScreen(Guid UserId)
        {
            InitializeComponent();
            DataContext = adminUserModel;
        }

       

        private void ApproveUser_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected user
            adminUserModel.ApproveUser((Users)PendingUserApprovalsDataGrid.SelectedItem);

        }

        private void RejectUser_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected user
            adminUserModel.RejectUser((Users)PendingUserApprovalsDataGrid.SelectedItem);
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit User screen
        }

        private void ApproveExpense_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected expense
            adminUserModel.ApproveExpense((Expenses)PendingExpenseApprovalsDataGrid.SelectedItem);
        }

        private void RejectExpense_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected expense
            adminUserModel.RejectExpense((Expenses)PendingExpenseApprovalsDataGrid.SelectedItem);
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit Expense screen
        }

        private void ViewAllPendingUsers_Click(object sender, RoutedEventArgs e)
        {
            var pendingUserApprovalsScreen = new PendingUserApprovalsScreen(ref adminUserModel);
            pendingUserApprovalsScreen.Show();
        }

        private void ViewAllPendingExpenses_Click(object sender, RoutedEventArgs e)
        {
            var pendingExpenseApprovalsScreen = new PendingExpenseApprovalsScreen(ref adminUserModel);
            pendingExpenseApprovalsScreen.Show();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            var userManagementScreen = new UserManagementScreen(ref adminUserModel);
            userManagementScreen.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Close();
        }
    }
}



