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
    public interface INotificationService
    {
        Task<List<IncidentNotification>> GetAllNotifications(string token);
        Task<bool> UpdateStatus(string token, string notificationId, string status);
        Task<bool> UpdateHubId(string token, string hubId);
    }

    public class NotificationService : INotificationService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
       
        private readonly ICommon commonService;
        private readonly IUserService userService;
        private string baseUrl = "https://imnet7mongodbapi.azurewebsites.net/api";
        public NotificationService(IConfiguration _configuration, IHttpClientFactory _clientFactory, ICommon _commonService, IUserService _userService)
        {
            configuration = _configuration;
            clientFactory = _clientFactory;            
            commonService = _commonService;
            userService = _userService;
        }

        public async Task<bool> UpdateHubId(string token, string hubId)
        {
            string userId = await userService.GetLoggedInUserId();
            var request = new HttpRequestMessage(HttpMethod.Post,
                 baseUrl + "/Users/UpdateHubId");
            request.Content = new StringContent(JsonSerializer.Serialize(new { HubId = hubId, UserId = userId }), Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", "Bearer " + token);
            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
              //  using var responseStream = await response.Content.ReadAsStreamAsync();
               // var userLoginInfo = await JsonSerializer.DeserializeAsync<UserLogin>(responseStream);


                return true;

            }
            else
                return false;
           
        }
        public async Task<List<IncidentNotification>> GetAllNotifications(string token)
        {
            string loggedInUser = await userService.GetLoggedInUserId();
            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Users/UserNotifications?userId=" + loggedInUser);
            request.Headers.Add("Authorization", "Bearer " + token);
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var notifications = new List<IncidentNotification>();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                notifications = await JsonSerializer.DeserializeAsync<List<IncidentNotification>>(responseStream);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)                
                    commonService.HandleUnauthorizedRequests("GetAllNotifications");
                
                commonService.HandleFailedRequests("GetAllNotifications", response.StatusCode);
            }
            return notifications;
        }

        public async Task<bool> UpdateStatus(string token, string notificationId, string status)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Users/UpdateIsRead" + "?notificationId=" + notificationId + "&isRead=" + status);
            request.Headers.Add("Authorization", "Bearer " + token);
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var notifications = new List<IncidentNotification>();

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    commonService.HandleUnauthorizedRequests("UpdateStatus");

                commonService.HandleFailedRequests("UpdateStatus", response.StatusCode);
            }
            return false; ;
        }



    }// end of class
}
