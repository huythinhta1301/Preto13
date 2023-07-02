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

        public async Task<JObject> CHECK_USER_EXIST_LOGIN(string INPUT)
        {
            string query = "UP_MB_CHECK_EXIST_LOGIN";
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
            param.Add("@LOGIN_TYPE", loginType);
            var data = await _handleData.handleSp(query, param);
            JObject obj = JObject.Parse(data[0].ToString());
            return obj;
        }

        public async Task<GenericResponse> GET_USER_INFO_BY_ID(string memberID)
        {
            string query = "UP_MB_INFO";
            GenericResponse result = new GenericResponse();
            DataHelper helper = new DataHelper();
            var param = new Dictionary<string, object>();
            param.Add("@MEMBER_ID", memberID);
            var data = await _handleData.handleSp(query, param);
            if (data.Count > 0)
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
            //string query = "UP_MB_LOGIN";
            LoginResponse result = new LoginResponse();
            DataHelper helper = new DataHelper();
            var param = new Dictionary<string, object>();

            JObject jo = await CHECK_USER_EXIST_LOGIN(loginDto.LoginIdentify);
            if (jo.GetValue("CODE").ToString() == "0")
            {
                string PWD_HASH = jo.GetValue("PWD_HASH").ToString();
                string PWD_SALT = jo.GetValue("PWD_SALT").ToString();
                Boolean checkPassword = helper.CompareHashedPassword(loginDto.Password, PWD_HASH, PWD_SALT);
                if (checkPassword) // correct password
                {
                    string memberID = jo.GetValue("MEMBER_ID").ToString();
                    param.Add("@LoginValue", loginDto.LoginIdentify);
                    try
                    {
                        GenericResponse dataUser = await GET_USER_INFO_BY_ID(memberID);
                        if (dataUser.data != null && dataUser.data.Count > 0)
                        {
                            JObject jwtData = JObject.Parse(dataUser.data[0].ToString());
                            // Use jwtData as needed
                            string token = helper.CreateJWT(jwtData);
                            result.code = "0";
                            result.token = token;
                            result.message = "LOGIN SUCCESS !";
                        }
                        
                        //    break;
                        //case "2": //WRONG PASSWORD
                        //    result.message = "PLEASE CHECK YOUR PASSWORD AGAIN !";
                        //    break;
                        //case "-1": //MEMBER NOT EXIST
                        //    result.message = "LOGIN IDENTIFY IS NOT EXIST";
                        //    break;
                        //default:
                        //    result.message = "LOGIN FAIL! PLEASE TRY AGAIN";
                        //    break;

                    }
                    catch (Exception ex)
                    {

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
                string salt = dataHelper.GenerateSalt();
                byte[] hashPwd = dataHelper.GetHash(regis.password, salt);
                string hashPwdString = Convert.ToBase64String(hashPwd);
                param.Add("@PWD_HASH", hashPwdString);
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
