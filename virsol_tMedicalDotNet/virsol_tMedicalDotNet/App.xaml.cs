using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace virsol_tMedicalDotNet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            View.LoginView window = new View.LoginView();
            ViewModel.LoginViewModel vm = new ViewModel.LoginViewModel();
            window.DataContext = vm;
            window.Show();
        }*/
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
