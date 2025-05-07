using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectVersion2.Utilities;
using ProjectVersion2.Model;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace ProjectVersion2.Services
{
    public class NotificationService
    {
        private string filePath = ConstantInfo.NotificationFilePath;

        public List<Notification> LoadSalaries()
        {
            if (!File.Exists(filePath)) return new List<Notification>();
            var serializer = new XmlSerializer(typeof(List<Notification>));
            using var stream = File.OpenRead(filePath);
            var result = serializer.Deserialize(stream) as List<Notification>;
            return result ?? new List<Notification>();
        }

        public void SaveSalaries(List<Notification> salaries)
        {
            var serializer = new XmlSerializer(typeof(List<Notification>));
            using var stream = File.Create(filePath);
            serializer.Serialize(stream, salaries);
        }

        public Dictionary<Guid, Notification> LoadNotificationsAsDictionary()
        {
            var notificationsList = LoadSalaries();
            return notificationsList.ToDictionary(s => s.Id);
        }

        public void SaveNotificationsFromDictionary(Dictionary<Guid, Notification> notificationDictionary)
        {
            var notificationsList = notificationDictionary.Values.ToList();
            SaveSalaries(notificationsList);
        }

    }
}
