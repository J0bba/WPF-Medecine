using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace virsol_tMedicalDotNet.Model
{
    public class User : ObservableObject
    {
        public string login { get; set; }
        public string pwd { get; set; }
        public string name { get; set; }
        public string firstname { get; set; }
        public Byte[] picture { get; set; }
        public string role { get; set; }
        public bool connected { get; set; }
    }
}
