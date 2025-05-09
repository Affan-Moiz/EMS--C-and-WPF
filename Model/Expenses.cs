using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectVersion2.Utilities;

namespace ProjectVersion2.Model
{
    public class Expenses : INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        private Guid _userId = Guid.Empty;
        public required Guid UserId
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

        private double _amount = 0;
        public required double Amount
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

        private string _description = "";
        public required string Description
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

        private string _category = "Other";
        public required string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged();
                }
            }
        }

        private PaymentMethod _pMethod = PaymentMethod.Cash;
        public PaymentMethod PMethod
        {
            get => _pMethod;
            set
            {
                if (_pMethod != value)
                {
                    _pMethod = value;
                    OnPropertyChanged();
                }
            }
        }

        private ExpenseStatus _status = ExpenseStatus.Pending;
        public ExpenseStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _date = DateTime.Now;
        public required DateTime Date
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

        private bool _isRecurring = false;
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

        private List<String>? _payees = null;
        public List<String>? Payees
        {
            get => _payees;
            set
            {
                if (_payees != value)
                {
                    _payees = value;
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
