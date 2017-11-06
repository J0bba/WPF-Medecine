using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virsol_tMedicalDotNet.Model;
using virsol_tMedicalDotNet.ServiceLive;
using virsol_tMedicalDotNet.ViewModel;

namespace virsol_tMedicalDotNet.Services
{
    public class ServiceLiveCallbackHandler : ServiceLive.IServiceLiveCallback
    {
        int j = 0;
        int k = 0;
        int SizeHeart = 100;
        int SizeTemp = 50;
        public MainViewModel ViewModel { get; set; }

        public ServiceLiveCallbackHandler(MainViewModel mainViewModel)
        {
            ViewModel = mainViewModel;
        }

        public void PushDataHeart(double requestData)
        {
            //System.Console.WriteLine("Heart : " + requestData.ToString());
            /*ViewModel.ChartDataHeart.Add(new Model.ChartData() { Name = DateTime.Now, Value = requestData });
            if (ViewModel.ChartDataHeart.Count > 50)
                ViewModel.ChartDataHeart.Remove(ViewModel.ChartDataHeart.First());*/
            //ViewModel.LiveHeart = requestData.ToString();
            if (ViewModel.SeriesCollectionHeart.First().Values.Count == 0)
            {
                var curr = DateTime.Now;
                for (int i = 0; i < SizeHeart; i++)
                {
                    ViewModel.ChartLabelsHeart.Add(curr.ToLongTimeString());
                    ViewModel.SeriesCollectionHeart.First().Values.Add(double.NaN);
                    curr = curr.AddSeconds(0.2);
                }
            }
            ViewModel.SeriesCollectionHeart.First().Values.RemoveAt(j);
            ViewModel.SeriesCollectionHeart.First().Values.Insert(j, requestData);
            j++;
            //ViewModel.ChartLabelsHeart.Add(DateTime.Now.ToString());
            if (j == SizeHeart)
            {
                j = 0;
                ViewModel.ChartLabelsHeart.Clear();
                ViewModel.SeriesCollectionHeart.First().Values.Clear();
            }
            /*foreach (var d in ViewModel.SeriesCollectionHeart.First().Values)
            {
                System.Console.Write(d + ", ");
            }
            System.Console.WriteLine(" ");*/
        }

        public void PushDataTemp(double requestData)
        {
            //System.Console.WriteLine("Temp : " + requestData.ToString());
            /*ViewModel.ChartDataTemp.Add(new Model.ChartData() { Name = DateTime.Now, Value = requestData });
            if (ViewModel.ChartDataTemp.Count > 50)
                ViewModel.ChartDataTemp.Remove(ViewModel.ChartDataTemp.First());*/
            /*ViewModel.SeriesCollectionTemp.First().Values.Add(requestData);
            ViewModel.ChartLabelsTemp.Add(DateTime.Now.ToString());
            if (ViewModel.ChartLabelsTemp.Count > 10)
            {
                ViewModel.ChartLabelsTemp.RemoveAt(0);
                ViewModel.SeriesCollectionTemp.First().Values.RemoveAt(0);
            }*/
            
            if (ViewModel.SeriesCollectionTemp.First().Values.Count == 0)
            {
                var curr = DateTime.Now;
                for (int i = 0; i < SizeTemp; i++)
                {
                    ViewModel.ChartLabelsTemp.Add(curr.ToLongTimeString());
                    ViewModel.SeriesCollectionTemp.First().Values.Add(double.NaN);
                    curr = curr.AddSeconds(1);
                }
            }
            ViewModel.SeriesCollectionTemp.First().Values.RemoveAt(k);
            ViewModel.SeriesCollectionTemp.First().Values.Insert(k, requestData);
            k++;
            //ViewModel.ChartLabelsHeart.Add(DateTime.Now.ToString());
            if (k == SizeTemp)
            {
                k = 0;
                ViewModel.ChartLabelsTemp.Clear();
                ViewModel.SeriesCollectionTemp.First().Values.Clear();
            }
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
