using System.Windows;
using virsol_tMedicalDotNet.ViewModel;

namespace virsol_tMedicalDotNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainView()
        {
            //InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}