using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Services
{
    class ExportSalaryService
    {
        public void ExportSalaryToCSV(List<Salary> salaries, bool flag)
        {
          
            
                string filePath = ConstantInfo.SalariesFilePathCSV;
            
            


            using (var writer = new StreamWriter(filePath))
            {
                // Write the header
                writer.WriteLine("ID,UserID,Description,Amount,Date,Type");
                // Write each salary record
                foreach (var salary in salaries)
                {
                    writer.WriteLine($"{salary.Id},{salary.UserId},{salary.Description},{salary.Amount},{salary.Date},{salary.SalaryType}");
                }
            }
        }
    }
}
