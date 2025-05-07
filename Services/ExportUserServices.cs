using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Services
{
    public class ExportUserServices
    {
        string filePath;
        public void ExportUsersToCSV(List<Users> users, bool flag)
        {
            if (flag)
            {
                filePath = Utilities.ConstantInfo.UsersFilePathCSV;

            }
            else
            {
                filePath = Utilities.ConstantInfo.PendingUsersFilePathCSV;
            }

            using (var writer = new StreamWriter(filePath))
            {
                // Write the header
                writer.WriteLine("ID,Username,Email,Role,IsApproved");
                // Write each expense
                foreach (var user in users)
                {
                    writer.WriteLine($"{user.Id},{user.Username},{user.Email},{user.Role},{user.IsApproved}");
                }
            }
        }
    }
}
