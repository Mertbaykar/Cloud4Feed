using Cloud4Feed.Application.Registration;
using Cloud4Feed.Application.BackgroundTask.Startup;
using Cloud4Feed.Application.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
bool autoMigrate = builder.Configuration.GetValue("automigrate", false);
string connString = builder.Configuration.GetConnectionString("ecommerceConnection")!;

builder.Services.RegisterECommerce(connString, autoMigrate);
builder.Services.AddScoped<BasicAuthenticationFilter>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
