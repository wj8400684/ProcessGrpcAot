using ProcessGrpcAot;
using ProcessGrpcAot.Services;

var socketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");

if (Path.Exists(socketPath))
    File.Delete(socketPath);

var builder = WebApplication.CreateSlimBuilder(args);
builder.WebHost.UseKestrel(s =>
{
    s.ListenNamedPipe("swatch");
    s.ListenUnixSocket(socketPath);
});
//builder.Services.AddNamedPipeTransportFactory();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();

app.Run();