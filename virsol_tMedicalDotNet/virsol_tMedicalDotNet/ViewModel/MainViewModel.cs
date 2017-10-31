using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
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
        private Visibility _isObservationVisible = Visibility.Visible;
        public Visibility IsObservationVisible
        {
            get
            {
                return _isObservationVisible;
            }
            set
            {
                _isObservationVisible = value;
                RaisePropertyChanged("IsObservationVisible");
            }
        }

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
                {
                    SelectedObservation = _selectedPatient.observations.First();
                    changeObsVisibility(Visibility.Visible);
                }
                else if (value != null && _selectedPatient.observations.Count == 0)
                    changeObsVisibility(Visibility.Hidden);
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
                if (value == null)
                    changeObsVisibility(Visibility.Hidden);
                else
                    changeObsVisibility(Visibility.Visible);
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
        public ICommand DisconnectCommand { get; set; }
        #endregion



        public MainViewModel()
        {

            DeleteUserCommand = new RelayCommand(DeleteUserMethod);
            NewUserCommand = new RelayCommand(NewUserMethod);
            NewObsCommand = new RelayCommand(NewObsMethod);
            DeletePatientCommand = new RelayCommand(DeletePatientMethod);
            NewPatientCommand = new RelayCommand(NewPatientMethod);
            DisconnectCommand = new RelayCommand(DisconnectMethod);
            _worker.DoWork += new DoWorkEventHandler((s, e) =>
            {
                ListUsers = Users.GetAllUsers();
                CurrUser = Users.GetUser(CurrUserLogin);
                setVisibility(CurrUserLogin);
            });
            _worker.RunWorkerAsync();
            _workerPatients.RunWorkerCompleted += new RunWorkerCompletedEventHandler((e, s) =>
            {
                SelectedPatient = PatientList.Count > 0 ?  PatientList.Last() : null;
            }
            );
            _workerPatients.DoWork += new DoWorkEventHandler((s, e) =>
            {
                PatientList = Patients.GetAllPatients();
            });
            _workerPatients.RunWorkerAsync();
        }

        public void UpdatePatientList()
        {
            _workerPatients.RunWorkerAsync();
        }

        #region Methods
        private void DisconnectMethod()
        {
            Users.DisconnectUser(CurrUserLogin);
            OpenLoginWindow();

        }

        private void OpenLoginWindow()
        {
            var current = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var app = new LoginView();
            var context = ServiceLocator.Current.GetInstance<LoginViewModel>();
            app.DataContext = context;
            app.Show();
            clearAllFields();
            current.Close();
        }

        private void NewPatientMethod()
        {
            var app = new NewPatientView();
            var context = ServiceLocator.Current.GetInstance<NewPatientViewModel>();
            context.lastWindow = this;
            app.DataContext = context;
            app.Show();
        }

        private void NewObsMethod()
        {
            var app = new NewObsView();
            var context = ServiceLocator.Current.GetInstance<NewObsViewModel>();
            context.lastWindow = this;
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

        private void changeObsVisibility(Visibility visibility)
        {
            IsObservationVisible = visibility;
        }

        private void NewUserMethod()
        {
            OpenNewUserWindow();
        }
        private void OpenNewUserWindow()
        {
            var app = new NewUserView();
            var context = ServiceLocator.Current.GetInstance<NewUserViewModel>();
            context.lastWindow = this;
            app.DataContext = context;
            app.Show();
        }
        private void CloseCurrentWindow()
        {
            var current = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            clearAllFields();
            current.Close();
        }
        private void clearAllFields()
        {

        }
        #endregion
    }
}
