using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ProjectVersion2.Model;
using ProjectVersion2.Utilities;

public class UserDataService
{
    private string FilePath = ConstantInfo.UsersFilePath;

    // Load users as a list
    public List<Users>? LoadUsers()
    {
        if (!File.Exists(FilePath)) return new List<Users>();
        var serializer = new XmlSerializer(typeof(List<Users>));
        using var stream = File.OpenRead(FilePath);
        return (List<Users>?)serializer.Deserialize(stream);
    }

    // Save users as a list
    public void SaveUsers(List<Users> users)
    {
        var serializer = new XmlSerializer(typeof(List<Users>));
        using var stream = File.Create(FilePath);
        serializer.Serialize(stream, users);
    }

    // New function to load users as a dictionary
    public Dictionary<Guid, Users>? LoadUsersAsDictionary()
    {
        var usersList = LoadUsers();
        return usersList?.ToDictionary(u => u.Id);
    }

    // New function to save users from a dictionary
    public void SaveUsersFromDictionary(Dictionary<Guid, Users> usersDictionary)
    {
        var usersList = usersDictionary.Values.ToList();
        SaveUsers(usersList);
    }
}

