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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace IM.Data
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers(string token);
        Task<string> GetUserNameById(string userId);
        Task<UserLogin> Authenticate(string username, string password);
        Task<UserPages> GetUsersWithPage(string token, int pageSize, int pageNumber, string search);
        Task<string> GetLoggedInUserId();

    }

    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        private readonly ICommon commonService;
        private string baseUrl = "https://imwebapicore.azurewebsites.net/api";
        private readonly ISession session;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserService(IConfiguration _configuration, IHttpClientFactory _clientFactory, IHttpContextAccessor _httpContextAccessor, ICommon _commonService)
        {
            configuration = _configuration;
            clientFactory = _clientFactory;
            commonService = _commonService;
            session = _httpContextAccessor.HttpContext.Session;
        }

        public async Task<UserLogin> Authenticate(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                 baseUrl + "/Users/authenticate");
            request.Content = new StringContent(JsonSerializer.Serialize(new { Username = username, Password = password }), Encoding.UTF8, "application/json");

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var userLoginInfo = await JsonSerializer.DeserializeAsync<UserLogin>(responseStream);
                return userLoginInfo;

                // await localStorage.SetItemAsync("token", userLoginInfo.Token);

                 var allUsers = await GetAllUsers(userLoginInfo.Token);
                session.SetString("Token", "The Doctor");
                // await localStorage.SetItemAsync("allUsers", allUsers);
                //  await localStorage.SetItemAsync("loggedInUserId", userLoginInfo.user.Id);

                

                
                              
            }
            else            
               return null;            
            
        }
        public async Task<List<User>> GetAllUsers(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                baseUrl + "/Users/AllUsers");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var users = new List<User>();

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                users = await JsonSerializer.DeserializeAsync<List<User>>(responseStream);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetAllUsers");
                }
                commonService.HandleFailedRequests("GetAllUsers", response.StatusCode);
            }

            return users;
        }

        public async Task<UserPages> GetUsersWithPage(string token, int pageSize, int pageNumber, string search)
        {           
            var request = new HttpRequestMessage(HttpMethod.Get,
             baseUrl + "/Users/GetUsersWithPage?PageSize=" + pageSize + "&PageNumber=" + pageNumber + "&SortBy=a&SortDirection=a&Search=" + search);

            request.Headers.Add("Authorization", "Bearer " + token);


            var client = clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var userPages = await JsonSerializer.DeserializeAsync<UserPages>(responseStream);
                return userPages;
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    commonService.HandleUnauthorizedRequests("GetUsersWithPage");
                }
                commonService.HandleFailedRequests("GetUsersWithPage", response.StatusCode);
                return null;
            }     
        }

        public async Task<string> GetUserNameById(string userId)
        {
            try
            {
                var users = JsonSerializer.Deserialize<List<User>>(session.GetString("allUsers"));

                var user = users.Where(user => user.Id == userId).FirstOrDefault();
                return user.FirstName + " " + user.LastName;
            }
            catch(Exception ex)
            {
                return "Error Erro";
            }
        }

        public async Task<string> GetLoggedInUserId()
        {
            return "1";// await localStorage.GetItemAsync<string>("loggedInUserId");
        }

    }
}
