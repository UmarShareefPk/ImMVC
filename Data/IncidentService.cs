using IM.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using IM.Models;
using IM.Helper;
using System.Net;

namespace IM.Data
{
    public interface IIncidentService
    {
        Task<IncidentPages> GetIncidentsWithPage(string token, int pageSize, int pageNumber, string search);
        Task<Incident> GetIncidentById(string token, string incidentId);
        Task<bool> UpdateIncident(string token, object parameters);
        Task<bool> DeleteFile(string token, string type, string incidentId, string commentId, string fileId, string fileName, string contentType);
        string GetBaseUrl();
    }

    public class IncidentService : IIncidentService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
       
        private readonly ICommon commonService;
        private readonly IUserService userService;
        string baseUrl = "https://imwebapicore.azurewebsites.net/api";
        public IncidentService(IConfiguration _configuration, IHttpClientFactory _clientFactory, ICommon _commonService, IUserService _userService)
        {
            configuration = _configuration;
            clientFactory = _clientFactory;
            commonService = _commonService;
            userService = _userService;
        }

        
        public string GetBaseUrl()
        {
            return baseUrl;
        }
        public async Task<IncidentPages> GetIncidentsWithPage(string token, int pageSize, int pageNumber, string search)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            baseUrl + "/Incidents/GetIncidentsWithPage?PageSize=" + pageSize + "&PageNumber=" + pageNumber + "&SortBy=a&SortDirection=a&Search=" + search);

            request.Headers.Add("Authorization", "Bearer " + token);


            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var incidentPages = await JsonSerializer.DeserializeAsync<IncidentPages>(responseStream);
                return incidentPages;
            }
            else
            {               
                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetIncidentsWithPage");
                }                
                commonService.HandleFailedRequests("GetIncidentsWithPage", response.StatusCode);                
             
                return null;
            }     
        }

        public async Task<Incident> GetIncidentById(string token, string incidentId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            baseUrl + "/Incidents/IncidentById?Id=" + incidentId);

            request.Headers.Add("Authorization", "Bearer " + token);


            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var incident = await JsonSerializer.DeserializeAsync<Incident>(responseStream);
                return incident;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetIncidentById");
                }
                commonService.HandleFailedRequests("GetIncidentById", response.StatusCode);
                return null;
            }
        }

        public async Task<bool> UpdateIncident(string token, object parameters)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
            baseUrl + "/Incidents/UpdateIncident");
            request.Content = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, "application/json");


            request.Headers.Add("Authorization", "Bearer " + token);


            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("UpdateIncident");
                }
                commonService.HandleFailedRequests("UpdateIncident", response.StatusCode);
                return false;
            }
        }

        public async Task<bool> DeleteFile(string token, string type, string incidentId, string commentId, string fileId, string fileName, string contentType)
        {
            string userId = await userService.GetLoggedInUserId();

            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Incidents/DeleteFile?"
                        + "type=" + type
                        + "&commentId=" + commentId
                        + "&incidentId=" + incidentId
                        + "&userId=" + userId
                        + "&fileId=" + fileId
                        + "&filename=" + fileName
                        + "&contentType=" + contentType
             );

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {                
                return true;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("DeleteFile");
                }
                commonService.HandleFailedRequests("DeleteFile", response.StatusCode);
                return false;
            }

        }



    }
}
