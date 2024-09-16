using static Video_Chat.DTO.ServiceResponses;
using Video_Chat.DTO;

namespace Video_Chat.Interfaces
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
    }
}
