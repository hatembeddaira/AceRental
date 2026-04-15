using AceRental.Api.Configuration.Swagger;
using AceRental.Api.Extensions;
using AceRental.Domain.Common;
using AceRental.Infrastructure.Persistence;
using AceRental.Infrastructure.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AceRental.Application.AssemblyReference).Assembly);
});



builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    // // c.SwaggerDoc("v1", new OpenApiInfo { Title = "AceRental API", Version = "v1" });

    // // Indispensable pour éviter que Swagger ne liste "key" deux fois
    options.OperationFilter<ODataKeyOperationFilter>();
    // c.DocumentFilter<ODataRouteFilter>();
    options.OperationFilter<ODataQueryOptionsFilter>();
    // // Ce filtre permet de nettoyer les paramètres OData dans Swagger
    options.ResolveConflictingActions(apiDescriptions =>
    {
        // On essaie de trouver une route qui contient les parenthèses OData
        var odataRoute = apiDescriptions.FirstOrDefault(a => a.RelativePath != null && a.RelativePath.Contains("("));

        // Si on en trouve une, on la prend, sinon on prend la première disponible
        return odataRoute ?? apiDescriptions.First();
    });

    // // Optionnel : Pour que Swagger comprenne que {key} est dans le path et non en query
    // c.CustomSchemaIds(type => type.FullName);
    options.OperationFilter<SwaggerDefaultValues>();

    options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    // options.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             // Reference = new openapireference
    //             // {
    //             //     Type = ReferenceType.SecurityScheme,
    //             //     Id = "JWT"
    //             // },
    //             Scheme = "Bearer",
    //             Name = "Bearer",
    //             In = ParameterLocation.Header,
    //         },
    //         System.Array.Empty<string>()
    //     }
    // });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("alloworigins", builder =>
    {
        // TODO: restreindre aux origines autorisées en production
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddProblemDetails();
// builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(AceRental.Application.AssemblyReference.Assembly);
builder.Services.AddControllers()
.AddOData(opt =>
    {
        opt.Count();
        opt.Filter();
        opt.OrderBy();
        opt.Expand();
        opt.Select();
        opt.SetMaxTop(null);
        opt.RouteOptions.EnableControllerNameCaseInsensitive = true;
        opt.RouteOptions.EnableKeyAsSegment = false;
        opt.RouteOptions.EnableActionNameCaseInsensitive = true;
        opt.RouteOptions.EnablePropertyNameCaseInsensitive = true;
        opt.EnableNoDollarQueryOptions = false;
        opt.RouteOptions.EnableKeyInParenthesis = true;
        opt.RouteOptions.EnableNonParenthesisForEmptyParameterFunction = true;
        opt.RouteOptions.EnableQualifiedOperationCall = false;
        opt.RouteOptions.EnableUnqualifiedOperationCall = true;
        // opt.AddRouteComponents("api/v1", ODataMainConfiguration.GetMasterEdmModel());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddOData(op =>
{
    op.ModelBuilder.ModelBuilderFactory = () => new ODataConventionModelBuilder();
    // op.AddRouteComponents("api");
    op.AddRouteComponents("api/v{version:apiVersion}");
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
// .AddOData(options => options
//     .Select()
//     .Filter()
//     .OrderBy()
//     .Expand()
//     .Count()
//     .SetMaxTop(100)

//     .AddRouteComponents("api/v1", ODataMainConfiguration.GetMasterEdmModel()));
var app = builder.Build();

app.UseGlobalExceptionHandler();
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHttpsRedirection();
app.UseCors("alloworigins");
app.UseRouting();
// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // Ceci applique les migrations automatiquement au démarrage
}

app.Run();


