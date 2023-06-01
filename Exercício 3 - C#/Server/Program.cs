using GrpcServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGet("/", () => "Servidor gRPC para calcular resultado final através da média entre três notas");
app.UseEndpoints(endpoints =>
{
	endpoints.MapGrpcService<ResultService>().EnableGrpcWeb();
}
);

app.Run();
