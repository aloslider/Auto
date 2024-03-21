using Auto.WebAPI.Database;
using Auto.WebAPI.Database.DbContext;
using Auto.WebAPI.Database.Repositories.Branches;
using Auto.WebAPI.Database.Repositories.Devices;
using Auto.WebAPI.Database.Repositories.Employees;
using Auto.WebAPI.Database.Repositories.Installations;
using Auto.WebAPI.Dtos;
using Auto.WebAPI.Models;
using Auto.WebAPI.Services.Managers.Installtions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalHelpers.OpenApi;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.Configure<DbConfig>(cfg => {
    cfg.ConnectionString = builder.Configuration.GetSection("ConnectionStrings:SqlServer").Value;
});

builder.Services.AddSingleton<IDbContext, SqlServerDbContext>();

builder.Services.AddScoped<IBranchesRepository, BranchesRepository>();
builder.Services.AddScoped<IBranchesManager, BranchesManager>();

builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<IEmployeesManager, EmployeesManager>();

builder.Services.AddScoped<IDevicesRepository, DeviceRepository>();
builder.Services.AddScoped<IDevicesManager, DevicessManager>();

builder.Services.AddScoped<IInstallationRepository, InstallationRepository>();
builder.Services.AddScoped<IInstallationsManager, InstallationManager>();

//builder.Services.AddSingleton<ISessionLineParser, SessionLineParser>();
//builder.Services.AddSingleton<IPrintService, PrintService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddFormFile();
});

#endregion

var app = builder.Build();

#region Endpoints

var routeBuilder = app.MapGroup("api");

#region Branches

var branchesGroup = routeBuilder.MapGroup("branches");

branchesGroup.MapGet("all",
    async Task<Ok<List<Branch>>> (IBranchesManager manager) =>
    {
        var branches = await manager.GetAllAsync();
        return TypedResults.Ok(branches);
    })
.WithTags("Branches")
.WithSummary("Get all branches")
.WithOpenApi();

#endregion

#region Devices

var devicesGroup = routeBuilder.MapGroup("devices");

devicesGroup.MapGet("all",
    async Task<Results<
            Ok<List<Device>>, 
            BadRequest<string>>> 
            (string? connectionType, IDevicesManager manager) =>
    {
        var devices = await manager.GetAllAsync(connectionType);
        return 
            devices.Match<Results<Ok<List<Device>>,BadRequest<string>>>(
                Right: ds => TypedResults.Ok(ds),
                Left: err => TypedResults.BadRequest(err));
    })
.WithTags("Devices")
.WithSummary("Get all devices")
.WithOpenApi(opt =>
{
    opt.Parameters[0].Description = "Connection type";
    return opt;
});

#endregion

#region Employees

var employeesGroup = routeBuilder.MapGroup("employees");

employeesGroup.MapGet("all",
    async Task<Ok<List<Employee>>> (IEmployeesManager manager) =>
    {
        var employees = await manager.GetAllAsync();
        return TypedResults.Ok(employees);
    })
.WithTags("Employees")
.WithSummary("Get all employees")
.WithOpenApi();

#endregion

#region Insallations

var installationsGroup = routeBuilder.MapGroup("installations");

installationsGroup.MapGet("all",
    async Task<Ok<List<Installation>>> (IInstallationsManager manager) =>
    {
        var installations = await manager.GetAllAsync();
        return TypedResults.Ok(installations);
    })
.WithTags("Installations")
.WithSummary("Get all installations")
.WithDescription("Get all installations.")
.WithOpenApi();

installationsGroup.MapGet("{id}",
    async Task<Results<Ok<Installation>, NotFound<string>>> (int id, IInstallationsManager manager) =>
    {
        var installation = await manager.GetByIdAsync(id);
        return 
            installation.Match<Results<Ok<Installation>, NotFound<string>>>(
                Right: i => TypedResults.Ok(i),
                Left: err => TypedResults.NotFound(err));
    })
.WithTags("Installations")
.WithSummary("Get installation by id")
.WithDescription("Get installation by id.")
.WithOpenApi(opt =>
{
    opt.Parameters[0].Description = "Installation id";
    return opt;
});

installationsGroup.MapPost("create",
    async Task<Results<CreatedAtRoute<int>, BadRequest<string>>> 
        (InstallationDto newInstallationDto, IInstallationsManager manager) =>
    {
        var result = await manager.CreateAsync(newInstallationDto);
        return 
            result.Match<Results<CreatedAtRoute<int>, BadRequest<string>>>(
                Right: id => TypedResults.CreatedAtRoute(id, routeName: "CreateInstallation"),
                Left: err => TypedResults.BadRequest(err));
    })
.WithName("CreateInstallation")
.WithTags("Installations")
.WithSummary("Create new installation")
.WithDescription(
    "Create new installation. " +
    "Order number should be uniqe. " +
    "At least one installation in the branch should be set as default.")
.WithOpenApi();

installationsGroup.MapDelete("delete", 
    async Task<Ok> (int id, IInstallationsManager manager) =>
    {
        await manager.DeleteAsync(id);
        return TypedResults.Ok();
    })
.WithTags("Installations")
.WithSummary("Delete specific installation")
.WithDescription(
    "Delete specific installation. " +
    "At least one default installation should exist in the branch.")
.WithOpenApi(opt =>
{
    opt.Parameters[0].Description = "Installation id";
    return opt;
});

#endregion

#endregion

#region Middlewares

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    if (app.Environment.IsDevelopment())
    {
        opt.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
        opt.ConfigObject.AdditionalItems.Add("theme", "agate");
    }
});

app.UseExceptionHandler(app =>
{
    app.Run(async ctx =>
    {
        var exFeature = ctx.Features.Get<IExceptionHandlerPathFeature>();
        var ex = exFeature?.Error;

        if (ex is not null)
        {
            await Results.Problem(detail: ex.Message).ExecuteAsync(ctx);
        }
    });
});

#endregion

await app.RunAsync();