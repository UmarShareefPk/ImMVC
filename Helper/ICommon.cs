using System.Net;

namespace IM.Helper
{
    public interface ICommon
    {
        string GetStatusNameByCode(string code);
        Task HandleUnauthorizedRequests(string method);
        void HandleFailedRequests(string method, HttpStatusCode statusCode);
    }
}