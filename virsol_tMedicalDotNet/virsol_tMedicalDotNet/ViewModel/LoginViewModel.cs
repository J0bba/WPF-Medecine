using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private string _login;
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                RaisePropertyChanged("Login");
            }
        }

        public LoginViewModel()
        {
            ConnectCommand = new RelayCommand<PasswordBox>(ConnectMethod);
        }

        public ICommand ConnectCommand { get; private set; }

        private void ConnectMethod(PasswordBox PasswordBox)
        {
            string login = _login;
            System.Console.WriteLine(_login);
            string pwd = PasswordBox.Password;
            var serviceUser = new ServiceUser.ServiceUserClient();
            if (serviceUser.Connect(login, pwd))
            {
                OpenHomeWindow(login);
            }
            else
            {
                // SHOW DIALOG BOX
                MessageBox.Show("Wrong login/password");
            }
        }

        private void OpenHomeWindow(string currLogin)
        {
            var current = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var app = new MainView();
            var context = new MainViewModel();
            context.CurrUserLogin = currLogin;
            app.DataContext = context;
            app.Show();
            current.Close();
        }

    }
}
