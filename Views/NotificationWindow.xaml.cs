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
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private UserViewModel _userViewModel;
        public NotificationWindow(ref UserViewModel userViewModel)
        {
            InitializeComponent();
            userViewModel.ReArrangeNotifications();
            DataContext = userViewModel;
            _userViewModel = userViewModel;
        }

        private void GridControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _userViewModel.ReArrangeNotifications();
        }
    }
}
