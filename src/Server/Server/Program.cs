using System.Net.WebSockets;
using System.Text;
using System.Collections.Concurrent;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.UseWebSockets();
var clients = new ConcurrentDictionary<string, WebSocket>();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var clientId = Guid.NewGuid().ToString();
            clients.TryAdd(clientId, webSocket);
            Console.WriteLine($"Client {clientId} connected");
            await HandleClient(clientId, webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});
async Task HandleClient(string clientId, WebSocket webSocket)
{
    var buffer = new byte[1024 * 4];
    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    while (!result.CloseStatus.HasValue)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        foreach (var client in clients)
        {
            if (client.Key != clientId)
            {
                var responseBytes = Encoding.UTF8.GetBytes($"Client {clientId}: {message}");
                await client.Value.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }
    clients.TryRemove(clientId, out _);
    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    Console.WriteLine($"Client {clientId} disconnected");
}
app.UseStaticFiles();
app.Run();
