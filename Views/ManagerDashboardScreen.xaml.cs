using ProjectVersion2.ViewModels;
using System.Windows;

namespace ProjectVersion2.Views
{
    public partial class ManagerDashboardScreen : Window
    {
        AdminUserModel adminUserModel = new AdminUserModel();
        public ManagerDashboardScreen(Guid UserId)
        {
            InitializeComponent();
            LoadPendingApprovals();
        }

        private void LoadPendingApprovals()
        {
            // Load pending user approvals and expenses from the data source
            // Example:
            // PendingUserApprovalsListBox.ItemsSource = GetPendingUserApprovals();
            // PendingExpenseApprovalsListBox.ItemsSource = GetPendingExpenseApprovals();
        }

        private void ApproveUser_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected user
        }

        private void RejectUser_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected user
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit User screen
        }

        private void ApproveExpense_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected expense
        }

        private void RejectExpense_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected expense
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit Expense screen
        }

        private void ViewAllPendingUsers_Click(object sender, RoutedEventArgs e)
        {
            var pendingUserApprovalsScreen = new PendingUserApprovalsScreen();
            pendingUserApprovalsScreen.Show();
        }

        private void ViewAllPendingExpenses_Click(object sender, RoutedEventArgs e)
        {
            var pendingExpenseApprovalsScreen = new PendingExpenseApprovalsScreen();
            pendingExpenseApprovalsScreen.Show();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            var userManagementScreen = new UserManagementScreen();
            userManagementScreen.Show();
        }
    }
}



