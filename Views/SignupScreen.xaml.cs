using ProjectVersion2.Services;
using ProjectVersion2.ViewModels;
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
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for SignupScreen.xaml
    /// </summary>
    public partial class SignupScreen : Window
    {
        public SignupScreen()
        {
            InitializeComponent();
            AuthService authService = new AuthService();
            UsernameTextBox.Focus();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void Signup_Click(object sender, RoutedEventArgs e)
        //{
        //    //Creating an instance of a user to send to the signup function in auth service
        //    Users newUser = new Users()
        //    {
        //        Username = UsernameTextBox.Text.Trim(),
        //        Email = EmailTextBox.Text.Trim(),
        //        HashedPassword = PasswordBox.Password.Trim(),
        //        IsApproved = true
        //    };

        //    if (RoleComboBox.SelectedItem.ToString() == "User")
        //    {
        //        newUser.Role = Role.User;
        //    }
        //    else if (RoleComboBox.SelectedItem.ToString() == "Admin")
        //    {
        //        newUser.Role = Role.Admin;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }


        //        //Input validation
        //        if (string.IsNullOrWhiteSpace(newUser.Username))
        //    {
        //        MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrWhiteSpace(newUser.HashedPassword))
        //    {
        //        MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    if (string.IsNullOrWhiteSpace(newUser.Email))
        //    {
        //        MessageBox.Show("Email cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //    // Attempt signup
        //    try
        //    {
        //        AuthService authService = new AuthService();
        //        bool isSignedUp = authService.SignUp(newUser.Username, newUser.HashedPassword, newUser.Email);
        //        if (isSignedUp)
        //        {
        //            MessageBox.Show("Signup successful! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //            Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Username or email already exists. Please try again.", "Signup Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred during signup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}


        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            // Creating an instance of a user to send to the signup function in auth service
            Users newUser = new Users()
            {
                Username = UsernameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                HashedPassword = PasswordBox.Password.Trim(),
                IsApproved = true
            };

            // Check if a role is selected
            if (RoleComboBox.SelectedItem is ComboBoxItem selectedRole)
            {
                string role = selectedRole.Content.ToString();
                if (role == "User")
                {
                    newUser.Role = Role.User;
                }
                else if (role == "Admin")
                {
                    newUser.Role = Role.Admin;
                }
            }
            else
            {
                MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Input validation
            if (string.IsNullOrWhiteSpace(newUser.Username))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(newUser.HashedPassword))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(newUser.Email))
            {
                MessageBox.Show("Email cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Attempt signup
            try
            {
                AuthService authService = new AuthService();
                bool isSignedUp = authService.SignUp(newUser.Username, newUser.HashedPassword, newUser.Email, newUser.Role);
                if (isSignedUp)
                {
                    MessageBox.Show("Signup successful! You can log in once an admin approves your request.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("Username or email already exists. Please try again.", "Signup Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during signup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
