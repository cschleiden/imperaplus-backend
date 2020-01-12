using System;
using Microsoft.AspNetCore.SignalR;

namespace ImperaPlus.Web.Providers
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
