using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Utilities
{
    public class ExpenseCategory
    {
        public List<string> Categories { get; set; }

        public ExpenseCategory()
        {
            Categories = new List<string>
            {
                "Food",
                "Transport",
                "Entertainment",
                "Utilities",
                "Health",
                "Education",
                "Travel",
                "Other"
            };
        }

        public void AddCategory(string category)
        {
            if (category != null)
            {
                if (!Categories.Contains(category))
                {
                    Categories.Add(category);
                }
                else
                {
                    Console.WriteLine("Category already exists.");
                }
            }
        }
    }
}
