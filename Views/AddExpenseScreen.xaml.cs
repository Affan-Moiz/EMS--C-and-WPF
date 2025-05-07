using System;
using System.Windows;
using System.Windows.Controls;
using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;
using ProjectVersion2.Utilities;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic.ApplicationServices;

namespace ProjectVersion2.Views
{
    public partial class AddExpenseScreen : Window
    {
        public UserViewModel userViewModel;
        public AdminViewModel adminView;
        private bool _isRecurring;
        private bool _isAdmin=false;
        private bool _isRequest = false;
        private bool _newExpenseCategory = false;


        public AddExpenseScreen(Guid userId, ref UserViewModel UVModel, bool IsRequest)
        {
           // DataContext = this;
            InitializeComponent();

            DataContext = UVModel;

            userViewModel = UVModel; // Initialize the UserViewModel with the current user ID


            AmountTextBox.Focus();

            if (IsRequest)
            {
                _isRequest = true;
                RequestFormInit();
            }


            }

        public AddExpenseScreen(ref AdminViewModel adminViewModel)
        {
            InitializeComponent();
            DataContext = adminViewModel;
            adminView = adminViewModel;
            IdTextBox.Visibility= Visibility.Visible;
            IdLabel.Visibility = Visibility.Visible;
            _isAdmin = true;

        }

        private void RequestFormInit()
        {
            SubmitButton.Content = "Request";
            PayeesLabel.Visibility = Visibility.Visible;
            PayeeStackPanel.Visibility = Visibility.Visible;
        }

        private void RecurringCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _isRecurring = true;
        }

        private void RecurringCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _isRecurring = false;
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

                if(_isRequest && SelectedPayeesListBox.Items.Count == 0)
                {
                    MessageBox.Show("Please select at least one payee.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create a new expense
                var newExpense = new Expenses
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Empty,
                    Amount = amount,
                    Description = DescriptionTextBox.Text,
                    Category = "Other",
                    PMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), PaymentMethodComboBox.SelectedItem.ToString()),
                    Status = ExpenseStatus.Completed,
                    Date = DateTime.Now,
                    IsRecurring = _isRecurring
                };

                if(_newExpenseCategory)
                {
                    if (!_isAdmin)
                    {
                        userViewModel.AddExpenseCategory(NewExpenseCategoryTextBox.Text);
                        newExpense.Category = NewExpenseCategoryTextBox.Text;
                    }
                    else
                    {
                        adminView.AddExpenseCategory(NewExpenseCategoryTextBox.Text);
                        newExpense.Category = NewExpenseCategoryTextBox.Text;
                    }
                }
                else
                {
                    newExpense.Category = CategoryComboBox.SelectedItem.ToString();
                }

                if (_isAdmin)
                {
                    newExpense.UserId = Guid.Parse(IdTextBox.Text);
                }
                else
                {
                    newExpense.UserId = userViewModel.GetUserID();
                }

                if (_isRequest)
                {
                    newExpense.Payees= new List<string>();
                    foreach (var payee in SelectedPayeesListBox.Items)
                    {
                        newExpense.Payees.Add(payee.ToString());
                    }
                    newExpense.Status = ExpenseStatus.Pending;

                }


                if (_isAdmin)
                {
                    adminView.AddExpense(newExpense);
                }
                else
                {
                    userViewModel.AddExpense(newExpense);
                }

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

        private void DeletePayee_Click(object sender, RoutedEventArgs e)
        {
            //Remove Payee at the index where the button was clicked from SelectedPayeesListBox and add it back to PayeeComboBox
            if (SelectedPayeesListBox.SelectedItem != null)
            {
                var selectedPayee = SelectedPayeesListBox.SelectedItem.ToString();
                SelectedPayeesListBox.Items.Remove(selectedPayee);

            }

        }

        private void PayeeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Add the selected payee to the SelectedPayeesListBox
            if (PayeeComboBox.SelectedItem != null)
            {
                var selectedPayee = PayeeComboBox.SelectedItem.ToString();
                if (!SelectedPayeesListBox.Items.Contains(selectedPayee))
                {
                    SelectedPayeesListBox.Items.Add(selectedPayee);
                }
                // Remove the selection from the combo box to prevent re-selection
                PayeeComboBox.SelectedItem = null;
            }
        }

        private void SelectedPayeesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check if the selected item is not null and enable the delete button
            if (SelectedPayeesListBox.SelectedItem != null)
            {
                DeletePayeeButton.IsEnabled = true;
            }
            else
            {
                DeletePayeeButton.IsEnabled = false;
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem != null && CategoryComboBox.SelectedItem.Equals("Add New")){
                NewExpenseCategoryTextBox.Visibility = Visibility.Visible;
                NewExpenseCategoryTextBox.Focus();
                _newExpenseCategory = true;
            }
        }
    }
}


