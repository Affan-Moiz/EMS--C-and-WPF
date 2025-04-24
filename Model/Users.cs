using ProjectVersion2.Utilities;
using System;
using System.ComponentModel;

namespace ProjectVersion2.Model
{
    public class Users: INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Username { get; set; }
        public required string HashedPassword { get; set; }
        public required string Email { get; set; }
        public Role Role { get; set; }
        public bool IsApproved
        {
            get { return _isApproved; }
            set
            {
                if (_isApproved != value)
                {
                    _isApproved = value;
                    OnPropertyChanged(nameof(IsApproved)); // Notify the UI about the change
                }
            }
        }

        private bool _isApproved=false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    


}
