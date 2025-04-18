using System;
using System.Windows;
using ProjectVersion2.Model;
using ProjectVersion2.ViewModels;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for ExpenseDetailsScreen.xaml
    /// </summary>
    public partial class ExpenseDetailsScreen : Window
    {
        readonly Expenses _expense;

        public ExpenseDetailsScreen( Expenses expense)
        {
            InitializeComponent();
            _expense = expense; // Store the expense to display its details
            LoadExpenseDetails();
        }

        private void LoadExpenseDetails()
        {
           
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to an edit expense screen (not implemented here)
            MessageBox.Show("Edit Expense functionality is not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}


