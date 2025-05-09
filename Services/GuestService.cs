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
    public class GuestService
    {
        string filePath = ConstantInfo.GuestFilePath;
        public GuestService() { }

        public List<Guest> LoadGuests()
        {
            if (!File.Exists(filePath)) return new List<Guest>();
            var serializer = new XmlSerializer(typeof(List<Guest>));
            using var stream = File.OpenRead(ConstantInfo.GuestFilePath);
            var result = serializer.Deserialize(stream) as List<Guest>;
            return result ?? new List<Guest>();
        }

        public void SaveGuests(List<Guest> guests)
        {
            var serializer = new XmlSerializer(typeof(List<Guest>));
            using var stream = File.Create(filePath);
            serializer.Serialize(stream, guests);
        }

        public Dictionary<Guid, Guest> LoadGuestsAsDictionary()
        {
            var guestList = LoadGuests();
            return guestList.ToDictionary(s => s.Id);
        }

        public void SaveGuestsFromDictionary(Dictionary<Guid, Guest> guestDictionary)
        {
            var guestList = guestDictionary.Values.ToList();
            SaveGuests(guestList);
        }
    }
}
