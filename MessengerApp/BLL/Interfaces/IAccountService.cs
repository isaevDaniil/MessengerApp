using MessengerApp.BLL.DTO;
using System.Threading.Tasks;

namespace MessengerApp.BLL.Interfaces
{
    public interface IAccountService
    {
        public Task<AuthorizationResponseDTO> SignUpAsync(UserLogInDTO userLogInDto);

        public Task<AuthorizationResponseDTO> SignInAsync(UserLogInDTO userLogInDto);
    }
}
