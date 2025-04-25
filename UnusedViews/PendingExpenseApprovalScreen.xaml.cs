using ProjectVersion2.ViewModels;
using ProjectVersion2.Model;
using System.Windows;

namespace ProjectVersion2.Views
{
    public partial class PendingExpenseApprovalsScreen : Window
    {

        public AdminViewModel _adminUserModel;

        public PendingExpenseApprovalsScreen(ref AdminViewModel adminUserModel)
        {
            InitializeComponent();
            DataContext = adminUserModel;
            _adminUserModel = adminUserModel;
        }

        

        private void ApproveExpense_Click(object sender, RoutedEventArgs e)
        {
            // Approve selected expense
            _adminUserModel.ApproveExpense((Expenses)PendingExpensesDataGrid.SelectedItem);
        }

        private void RejectExpense_Click(object sender, RoutedEventArgs e)
        {
            // Reject selected expense
            _adminUserModel.RejectExpense((Expenses)PendingExpensesDataGrid.SelectedItem);
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Edit Expense screen
        }
    }
}

