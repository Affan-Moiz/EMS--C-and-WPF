using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Services
{
    class SalaryService
    {
        // This class will handle the loading and saving of salary data on a file

        public SalaryService() { }

        public List<Salary> LoadSalaries()
        {
            if (!File.Exists(ConstantInfo.SalariesFilePath)) return new List<Salary>();
            var serializer = new XmlSerializer(typeof(List<Salary>));
            using var stream = File.OpenRead(ConstantInfo.SalariesFilePath);
            var result = serializer.Deserialize(stream) as List<Salary>;
            return result ?? new List<Salary>();
        }

        public void SaveSalaries(List<Salary> salaries)
        {
            var serializer = new XmlSerializer(typeof(List<Salary>));
            using var stream = File.Create(ConstantInfo.SalariesFilePath);
            serializer.Serialize(stream, salaries);
        }

        //Load Salaries as a dictionary
        public Dictionary<Guid, Salary> LoadSalariesAsDictionary()
        {
            var salariesList = LoadSalaries();
            return salariesList.ToDictionary(s => s.Id);
        }

        // Save Salaries from a dictionary
        public void SaveSalariesFromDictionary(Dictionary<Guid, Salary> salariesDictionary)
        {
            var salariesList = salariesDictionary.Values.ToList();
            SaveSalaries(salariesList);
        }
    }
}
