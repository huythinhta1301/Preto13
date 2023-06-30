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

        public async Task<bool> CHECK_USER_EXIST(string INPUT)
        {
            string query = "UP_MB_CHECK_EXIST";
            DataHelper helper = new DataHelper();
            var param = new Dictionary<string, object>();
            int loginType = 1; //default username
            if (helper.IsMatch(INPUT, RegexPattern.EmailPattern))
            {
                loginType = 2; //email
            }
            if (helper.IsMatch(INPUT, RegexPattern.PhonePattern))
            {
                loginType = 3; //phone
            }
            param.Add("@LoginValue", INPUT);
            var data = await _handleData.handleSp(query, param);
            JObject obj = JObject.Parse(data[0].ToString());
            if (obj.ContainsKey("CODE"))
            {
                if (obj.GetValue("CODE").ToString().Equals("0"))
                {
                   return true;
                }
            }
            return false;
        }

        public async Task<GenericResponse> GET_USER_INFO_BY_ID(string memberID)
        {
            string query = "UP_MB_LOGIN";
            GenericResponse result = new GenericResponse();
            DataHelper helper = new DataHelper();
            var param = new Dictionary<string, object>();
            param.Add("@MEMBER_ID", memberID);
            var data = await _handleData.handleSp(query, param);
            if(data.Count> 0)
            {
                result.status = true;
                result.data = data;
                result.message = "GET INFO SUCCESS";
            }
            else
            {
                result.message = "GET INFO FAIL";
            }
            return result;
        }

        public async Task<LoginResponse> LOGIN(USER_LOGIN_DTO loginDto)
        {
            string query = "UP_MB_LOGIN";
            LoginResponse result = new LoginResponse();
            DataHelper helper = new DataHelper();
            var param = new Dictionary<string, object>();

            Boolean check = await CHECK_USER_EXIST(loginDto.LoginIdentify);
            if(check)
            {
                param.Add("@LoginValue", loginDto.LoginIdentify);

            }
            
            helper.encryptPassword(loginDto.Password, out byte[] pwdHash, out byte[] pwdSalt);
            string hash = helper.pwdByteToString(pwdHash);
            string salt = helper.pwdByteToString(pwdSalt);
            param.Add("@PWD_HASH", hash);
            param.Add("@PWD_SALT", salt);
            //param.Add("@LOGIN_TYPE", loginType);
            try
            {
                var data = await _handleData.handleSp(query, param);
                result.status = true;
                JObject obj = JObject.Parse(data[0].ToString());
                if (data.Count > 0)
                {
                    if (obj.ContainsKey("CODE"))
                    {
                        string resultCode = obj.GetValue("CODE").ToString();
                        result.code = resultCode;
                        switch (resultCode)
                        {
                            
                            case "0": //LOGIN SUCCESS
                                string memberID = obj.GetValue("MEMBER_ID").ToString();
                                GenericResponse dataUser = await GET_USER_INFO_BY_ID(memberID);
                                JObject jwtData = JObject.Parse(dataUser.data.ToString());
                                string token = helper.CreateJWT(jwtData);
                                result.token = token;
                                result.message = "LOGIN SUCCESS !";
                                break;
                            case "2": //WRONG PASSWORD
                                result.message = "PLEASE CHECK YOUR PASSWORD AGAIN !";
                                break;
                            case "-1": //MEMBER NOT EXIST
                                result.message = "LOGIN IDENTIFY IS NOT EXIST";
                                break;
                            default:
                                result.message = "LOGIN FAIL! PLEASE TRY AGAIN";
                                break;
                        }
                    }
                    else
                    {
                        result.message = "SOMETHING WENT WRONG! MISSING CODE";
                    }
                }
                else
                {
                    result.message = "SOMETHING WENT WRONG! PLEASE TRY AGAIN";
                }
            }
            catch(Exception ex)
            {
                result.message = ex.Message;
            }
            return result;

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
                    if (data.Count > 0)
                    {
                        JObject obj = JObject.Parse(data[0].ToString());
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
