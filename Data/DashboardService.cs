using IM.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using IM.Helper;
using System.Net;

namespace IM.Data
{
    public interface IDashboardService
    {
        Task<KpiData> GetKpi(string token, string userId);
        Task<List<Incident>> GetLast5Incidents(string token);
        Task<object> GetMostAssignedToUsers(string token);
        Task<List<Incident>> GetOldest5UnresolvedIncidents(string token);
        Task<KpiData> GetOverallWidget(string token);
    }

    public class DashboardService : IDashboardService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly IUserService userService;
        private readonly ICommon commonService;
        private string baseUrl = "https://imwebapicore.azurewebsites.net/api";
        public DashboardService(IConfiguration _configuration, IHttpClientFactory _clientFactory,  IUserService _userService, ICommon _commonService)
        {
            configuration = _configuration;
            clientFactory = _clientFactory;
            userService = _userService;
            commonService = _commonService;
        }

        public async Task<KpiData> GetKpi(string token, string userId)
        {            
            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Incidents/KPI?userId=" + userId);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<KpiData>(responseStream);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetKpi");
                }
                commonService.HandleFailedRequests("GetKpi", response.StatusCode);
                return null;
            }
        }

        public async Task<KpiData> GetOverallWidget(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
               baseUrl + "/Incidents/OverallWidget");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<KpiData>(responseStream);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetOverallWidget");
                }
                commonService.HandleFailedRequests("GetOverallWidget", response.StatusCode);
                return null;
            }
        }

        public async Task<List<Incident>> GetLast5Incidents(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Incidents/Last5Incidents");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<Incident>>(responseStream);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetLast5Incidents");
                }
                commonService.HandleFailedRequests("GetLast5Incidents", response.StatusCode);
                return null;
            }
        }

        public async Task<List<Incident>> GetOldest5UnresolvedIncidents(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Incidents/Oldest5UnresolvedIncidents");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<List<Incident>>(responseStream);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetOldest5UnresolvedIncidents");
                }
                commonService.HandleFailedRequests("GetOldest5UnresolvedIncidents", response.StatusCode);
                return null;
            }
        }

        public async Task<object> GetMostAssignedToUsers(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Incidents/MostAssignedToUsersIncidents");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<object>(responseStream);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetMostAssignedToUsers");
                }
                commonService.HandleFailedRequests("GetMostAssignedToUsers", response.StatusCode);
                return null;
            }
        }

    } //end of class
}
