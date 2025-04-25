using ProjectVersion2.ViewModels;
using ProjectVersion2.Model;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace ProjectVersion2.Views
{
    public partial class UserManagementScreen : Window
    {

        AdminViewModel adminUser;

        public UserManagementScreen(ref AdminViewModel adminUserModel)
        {
            InitializeComponent();
            DataContext = adminUserModel;
            adminUser = adminUserModel; // Initialize the AdminUserModel

        }

        

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Add User screen
            var addEditUserWindow = new AddEditUserWindow(ref adminUser, Guid.Empty);
            addEditUserWindow.Show();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit User screen
            var selectedUser = (Users)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                var addEditUserWindow = new AddEditUserWindow(ref adminUser, selectedUser.Id);
                addEditUserWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select a user to edit.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            // Remove selected user
            adminUser.RemoveUser((Users)UsersDataGrid.SelectedItem);
        }

        private void ApproveUser_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected user
            adminUser.ApproveUser((Users)UsersDataGrid.SelectedItem);
        }

        private void RejectUser_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected user
            adminUser.RejectUser((Users)UsersDataGrid.SelectedItem);
        }
    }
}


