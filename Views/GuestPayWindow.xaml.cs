using ProjectVersion2.Model;
using ProjectVersion2.Utilities;
using ProjectVersion2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectVersion2.Views
{
    /// <summary>
    /// Interaction logic for GuestPayWindow.xaml
    /// </summary>
    public partial class GuestPayWindow : Window
    {
        private GuestViewModel _guestViewModel;
        private Expenses _expense;
        public GuestPayWindow(ref GuestViewModel guestViewModel, Expenses expense)
        {
            InitializeComponent();
            DataContext = guestViewModel;
            _guestViewModel = guestViewModel;
            _expense = expense;
        }

        private void SubmitExpense_Click(object sender, RoutedEventArgs e)
        {
            double paidAmount = double.Parse(AmountTextBox.Text);
            string description = DescriptionTextBox.Text;

            //if paid amount is less that or equal to zero or greater then the expense amount
            if (paidAmount <= 0 || paidAmount > _expense.Amount)
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }
            //if description is empty
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please enter a description.");
                return;
            }

            _expense.Amount -= paidAmount;

            if(_expense.Amount == 0)
            {
                _expense.Status = ExpenseStatus.Completed;
            }
           
            Notification notification = new Notification()
            {
                Id = Guid.NewGuid(),
                UserId = _expense.UserId,
                Message = $"{_guestViewModel.GetCurrentGuest().Username} has paid {paidAmount} for {_expense.Description}. Remaining amount is {_expense.Amount}.",
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            //Add the notification to the guest
            _guestViewModel.AddNotification(notification);


            _guestViewModel.UpdateExpense(_expense);

            //Tell the user that the expense amount was paid
            MessageBox.Show($"You have paid {paidAmount} for {_expense.Description}. Remaining amount is {_expense.Amount}.", "Payment Successful");

            Log log = new Log()
            {
                Id = Guid.NewGuid(),
                Message = $"{_guestViewModel.GetCurrentGuest().Username} has paid {paidAmount} for {_expense.Description} with the ID {_expense.Id} for the user with the ID {_expense.UserId}. Remaining amount is {_expense.Amount}.",
                Timestamp = DateTime.Now
            };

            _guestViewModel.AddLog(log);
            //Close the window
            this.Close();

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _guestViewModel.Save();
        }
    }
}
