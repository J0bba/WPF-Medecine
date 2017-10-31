using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virsol_tMedicalDotNet.Model;
using virsol_tMedicalDotNet.ServicePatient;
using virsol_tMedicalDotNet.ServiceUser;

namespace virsol_tMedicalDotNet.Services
{
    class Patients
    {
        public static bool CreatePatient(Model.Patient patient)
        {
            ServicePatientClient servicePatient = new ServicePatientClient();
            bool result = false;
            try
            {
                ServicePatient.Patient patientS = new ServicePatient.Patient()
                {
                    Birthday = patient.birthday,
                    Firstname = patient.firstname,
                    Name = patient.name,
                    Observations = null
                };
                result = servicePatient.AddPatient(patientS);
            }
            catch (Exception) { }
            finally { servicePatient.Close(); }
            return result;
        }

        public static bool DeletePatient(int id)
        {
            ServicePatientClient servicePatient = new ServicePatientClient();
            bool result = false;
            try
            {
                result = servicePatient.DeletePatient(id);
            }
            catch (Exception) { }
            finally { servicePatient.Close(); }
            return result;
        }

        public static ObservableCollection<Model.Patient> GetAllPatients()
        {
            ServicePatientClient servicePatient = new ServicePatientClient();
            ObservableCollection<Model.Patient> result = new ObservableCollection<Model.Patient>();
            try
            {
                var response = servicePatient.GetListPatient();
                foreach (var patient in response)
                {
                    ObservableCollection<Model.Observation> observations = new ObservableCollection<Model.Observation>();
                    Model.Patient newP = new Model.Patient()
                    {
                        birthday = patient.Birthday,
                        firstname = patient.Firstname,
                        id = patient.Id,
                        name = patient.Name
                    };
                    if (patient.Observations != null)
                    {
                        foreach (var observation in patient.Observations)
                        {
                            Model.Observation newOb = new Model.Observation()
                            {
                                bloodPressure = observation.BloodPressure,
                                comment = observation.Comment,
                                date = observation.Date,
                                pictures = observation.Pictures,
                                prescription = observation.Prescription,
                                weight = observation.Weight
                            };
                            observations.Add(newOb);
                        }
                    }
                    newP.observations = observations;
                    result.Add(newP);
                }
            }
            catch (Exception e) { System.Console.WriteLine(e.Message); }
            finally { servicePatient.Close(); }
            return result;
        }
    }
}
