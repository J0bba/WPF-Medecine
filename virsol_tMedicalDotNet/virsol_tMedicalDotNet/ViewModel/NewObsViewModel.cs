using BespokeFusion;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using virsol_tMedicalDotNet.Services;
using virsol_tMedicalDotNet.View;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class NewObsViewModel : ViewModelBase
    {
        private int _bloodPressure;
        public string BloodPressure
        {
            get
            {
                return _bloodPressure.ToString();
            }
            set
            {
                _bloodPressure = Int32.Parse(value);
                RaisePropertyChanged("BloodPressure");
            }
        }

        private int _weight;
        public string Weight
        {
            get
            {
                return _weight.ToString();
            }
            set
            {
                _weight = Int32.Parse(value);
                RaisePropertyChanged("Weight");
            }
        }
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                RaisePropertyChanged("Date");
            }
        }
        public string Comment { get; set; }
        public string Prescriptions { get; set; }
        private ObservableCollection<Byte[]> _pictures = new ObservableCollection<byte[]>();
        public ObservableCollection<Byte[]> Pictures
        {
            get
            {
                return _pictures;
            }
            set
            {
                _pictures = value;
                RaisePropertyChanged("Pictures");
            }
        }
        public MainViewModel lastWindow { get; set; }
        public ICommand AddImagesCommand { get; set; }
        public ICommand DeletePictureCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CreateObsCommand { get; set; }

        public string PatientNameLabel
        {
            get
            {
                return lastWindow == null ? "Patient : " : "Patient : " + lastWindow.SelectedPatient.prettyname;
            }
        }
        public NewObsViewModel()
        {
            DeletePictureCommand = new RelayCommand<Byte[]>(DeletePictureMethod);
            AddImagesCommand = new RelayCommand(AddImagesMethod);
            CancelCommand = new RelayCommand(CloseWindow);
            CreateObsCommand = new RelayCommand(CreateObsMethod);
        }

        private void DeletePictureMethod(Byte[] img)
        {
            Pictures.RemoveAt(Pictures.IndexOf(img));
        }

        private void CreateObsMethod()
        {
            if (Prescriptions == null || Prescriptions.Equals("") || BloodPressure.Equals("0") || Weight.Equals("0"))
            {
                MaterialMessageBox.ShowError("Remplir les champs obligatoire de l'observation (poids, pression sanguine, prescription");
                return;
            }
            Model.Observation obs = new Model.Observation()
            {
                bloodPressure = _bloodPressure,
                comment = Comment,
                date = Date,
                pictures = Pictures.ToArray(),
                prescription = Prescriptions.Split('\n'),
                weight = _weight
            };
            int patientId = lastWindow.SelectedPatient.id;
            Observations.CreateObservation(patientId, obs);
            lastWindow.SelectedPatient.observations.Add(obs);
            lastWindow.SelectedObservation = obs;
            CloseWindow();
        }

        private void AddImagesMethod()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = true;
            op.Title = "Choisir une image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                foreach (var file in op.FileNames)
                {
                    MemoryStream memStream = new MemoryStream();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(file))));
                    encoder.Save(memStream);
                    Pictures.Add(memStream.ToArray());
                }
            }
        }
        private void CloseWindow()
        {
            var current = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            clearAllFields();
            current.Close();
        }
        public void onClose(object sender, CancelEventArgs e)
        {
            clearAllFields();
        }
        private void clearAllFields()
        {
            _bloodPressure = 0;
            _weight = 0;
            Date = DateTime.Now;
            Comment = "";
            Prescriptions = "";
            Pictures.Clear();
        }

    }
}
