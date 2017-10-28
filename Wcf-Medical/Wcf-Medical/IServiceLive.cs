using System.ServiceModel;

namespace Wcf_Medical
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceLive" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDuplexCallback))]
    public interface IServiceLive
    {
        [OperationContract(IsOneWay=true)]
        void Subscribe();
    }
}
