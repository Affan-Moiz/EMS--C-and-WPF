using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Model
{
    public class DataPoint
    {
        public string Argument { get; set; }
        public double Value { get; set; }
        public DataPoint(string argument, double value)
        {
            Argument = argument;
            Value = value;
        }
    }
}
