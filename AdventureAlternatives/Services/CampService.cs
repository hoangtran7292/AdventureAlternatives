using AdventureAlternatives.Helpers;
using AdventureAlternatives.Models;
using AdventureAlternatives.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventureAlternatives.Services
{
    public class CampService : ICampService
    {
        private readonly string ApiEndpoint = "https://www.zohoapis.com/crm/v2";
        private readonly string CampModule = "Camps";
        private readonly IAuthService _zohoAuthService;

        public CampService(IAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<CampResponse>> GetCampById(string id)
        {
            var apiResult = new ApiResultDto<CampResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = Constants.GetCampById_400
            };
            try
            {
                string accessToken = await _zohoAuthService.GetAccessToken();
                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = Constants.Unauthorized;
                    return apiResult;
                }
                string endpoint = $"{ApiEndpoint}/{CampModule}/{id}";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<CampResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = Constants.GetCampById_200;
                    apiResult.Data = responseObj;
                    return apiResult;
                }
                else
                {
                    return apiResult;
                }
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public  ApiResultDto<CampForZohoSites> ConvertCampToZohoSites(CampResponse camp)
        {
            var apiResult = new ApiResultDto<CampForZohoSites>()
            {
                Code = ResultCode.BadRequest,
                Message = Constants.ConvertCamp_400
            };
            try
            {
                var campForZohoSites = new CampForZohoSites();
                var campData = camp.data.First();
                // Handle Camp Name
                string campName = campData.Name;
                if (!string.IsNullOrEmpty(campName))
                {
                    campForZohoSites.Name = campName;
                }

                // Handle Camp Year
                string campYear = campData.Year_Level;
                if (!string.IsNullOrEmpty(campYear))
                {
                    campForZohoSites.Year = campYear;
                }

                // Handle Camp Date
                string campStartDate = string.Empty;
                var campStartStr = campData.Camp_Start_Date;
                if (!string.IsNullOrEmpty(campStartStr))
                {
                    campStartDate = DateTime.Parse(campStartStr).ToString("dd MMMM yyyy");
                }
                string campEndDate = string.Empty;
                var campEndStr = campData.Camp_Due_Date;
                if (!string.IsNullOrEmpty(campEndStr))
                {
                    campEndDate = DateTime.Parse(campEndStr).ToString("dd MMMM yyyy");
                }
                if (!string.IsNullOrEmpty(campStartDate) && !string.IsNullOrEmpty(campEndDate))
                {
                    campForZohoSites.Date = "DATE: " + campStartDate + " to " + campEndDate;
                }

                // Handle Camp Style
                string campStyle = string.Empty;
                var campStyles = campData.Camp_Styles;
                foreach (var styleData in campStyles)
                {
                    string style = styleData.Camp_Style;
                    int number = styleData.Nights.Value;
                    string day = styleData.Day_or_Night;

                    string daySuffix = number > 1 ? "s" : ""; 
                    string styleSentence = number + " " + day + daySuffix + " " + style;

                    if (string.IsNullOrEmpty(campStyle))
                    {
                        campStyle = "STYLE: " + styleSentence;
                    }
                    else
                    {
                        campStyle += ", " + styleSentence;
                    }
                }
                campForZohoSites.Style = campStyle;

                // Handle Camp Activity
                string campActivity = string.Empty;
                var campActivities = campData.Related_Camp_Activities;
                foreach (var activityData in campActivities)
                {
                    if (activityData.Activity != null)
                    {
                        string activity = activityData.Activity.name;
                        if (string.IsNullOrEmpty(campActivity))
                        {
                            campActivity = "ACTIVITIES: " + activity;
                        }
                        else
                        {
                            campActivity += ", " + activity;
                        }
                    }
                }
                campForZohoSites.Activities = campActivity;
                campForZohoSites.MedicalForm = campData.Camp_Medical_Dietary_Reponses_Link;
                campForZohoSites.Location = "LOCATION: " + campData.Camp_Location;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = Constants.ConvertCamp_200;
                apiResult.Data = campForZohoSites;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + ": " + ex.StackTrace;
                return apiResult;
            }
        }
    }
}
