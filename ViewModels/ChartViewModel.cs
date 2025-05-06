using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ProjectVersion2.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.ViewModels
{
    public class ChartViewModel
    {
        public Collection<DataPoint> Data { get; private set; }
        public ChartViewModel()
        {
            Data = new Collection<DataPoint> {
                        new DataPoint ("Bikes", 142345),
                        new DataPoint ("Accessories", 266344),
                        new DataPoint ("Components", 631359),
                        new DataPoint ("Clothing", 120007)
            };
        }
    }
}
