using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task<int> CheckOrCreate(string phone)
        {
            var from = new Dictionary<string, string>
            {
                { "phone",phone }
            };
            var content = new FormUrlEncodedContent(from);
            var response = await _httpClient.PostAsync(_userServiceUrl + "/api/users/check-or-create", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var userId = await response.Content.ReadAsStringAsync();
                int.TryParse(userId,out int UserIdInt);
                return UserIdInt;
            }
            return 0;
        }
    }
}
 