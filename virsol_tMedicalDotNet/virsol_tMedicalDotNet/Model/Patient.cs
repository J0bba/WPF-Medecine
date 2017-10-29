using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace virsol_tMedicalDotNet.Model
{
    public class Patient : ObservableObject
    {
        public string name { get; set; }
        public string firstname { get; set; }
        public int id { get; set; }
        public DateTime birthday { get; set; }
        public ObservableCollection<Observation> observations { get; set; }

        public string prettyname { get
            {
                return firstname + " " + name;
            } }
        public string prettybirthdate
        {
            get
            {
                return birthday.ToShortDateString();
            }
        }
    }
}
