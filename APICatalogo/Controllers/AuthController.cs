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
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
                           IConfiguration configuration, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Verifica as credenciais do usuário e gera um token JWT se forem válidas.
    /// </summary>
    /// <param name="model">Um objeto do tipo UsuarioDTO.</param>
    /// <returns>Status 200 (OK) e o token para credenciais válidas.</returns>
    /// <remarks>Retorna o Satatus 200 (OK) com o token.</remarks>
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
                new Claim("id", user.UserName!),
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

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// </summary>
    /// <param name="model">Um objeto do tipo UsuarioDTO.</param>
    /// <returns>retorna Status 200 (OK) se o usuário for criado com sucesso.</returns>
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

    /// <summary>
    /// Aplica o refresh token para gerar um novo access token.
    /// </summary>
    /// <param name="token">Um objeto do tipo TokenModel que contém o access token e o refresh token.</param>
    /// <returns>Retorna Status 200 (OK) com o novo access token e refresh token se o refresh for bem-sucedido.</returns>
    /// <exception cref="ArgumentException">Ele é lançado se o token for nulo ou se o access token ou refresh token estiverem ausentes.</exception>
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


    /// <summary>
    /// Revoca o refresh token de um usuário específico.
    /// </summary>
    /// <param name="username">Usename do usuário cujo refresh token será revogado.</param>
    /// <returns>Retorna Status 204 (No Content) se o refresh token for revogado com sucesso.</returns>
    [Authorize(Policy = "ExclusivePolicyOnly")]
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


    /// <summary>
    /// Cria uma nova role (papel) no sistema.
    /// </summary>
    /// <param name="roleName">Constante que representa o nome da role a ser criada.</param>
    /// <returns>
    /// Retorna Status 200 (OK) se a role for criada com sucesso, ou Status 400 (Bad Request) se a role já existir ou ocorrer um erro ao criar a role.
    /// </returns>
    [Authorize(Policy = "SuperAdminOnly")]
    [HttpPost]
    [Route("create-role")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (roleResult.Succeeded)
            {
                _logger.LogInformation(1, "Role criada.");
                return StatusCode(StatusCodes.Status200OK, 
                    new Response
                    {
                        Status = "Success",
                        Message = $"Role {roleName} criada com sucesso!"
                    });
            }
            else
            {
                _logger.LogInformation(2, "Erro.");

                return StatusCode(StatusCodes.Status400BadRequest, 
                    new Response
                    {
                        Status = "Error",
                        Message = $"Erro ao criar role {roleName}!"
                    });
            }

        }

        return StatusCode(StatusCodes.Status400BadRequest, 
            new Response
            {
                Status = "Error",
                Message = "Role já existe!"
            });

    }

    /// <summary>
    /// Adiciona um usuário a uma role (papel) específica.
    /// </summary>
    /// <param name="userEmail">Usário a ser adicionado à role. Deve ser o email do usuário.</param>
    /// <param name="roleName">Nome da role (papel) a ser atribuída ao usuário. Deve ser uma role já existente no sistema.</param>
    /// <returns>
    /// Retorna Status 200 (OK) se o usuário for adicionado à role com sucesso, ou Status 400 (Bad Request) se ocorrer um erro ao adicionar o usuário à role ou se o usuário não for encontrado.
    /// </returns>
    [Authorize(Policy = "SuperAdminOnly")]
    [HttpPost]
    [Route("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is not null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, $"Role {roleName} adicionada ao usuário {user.Email}.");
                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Success",
                    Message = $"Role {roleName} adicionada ao usuário {userEmail} com sucesso!"
                });
            }
            else
            {
                _logger.LogInformation(1, $"Erro ao adicionar role {roleName} ao usuário {userEmail}.");

                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    Status = "Error",
                    Message = $"Erro ao adicionar role {roleName} ao usuário {userEmail}!"
                });
            }
        }

        return BadRequest(new { error = "Usuário não encontrado!" });
    }

}
