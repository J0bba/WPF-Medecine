using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virsol_tMedicalDotNet.ServiceLive;
using virsol_tMedicalDotNet.ViewModel;

namespace virsol_tMedicalDotNet.Services
{
    public class ServiceLiveCallbackHandler : ServiceLive.IServiceLiveCallback
    {
        public MainViewModel ViewModel { get; set; }

        public ServiceLiveCallbackHandler(MainViewModel mainViewModel)
        {
            ViewModel = mainViewModel;
        }

        public void PushDataHeart(double requestData)
        {
            //System.Console.WriteLine("Heart : " + requestData.ToString());
            ViewModel.ChartDataHeart.Add(new Model.ChartData() { Name = DateTime.Now, Value = requestData });
            if (ViewModel.ChartDataHeart.Count > 50)
                ViewModel.ChartDataHeart.Remove(ViewModel.ChartDataHeart.First());
            //ViewModel.LiveHeart = requestData.ToString();
        }

        public void PushDataTemp(double requestData)
        {
            //System.Console.WriteLine("Temp : " + requestData.ToString());
            ViewModel.ChartDataTemp.Add(new Model.ChartData() { Name = DateTime.Now, Value = requestData });
            if (ViewModel.ChartDataTemp.Count > 50)
                ViewModel.ChartDataTemp.Remove(ViewModel.ChartDataTemp.First());
            //ViewModel.LiveTemp = requestData.ToString();
        }
    }

    class Live
    {
        public static ServiceLiveClient Subscribe(MainViewModel mainViewModel)
        {
            ServiceLiveClient serviceLive = null;
            try
            {
                System.ServiceModel.InstanceContext context = new System.ServiceModel.InstanceContext(new ServiceLiveCallbackHandler(mainViewModel));
                serviceLive = new ServiceLiveClient(context);
                serviceLive.Subscribe();
            }
            catch (Exception) { }
            return serviceLive;
        }


    }
}
