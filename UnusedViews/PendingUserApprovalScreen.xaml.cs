using ProjectVersion2.ViewModels;
using ProjectVersion2.Model;
using System.Windows;

namespace ProjectVersion2.Views
{
    public partial class PendingUserApprovalsScreen : Window
    {

        AdminViewModel adminUserModel;

        public PendingUserApprovalsScreen(ref AdminViewModel adminUserModel)
        {
            InitializeComponent();
            DataContext = adminUserModel;
            this.adminUserModel = adminUserModel;
        }


        private void ApproveUser_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected user
            adminUserModel.ApproveUser((Users)PendingUsersDataGrid.SelectedItem);
        }

        private void RejectUser_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected user
            adminUserModel.RejectUser((Users)PendingUsersDataGrid.SelectedItem);
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit User screen
        }
    }
}

