using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.WebSocketHubs
{
    public sealed class LogHub : Hub
    {
        private readonly ILogger<LogHub> _log;

        public LogHub(ILogger<LogHub> log)
        {
            _log = log;
        }

        public override async Task OnConnectedAsync()
        {
            _log.LogDebug("on connected");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _log.LogDebug(exception, "on disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}