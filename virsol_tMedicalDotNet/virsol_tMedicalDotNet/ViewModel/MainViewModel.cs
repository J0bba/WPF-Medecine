using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public string CurrUserLogin { get; set; }
        public MainViewModel()
        {
            ConnectCommand = new RelayCommand(ConnectMethod);
        }

        public ICommand ConnectCommand { get; private set; }

        private void ConnectMethod()
        {
        }

    }
}
