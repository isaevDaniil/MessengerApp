using MessengerApp.BLL.DTO;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Interfaces
{
    public interface IBotService
    {
        public Task RegisterBotAsync(string botName);

        public void HandleResponseFromBotAPI(ResponseFromBotDTO responseFromBotDto);
    }
}
