using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace virsol_tMedicalDotNet.Model
{
    public class Observation : ObservableObject
    {
        public DateTime date { get; set; }
        public string comment { get; set; }
        public string[] prescription { get; set; }
        public Byte[][] pictures { get; set; }
        public int weight { get; set; }
        public int bloodPressure { get; set; }
        public string prettyname
        {
            get
            {
                return date.ToShortDateString() + " " + date.ToShortTimeString();
            }
        }
        public string prettyweight { get
            {
                return weight.ToString() + " kg"; 
            } }
    }
}
