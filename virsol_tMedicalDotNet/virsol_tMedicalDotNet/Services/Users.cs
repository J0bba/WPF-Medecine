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
        public static bool CreateUser(Model.User user)
        {
            ServiceUserClient serviceUser = new ServiceUserClient();
            bool result = false;
            try
            {
                result = serviceUser.AddUser(new ServiceUser.User()
                {
                    Login = user.login,
                    Pwd = user.pwd,
                    Name = user.name,
                    Firstname = user.firstname,
                    Picture = user.picture,
                    Role = user.role
                });
            }
            catch (Exception) { }
            finally { serviceUser.Close(); }
            return result;
        }

        public static string GetRole(string login)
        {
            ServiceUserClient serviceUser = new ServiceUserClient();
            string role = "";
            try
            {
                role = serviceUser.GetRole(login);
            }
            catch (Exception) { }
            finally { serviceUser.Close(); }
            return role;
        }

        public static Model.User GetUser(string login)
        {
            Model.User user = new Model.User();
            ServiceUserClient serviceUser = new ServiceUserClient();
            try
            {
                var userS = serviceUser.GetUser(login);
                user.login = userS.Login;
                user.pwd = userS.Pwd;
                user.role = userS.Role;
                user.name = userS.Name;
                user.picture = userS.Picture;
                user.firstname = userS.Firstname;
                return user;

            }
            catch (Exception) { }
            finally { serviceUser.Close(); }
            return user;

        }

        public static ObservableCollection<Model.User> GetAllUsers()
        {
            ObservableCollection<Model.User> result = new ObservableCollection<Model.User>();
            ServiceUserClient serviceUser = new ServiceUserClient();
            try
            {
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
            }
            catch (Exception) { }
            finally { serviceUser.Close(); }
            return result;
        }

        public static bool DeleteUser(string login)
        {
            ServiceUserClient serviceUser = new ServiceUserClient();
            bool response = false;
            try
            {
                response = serviceUser.DeleteUser(login);
            }
            catch (Exception) { }
            finally
            {
                serviceUser.Close();
            }
            return response;
        }
    }
}
