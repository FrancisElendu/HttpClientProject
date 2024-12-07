using HttpClientProject.Extensions;
using HttpClientProject.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//// Configure the HttpClient separately
//builder.Services.AddHttpClient("ApiHttpClientConfig", client =>
//{
//    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//    client.Timeout = TimeSpan.FromSeconds(60); // Optional timeout configuration
//})
//.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//{
//    MaxConnectionsPerServer = 10  // Optional: limit max connections to server
//});

//// Register ApiService as a singleton to inject IHttpClientFactory
//builder.Services.AddSingleton<IApiService, ApiService>();

// Configure and register HttpClient with policies
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(60);
})
//.AddPolicies(builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ApiService>>())
.AddPolicies(provider => provider.GetRequiredService<ILogger<ApiService>>())
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    MaxConnectionsPerServer = 10
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
