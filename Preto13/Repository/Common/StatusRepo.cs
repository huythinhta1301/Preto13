using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Preto13.Config;
using Preto13.Data.Common;
using Preto13.Model;
using Preto13.Utils;
using System.Data;
using System.Reflection.Metadata;

namespace Preto13.Repository.Common
{
    public class StatusRepo : iStatus
    {
        private readonly HandleData _handleData;

        public StatusRepo(HandleData handleData)
        {
            _handleData = handleData;
        }

        public async Task<GenericResponse> INSERT_NEW_STATUS(string VN_NAME, string EN_NAME, string DESCRIPTION)
        {
            string query = "UP_CM_INSERT_STATUS";
            GenericResponse result = new GenericResponse();
            var param = new Dictionary<string, object>();
            param.Add("@STATUS_NAME_VN", VN_NAME);
            param.Add("@STATUS_NAME_EN", EN_NAME);
            param.Add("@STATUS_DESCRIPTION", DESCRIPTION);
            try
            {
                var resp = await _handleData.handleSp(query, param);
                JObject firstObject = resp[0] as JObject;
                if (firstObject.ContainsKey("RESULTCODE"))
                {
                    result.status = true;
                    string codeRP = firstObject["RESULTCODE"].ToString();
                    result.code = codeRP;
                    if (codeRP.Equals("1"))
                    {
                        result.message = "SUCCESS";
                    }
                    else
                    {
                        result.message = "ERROR";
                    }
                }
                else
                {
                    result.message = "MISSING RESULT";
                }
            }
            catch (Exception ex)
            {
                result.code = "0";
                result.message = "FAIL";
            }
            return result;
        }
        public async Task<GenericResponse> SHOW_ALL_STATUS()
        {
            string query = "UP_CM_SELECT_STATUS";
            GenericResponse result = new GenericResponse();
            var param = new Dictionary<string, object>();
            try
            {
                var dataRP = await _handleData.handleSp(query, param);

                if (dataRP.Count > 0)
                {
                    result.data = dataRP;
                    result.message = "SUCESS";
                    result.code = "1";
                    result.status = true;
                }
                else
                {
                    result.message = "EMPTY";
                    result.code = "2";
                    result.status = true;
                }
            }
            catch (Exception ex)
            {
                result.message = "FAIL";
                result.code = "0";
                result.status = false;
            }
            return result;
        }
    }
}
