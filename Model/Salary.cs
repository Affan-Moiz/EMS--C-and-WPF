using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Model
{
    public class Salary : INotifyPropertyChanged
    {
        private Guid _id;
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

        private Guid _userId;
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

        private double _amount;
        public double Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private SalaryType _salaryType;
        public SalaryType SalaryType
        {
            get => _salaryType;
            set
            {
                if (_salaryType != value)
                {
                    _salaryType = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isRecurring;
        public bool IsRecurring
        {
            get => _isRecurring;
            set
            {
                if (_isRecurring != value)
                {
                    _isRecurring = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
