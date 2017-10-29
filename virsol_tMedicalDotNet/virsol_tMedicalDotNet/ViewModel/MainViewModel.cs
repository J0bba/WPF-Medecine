using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using virsol_tMedicalDotNet.Model;
using virsol_tMedicalDotNet.Services;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Visibility _isNotInfirmiere;
        public Visibility isNotInfirmiere {
            get
            {
                return _isNotInfirmiere;
            }
            set
            {
                _isNotInfirmiere = value;
                RaisePropertyChanged("isNotInfirmiere");
            }
        }
        private BackgroundWorker _worker = new BackgroundWorker();
        public string CurrUserLogin { get; set; }
        public User CurrUser { get; set; }

        private void setVisibility(string login)
        {
            if (Users.GetRole(login).Equals("Infirmière"))
            {
                isNotInfirmiere = Visibility.Hidden;
            }
            else
            {
                isNotInfirmiere = Visibility.Visible;
            }
        }

        private ObservableCollection<User> _listUsers;
        public ObservableCollection<User> ListUsers {
            get
            {
                return _listUsers;
            }
            set
            {
                _listUsers = value;
                RaisePropertyChanged("ListUsers");
            }
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                RaisePropertyChanged("SelectedUser");
            }
        }



        public MainViewModel()
        {
            _worker.DoWork += new DoWorkEventHandler((s, e) =>
            {
                ListUsers = Users.GetAllUsers();
                setVisibility(CurrUserLogin);

            });
            _worker.RunWorkerAsync();
        }
    }
}
