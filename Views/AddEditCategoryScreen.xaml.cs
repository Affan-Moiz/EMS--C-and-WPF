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
    /// Interaction logic for AddEditCategoryScreen.xaml
    /// </summary>
    public partial class AddEditCategoryScreen : Window
    {
        private AdminViewModel _adminViewModel;
        private bool _isEdit;
        private string previousContent;
        public AddEditCategoryScreen(ref AdminViewModel adminViewModel, string content)
        {
            InitializeComponent();
            DataContext = adminViewModel;
            _adminViewModel = adminViewModel;
            if (content != null)
            {
                _isEdit = true;
                CategoryTextBox.Text = content;
            }
            else
            {
                _isEdit = false;
            }

            previousContent = content;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isEdit)
            {
                _adminViewModel.EditExpenseCategory(previousContent, CategoryTextBox.Text);
                MessageBox.Show("Category edited successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                _adminViewModel.AddExpenseCategory(CategoryTextBox.Text);
                MessageBox.Show("Category added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
