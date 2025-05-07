using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Utilities
{
    class ConstantInfo
    {
        public const string UsersFilePath = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\users.xml";
        public const string ExpensesFilePath = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\expenses.xml";
        public const string SalariesFilePath = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\salary.xml";

        public const string UsersFilePathCSV = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\users.csv";
        public const string ExpensesFilePathCSV = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\expenses.csv";
        public const string SalariesFilePathCSV = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\salary.csv"; 
        public const string PendingUsersFilePathCSV = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\pendingUsers.csv";
        public const string PendingExpensesFilePathCSV = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\pendingExpenses.csv";

        public const string ExpenseCategoriesFilePath = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\customExpenses.xml";
        public const string NotificationFilePath = "C:\\Users\\amoiz\\source\\repos\\ProjectVersion2\\files\\notification.xml";


        public const string LowBalanceMessage = "Your Balance is low. Decrease your expenses or add more capital to you account";
        public const string BudgetApproaching = "Your Expenses are close to the set budget. Please spend carefully";
        public const string BudgetConsumed = "Your Expenses crossed the budget threshold. Please avoid any expenses before the next salary";
        public const string RequestApproved = "Your Expense request was approved by an administrator";
        public const string RequestRejected = "Your Expense request was rejected by an administrator";
        public const string RequestAssigned = "You have an assigned Expense by an administrator";


    }
}
