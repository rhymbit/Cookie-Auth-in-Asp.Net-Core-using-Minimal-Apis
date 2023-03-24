using Api.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Endpoints;

public static class Login
{
    private static DateTime _accessTokenExpirationTime = DateTime.Now.AddSeconds(2000);
    private static DateTime _refreshTokenExpirationTime = DateTime.Now.AddSeconds(3000);

    public static WebApplication MapLoginEndpoints(this WebApplication app)
    {
        app.MapGet("/login", LoginUser)
            .AllowAnonymous()
            .WithName("Login endpoint for users");

        app.MapGet("/refresh-tokens", GetNewTokens)
            .AllowAnonymous()
            .WithName("Get new access & refresh tokens");

        app.MapGet("/clear-cookies", ClearCookies)
            .WithName("Clear Cookies");

        return app;
    }

    public static IResult ClearCookies(HttpContext httpContext, CookieService cookieService)
    {
        cookieService.SetAccessTokenCookie(httpContext, "");
        cookieService.SetRefreshTokenCookie(httpContext, "");
        return Results.Ok("Cookies have been set to empty strings");
    }

    public static IResult LoginUser(
        HttpContext httpContext,
        IConfiguration configuration,
        JwtService jwtService,
        CookieService cookieService,
        string username)
    {

        if (string.IsNullOrEmpty(username))
        {
            return Results.BadRequest("Username is required");
        }

        string accessToken = GetAccessToken(jwtService, username);
        string refreshToken = GetRefreshToken(jwtService, username);

        cookieService.SetAccessTokenCookie(httpContext, accessToken);
        cookieService.SetRefreshTokenCookie(httpContext, refreshToken);

        return Results.Ok("User Logged In. Check in browser's Cookie Storage for Tokens");
    }

    public static IResult GetNewTokens(
        HttpContext httpContext,
        JwtService jwtService,
        CookieService cookieService)
    {
        var refreshTokenReceived = cookieService.GetRefreshTokenFromCookie(httpContext);
        var (principal, exceptionMessage) = jwtService.GetClaimsPrincipalFromToken(TokenType.RefreshToken, refreshTokenReceived);
        if (exceptionMessage is not null || principal is null)
        {
            return Results.Json(new
                {
                    message = exceptionMessage
                },
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        var username = jwtService.GetNameFromPrincipal(principal);
        if (username is null)
        {
            return Results.Json(new
                {
                    message = $"Could not extract {JwtRegisteredClaimNames.Name} from token"
                },
                statusCode: StatusCodes.Status401Unauthorized
            );
        }

        string newAccessToken = GetAccessToken(jwtService, username);
        string newRefreshToken = GetRefreshToken(jwtService, username);

        cookieService.SetAccessTokenCookie(httpContext, newAccessToken);
        cookieService.SetRefreshTokenCookie(httpContext, newRefreshToken);

        return Results.Ok("New Tokens Generated. Check in browser's Cookie Storage for new Tokens");
    }


    // ********* Not Endpoints *********
    private static string GetAccessToken(JwtService jwtService, string username)
    {
        return jwtService.GenerateToken(
            TokenType.AccessToken,
            username,
            _accessTokenExpirationTime
        );
    }

    private static string GetRefreshToken(JwtService jwtService, string username)
    {
        return jwtService.GenerateToken(
            TokenType.RefreshToken,
            username,
            _refreshTokenExpirationTime
        );
    }
}
