var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCors();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist/revue-rem-frontend/browser";
});
var reverseProxySection = builder.Configuration.GetSection("RemFrontendReverseProxy");

builder.Services.AddReverseProxy().LoadFromConfig(reverseProxySection);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors();

// Map YARP proxy for API routes
app.MapReverseProxy();

app.UseStaticFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.MapWhen(context => !context.Request.Path.Value!.StartsWith("/api", StringComparison.OrdinalIgnoreCase), config =>
{
    config.UseSpa(spa =>
    {
        spa.Options.SourcePath = @"ClientApp";
        if (app.Environment.IsDevelopment())
        {
            spa.UseProxyToSpaDevelopmentServer(builder.Configuration["FrontUrl"]!);
        }
    });
});

app.Run();