using ProjectVersion2.Model;
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
    /// Interaction logic for GuestDashboard.xaml
    /// </summary>
    public partial class GuestDashboard : Window
    {
        private GuestViewModel _guestViewModel;
        public GuestDashboard(Guid guestId)
        {
            InitializeComponent();
            _guestViewModel = new GuestViewModel(guestId);
            DataContext= _guestViewModel;
        }

        private void PendingExpensesGridControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ApproveExpense_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _guestViewModel.Save();
            LoginSignupWindow loginSignupWindow = new LoginSignupWindow();
            loginSignupWindow.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PayExpense_Click(object sender, RoutedEventArgs e)
        {
            GuestPayWindow guestPayWindow = new GuestPayWindow(ref _guestViewModel, (Expenses)PendingExpensesGridControl.SelectedItem);
            guestPayWindow.Show();
        }
    }
}
