using System.ServiceModel;

namespace Wcf_Medical
{
    public interface IDuplexCallback
    {
        [OperationContract(IsOneWay = true)]
        void PushDataHeart(double requestData);

        [OperationContract(IsOneWay = true)]
        void PushDataTemp(double requestData);
    }
}