using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using User.Identity.Dtos;
using Newtonsoft.Json;
using Consul;
using DnsClient;
using Microsoft.Extensions.Options;

namespace User.Identity.Service
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        //private string _userServiceUrl = "http://localhost:18000";
        private ConsulClient _consulClient;
        private string _userServiceUrl;
        public UserService(HttpClient httpClient, 
            ConsulClient consulClient,
            IDnsQuery dnsQuery,
            IOptions<ServiceDiscoveryOptions> options)
        {
            _httpClient = httpClient;
            _consulClient = consulClient;
            var address = dnsQuery.ResolveService("service.consul", options.Value.ServiceName);
            var host = address.First().AddressList.Any()? address.First().AddressList.First().ToString(): address.First().HostName;
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
        }

        public async Task<UserInfo> CheckOrCreate(string phone)
        {
            //var consulResult = await _consulClient.Catalog.Service(_options.Value.ServiceName);
            //var healthResult = await _consul.Health.Service(_options.Value.ServiceName, tag: null, passingOnly: true);

            //
            var from = new Dictionary<string, string>
            {
                { "phone",phone }
            };
            var content = new FormUrlEncodedContent(from);
            var response = await _httpClient.PostAsync(_userServiceUrl + "/api/users/check_or_create", content);
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
 