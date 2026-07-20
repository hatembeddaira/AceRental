using AceRental.Api.Configuration.OData;
using AceRental.Api.Configuration.Swagger;
using AceRental.Api.Extensions;
using AceRental.Domain.Common;
using AceRental.Infrastructure.Persistence;
using AceRental.Infrastructure.Persistence.Repositories;
using AceRental.Infrastructure.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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
    // options.OperationFilter<ODataKeyOperationFilter>();
    // options.DocumentFilter<ODataRouteFilter>();
    options.OperationFilter<ODataQueryOptionsFilter>();
    // // Ce filtre permet de nettoyer les paramètres OData dans Swagger
    // options.ResolveConflictingActions(apiDescriptions =>
    // {
    //     // On essaie de trouver une route qui contient les parenthèses OData
    //     var odataRoute = apiDescriptions.FirstOrDefault(a => a.RelativePath != null && a.RelativePath.Contains("("));

    //     // Si on en trouve une, on la prend, sinon on prend la première disponible
    //     return odataRoute ?? apiDescriptions.First();
    // });

    // // Optionnel : Pour que Swagger comprenne que {key} est dans le path et non en query
    // c.CustomSchemaIds(type => type.FullName);
    options.OperationFilter<SwaggerDefaultValues>();

    // options.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
    // {
    //     Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n " +
    //         "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    //     Name = "Authorization",
    //     In = ParameterLocation.Header,
    //     Type = SecuritySchemeType.ApiKey,
    //     Scheme = "Bearer",
    // });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n " +
            "Enter your token in the text input below.\r\n\r\nExample: \"12345abcdef\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
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
.AddJsonOptions(options =>
    {
        // Ignore les boucles circulaires au lieu de générer une exception
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    })
.AddOData(opt =>
    {
        opt.Count();
        opt.Filter();
        opt.OrderBy();
        opt.Expand();
        opt.Select();
        opt.SetMaxTop(null).Expand();
        opt.RouteOptions.EnableControllerNameCaseInsensitive = true;
        opt.RouteOptions.EnableKeyAsSegment = false;
        opt.RouteOptions.EnableActionNameCaseInsensitive = true;
        opt.RouteOptions.EnablePropertyNameCaseInsensitive = true;
        opt.EnableNoDollarQueryOptions = false;
        opt.RouteOptions.EnableKeyInParenthesis = true;
        opt.RouteOptions.EnableNonParenthesisForEmptyParameterFunction = true;
        opt.RouteOptions.EnableQualifiedOperationCall = false;
        opt.RouteOptions.EnableUnqualifiedOperationCall = true;
        // On passe la version dynamiquement au MasterModel
        // opt.AddRouteComponents("api/v{version:apiVersion}", ODataMainConfiguration.GetMasterEdmModel(new ApiVersion(1, 0)));
        opt.AddRouteComponents("api/v1", ODataMainConfiguration.GetMasterEdmModel(new ApiVersion(1, 0)));
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.Policies.Sunset(0.9)
                    .Effective(DateTimeOffset.Now.AddDays(60))
                    .Link("policy.html")
                        .Title("Versioning Policy")
                        .Type("text/html");
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // IMPORTANT : Dans un environnement conteneurisé (Docker), l'API (acerental-api) doit appeler
        // IdentityServer via son nom de service ('identityserver'), et non 'localhost'.
        options.Authority = "http://identityserver:5001";
        options.Audience = "api"; // L'audience de votre API, définie dans IdentityServer
        options.RequireHttpsMetadata = false; // À définir à true en production

        // La configuration manuelle du ConfigurationManager est nécessaire pour désactiver HTTPS en développement DANS Docker.
        options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            options.Authority + "/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever(),
            new HttpDocumentRetriever { RequireHttps = false });
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            // L'émetteur (issuer) est l'URL publique d'IdentityServer, celle que le client Angular utilise.
            ValidIssuer = "http://localhost:5001",
            ValidateAudience = false, // Désactivé pour simplifier le test
            ValidateLifetime = true
        };
        
        // ASTUCE : Ajoute ces events pour voir L'ERREUR EXACTE dans tes logs Docker
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Auth failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IReservationEquipmentsRepository, ReservationEquipmentsRepository>();


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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // Ceci applique les migrations automatiquement au démarrage
}

app.Run();
