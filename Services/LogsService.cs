using ProjectVersion2.Model;
using ProjectVersion2.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProjectVersion2.Services
{
    public class LogsService
    {
        private string filePath = ConstantInfo.LogsFilePath;

        public List<Log> LoadLogs()
        {
            if (!File.Exists(filePath)) return new List<Log>();
            var serializer = new XmlSerializer(typeof(List<Log>));
            using var stream = File.OpenRead(filePath);
            var result = serializer.Deserialize(stream) as List<Log>;
            return result ?? new List<Log>();
        }

        public void SaveLogs(List<Log> logs)
        {
            var serializer = new XmlSerializer(typeof(List<Log>));
            using var stream = File.Create(filePath);
            serializer.Serialize(stream, logs);
        }

        public Dictionary<Guid, Log> LoadLogsAsDictionary()
        {
            var LogsList = LoadLogs();
            return LogsList.ToDictionary(s => s.Id);
        }

        public void SaveLogsFromDictionary(Dictionary<Guid, Log> logsDictionary)
        {
            var logsList = logsDictionary.Values.ToList();
            SaveLogs(logsList);
        }
    }
}
