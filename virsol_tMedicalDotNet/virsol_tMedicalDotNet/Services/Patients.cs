using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    newP.observations = observations;
                    result.Add(newP);
                }
            }
            catch (Exception) { }
            finally { servicePatient.Close(); }
            return result;
        }
    }
}
