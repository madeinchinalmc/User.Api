using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using User.Identity.Dtos;
using Newtonsoft.Json;

namespace User.Identity.Service
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        private string _userServiceUrl = "http://localhost";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfo> CheckOrCreate(string phone)
        {
            var from = new Dictionary<string, string>
            {
                { "phone",phone }
            };
            var content = new FormUrlEncodedContent(from);
            var response = await _httpClient.PostAsync(_userServiceUrl + "/api/users/check-or-create", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var userinfo = JsonConvert.DeserializeObject<UserInfo>(result);

                //int.TryParse(userId,out int UserIdInt);
                return userinfo;
            }
            return null;
        }
    }
}
 