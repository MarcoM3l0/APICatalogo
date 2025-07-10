using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
                           IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username!);

        if(user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _tokenService.GenerateAcesseToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["Jwt:RefreshTokenExpirationDays"], out int refreshTokenExpirationDays);

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);

            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                RefreshToken = refreshToken
            });
        }

        return Unauthorized("Usuário ou senha inválidos!");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username!);

        if (userExists is not null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new Response
                {
                    Status = "Error",
                    Message = "Usuário já existe!"
                });
        }

        ApplicationUser user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
        };

        var result = await _userManager.CreateAsync(user, model.Password!);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new Response
                {
                    Status = "Error",
                    Message = "Erro ao criar usuário!"
                });
        }

        return Ok(new Response
        {
            Status = "Success",
            Message = "Usuário criado com sucesso!"
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenModel token)
    {
        if (token is null)
        {
            return BadRequest("solicitação inválida do cliente!");
        }

        string? accessToken = token.AccessToken ?? throw new ArgumentException(nameof(token));
        string? refreshToken = token.RefreshToken ?? throw new ArgumentException(nameof(token));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal is null)
        {
            return BadRequest("Token inválido ou expirado!");
        }

        string? userName = principal.Identity?.Name;

        var user = await _userManager.FindByNameAsync(userName!);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Token inválido ou expirado!");
        } 

        var newAccessToken = _tokenService.GenerateAcesseToken(principal.Claims, _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        await _userManager.UpdateAsync(user);

        return Ok(new TokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return BadRequest("Nome de usuário inválido!");
        }

        user.RefreshToken = null;
        
        await _userManager.UpdateAsync(user);

        return NoContent();
    }

}
