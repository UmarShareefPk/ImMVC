using Microsoft.AspNetCore.Components;
using System.Net;

namespace IM.Helper
{
    public class Common : ICommon
    {
       private readonly NavigationManager navigationManager;
     
        public Common(NavigationManager _navigationManager)
        {
            navigationManager = _navigationManager;           
        }

        public string GetStatusNameByCode(string code)
        {
            switch (code)
            {
                case "N":
                    return "New";
                case "C":
                    return "Closed";
                case "A":
                    return "Approved";
                case "I":
                    return "In Progress";
                default:
                    return code;
            }
        }
        
        public async Task HandleUnauthorizedRequests(string method)
        {
                       
            navigationManager.NavigateTo("/", forceLoad: true);
        }

        public void HandleFailedRequests(string method, HttpStatusCode statusCode)
        {

        }


    }// end of class
}
