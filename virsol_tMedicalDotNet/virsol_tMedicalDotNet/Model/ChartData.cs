using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace virsol_tMedicalDotNet.Model
{
    public class ChartData
    {
        DateTime _Name;
        double _Value;

        public DateTime Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

    }
}
