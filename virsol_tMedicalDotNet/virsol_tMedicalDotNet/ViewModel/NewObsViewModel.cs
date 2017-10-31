using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using virsol_tMedicalDotNet.View;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class NewObsViewModel : ViewModelBase
    {
        public MainViewModel lastWindow { get; set; }
        public string PatientNameLabel
        {
            get
            {
                return lastWindow == null ? "Patient : " : "Patient : " + lastWindow.SelectedPatient.prettyname;
            }
        }
        public NewObsViewModel()
        {
            System.Console.WriteLine("Dans le constructeur ");
        }

    }
}
