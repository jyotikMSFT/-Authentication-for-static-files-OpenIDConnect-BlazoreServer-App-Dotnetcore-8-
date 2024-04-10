using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
/*app.MapWhen(context => context.Request.Path.StartsWithSegments("/css"), appBuilder =>
{
    _ = appBuilder.Use(async (context, next) =>
    {
        // Check if the user is authenticated
        if (!context.User.Identity.IsAuthenticated)
        {
            // User is not authenticated, return 401 Unauthorized
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // User is authenticated, proceed to the next middleware
        await next();
    });
});

app.MapWhen(context => context.Request.Path.StartsWithSegments("/_content"), appBuilder =>
{
    _ = appBuilder.Use(async (context, next) =>
    {
        // Check if the user is authenticated
        if (!context.User.Identity.IsAuthenticated)
        {
            // User is not authenticated, return 401 Unauthorized
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // User is authenticated, proceed to the next middleware
        await next();
    });
});

app.MapWhen(context => context.Request.Path.StartsWithSegments("/_framework"), appBuilder =>
{
    _ = appBuilder.Use(async (context, next) =>
    {
        // Check if the user is authenticated
        if (!context.User.Identity.IsAuthenticated)
        {
            // User is not authenticated, return 401 Unauthorized
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // User is authenticated, proceed to the next middleware
        await next();
    });
});
*/
app.UseRouting();

app.UseAuthentication();
var defaultPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
app.UseStaticFiles(); 
app.UseAuthorization();
app.MapRazorPages()
    .RequireAuthorization(); // Apply the authorization policy to Razor Pages

app.MapControllers()
    .RequireAuthorization(); // Apply the authorization policy to Controllers

app.Run();

