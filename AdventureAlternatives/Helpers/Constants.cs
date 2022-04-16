using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureAlternatives.Helpers
{
    public static class Constants
    {
        #region Common
        public const string Unauthorized = "You don't have permission to access this resource.";
        #endregion

        #region Camp
        public const string GetCampById_200 = "Get Camp by Id successfully!";
        public const string GetCampById_400 = "Get Camp by Id failed!";

        public const string ConvertCamp_200 = "Get Camp Details for Zoho Sites data successfully!";
        public const string ConvertCamp_400 = "Get Camp Details for Zoho Sites data failed!";
        #endregion
    }
}
