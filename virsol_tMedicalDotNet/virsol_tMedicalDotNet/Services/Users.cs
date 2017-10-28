using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virsol_tMedicalDotNet.Model;
using virsol_tMedicalDotNet.ServiceUser;

namespace virsol_tMedicalDotNet.Services
{
    class Users
    {
        public static ObservableCollection<Model.User> GetAllUsers()
        {
            ObservableCollection<Model.User> result = new ObservableCollection<Model.User>();
            ServiceUserClient serviceUser = new ServiceUserClient();
            var users = serviceUser.GetListUser();
            foreach (var user in users)
            {
                Model.User Muser = new Model.User()
                {
                    login = user.Login,
                    pwd = user.Pwd,
                    name = user.Name,
                    firstname = user.Firstname,
                    picture = user.Picture,
                    connected = user.Connected,
                    role = user.Role
                };
                result.Add(Muser);
            }
            serviceUser.Close();
            return result;
        }
    }
}
