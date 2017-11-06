using BespokeFusion;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using virsol_tMedicalDotNet.Model;
using virsol_tMedicalDotNet.ServiceLive;
using virsol_tMedicalDotNet.Services;
using virsol_tMedicalDotNet.View;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            initCharts();

            DeleteUserCommand = new RelayCommand(DeleteUserMethod);
            NewUserCommand = new RelayCommand(NewUserMethod);
            NewObsCommand = new RelayCommand(NewObsMethod);
            DeletePatientCommand = new RelayCommand(DeletePatientMethod);
            NewPatientCommand = new RelayCommand(NewPatientMethod);
            DisconnectCommand = new RelayCommand(DisconnectMethod);
            RefreshPatientList = new RelayCommand(RefreshPatientListMethod);
            RefreshUserList = new RelayCommand(RefreshUserListMethod);
            _worker.DoWork += new DoWorkEventHandler((s, e) =>
            {
                ListUsers = Users.GetAllUsers();
                CurrUser = Users.GetUser(CurrUserLogin);
                SelectedUser = ListUsers.First();
                FullListUser = new ObservableCollection<User>(ListUsers);
                setVisibility(CurrUserLogin);
            });
            _worker.RunWorkerAsync();
            _workerPatients.RunWorkerCompleted += new RunWorkerCompletedEventHandler((e, s) =>
            {
                SelectedPatient = PatientList.Count > 0 ? PatientList.Last() : null;
            }
            );
            _workerPatients.DoWork += new DoWorkEventHandler((s, e) =>
            {
                PatientList = Patients.GetAllPatients();
                FullListPatient = new ObservableCollection<Patient>(PatientList);
            });
            _workerPatients.RunWorkerAsync();
            _searchUserWorker.DoWork += new DoWorkEventHandler((e, s) =>
            {
                DoWorkSearchUser(SearchBoxTextUser);
            });
            _searchPatientWorker.DoWork += new DoWorkEventHandler((e, s) =>
            {
                DoWorkSearchPatient(SearchBoxTextPatient);
            });
        }

        #region Variables
        private ObservableCollection<User> FullListUser;
        private ObservableCollection<Patient> FullListPatient;
        private BackgroundWorker _searchUserWorker = new BackgroundWorker();
        private BackgroundWorker _searchPatientWorker = new BackgroundWorker();
        private string _searchBoxTextUser;
        public string SearchBoxTextUser
        {
            get
            {
                return _searchBoxTextUser;
            }
            set
            {
                _searchBoxTextUser = value;
                RaisePropertyChanged("SearchBoxTextUser");
                _searchUserWorker.RunWorkerAsync();
            }
        }
        private string _searchBoxTextPatient;
        public string SearchBoxTextPatient
        {
            get
            {
                return _searchBoxTextPatient;
            }
            set
            {
                _searchBoxTextPatient = value;
                RaisePropertyChanged("SearchBoxTextPatient");
                _searchPatientWorker.RunWorkerAsync();
            }
        }
        #region Chart
        public Func<double, string> YFormater { get; set; }
        public SeriesCollection SeriesCollectionTemp { get; set; }
        public LineSeries lineSeriesTemp { get; set; }
        private ObservableCollection<string> _chartLabelsTemp = new ObservableCollection<string>();
        public ObservableCollection<string> ChartLabelsTemp
        {
            get
            {
                return _chartLabelsTemp;
            }
            set
            {
                _chartLabelsTemp = value;
                RaisePropertyChanged("ChartLabelsTemp");
            }
        }
        public SeriesCollection SeriesCollectionHeart { get; set; }
        public LineSeries lineSeriesHeart { get; set; }
        private ObservableCollection<string> _chartLabelsHeart = new ObservableCollection<string>();
        public ObservableCollection<string> ChartLabelsHeart
        {
            get
            {
                return _chartLabelsHeart;
            }
            set
            {
                _chartLabelsHeart = value;
                RaisePropertyChanged("ChartLabelsHeart");
            }
        }
        /*private ObservableCollection<ChartData> _chartDataHeart = new ObservableCollection<Model.ChartData>();
        public ObservableCollection<ChartData> ChartDataHeart
        {
            get
            {
                return _chartDataHeart;
            }
            set
            {
                _chartDataHeart = value;
                RaisePropertyChanged("ChartDataHeart");
            }
        }
        private ObservableCollection<ChartData> _chartDataTemp = new ObservableCollection<Model.ChartData>();
        public ObservableCollection<ChartData> ChartDataTemp
        {
            get
            {
                return _chartDataTemp;
            }
            set
            {
                _chartDataTemp = value;
                RaisePropertyChanged("ChartDataTemp");
            }
        }*/
        #endregion Chart

        public ServiceLiveClient ServiceLive { get; set; }

        private TabItem _selectedTab;

        public TabItem SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                _selectedTab = value;
                if (value.Header.Equals("Live")) {
                    // Start Live Thing
                    ServiceLive = Live.Subscribe(this);
                }
                else
                {
                    //Cancel live thing here
                    if (ServiceLive != null)
                    {
                        try
                        {
                            ServiceLive.Close();
                            clearCharts();
                            //ChartDataHeart.Clear();
                            //ChartDataTemp.Clear();
                        }
                        catch (Exception) { }
                        ServiceLive = null;
                    }
                }


            }
        }
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
        public ICommand RefreshUserList { get; set; }
        public ICommand RefreshPatientList { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand NewUserCommand { get; set; }
        public ICommand NewObsCommand { get; set; }
        public ICommand DeletePatientCommand { get; set; }
        public ICommand NewPatientCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }
        #endregion


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
                var msg = Utils.Utils.createMessageBox("Le patient " + SelectedPatient.prettyname + " a été supprimé avec succès !");
                msg.Show();
                //MessageBox.Show("Le patient " + SelectedPatient.prettyname + " a été supprimé avec succès !");
                PatientList.Remove(PatientList.Where(i => i.id == id).Single());
                SelectedPatient = PatientList.Count > 0 ? PatientList.First() : null;

            }
            else
            {

                MaterialMessageBox.ShowError("Le patient " + SelectedPatient.prettyname + " n'a pas put être supprimé Réessayez plus tard !");
                //MessageBox.Show("Le patient " + SelectedPatient.prettyname + " n'a pas put être supprimé Réessayez plus tard !");
            }
        }

        private void DeleteUserMethod()
        {
            string login = SelectedUser.login;
            if (Users.DeleteUser(login))
            {
                var msg = Utils.Utils.createMessageBox("L'utilisateur " + login + " a été supprimé avec succès !");
                msg.Show();
                ListUsers.Remove(ListUsers.Where(i => i.login == login).Single());
                SelectedUser = ListUsers.First();

            }
            else
            {
                MaterialMessageBox.ShowError("L'utilisateur " + login + " n'a pas put être supprimé Réessayez plus tard !");
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
            ListUsers.Clear();
            PatientList.Clear();
        }

        private void initCharts()
        {
            lineSeriesTemp = new LineSeries()
            {
                Title = "Température",
                Stroke = Brushes.RoyalBlue,
                Fill = Brushes.Transparent,
                LineSmoothness = 0,
                Values = new ChartValues<double> { }
                
            };
            SeriesCollectionTemp = new SeriesCollection()
            {
                lineSeriesTemp
            };

            lineSeriesHeart = new LineSeries()
            {
                Title = "Coeur",
                LineSmoothness = 0,
                Stroke = Brushes.OrangeRed,
                Fill = Brushes.Transparent,
                Values = new ChartValues<double> { }
            };
            SeriesCollectionHeart = new SeriesCollection()
            {
                lineSeriesHeart
            };
        }
        private void clearCharts()
        {
            ChartLabelsHeart.Clear();
            ChartLabelsTemp.Clear();
            SeriesCollectionHeart.First().Values.Clear();
            SeriesCollectionTemp.First().Values.Clear();
        }

        private void DoWorkSearchUser(string text)
        {
            if (text.Equals(""))
            {
                ListUsers = FullListUser;
                return;
            }
            ObservableCollection<Model.User> users = new ObservableCollection<User>();
            foreach (var user in FullListUser)
            {
                if (user.prettyname.ToLower().Contains(text.ToLower()))
                    users.Add(user);
            }
            if (users.Count > 0)
                SelectedUser = users.First();
            ListUsers = users;
        }
        private void DoWorkSearchPatient(string text)
        {
            if (text.Equals(""))
            {
                PatientList = FullListPatient;
                return;
            }
            ObservableCollection<Patient> patients = new ObservableCollection<Patient>();
            foreach (var patient in FullListPatient)
            {
                if (patient.prettyname.ToLower().Contains(text.ToLower()))
                    patients.Add(patient);
            }
            if (patients.Count > 0)
                SelectedPatient = patients.First();
            PatientList = patients;
        }
        private void RefreshUserListMethod()
        {
            _worker.RunWorkerAsync();
        }

        private void RefreshPatientListMethod()
        {
            _workerPatients.RunWorkerAsync();
        }

        #endregion
    }
}
