namespace Api.Services;

public class CookieService
{
    private IConfiguration _config;

    public CookieService(IConfiguration config) => _config = config;

    public void SetAccessTokenCookie(HttpContext httpContext, string token) =>
        httpContext.Response.Cookies.Append(_config["Jwt:AccessToken:Name"], token, GetCookieOptions());

    public void SetRefreshTokenCookie(HttpContext httpContext, string token) =>
        httpContext.Response.Cookies.Append(_config["Jwt:RefreshToken:Name"], token, GetCookieOptions());

    public string GetRefreshTokenFromCookie(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue(_config["Jwt:RefreshToken:Name"], out var refreshToken))
        {
            return refreshToken;
        }
        return string.Empty;
    }

    private CookieOptions GetCookieOptions()
    {
        return new()
        {
            Domain = _config["CookieOptions:Domain"],
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            //Secure = true,
        };
    }
}
