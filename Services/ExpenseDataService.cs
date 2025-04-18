using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Services
{
    class ExpenseDataService
    {
        public ExpenseDataService() { }

        private string FilePath = ConstantInfo.ExpensesFilePath;

        public List<Expenses> LoadExpenses()
        {
            if (!File.Exists(FilePath)) return new List<Expenses>();
            var serializer = new XmlSerializer(typeof(List<Expenses>));
            using var stream = File.OpenRead(FilePath);
            var result = serializer.Deserialize(stream) as List<Expenses>;
            return result ?? new List<Expenses>();
        }

        public void SaveExpenses(List<Expenses> expenses)
        {
            var serializer = new XmlSerializer(typeof(List<Expenses>));
            using var stream = File.Create(FilePath);
            serializer.Serialize(stream, expenses);
        }

        // New function to load expenses as a dictionary
        public Dictionary<Guid, Expenses> LoadExpensesAsDictionary()
        {
            var expensesList = LoadExpenses();
            return expensesList.ToDictionary(e => e.Id);
        }

        // New function to save expenses from a dictionary
        public void SaveExpensesFromDictionary(Dictionary<Guid, Expenses> expensesDictionary)
        {
            var expensesList = expensesDictionary.Values.ToList();
            SaveExpenses(expensesList);
        }
    }
}
