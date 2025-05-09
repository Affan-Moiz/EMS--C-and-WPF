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
    /// Interaction logic for MonthlyYearlyWindow.xaml
    /// </summary>
    public partial class MonthlyYearlyWindow : Window
    {
        public int option;

        public MonthlyYearlyWindow()
        {
            InitializeComponent();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            option = -1;
            this.DialogResult = true;
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (MonthlyOption.IsChecked == true)
            {
                option = 0;
            }
            else if (YearlyOption.IsChecked == true)
            {
                option = 1;
            }
            else if (TotalOption.IsChecked == true)
            {
                option = 2;
            }
            else
            {
                MessageBox.Show("Please select a valid option.", "Invalid Option", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
