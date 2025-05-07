using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;
using ProjectVersion2.ViewModels;
using System.Windows;

namespace ProjectVersion2.Views
{
    public partial class ManagerDashboardScreen : Window
    {
        AdminViewModel adminUserModel;
        public ManagerDashboardScreen(Guid UserId)
        {
            InitializeComponent();
            adminUserModel = new AdminViewModel(UserId);
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
                addEditUserWindow.ShowDialog();
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


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var addEditUserWindow = new AddEditUserWindow(ref adminUserModel, Guid.Empty);
            addEditUserWindow.ShowDialog();
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

            if (gridControl != null)
            {
                var IdColumn = gridControl.Columns["Id"];
                var IsApprovedColumn = gridControl.Columns["IsApproved"];
                var HashedPasswordColumn = gridControl.Columns["HashedPassword"];

                if (IdColumn != null)
                {
                    IdColumn.Visible = false;
                }

                if (IsApprovedColumn != null)
                {
                    IsApprovedColumn.Visible = false;
                }
                if (HashedPasswordColumn != null)
                {
                    HashedPasswordColumn.Visible = false;
                }
                }
        }

        private void PendingExpensesApprovalsGridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var gridControl = sender as DevExpress.Xpf.Grid.GridControl;

            if (gridControl != null)
            {
                var IdColumn = gridControl.Columns["Id"];
                var UserIdColumn = gridControl.Columns["UserId"];
                var PayeeColumn = gridControl.Columns["Payees"];

                if(PayeeColumn != null)
                {
                    PayeeColumn.Header = "Payees Name";
                    PayeeColumn.Visible = true;
                }

                if (IdColumn != null)
                {
                    IdColumn.Visible = false;
                }

                if (UserIdColumn != null)
                {
                    UserIdColumn.Visible = false;
                }
            }
        }

        private void UsersGridControl_Loaded(object sender, RoutedEventArgs e)
        {
            var gridControl = sender as DevExpress.Xpf.Grid.GridControl;

            if (gridControl != null)
            {
                var IsApprovedColumn = gridControl.Columns["IsApproved"];
                var HashedPasswordColumn = gridControl.Columns["HashedPassword"];

                if (IsApprovedColumn != null)
                {
                    // Change the heading of the IsApproved column to "Account Enabled"
                    IsApprovedColumn.Header = "Account Enabled";
                }
                if (HashedPasswordColumn != null)
                {
                    HashedPasswordColumn.Visible = false;
                }
            }
        }

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IsApproved")
            {
                var user = (Users)e.Row;
                if (user!=null)
                {
                    adminUserModel.UpdateUser(user);
                }else
                {
                    MessageBox.Show("User not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
         }

        private void PendingUserApprovalsGridControl_AutoGeneratedColumns(object sender, RoutedEventArgs e)
        {
        }

        

        private void ExpenseTableView_Loaded(object sender, RoutedEventArgs e)
        {
            var gridControl = sender as GridControl;

            if (gridControl != null)
            {
                var RecurringColumn = gridControl.Columns["IsRecurring"];

                if (RecurringColumn != null)
                {
                    RecurringColumn.Header = "Recurring";
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //CenterWindow();
        }

        

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpenseScreen addExpenseScreen = new(ref adminUserModel);
            addExpenseScreen.Show();
        }

        private void RemoveExpense_Click(object sender, RoutedEventArgs e)
        {
            adminUserModel.RemoveExpense((Expenses)ExpensesGridControl.SelectedItem);
        }

        private void RemoveSalay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddSalary_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SalaryTableView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TableView_CellValueChanged_1(object sender, CellValueChangedEventArgs e)
        {

        }

        private void ExpenseTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            
                var expense = (Expenses)e.Row;
                if (expense != null)
                {
                    adminUserModel.UpdateExpense(expense);
                }
                else
                {
                    MessageBox.Show("Expense not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            

            
        }

        private void ExportPendingUserData_Click(object sender, RoutedEventArgs e)
        {
            adminUserModel.ExportUsersToCSV(adminUserModel.PendingUserList,false);
            MessageBox.Show("Pending user data has been exported to CSV.", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportUserData_Click(object sender, RoutedEventArgs e)
        {
            adminUserModel.ExportUsersToCSV(adminUserModel.UsersList,true);
            MessageBox.Show("Total user data has been exported to CSV.", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportPendingExpensesData_Click(object sender, RoutedEventArgs e)
        {
            adminUserModel.ExportExpensesToCSV(adminUserModel.PendingExpensesList, false);
            MessageBox.Show("Pending expenses data has been exported to CSV.", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportExpensesData_Click(object sender, RoutedEventArgs e)
        {
            adminUserModel.ExportExpensesToCSV(adminUserModel.ExpensesList, true);
            MessageBox.Show("Total expenses data has been exported to CSV.", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportSalaryData_Click(object sender, RoutedEventArgs e)
        {
            adminUserModel.ExportSalariesToCSV(adminUserModel.SalariesList, true);
            MessageBox.Show("Total salary data has been exported to CSV.", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditCategoryScreen addEditCategoryScreen = new(ref adminUserModel, null);
            addEditCategoryScreen.ShowDialog();
        }

        private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditCategoryScreen editCategoryScreen = new(ref adminUserModel, (string)ExpensesCategoriesGridControl.SelectedItem);
            editCategoryScreen.ShowDialog();
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            adminUserModel.RemoveExpenseCategory((string)ExpensesCategoriesGridControl.SelectedItem);

        }

        private void CategoriesCellValueChanged(object sender, CellValueChangedEventArgs e)
        {

        }
    }
}



