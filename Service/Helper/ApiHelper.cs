using IAI.Membership.Model.Membership.Anggota;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IAI.Membership.Service.Helper
{
    public class ApiHelper
    {
        static MediaTypeWithQualityHeaderValue Json = new MediaTypeWithQualityHeaderValue("application/json");
        // Get infomation about the current logged in user.
        public static async Task<string> GetUserInfoAsync(Anggota model)
        {
            string access_token = "";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["Office365Login"]))
                {
                    request.Headers.Accept.Add(Json);

                    var values = new Dictionary<string, string> { };
                    values.Add("client_id", ConfigurationManager.AppSettings["ClientIDKey"]);
                    values.Add("client_secret", ConfigurationManager.AppSettings["ClientSecretKey"]);
                    values.Add("grant_type", "password");
                    values.Add("resource", "https://graph.microsoft.com");
                    values.Add("username", model.EmailIAI);
                    values.Add("password", model.Password);
                    values.Add("scope", "openid");
                    request.Content = new FormUrlEncodedContent(values);

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                            access_token = (string)json.SelectToken("access_token");
                        }
                        else
                        {
                            access_token = "";
                        }
                    }
                }
            }

            return access_token;
        }
        public static async Task<bool> PatchChangePassword(string emailIAI, string NewPassword)
        {
            string adminToken = await GetAdminInfoAsync();

            using (var client = new HttpClient())
            {
                var method = new HttpMethod("PATCH");
                string url = ConfigurationManager.AppSettings["Office365ChangePassword"] + "/" + emailIAI;
                using (var request = new HttpRequestMessage(method, url))
                {
                    request.Headers.Accept.Add(Json);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);



                    var data = new Dictionary<string, object> {
                         { "password", NewPassword },
                        { "forceChangePasswordNextSignIn", false}
                    };

                    var values = new Dictionary<string, object>
                    {
                        {"passwordProfile", data }
                    };


                    string content = JsonConvert.SerializeObject(values);

                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");


                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.NoContent)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

        }
        public static async Task<string> GetAdminInfoAsync()
        {
            string access_token = "";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["Office365Login"]))
                {
                    request.Headers.Accept.Add(Json);

                    var values = new Dictionary<string, string> { };
                    values.Add("client_id", ConfigurationManager.AppSettings["ClientIDKey"]);
                    values.Add("client_secret", ConfigurationManager.AppSettings["ClientSecretKey"]);
                    values.Add("grant_type", "client_credentials");
                    values.Add("resource", "https://graph.microsoft.com");
                    values.Add("username", ConfigurationManager.AppSettings["EmailAdmin"]);
                    values.Add("password", ConfigurationManager.AppSettings["EmailPassword"]);
                    values.Add("scope", "openid");
                    request.Content = new FormUrlEncodedContent(values);
                    try
                    {
                        using (var response = await client.SendAsync(request))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var responseString = await response.Content.ReadAsStringAsync();
                                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                                access_token = (string)json.SelectToken("access_token");
                            }
                            else
                            {
                                access_token = "";
                            }
                        }

                    }
                    catch (System.Exception ex)
                    {
                        throw;
                    }

                }
            }

            return access_token;
        }
        public static void DeleteAnggotaMany(string EmailAnggota)
        {
            new Thread(async () =>
            {
                await DeleteAnggotaDataAsync(EmailAnggota);
            }).Start();
        }
        public static async Task<bool> DeleteAnggotaDataAsync(string EmailAnggota)
        {
            string adminToken = await GetAdminInfoAsync();

            var chunkListRecipient = EmailAnggota.Split(',');
            foreach (var recipient in chunkListRecipient)
            {
                using (var client = new HttpClient())
                {
                    var method = new HttpMethod("DELETE");
                    string url = ConfigurationManager.AppSettings["Office365DeleteUser"] + "/" + recipient;
                    using (var request = new HttpRequestMessage(method, url))
                    {
                        request.Headers.Accept.Add(Json);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);


                        using (var response = await client.SendAsync(request))
                        {
                            if (response.StatusCode == HttpStatusCode.NoContent)
                            {

                            }
                            else
                            {

                            }
                        }
                    }
                }
            }

            return true;
        }
        public static async Task<bool> RemoveLicense(string EmailAnggota)
        {
            string adminToken = await GetAdminInfoAsync();
            List<string> userLicenses = await GetUserLicense(EmailAnggota, adminToken);

            using (var client = new HttpClient())
            {
                var method = new HttpMethod("POST");
                string url = ConfigurationManager.AppSettings["Office365RemoveLicenses"];
                url = url.Replace("{EMAILIAI}", EmailAnggota);

                using (var request = new HttpRequestMessage(method, url))
                {
                    request.Headers.Accept.Add(Json);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
                    JArray removeLicenses = new JArray();
                    foreach (string item in userLicenses)
                    {
                        removeLicenses.Add(item);
                    }
                    JArray addLicenses = new JArray();



                    var values = new Dictionary<string, JArray>
                    {
                        {"addLicenses", addLicenses },
                        {"removeLicenses", removeLicenses }
                    };


                    string content = JsonConvert.SerializeObject(values);

                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");


                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public static async Task<List<string>> GetUserLicense(string EmailAnggota, string adminToken)
        {
            List<string> userLicenses = new List<string>();

            using (var client = new HttpClient())
            {
                var method = new HttpMethod("GET");
                string url = ConfigurationManager.AppSettings["Office365GetUserLicenses"];
                url = url.Replace("{EMAILIAI}", EmailAnggota);

                using (var request = new HttpRequestMessage(method, url))
                {
                    request.Headers.Accept.Add(Json);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);


                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                            JArray values = (JArray)json.SelectToken("value");
                            for (int i = 0; i < values.Count; i++)
                            {
                                userLicenses.Add((string)values[i].SelectToken("skuId"));
                            }
                        }
                        else
                        {
                            return userLicenses;
                        }
                    }
                }
            }
            return userLicenses;

        }
        public static async Task<bool> AssignLicense(string EmailAnggota)
        {
            string adminToken = await GetAdminInfoAsync();

            using (var client = new HttpClient())
            {
                var method = new HttpMethod("POST");
                string url = ConfigurationManager.AppSettings["Office365AssignLicenses"];
                url = url.Replace("{EMAILIAI}", EmailAnggota);
                using (var request = new HttpRequestMessage(method, url))
                {
                    request.Headers.Accept.Add(Json);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);



                    JArray removeLicenses = new JArray();

                    JArray addLicenses = new JArray();
                    var addLicensesValue = new Dictionary<string, object>
                    {
                        {"disabledPlans", new JArray() },
                        {"skuId", ConfigurationManager.AppSettings["Office365ForStudentLicenseID"].ToString() }
                    };
                    ;
                    addLicenses.Add(JObject.FromObject(addLicensesValue));


                    var values = new Dictionary<string, JArray>
                    {
                        {"addLicenses", addLicenses },
                        {"removeLicenses", removeLicenses }
                    };


                    string content = JsonConvert.SerializeObject(values);

                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");


                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public static async Task<bool> CreateUser(Anggota anggota)
        {
            string adminToken = await GetAdminInfoAsync();

            using (var client = new HttpClient())
            {
                var method = new HttpMethod("POST");
                string url = ConfigurationManager.AppSettings["Office365CreateUser"];
                using (var request = new HttpRequestMessage(method, url))
                {
                    request.Headers.Accept.Add(Json);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);


                    var data = new Dictionary<string, object> {
                         { "password",  ConfigurationManager.AppSettings["DefaultPassword"].Replace("{ddmmyyyy}",anggota.TanggalLahir.Date.ToString("ddMMyyyy")) },
                         { "forceChangePasswordNextSignIn", false}
                    };

                    var values = new Dictionary<string, object>
                    {
                           {"accountEnabled", true },
                           {"displayName", anggota.NamaLengkap },
                           {"givenName", anggota.NamaLengkap },
                           {"mailNickname", anggota.EmailIAI.Replace(ConfigurationManager.AppSettings["AnggotaDomain"],"") },
                           { "userPrincipalName", anggota.EmailIAI },
                           {"usageLocation", "ID" },
                           {"passwordProfile", data }
                    };



                    string content = JsonConvert.SerializeObject(values);

                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");


                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public static async Task<bool> getUser(string EmailAnggota)
        {
            string adminToken = await GetAdminInfoAsync();

            using (var client = new HttpClient())
            {
                var method = new HttpMethod("GET");
                string url = ConfigurationManager.AppSettings["Office365GetUser"];
                url = url.Replace("{EMAILIAI}", EmailAnggota);

                using (var request = new HttpRequestMessage(method, url))
                {
                    request.Headers.Accept.Add(Json);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);


                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public static async Task<bool> importCheckUser(Anggota anggota)
        {
            bool isOffice365User = await getUser(anggota.EmailIAI);

            return isOffice365User;
        }

        public static async Task<bool> importResetPasswordUser(Anggota anggota)
        {
            return await PatchChangePassword(anggota.EmailIAI, ConfigurationManager.AppSettings["DefaultPassword"].Replace("{ddmmyyyy}", anggota.TanggalLahir.Date.ToString("ddMMyyyy")));
        }
    }
}
