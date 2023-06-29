using Preto13.Model;
using Preto13.Model.DTO;

namespace Preto13.Data.Member
{
    public interface iMemberAccount
    {
        Task<GenericResponse> REGISTER(USER_REGISTER_DTO regis);
        Task<GenericResponse> LOGIN(USER_LOGIN_DTO login);
    }
}
