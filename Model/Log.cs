using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Model
{
    public class Log : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        private string _message;
        private DateTime _timestamp;

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

        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
