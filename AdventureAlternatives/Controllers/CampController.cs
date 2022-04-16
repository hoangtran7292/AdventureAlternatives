using AdventureAlternatives.Helpers;
using AdventureAlternatives.Models;
using AdventureAlternatives.Models.Common;
using AdventureAlternatives.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureAlternatives.Controllers
{
    [Route("api/camps")]
    [ApiController]
    public class CampController : ControllerBase
    {
        private readonly ICampService _campService;

        public CampController(ICampService campService)
        {
            _campService = campService;
        }

        [HttpGet("{campId}")]
        public async Task<IActionResult> GetCampForZohoSites(string campId)
        {
            var apiResult = new ApiResultDto<CampForZohoSites>()
            {
                Code = ResultCode.BadRequest,
                Message = Constants.ConvertCamp_400
            };
            try
            {
                var getCampResult = await _campService.GetCampById(campId);
                if (getCampResult.Code != ResultCode.OK)
                {
                    return BadRequest(apiResult);
                }
                apiResult = _campService.ConvertCampToZohoSites(getCampResult.Data);
                switch (apiResult.Code)
                {
                    case ResultCode.OK:
                        return Ok(apiResult);
                    default:
                        return BadRequest(apiResult);
                }
            }
            catch (Exception)
            {
                return BadRequest(apiResult);
            }
        }
    }
}
