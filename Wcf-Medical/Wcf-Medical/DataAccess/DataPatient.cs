﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;

namespace Wcf_Medical.DataAccess
{
    public class DataPatient : BaseData
    {
        [ImportMany]
        public Lazy<Interfaces.IPatient, IDictionary<string, object>>[] LoadedDatas { get; set; }
        
        public DataPatient()
        {            
            Compose();                       
        }

        public List<Dbo.Patient> CreateListPatient()
        {
            if (LoadedDatas != null)
            {
                foreach (var item in LoadedDatas)
                {
                    if (item.Metadata["type"].ToString() == ConfigurationManager.AppSettings["type"])
                    {
                        return item.Value.CreateListPatient();
                    }
                }
            }
            return new List<Dbo.Patient>();
        }
    }
}