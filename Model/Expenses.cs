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
        public required Guid UserId { get; set; }
        public required decimal Amount { get; set; }
        public required string Description { get; set; }
        public required ExpenseCategories Category { get; set; }
        public PaymentMethod PMethod { get; set; }
        public ExpenseStatus Status { get; set; } 
        public required DateTime Date { get; set; } 
        public bool IsRecurring { get; set; } = false;
        public List<Guid>? Payees { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
