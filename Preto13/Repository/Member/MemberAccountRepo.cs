using Newtonsoft.Json.Linq;
using Preto13.Data.Member;
using Preto13.Model;
using Preto13.Model.DTO;
using Preto13.Utils;

namespace Preto13.Repository.Member
{
    public class MemberAccountRepo : iMemberAccount
    {
        private readonly HandleData _handleData;

        public MemberAccountRepo(HandleData handle)
        {
            _handleData = handle;
        }

        public Task<GenericResponse> LOGIN(USER_LOGIN_DTO login)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResponse> REGISTER(USER_REGISTER_DTO regis)
        {
            string query = "UP_MB_REGISTER";
            GenericResponse result = new GenericResponse();
            DataHelper dataHelper = new DataHelper();
            var param = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(regis.password))
            {
                param.Add("@USERNAME", regis.username);
                param.Add("@EMAIL", regis.email);
                param.Add("@PHONE", regis.phone);

                //Handle pwd
                dataHelper.encryptPassword(regis.password, out byte[] pwdHash, out byte[] pwdSalt);
                string hash = Convert.ToBase64String(pwdHash);
                string salt = Convert.ToBase64String(pwdSalt);
                param.Add("@PWD_HASH", hash);
                param.Add("@PWD_SALT", salt);
                try
                {
                    var data = await _handleData.handleSp(query, param);
                    result.status = true;
                    JObject obj = JObject.Parse(data[0].ToString());
                    if (data.Count > 0)
                    {
                        if (obj.ContainsKey("CODE"))
                        {
                            result.code = obj.GetValue("CODE").ToString();
                            if (obj.GetValue("CODE").ToString().Equals("0"))
                            {
                                result.message = "CREATE ACCOUNT COMPLETE";

                            }
                            else
                            {
                                result.message = "SOMETHING WENT WRONG! PLEASE TRY AGAIN";
                            }
                        }
                    }
                    else
                    {
                        result.code = "-10001";
                        result.message = "MISSING CODE";
                    }
                }
                catch (Exception ex)
                {
                    result.code = "0";
                    result.message = ex.Message;
                }
            }
            return result;
        }
    }
}
