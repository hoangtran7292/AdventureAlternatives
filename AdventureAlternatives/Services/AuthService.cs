using AdventureAlternatives.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventureAlternatives.Services
{
    public class AuthService : IAuthService
    {
        private const string AuthEndpoint = "https://accounts.zoho.com/oauth/v2/token";
        
        private const string ClientId = "1000.LKGJ9TX91V0B8BWFVMOKQ58LMU2AJA";
        private const string ClientSecret = "eae2c3015b3c3a070a6d2bbadf8c13873a188bee72";

        private const string RefreshToken = "1000.6c84a07599bd94bd26ef8e6951b21cad.ac54209467ed8f5071b854d3f53a2788";
        private const string RefreshToken_Sandbox = "1000.c25e5c11a6cd5c324f3058fde7c68c17.fd4b1581091462d91afa422825b25056";

        private const string AccessTokenFile = "access_token.txt";
        private readonly string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public async Task<string> GetAccessToken(bool isDevelopment = true)
        {
            try
            {
                AccessTokenModel tokenModel = null;
                string txtFilePath = $"{DocumentPath}\\{AccessTokenFile}";
                if (!Directory.Exists(DocumentPath))
                {
                    Directory.CreateDirectory(DocumentPath);
                }
                if (File.Exists(txtFilePath))
                {
                    string text = File.ReadAllText(txtFilePath);
                    if (!string.IsNullOrEmpty(text))
                    {
                        tokenModel = JsonConvert.DeserializeObject<AccessTokenModel>(text);
                        if (DateTime.UtcNow < tokenModel.ExpiredTime)
                        {
                            return tokenModel.AccessToken;
                        }
                    }
                }

                string grantType = "refresh_token";
                string endpoint = string.Empty;
                if (isDevelopment)
                {
                    endpoint = $"{AuthEndpoint}?refresh_token={RefreshToken_Sandbox}&client_id={ClientId}&client_secret={ClientSecret}&grant_type={grantType}";
                }
                else
                {
                    endpoint = $"{AuthEndpoint}?refresh_token={RefreshToken}&client_id={ClientId}&client_secret={ClientSecret}&grant_type={grantType}";
                }
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint);

                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<TokenResponse>(responseData);
                string accessToken = responseObj.access_token;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var accessTokenForSaving = new AccessTokenModel()
                    {
                        AccessToken = accessToken,
                        ExpiredTime = DateTime.UtcNow.AddMinutes(10),
                    };
                    File.WriteAllText(txtFilePath, JsonConvert.SerializeObject(accessTokenForSaving));
                }
                return accessToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
