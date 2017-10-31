using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using virsol_tMedicalDotNet.Services;
using virsol_tMedicalDotNet.ServiceUser;
using virsol_tMedicalDotNet.View;

namespace virsol_tMedicalDotNet.ViewModel
{
    public class NewUserViewModel : ViewModelBase
    {
        public MainViewModel lastWindow { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Role { get; set; }
        private Byte[] _picture;
        public Byte[] Picture {
            get
            {
                return _picture;
            }
            set
            {
                _picture = value;
                RaisePropertyChanged("Picture");
            }
        }
        
        public ICommand CreateUserCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public NewUserViewModel()
        {
            CreateUserCommand = new RelayCommand<PasswordBox>(CreateUserMethod);
            AddImageCommand = new RelayCommand(AddImageMethod);
            CancelCommand = new RelayCommand(CancelMethod);
        }

        private void CreateUserMethod(PasswordBox passwordBox)
        {
            Model.User newUser = new Model.User()
            {
                login = Login,
                pwd = passwordBox.Password,
                name = Name,
                firstname = Firstname,
                role = Role,
                picture = Picture
            };
            if (Users.CreateUser(newUser))
            {
                lastWindow.ListUsers.Add(newUser);
                lastWindow.SelectedUser = newUser;
                CloseCurrentWindow();
                MessageBox.Show("Utilisateur créé avec succès !");
            }
            else
            {
                MessageBox.Show("Impossible de créer un nouvel utilisateur !");
            }
        }

        private void AddImageMethod()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Choisir une image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                MemoryStream memStream = new MemoryStream();
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(op.FileName))));
                encoder.Save(memStream);
                Picture = memStream.ToArray();
            }
        }

        private void CancelMethod()
        {
            CloseCurrentWindow();
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
            Login = "";
            Name = "";
            Firstname = "";
            Role = "";
            Picture = null;
        }
    }
}
