using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Preto13;
using Preto13.Config;
using Preto13.Data;
using Preto13.Data.Common;
using Preto13.Data.Member;
using Preto13.Model;
using Preto13.Repository.Common;
using Preto13.Repository.Member;
using Preto13.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddSingleton<DatabaseConnection>();
builder.Services.AddControllers();
builder.Services.AddTransient<HandleData>();
builder.Services.AddTransient<SPHandler>();
builder.Services.AddScoped<iStatus, StatusRepo>();
builder.Services.AddScoped<iMemberAccount, MemberAccountRepo>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapControllerRoute(
    name: "error",
    pattern: "/error",
    defaults: new { controller = "ErrorHandle", action = "Error" }
);
var logger = app.Logger;
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
