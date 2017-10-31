using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using virsol_tMedicalDotNet.Model;
using virsol_tMedicalDotNet.Services;
using virsol_tMedicalDotNet.View;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        #region Variables
        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get
            {
                return _selectedPatient;
            }
            set
            {
                _selectedPatient = value;
                if (value != null && _selectedPatient.observations.Count != 0)
                    SelectedObservation = _selectedPatient.observations.First();
                RaisePropertyChanged("SelectedPatient");
            }
        }
        private Observation _selectedObservation;
        public Observation SelectedObservation {
            get
            {
                return _selectedObservation;
            }
            set
            {
                _selectedObservation = value;
                RaisePropertyChanged("SelectedObservation");
            }
        }

        private ObservableCollection<Patient> _patientList;
        public ObservableCollection<Patient> PatientList
        {
            get
            {
                return _patientList;
            }
            set
            {
                _patientList = value;
                RaisePropertyChanged("PatientList");
            }
        }

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
        private BackgroundWorker _workerPatients = new BackgroundWorker();
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
        #endregion
        #region Commands
        public ICommand DeleteUserCommand { get; set; }
        public ICommand NewUserCommand { get; set; }
        public ICommand NewObsCommand { get; set; }
        public ICommand DeletePatientCommand { get; set; }
        public ICommand NewPatientCommand { get; set; }
        #endregion



        public MainViewModel()
        {

            DeleteUserCommand = new RelayCommand(DeleteUserMethod);
            NewUserCommand = new RelayCommand(NewUserMethod);
            NewObsCommand = new RelayCommand(NewObsMethod);
            DeletePatientCommand = new RelayCommand(DeletePatientMethod);
            NewPatientCommand = new RelayCommand(NewPatientMethod);
            _worker.DoWork += new DoWorkEventHandler((s, e) =>
            {
                ListUsers = Users.GetAllUsers();
                CurrUser = Users.GetUser(CurrUserLogin);
                setVisibility(CurrUserLogin);
            });
            _worker.RunWorkerAsync();
            _workerPatients.DoWork += new DoWorkEventHandler((s, e) =>
            {
                PatientList = Patients.GetAllPatients();
            });
            _workerPatients.RunWorkerAsync();
        }

        public void UpdatePatientList()
        {
            System.Console.WriteLine("Ca passe par la pourtant !");
            _workerPatients.RunWorkerAsync();
        }

        #region Methods
        private void NewPatientMethod()
        {
            var app = new NewPatientView();
            var context = new NewPatientViewModel();
            context.lastWindow = this;
            app.DataContext = context;
            app.Show();
        }

        private void NewObsMethod()
        {
            var app = new NewObsView();
            var context = new NewObsViewModel();
            
            context.lastWindow = this;
            System.Console.WriteLine("this : " + this, "context.lastwindow : " + context.lastWindow);
            System.Console.WriteLine("context.lastwindow : " + context.lastWindow);
            app.DataContext = context;
            app.Show();
        }

        private void DeletePatientMethod()
        {
            int id = SelectedPatient.id;
            if (Patients.DeletePatient(id))
            {
                MessageBox.Show("Le patient " + SelectedPatient.prettyname + " a été supprimé avec succès !");
                PatientList.Remove(PatientList.Where(i => i.id == id).Single());
                SelectedPatient = PatientList.Count > 0 ? PatientList.First() : null;

            }
            else
            {
                MessageBox.Show("Le patient " + SelectedPatient.prettyname + " n'a pas put être supprimé Réessayez plus tard !");
            }
        }

        private void DeleteUserMethod()
        {
            string login = SelectedUser.login;
            if (Users.DeleteUser(login))
            {
                MessageBox.Show("L'utilisateur " + login + " a été supprimé avec succès !");
                ListUsers.Remove(ListUsers.Where(i => i.login == login).Single());
                SelectedUser = ListUsers.First();

            }
            else
            {
                MessageBox.Show("L'utilisateur " + login + " n'a pas put être supprimé Réessayez plus tard !");
            }
        }

        private void NewUserMethod()
        {
            OpenNewUserWindow();
        }
        private void OpenNewUserWindow()
        {
            var app = new NewUserView();
            var context = new NewUserViewModel();
            context.lastWindow = this;
            app.DataContext = context;
            app.Show();
        }
        #endregion
    }
}
