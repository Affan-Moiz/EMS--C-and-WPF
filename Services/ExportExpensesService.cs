using DevExpress.Data.Controls.ExpressionEditor;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Services
{
    class ExportExpensesService
    {
        string filePath;
        public void ExportExpensesToCSV(List<Expenses> expenses, bool flag)
        {
            if (flag)
            {
                filePath = Utilities.ConstantInfo.ExpensesFilePathCSV;

            }
            else
            {
                filePath = Utilities.ConstantInfo.PendingExpensesFilePathCSV;
            }

                using (var writer = new StreamWriter(filePath))
                {
                    // Write the header
                    writer.WriteLine("ID,UserID,Description,Amount,Category,Date");
                    // Write each expense
                    foreach (var expense in expenses)
                    {
                        writer.WriteLine($"{expense.Id},{expense.UserId},{expense.Description},{expense.Amount},{expense.Category},{expense.Date}");
                    }
                }
        }
    }
}
