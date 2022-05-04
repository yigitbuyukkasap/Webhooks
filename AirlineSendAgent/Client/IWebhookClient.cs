using System.Threading.Tasks;
using AirlineSendAgent.Dtos;

namespace AirlineSendAgent.Clients 
{
    public interface IWebHookClient
    {
        Task SendWebHook(FlightDetailChangePayloadDto flightDetailChangePayloadDto);
    }
}