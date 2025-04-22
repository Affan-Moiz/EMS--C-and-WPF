using System;
using System.Windows;
using System.Windows.Controls;
using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;
using ProjectVersion2.Utilities;
using System.Collections.ObjectModel;

namespace ProjectVersion2.Views
{
    public partial class AddExpenseScreen : Window
    {
        public UserViewModel userViewModel;
        private bool _isRecurring;
       

        public AddExpenseScreen(Guid userId, ref UserViewModel UVModel)
        {
           // DataContext = this;
            InitializeComponent();

            DataContext = UVModel;

            userViewModel = UVModel; // Initialize the UserViewModel with the current user ID

            

           // CategoryComboBox.ItemsSource = Categories;
        }

        private void RecurringCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _isRecurring = true;
            StartDateLabel.Visibility = Visibility.Visible;
            StartDatePicker.Visibility = Visibility.Visible;
            FrequencyLabel.Visibility = Visibility.Visible;
            FrequencyComboBox.Visibility = Visibility.Visible;
            EndDateLabel.Visibility = Visibility.Visible;
            EndDatePicker.Visibility = Visibility.Visible;
        }

        private void RecurringCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _isRecurring = false;
            StartDateLabel.Visibility = Visibility.Collapsed;
            StartDatePicker.Visibility = Visibility.Collapsed;
            FrequencyLabel.Visibility = Visibility.Collapsed;
            FrequencyComboBox.Visibility = Visibility.Collapsed;
            EndDateLabel.Visibility = Visibility.Collapsed;
            EndDatePicker.Visibility = Visibility.Collapsed;
        }

        private void SubmitExpense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(AmountTextBox.Text) || !decimal.TryParse(AmountTextBox.Text, out var amount))
                {
                    MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (CategoryComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (PaymentMethodComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a payment method.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create a new expense
                var newExpense = new Expenses
                {
                    Id = Guid.NewGuid(),
                    UserId = userViewModel.GetUserID(),
                    Amount = amount,
                    Description = DescriptionTextBox.Text,
                    Category = (ExpenseCategories)Enum.Parse(typeof(ExpenseCategories), CategoryComboBox.SelectedItem.ToString()),
                    PMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), PaymentMethodComboBox.SelectedItem.ToString()),
                    Status = ExpenseStatus.Pending,
                    Date = DateTime.Now, 
                    IsRecurring = _isRecurring
                };

                // Handle recurring expense details
                if (_isRecurring)
                {
                    if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
                    {
                        MessageBox.Show("Please fill in all recurring expense details.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    newExpense.Date = StartDatePicker.SelectedDate.Value;
                }
                

                // Add the expense using the ViewModel
                userViewModel.AddExpense(newExpense);

                MessageBox.Show("Expense submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CategoryComboBox.ItemsSource = Enum.GetValues(typeof(ExpenseCategories));
            //PaymentMethodComboBox.ItemsSource = Enum.GetValues(typeof(PaymentMethod));
            //CategoryComboBox.SelectedIndex = 0;
            //PaymentMethodComboBox.SelectedIndex = 0;
        }


        private void AmountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            // Allow only numeric input and a single decimal point  
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"^[0-9]*(\.[0-9]*)?$");
        }

        private void AmountTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Allow only numeric input and a single decimal point
            if (e.Key == System.Windows.Input.Key.Back || e.Key == System.Windows.Input.Key.Delete)
            {
                e.Handled = false; // Allow backspace and delete
            }
            else if (e.Key == System.Windows.Input.Key.Enter)
            {
                SubmitExpense_Click(sender, e);
            }
            else
            {
                e.Handled = !IsTextAllowed(e.Key.ToString());
            }
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AmountTextBox.Text) && !decimal.TryParse(AmountTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AmountTextBox.Clear();
            }
        }

        private void AmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AmountTextBox.Text) && !decimal.TryParse(AmountTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AmountTextBox.Clear();

            }
        }

        private void AmountTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AmountTextBox.Text))
            {
                AmountTextBox.Clear();
            }
        }

        private void PaymentMethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}


