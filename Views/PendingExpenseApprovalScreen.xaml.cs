using System.Windows;

namespace ProjectVersion2.Views
{
    public partial class PendingExpenseApprovalsScreen : Window
    {
        public PendingExpenseApprovalsScreen()
        {
            InitializeComponent();
            LoadPendingExpenses();
        }

        private void LoadPendingExpenses()
        {
            // Load pending expenses from the data source
            // Example:
            // PendingExpensesListBox.ItemsSource = GetPendingExpenses();
        }

        private void ApproveExpense_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected expense
        }

        private void RejectExpense_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected expense
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit Expense screen
        }
    }
}

