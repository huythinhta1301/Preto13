using Preto13.Model;
using Preto13.Model.DTO;

namespace Preto13.Data.Member
{
    public interface iMemberAccount
    {
        Task<GenericResponse> REGISTER(USER_REGISTER_DTO regis);
        Task<LoginResponse> LOGIN(USER_LOGIN_DTO login);
        Task<GenericResponse> GET_USER_INFO_BY_ID(string memberID);
        Task<Boolean> CHECK_USER_EXIST(string memberID);
    }
}
