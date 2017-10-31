using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using virsol_tMedicalDotNet.ServiceObservation;

namespace virsol_tMedicalDotNet.Services
{
    class Observations
    {
        public static bool CreateObservation(int idPatient, Model.Observation observation)
        {
            ServiceObservationClient serviceObservation = new ServiceObservationClient();
            bool result = false;
            try
            {
                ServiceObservation.Observation obs = new Observation()
                {
                    BloodPressure = observation.bloodPressure,
                    Comment = observation.comment,
                    Date = observation.date,
                    Pictures = observation.pictures,
                    Prescription = observation.prescription,
                    Weight = observation.weight
                };
                System.Console.WriteLine("on se met mal avant");
                result = serviceObservation.AddObservation(idPatient, obs);
                System.Console.WriteLine("on se met mal après");
            }
            catch (Exception e) { Console.WriteLine("Exception : " + e.Message); }
            finally { serviceObservation.Close(); }
            return result;
            
        }
    }
}