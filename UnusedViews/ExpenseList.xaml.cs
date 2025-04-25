using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;
using MessageBox = System.Windows.MessageBox;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for ExpenseList.xaml
    /// </summary>
    public partial class ExpenseList : Window
    {
        private  UserViewModel _userViewModel;

        public ExpenseList(Guid userId,ref UserViewModel UVModel)
        {
            InitializeComponent();
            DataContext = UVModel;
            _userViewModel = UVModel; // Initialize the UserViewModel with the current user ID
      
        }

        

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            // Open the AddExpenseScreen to add a new expense
            var addExpenseScreen = new AddExpenseScreen(_userViewModel.GetUserID(), ref _userViewModel);
            addExpenseScreen.ShowDialog();
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
                   
                }
            }
            else
            {
                MessageBox.Show("Please select an expense to remove.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        

       

        

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }


    }
}
