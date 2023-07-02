using Newtonsoft.Json.Linq;
using Preto13.Model;
using Preto13.Model.DTO;

namespace Preto13.Data.Member
{
    public interface iMemberAccount
    {
        Task<GenericResponse> REGISTER(USER_REGISTER_DTO regis);
        Task<LoginResponse> LOGIN(USER_LOGIN_DTO login);
        Task<GenericResponse> GET_USER_INFO_BY_ID(string memberID);
        Task<JObject> CHECK_USER_EXIST_LOGIN(string memberID);
    }
}
