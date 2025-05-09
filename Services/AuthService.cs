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
        Dictionary<Guid, Guest> guests;
        EncryptorDecryptor encryptorDecryptor = new();
        UserDataService userDataService = new UserDataService();
        GuestService guestService = new GuestService();

        public AuthService() {
            users = userDataService.LoadUsersAsDictionary();
            guests = guestService.LoadGuestsAsDictionary();
        }


        public Users? Login(string username, string password)
        {
            var user = users.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null || !VerifyPassword(password, user.HashedPassword)) return null;
            return user;
        }

        public Guest? Login(string username)
        {
            var guest = guests.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (guest == null) return null;
            return guest;
        }

        private bool VerifyPassword(string input, string hashed)
        {
            string hashedInput = encryptorDecryptor.MD5Hash(input);
            return hashedInput.Equals(hashed);
        }

        //Sign Up 
        public bool SignUp(string username, string password, string email, Role role)
        {
            if (users.Values.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) return false;
            var newUser = new Users
            {
                Id = Guid.NewGuid(),
                Username = username,
                HashedPassword = encryptorDecryptor.MD5Hash(password),
                Email = email,
                IsApproved = false,
                Role = role
            };
            users[newUser.Id] = newUser;
            SaveUsers();
            return true;
        }

        public bool SignUp(string username,string email, bool isApproved)
        {
            if (users.Values.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) return false;
            var newUser = new Guest
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                IsApproved = isApproved,
                Role = Role.Guest
            };
            guests[newUser.Id] = newUser;
            SaveGuests();
            return true;
        }

        public bool SignUp(Users user)
        {
            if (users.Values.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase))) return false;
            user.Id = Guid.NewGuid();
            user.IsApproved = true;
            user.HashedPassword = encryptorDecryptor.MD5Hash(user.HashedPassword);
            users[user.Id] = user;
            SaveUsers();
            return false;
        }

        public bool SignUp(Guest guest)
        {
            if (guests.Values.Any(u => u.Username.Equals(guest.Username, StringComparison.OrdinalIgnoreCase))) return false;
            guest.Id = Guid.NewGuid();
            guest.IsApproved = true;
            guests[guest.Id] = guest;
            SaveGuests();
            return false;
        }

        private void SaveUsers()
        {
            userDataService.SaveUsersFromDictionary(users);
        }

        private void SaveGuests()
        {
            guestService.SaveGuestsFromDictionary(guests);
        }
    }
}

