using ProjectVersion2.Utilities;
using System;

namespace ProjectVersion2.Model
{
    public class Users
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get; set; }
        public required string HashedPassword { get; set; }
        public required string Email { get; set; }
        public Role Role { get; set; }
        public bool IsApproved { get; set; } = false;
    }


}
