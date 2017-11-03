using BespokeFusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace virsol_tMedicalDotNet.Utils
{
    class Utils
    {
        public static CustomMaterialMessageBox createMessageBox(string msg)
        {
            return new CustomMaterialMessageBox
            {
                TxtMessage = { Text = msg },
                TxtTitle = { Text = "Succés", Foreground = Brushes.White },
                BtnOk = { Content = "Ok" , Background = Brushes.BlueViolet, BorderBrush = Brushes.Transparent},
                BtnCancel = { Visibility = Visibility.Collapsed},
                TitleBackgroundPanel = { Background = Brushes.BlueViolet },
                BorderBrush = Brushes.Transparent
            };
        }
    }
}
