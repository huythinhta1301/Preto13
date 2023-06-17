using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Preto13.Data.iRepo.Common;
using Preto13.Data.Repository.Common;
using Preto13.Model.Response;

namespace Preto13.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly iStatus _iStatus;

        public StatusController(iStatus istatus)
        {
            _iStatus = istatus;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse>> GetAllStatus()
        {
            try
            {
                GenericResponse response = await _iStatus.SHOW_ALL_STATUS();
                if (response.status)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("add")]
        public async Task<ActionResult<GenericResponse>> InsertNewStatus(string name_vn, string name_en, string desc)
        {
            try
            {
                GenericResponse response = await _iStatus.INSERT_NEW_STATUS(name_vn, name_en, desc);
                if (response.status)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
