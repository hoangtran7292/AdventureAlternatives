using AdventureAlternatives.Models;
using AdventureAlternatives.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureAlternatives.Services
{
    public interface ICampService
    {
        Task<ApiResultDto<CampResponse>> GetCampById(string id);
        ApiResultDto<CampForZohoSites> ConvertCampToZohoSites(CampResponse camp);
    }
}
