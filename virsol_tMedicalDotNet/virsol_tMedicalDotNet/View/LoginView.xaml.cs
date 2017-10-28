using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;
using virsol_tMedicalDotNet.ViewModel;

namespace virsol_tMedicalDotNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public LoginView()
        {
            //InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage msg)
        {
            System.Console.WriteLine(msg.Notification);
            if (msg.Notification == "ShowMainView")
            {
                var view2 = new MainView();
                view2.Show();
            }
        }
    }
}