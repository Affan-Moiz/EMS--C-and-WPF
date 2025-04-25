using System;
using System.Windows;
using ProjectVersion2.Services;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

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
                AuthService authService = new AuthService();
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

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            // Open Sign Up screen
            SignupScreen signUpScreen = new SignupScreen();
            signUpScreen.Show();
        }
    }
}
