using System;
using System.Collections.ObjectModel;
using System.Windows;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;
using ProjectVersion2.ViewModels;

namespace ProjectVersion2.Views
{
    public partial class AddSalaryScreen : Window
    {
        private readonly UserViewModel _userViewModel;

        public AddSalaryScreen(Guid userId, ref UserViewModel UVModel)
        {
            InitializeComponent();
            DataContext = UVModel;

            _userViewModel = UVModel;
            


            
        }

        private void SubmitSalary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(AmountTextBox.Text) || !decimal.TryParse(AmountTextBox.Text, out var amount))
                {
                    MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (SalaryTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a salary type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please select a date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create a new salary
                var newSalary = new Salary
                {
                    Id = Guid.NewGuid(),
                    UserId = _userViewModel.GetUserID(),
                    Amount = decimal.Parse(AmountTextBox.Text),
                    Description = DescriptionTextBox.Text,
                    SalaryType = (SalaryType)Enum.Parse(typeof(SalaryType), SalaryTypeComboBox.SelectedItem.ToString()),
                    Date = DatePicker.SelectedDate.Value,
                    IsRecurring= IsRecurringCheckBox.IsChecked == true
                };

                // Save the salary
                _userViewModel.AddSalary(newSalary);
                MessageBox.Show("Salary added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
    }
}
