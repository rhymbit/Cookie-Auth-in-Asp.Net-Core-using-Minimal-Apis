using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public class JwtService
{
    private IConfiguration _config;
    private SymmetricSecurityKey _issuerSigningKeyAccessToken;
    private SymmetricSecurityKey _issuerSigningKeyRefreshToken;
    public JwtService(IConfiguration configuration)
    {
        _config = configuration;
        _issuerSigningKeyAccessToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:AccessToken:SigningKey"]));
        _issuerSigningKeyRefreshToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:RefreshToken:SigningKey"]));
    }

    public string GenerateToken(TokenType tokenType, string username, DateTime expires)
    {
        var issuer = tokenType switch
        {
            TokenType.AccessToken => _config["Jwt:AccessToken:Issuer"],
            TokenType.RefreshToken => _config["Jwt:RefreshToken:Issuer"],
            _ => throw new NotImplementedException(),
        };

        var audience = tokenType switch
        {
            TokenType.AccessToken => _config["Jwt:AccessToken:Audience"],
            TokenType.RefreshToken => _config["Jwt:RefreshToken:Audience"],
            _ => throw new NotImplementedException(),
        };

        var signingKey = tokenType == TokenType.AccessToken
            ? _issuerSigningKeyAccessToken
            : _issuerSigningKeyRefreshToken;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, username)
        };

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return encodedToken;
    }

    public (ClaimsPrincipal?, string?) GetClaimsPrincipalFromToken(TokenType tokenType, string? token)
    {
        var tokenValidationParameters = GetTokenValidationParameters(tokenType);
        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var validatedToken);
            return (principal, null);
        }
        catch (Exception ex)
        {
            return (null, $"Token Validation Failed :- {ex.Message}");
        }
    }

    public string? GetNameFromPrincipal(ClaimsPrincipal principal)
    {
        var username = principal.FindFirstValue(JwtRegisteredClaimNames.Name);
        return username;
    }

    public TokenValidationParameters GetTokenValidationParameters(TokenType tokenType)
    {
        var validIssuer = tokenType switch
        {
            TokenType.AccessToken => _config["Jwt:AccessToken:Issuer"],
            TokenType.RefreshToken => _config["Jwt:RefreshToken:Issuer"],
            _ => throw new NotImplementedException(),
        };

        var validAudience = tokenType switch
        {
            TokenType.AccessToken => _config["Jwt:AccessToken:Audience"],
            TokenType.RefreshToken => _config["Jwt:RefreshToken:Audience"],
            _ => throw new NotImplementedException(),
        };

        var signingKey = tokenType == TokenType.AccessToken
            ? _issuerSigningKeyAccessToken
            : _issuerSigningKeyRefreshToken;

        return new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = signingKey
        };
    }
}

public enum TokenType
{
    AccessToken,
    RefreshToken,
}
