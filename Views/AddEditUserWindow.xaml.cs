using ProjectVersion2.ViewModels;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;
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

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        public AdminUserModel adminUser;

        private Guid _guid;

        private bool IsEditMode = false;

        public AddEditUserWindow(ref AdminUserModel adminUserModel, Guid UserID)
        {
            InitializeComponent();
            DataContext = adminUserModel;
            _guid = UserID;
            adminUser = adminUserModel;

            if (_guid != Guid.Empty)
            {
                IsEditMode = true;
                // Load user data for editing
                var user = adminUserModel.GetUserById(_guid);
                if (user != null)
                {
                    UsernameTextBox.Text = user.Username;
                    EmailTextBox.Text = user.Email;
                    PasswordBox.Password = user.HashedPassword; // Set Password directly
                    RoleComboBox.SelectedIndex = Array.IndexOf(Enum.GetValues(typeof(Role)), user.Role);
                    IsApprovedCheckBox.IsChecked = user.IsApproved;
                }
            }
            else
            {
                // Set default values for adding a new user
                RoleComboBox.SelectedItem = Role.Admin; // Default to "User" role
                IsApprovedCheckBox.IsChecked = false;
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Users updatedUser = new Users()
            {
                Id = _guid,
                Username = UsernameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                HashedPassword = PasswordBox.Password.Trim(),
                Role = (Role)Enum.Parse(typeof(Role), RoleComboBox.SelectedItem.ToString()),
                IsApproved = IsApprovedCheckBox.IsChecked == true
            };
            // Validate the input
            if (string.IsNullOrWhiteSpace(updatedUser.Username) || string.IsNullOrWhiteSpace(updatedUser.Email) || string.IsNullOrWhiteSpace(updatedUser.HashedPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Check if the username already exists
            if (adminUser.UsersList.Any(u => u.Username.Equals(updatedUser.Username, StringComparison.OrdinalIgnoreCase) && u.Id != updatedUser.Id))
            {
                MessageBox.Show("Username already exists. Please choose a different one.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsEditMode)
            {

                // Update the user in the adminUserModel
                adminUser.UpdateUser(updatedUser);
                this.Close();
            }
            else
            {
                // Add the new user to the adminUserModel
                updatedUser.Id = Guid.NewGuid(); // Assign a new ID for the new user
                adminUser.AddUser(updatedUser);
                this.Close();
            }
            }
    }
}
