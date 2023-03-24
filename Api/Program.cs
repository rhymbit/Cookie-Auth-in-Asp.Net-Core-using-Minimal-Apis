using Api.Endpoints;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

// ************ Services *****************

var corsDomainsAllowed = builder.Configuration
    .GetSection("CorsDomainsAllowed")
    .AsEnumerable()
    .Select(item => item.Value)
    .Where(item => !string.IsNullOrEmpty(item))
    .ToArray();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins(corsDomainsAllowed)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        var tokenValidationParameters = new JwtService(builder.Configuration).GetTokenValidationParameters(TokenType.AccessToken);
        options.TokenValidationParameters = tokenValidationParameters;
        options.Events = GetJwtBearerEvents();
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<CookieService>();


// ************ WebApplication ****************
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// **************** Endpoints ****************

app.MapGet("/", () =>
{
    var endpointsInfo = new Dictionary<string, string>
    {
        { "https://localhost:7032/login?username=some-name", "To Login & set the tokens in browser cookie." },
        { "https://localhost:7032/refresh-tokens", "To get the new access & refresh tokens. " +
            "This only works after user's already logged in. Won't work if refresh token's expired." },
        { "https://localhost:7032/test-authentication", "Won't work if user's not logged in or tokens are expired." },
        { "https://localhost:7032/clear-cookies", "To clear the cookies." }
    };

    return Results.Ok(endpointsInfo);
});

app.MapLoginEndpoints();

app.MapGet("/test-authentication", (HttpContext httpContext) =>
{
    return Results.Ok("Hi👋🏻, You're Authenticated");
}).RequireAuthorization();


app.Run();


// ************** Extra Methods *****************
JwtBearerEvents GetJwtBearerEvents()
{
    return new()
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue(builder.Configuration["Jwt:AccessToken:Name"], out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
}