using System.Reflection;
using Auto.WebAPI.Database.Models;
using Auto.WebAPI.Features.Installations;
using Auto.WebAPI.Features.PrintTasks;
using Auto.WebAPI.Services;
using Auto.WebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MinimalHelpers.OpenApi;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CompanyDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:SqlServer").Value);
});
builder.Services.AddOptions<InstallationsOptions>().BindConfiguration("Installations");
builder.Services.AddOptions<CsvFileParsingOptions>().BindConfiguration("CsvFileParsing");
builder.Services.AddSingleton<ISessionLineParser, SessionLineParser>();
builder.Services.AddSingleton<IPrintService, PrintService>();

builder.Services.AddMinimalEndpoints();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddFormFile();
});

var app = builder.Build();

app.RegisterMinimalEndpoints(routePrefix: "api"); 

app.UseSwagger();
app.UseSwaggerUI();

app.Run(); 