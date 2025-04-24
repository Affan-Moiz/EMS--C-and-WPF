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
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Description { get; set; }

        public SalaryType SalaryType { get; set; }

        public bool IsRecurring { get; set; }    

    public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
