using Preto13.Config;
using Preto13.Data;
using Preto13.Data.iRepo.Common;
using Preto13.Data.Repository.Common;
using Preto13.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DatabaseConnection>();
builder.Services.AddControllers();
builder.Services.AddTransient<HandleData>();
builder.Services.AddTransient<SPHandler>();
builder.Services.AddTransient<iStatus, StatusRepo>();
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
