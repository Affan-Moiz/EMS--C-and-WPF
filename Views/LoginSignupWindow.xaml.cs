using ProjectVersion2.Model;
using ProjectVersion2.Services;
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
    /// Interaction logic for LoginSignupWindow.xaml
    /// </summary>
    public partial class LoginSignupWindow : Window
    {
        AuthService authService = new AuthService();
        public LoginSignupWindow()
        {
            InitializeComponent();
            LoginUsernameTextBox.Focus();
        }

        private void ShowSignUp_Click(object sender, RoutedEventArgs e)
        {
            LoginGrid.Visibility = Visibility.Collapsed;
            SignupGrid.Visibility = Visibility.Visible;
            SignupUsernameTextBox.Focus();
        }

        private void ShowLogin_Click(object sender, RoutedEventArgs e)
        {
            SignupGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Visible;
            LoginUsernameTextBox.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginUsernameTextBox.Text;
            string password = LoginPasswordBox.Password;

            // Input validation
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Attempt login
            try
            {
                Users? CurrUser = authService.Login(username, password);

                if (CurrUser != null)
                {
                    if (CurrUser.IsApproved)
                    {
                        if (CurrUser.Role == Role.Admin)
                        {
                            // Open Admin Dashboard
                            ManagerDashboardScreen adminDashboard = new ManagerDashboardScreen(CurrUser.Id);
                            adminDashboard.Show();
                            this.Close();
                        }
                        else
                        {
                            // Open User Dashboard
                            DashboardScreen userDashboard = new DashboardScreen(CurrUser.Id);
                            userDashboard.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your account is not approved yet. Please contact the administrator.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            Users newUser = new Users()
            {
                Username = SignupUsernameTextBox.Text.Trim(),
                Email = SignupEmailTextBox.Text.Trim(),
                HashedPassword = SignupPasswordBox.Password.Trim(),
                IsApproved = true
            };

            // Check if a role is selected
            if (SignupRoleComboBox.SelectedItem is ComboBoxItem selectedRole)
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
                    SignupGrid.Visibility = Visibility.Collapsed;
                    LoginGrid.Visibility = Visibility.Visible;
                    LoginUsernameTextBox.Focus();
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
