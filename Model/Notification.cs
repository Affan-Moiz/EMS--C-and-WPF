using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Model
{
    public class Notification: INotifyPropertyChanged
    {
        private Guid _id;
        private Guid _userId;
        private string _message;
        private DateTime _createdAt;
        private bool _isRead;

        public Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guid UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRead
        {
            get => _isRead;
            set
            {
                if (_isRead != value)
                {
                    _isRead = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        


        public Notification() { }

        public Notification(Guid id, Guid userId, string message)
        {
            Id = id;
            UserId = userId;
            Message = message;
            CreatedAt = DateTime.Now;
            IsRead = false;
        }

        public Notification(Guid id, Guid userId, string message, DateTime createdAt, bool isRead)
        {
            Id = id;
            UserId = userId;
            Message = message;
            CreatedAt = createdAt;
            IsRead = isRead;
        }

        public Notification(Guid id, Guid userId, string message, bool isRead)
        {
            Id = id;
            UserId = userId;
            Message = message;
            CreatedAt = DateTime.Now;
            IsRead = isRead;

        }

        
    }
}
