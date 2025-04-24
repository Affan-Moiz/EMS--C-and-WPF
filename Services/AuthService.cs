using ProjectVersion2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectVersion2.Services;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Services
{
    class AuthService
    {

        Dictionary<Guid, Users> users;

        public AuthService() {
            UserDataService userDataService = new UserDataService();
            users = userDataService.LoadUsersAsDictionary();
        }


        public Users? Login(string username, string password)
        {
            var user = users.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null || !VerifyPassword(password, user.HashedPassword)) return null;
            return user.IsApproved ? user : null;
        }
        //public Users? Login(string username, string password, Dictionary<Guid, Users> usersDictionary)
        //{
        //    var user = usersDictionary.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        //    if (user == null || !VerifyPassword(password, user.HashedPassword)) return null;
        //    return user.IsApproved ? user : null;
        //}

        private bool VerifyPassword(string input, string hashed)
        {
            return input == hashed;
        }

        //Sign Up 
        public bool SignUp(string username, string password, string email, Role role)
        {
            if (users.Values.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) return false;
            var newUser = new Users
            {
                Id = Guid.NewGuid(),
                Username = username,
                HashedPassword = password,
                Email = email,
                IsApproved = false,
                Role = role
            };
            users[newUser.Id] = newUser;
            SaveUsers();
            return true;
        }

        public bool SignUp(Users user)
        {
            if (users.Values.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase))) return false;
            user.Id = Guid.NewGuid();
            user.IsApproved = true;
            users[user.Id] = user;
            SaveUsers();
            return false;
        }
        private void SaveUsers()
        {
            UserDataService userDataService = new UserDataService();
            userDataService.SaveUsersFromDictionary(users);
        }
    }
}

