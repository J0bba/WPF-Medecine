﻿using BespokeFusion;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using virsol_tMedicalDotNet.Services;
using virsol_tMedicalDotNet.View;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class NewPatientViewModel : ViewModelBase
    {
        #region Variables
        public MainViewModel lastWindow { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public DateTime Birthdate {
            get
            {
                return _birthDate;
            }
            set
            {
                _birthDate = value;
                RaisePropertyChanged("BirthDate");
            }
        }
        private DateTime _birthDate = DateTime.Now;
        public ICommand CreatePatientCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion
        public NewPatientViewModel()
        {
            CreatePatientCommand = new RelayCommand(CreatePatientMethod);
            CancelCommand = new RelayCommand(CloseCurrentWindow);
        }
        #region Methods
        private void CreatePatientMethod()
        {
            if (Firstname == null || Firstname.Equals("") || Name == null ||Name.Equals(""))
            {
                MaterialMessageBox.ShowError("Le patient doit avoir un nom/prénom !");
                return;
            }
            Model.Patient patient = new Model.Patient()
            {
                birthday = Birthdate,
                firstname = Firstname,
                name = Name
            };

            if (Patients.CreatePatient(patient))
            {
                lastWindow.UpdatePatientList();
                CloseCurrentWindow();
                var msg = Utils.Utils.createMessageBox("Patient crée avec succés");
                msg.Show();

            }
            else
            {
                MaterialMessageBox.ShowError("Impossible de créer un nouveau patient !");
            }

        }

        private void CloseCurrentWindow()
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
            lastWindow = null;
            Name = "";
            Firstname = "";
            Birthdate = DateTime.Now;
        }
        #endregion Methods
    }
}
