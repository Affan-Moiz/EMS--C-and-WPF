using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for ExpenseList.xaml
    /// </summary>
    public partial class ExpenseList : Window
    {
        private readonly UserViewModel _userViewModel;
        public ObservableCollection<Expenses> ExpensesList { get; set; }

        public ExpenseList(Guid userId)
        {
            DataContext = this;
            InitializeComponent();
            _userViewModel = new UserViewModel(userId); // Initialize the UserViewModel with the current user ID
            LoadExpenses();
        }

        private void LoadExpenses()
        {
            try
            {
                // Convert the List<Expenses> to ObservableCollection<Expenses>
                var expensesList = _userViewModel.GetExpenses();
                ExpensesList = new ObservableCollection<Expenses>(expensesList);

                // Bind the ObservableCollection to the DataGrid
                ExpenseDataGrid.ItemsSource = ExpensesList; // Assuming the DataGrid is named ExpenseDataGrid in the XAML
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading expenses: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewExpenseDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ExpenseDataGrid.SelectedItem is Expenses selectedExpense)
            {
                // Open the ExpenseDetailsScreen for the selected expense
                var expenseDetailsScreen = new ExpenseDetailsScreen( selectedExpense);
                expenseDetailsScreen.ShowDialog();
                LoadExpenses(); // Reload the expenses in case they were updated
                
            }
            else
            {
                MessageBox.Show("Please select an expense to view details.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            // Open the AddExpenseScreen to add a new expense
            var addExpenseScreen = new AddExpenseScreen(_userViewModel.GetUserID());
            addExpenseScreen.ShowDialog();
            LoadExpenses(); // Reload the expenses after adding a new one
        }

        private void RemoveExpense_Click(object sender, RoutedEventArgs e)
        {
            if (ExpenseDataGrid.SelectedItem is Expenses selectedExpense)
            {
                // Confirm removal
                var result = MessageBox.Show("Are you sure you want to remove this expense?", "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove the selected expense
                    _userViewModel.RemoveExpense(selectedExpense.Id);
                    MessageBox.Show("Expense removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadExpenses(); // Reload the expenses after removal
                }
            }
            else
            {
                MessageBox.Show("Please select an expense to remove.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //private void EditExpense_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ExpenseDataGrid.SelectedItem is Expenses selectedExpense)
        //    {
        //        // Open the AddExpenseScreen in edit mode
        //        var editExpenseScreen = new AddExpenseScreen(_userViewModel.GetUserID(), selectedExpense);
        //        editExpenseScreen.ShowDialog();
        //        LoadExpenses(); // Reload the expenses after editing
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select an expense to edit.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}

        private void DeleteExpense_Click(object sender, RoutedEventArgs e)
        {
            RemoveExpense_Click(sender, e); // Reuse the RemoveExpense logic
        }

        private void ViewExpense_Click(object sender, RoutedEventArgs e)
        {
            ViewExpenseDetails_Click(sender, e); // Reuse the ViewExpenseDetails logic
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the ExpenseList window
        }


    }
}
