using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Services
{
    public class ExpenseCategoriesService
    {
        public ExpenseCategoriesService() { }

        private string FilePath = ConstantInfo.ExpenseCategoriesFilePath;

        public List<String> LoadExpenseCategories()
        {
            if (!File.Exists(FilePath)) return new List<String>();
            var serializer = new XmlSerializer(typeof(List<String>));
            using var stream = File.OpenRead(FilePath);
            var result = serializer.Deserialize(stream) as List<String>;
            return result ?? new List<String>();
        }

        public void SaveExpenseCategories(List<String> expenseCategories)
        {
            var serializer = new XmlSerializer(typeof(List<String>));
            using var stream = File.Create(FilePath);
            serializer.Serialize(stream, expenseCategories);
        }
    }
}
