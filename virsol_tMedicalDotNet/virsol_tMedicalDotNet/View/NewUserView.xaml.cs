using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using virsol_tMedicalDotNet.ViewModel;

namespace virsol_tMedicalDotNet.View
{
    /// <summary>
    /// Logique d'interaction pour NewUserView.xaml
    /// </summary>
    public partial class NewUserView : Window
    {
        public NewUserView()
        {
            InitializeComponent();
            Closing += ServiceLocator.Current.GetInstance<NewUserViewModel>().onClose;
        }
    }
}
