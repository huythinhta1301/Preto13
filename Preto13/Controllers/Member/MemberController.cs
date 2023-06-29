using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Preto13.Data.Member;
using Preto13.Model;
using Preto13.Model.DTO;
using Preto13.Utils;
using System.Drawing;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Preto13.Controllers.Member
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly iMemberAccount _account;
        public MemberController(iMemberAccount account)
        {
            _account = account;
        }
        [HttpPost("REGISTER")]
        public async Task<ActionResult<GenericResponse>> REGISTER_USER (USER_REGISTER_DTO regis)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                resp = await _account.REGISTER(regis);
                if (resp.code.Equals("0"))
                {
                    return Ok(resp);
                }
                else
                {
                    // Handle the registration failure case
                    return BadRequest(resp);
                }
            }
            catch(Exception ex)
            {
                resp.message = ex.Message;
                Response.StatusCode = 500;
            }
            return resp;
        }

        //public async Task<ActionResult<GenericResponse>> LOGIN (USER_LOGIN_DTO login)
        //{
        //    GenericResponse resp = new GenericResponse();
        //    try
        //    {
        //        resp = await _account.REGISTER(regis.username, regis.email, regis.phone, regis.password);
        //    }
        //}
    }
}
