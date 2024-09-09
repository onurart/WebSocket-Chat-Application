using System.Net.WebSockets;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("WebSocket Client Running...");
});
app.MapGet("/ws-connect", async context =>
{
    using (var client = new ClientWebSocket())
    {
        await client.ConnectAsync(new Uri("ws://localhost:5001/ws"), CancellationToken.None);
        await context.Response.WriteAsync("Connected to WebSocket server");
        string message = "Hello from Client!";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        await client.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
        var buffer = new byte[1024 * 4];
        var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var serverMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
        await context.Response.WriteAsync($"\nReceived from server: {serverMessage}");
    }
});
app.UseStaticFiles();
app.Run();

