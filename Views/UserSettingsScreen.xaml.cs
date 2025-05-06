using ProjectVersion2.ViewModels;
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
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for UserSettingsScreen.xaml
    /// </summary>
    public partial class UserSettingsScreen : Window
    {
        private UserViewModel _userViewModel;
        private Users _currentUser;
        EncryptorDecryptor encryptorDecryptor;

        public UserSettingsScreen(ref UserViewModel UVModel)
        {
            InitializeComponent();
            DataContext = UVModel;
            _userViewModel = UVModel;
            _currentUser = _userViewModel.GetUser();

            UsernameTextBox.Text = _currentUser.Username;
            EmailTextBox.Text = _currentUser.Email;
            RoleTextBox.Text = _currentUser.Role.ToString();
            IsApprovedTextBox.Text = _currentUser.IsApproved.ToString();
            encryptorDecryptor = new();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the user information
            _currentUser.Username = UsernameTextBox.Text;
            _currentUser.Email = EmailTextBox.Text;
            // Save the changes to the database or any other storage
            _userViewModel.UpdateUser(_currentUser);
            //Show a success message
            MessageBox.Show("User settings updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();

        }

        private void UpdatePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            EditDetailsGrid.Visibility = Visibility.Collapsed;
            PasswordUpdateGrid.Visibility = Visibility.Visible;

        }

        private void BackToEditDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            EditDetailsGrid.Visibility = Visibility.Visible;
            PasswordUpdateGrid.Visibility = Visibility.Collapsed;
        }

        private void SavePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = CurrentPasswordBox.Password;
            string newPassword = UpdatedPasswordBox.Password;
            string userPassword = _currentUser.HashedPassword;

            if(string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (encryptorDecryptor.MD5Hash(currentPassword) != userPassword)
            {
                MessageBox.Show("Current password is incorrect.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }else
            {
                _currentUser.HashedPassword = encryptorDecryptor.MD5Hash(newPassword);
                _userViewModel.UpdateUser(_currentUser);
                MessageBox.Show("Password updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                PasswordUpdateGrid.Visibility = Visibility.Collapsed;
                EditDetailsGrid.Visibility = Visibility.Visible;
            }


        }
    }
}
