using Google.Cloud.Firestore;
using Google.Cloud.Storage.V1;
using System.Text.Json.Serialization;
using SaveVault.Repositories;
using SaveVault.Repositories.Implementation;
using SaveVault.Services;
using SaveVault.Services.Implementation;
using RabbitMQ.Client;


Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "save-vault-gac.json");

ConnectionFactory connectionFactory = new() { HostName = "localhost" };
IConnection connection = connectionFactory.CreateConnection();
IModel channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "SaveVault", type: ExchangeType.Topic, durable: true);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers()
	.AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(logging =>
{
	logging.AddConsole();
	logging.AddDebug();
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IConversionService, ConversionService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IDownloadService, DownloadService>();
builder.Services.AddScoped<IPublishService, PublishService>();

builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddScoped<IDownloadRepository, DownloadRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IFirebaseRepository, FirebaseRepository>();
builder.Services.AddSingleton(provider => FirestoreDb.Create("save-vault"));
builder.Services.AddSingleton(provider => StorageClient.Create());
builder.Services.AddSingleton(provider => channel);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
