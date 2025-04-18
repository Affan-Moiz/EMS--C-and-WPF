using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for SalaryList.xaml
    /// </summary>
    public partial class SalaryList : Window
    {
        private readonly UserViewModel _userViewModel;
        private List<Salary> _salariesList;

        public SalaryList(Guid userId)
        {
            InitializeComponent();
            DataContext = this; // Set the DataContext for data binding
            _userViewModel = new UserViewModel(userId); // Initialize the UserViewModel with the current user ID
            LoadSalaries();
        }

        private void LoadSalaries()
        {
            try
            {
                // Load the salaries for the current user and bind them to the DataGrid
                _salariesList = _userViewModel.GetSalariesByUserId(_userViewModel.GetUserID());
                SalaryDataGrid.ItemsSource = _salariesList; // Assuming the DataGrid is named SalaryDataGrid in the XAML
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading salaries: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddSalary_Click(object sender, RoutedEventArgs e)
        {
            AddSalaryScreen addSalaryScreen = new AddSalaryScreen(_userViewModel.GetUserID());
            addSalaryScreen.ShowDialog();
            LoadSalaries(); // Reload the salaries after adding a new one
        }

        //private void ViewSalaryDetails_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
        //    {
        //        // Open the SalaryDetailsScreen for the selected salary
        //        var salaryDetailsScreen = new SalaryDetailsScreen(selectedSalary);
        //        salaryDetailsScreen.ShowDialog();
        //        LoadSalaries(); // Reload the salaries in case they were updated
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a salary to view details.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}

        //private void AddSalary_Click(object sender, RoutedEventArgs e)
        //{
        //    // Open the AddSalaryScreen to add a new salary
        //    var addSalaryScreen = new AddSalaryScreen(_userViewModel.GetUserID());
        //    addSalaryScreen.ShowDialog();
        //    LoadSalaries(); // Reload the salaries after adding a new one
        //}

        //private void RemoveSalary_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
        //    {
        //        // Confirm removal
        //        var result = MessageBox.Show("Are you sure you want to remove this salary?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            // Remove the selected salary
        //            _userViewModel.RemoveSalary(selectedSalary.Id);
        //            MessageBox.Show("Salary removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //            LoadSalaries(); // Reload the salaries after removal
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a salary to remove.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}

        //private void DeleteSalary_Click(object sender, RoutedEventArgs e)
        //{
        //    RemoveSalary_Click(sender, e); // Reuse the RemoveSalary logic
        //}

        //private void ViewSalary_Click(object sender, RoutedEventArgs e)
        //{
        //    ViewSalaryDetails_Click(sender, e); // Reuse the ViewSalaryDetails logic
        //}

        //private void EditSalary_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SalaryDataGrid.SelectedItem is Salary selectedSalary)
        //    {
        //        // Open the AddSalaryScreen in edit mode
        //        var editSalaryScreen = new AddSalaryScreen(_userViewModel.GetUserID(), selectedSalary);
        //        editSalaryScreen.ShowDialog();
        //        LoadSalaries(); // Reload the salaries after editing
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select a salary to edit.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}
    }
}

