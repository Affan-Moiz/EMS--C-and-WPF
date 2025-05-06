using ProjectVersion2.Utilities;
using System;
using System.ComponentModel;

namespace ProjectVersion2.Model
{
    public class Users: INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get { return _username; } set { _username = value; OnPropertyChanged(); } }
        public required string HashedPassword { get { return _password; } set { _password = value; OnPropertyChanged(); } }
        public required string Email { get { return _email; } set { _email = value; OnPropertyChanged(); } }
        public Role Role { get { return _role; } set { _role = value; OnPropertyChanged(); } }
        public bool IsApproved { get { return _isApproved; } set { _isApproved = value; OnPropertyChanged(); } }


        private string _username = "";
        private string _password = "";
        private string _email="";
        private Role _role;
        private bool _isApproved=false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    


}
