using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;
using System.Windows;

namespace ProjectVersion2.Views
{
    public partial class ManagerDashboardScreen : Window
    {
        AdminViewModel adminUserModel = new();
        public ManagerDashboardScreen(Guid UserId)
        {
            InitializeComponent();
            DataContext = adminUserModel;
        }

       

        private void ApproveUser_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected user
            adminUserModel.ApproveUser((Users)PendingUserApprovalsGridControl.SelectedItem);

        }

        private void RejectUser_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected user
            adminUserModel.RejectUser((Users)PendingUserApprovalsGridControl.SelectedItem);
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit User screen
            var selectedUser = (Users)UsersGridControl.SelectedItem;
            //var selectedUser = (Users)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                var addEditUserWindow = new AddEditUserWindow(ref adminUserModel, selectedUser.Id);
                addEditUserWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select a user to edit.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApproveExpense_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected expense
            adminUserModel.ApproveExpense((Expenses)PendingExpensesApprovalsGridControl.SelectedItem);
        }

        private void RejectExpense_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected expense
            adminUserModel.RejectExpense((Expenses)PendingExpensesApprovalsGridControl.SelectedItem);
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
            this.Close();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addEditUserWindow = new AddEditUserWindow(ref adminUserModel, Guid.Empty);
            addEditUserWindow.Show();
        }

        private void RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            // Show a message box that asks for confirmation
            MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this user?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return; // User clicked "No", so do nothing
            }
            else if (result == MessageBoxResult.Yes)
            {
                //Check if the user has pending expenses
                if (adminUserModel.HasPendingExpenses((Users)UsersGridControl.SelectedItem))
                {
                    MessageBox.Show("This user has pending expenses and cannot be removed. Please complete all pending expenses before deletion", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    adminUserModel.RemoveUser((Users)UsersGridControl.SelectedItem);
                    MessageBox.Show("User removed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

               
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            adminUserModel.Save();
            var loginScreen = new LoginScreen();
            loginScreen.Show();
      
        }

        public void ShowPendingUserApprovals()
        {
            var pendingUserApprovalsScreen = new PendingUserApprovalsScreen(ref adminUserModel);
            pendingUserApprovalsScreen.Show();  
        }

        private void PendingUserApprovalsGridControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        
            var gridControl = sender as DevExpress.Xpf.Grid.GridControl;

            var IdColumn = gridControl?.Columns["Id"];
            var IsApprovedColumn = gridControl?.Columns["IsApproved"];

            IdColumn.Visible = false;
            IsApprovedColumn.Visible = false;

        }

        private void PendingExpensesApprovalsGridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var gridControl = sender as DevExpress.Xpf.Grid.GridControl;

            var IdColumn = gridControl?.Columns["Id"];
            var UserIdColumn = gridControl?.Columns["UserId"];

            IdColumn.Visible = false;
            UserIdColumn.Visible = false;
        }

        private void UsersGridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var gridControl = sender as DevExpress.Xpf.Grid.GridControl;

            var IsApprovedColumn = gridControl?.Columns["IsApproved"];

            // Change the heading of the IsApproved column to "Account Enabled"
            if (IsApprovedColumn != null)
            {
                IsApprovedColumn.Header = "Account Enabled";
            }
        }
    }
}



