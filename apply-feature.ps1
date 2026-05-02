# apply-feature.ps1
# Run this from the ROOT of your local repos folder (where backend/ and frontend/ folders are).
# Usage: .\apply-feature.ps1
#
# What it does:
#   1. Creates branch feature/identity-and-cart in the backend repo
#   2. Copies all new Identity module files
#   3. Commits and pushes
#   4. Creates branch feature/identity-and-cart in the frontend repo
#   5. Copies all new cart/auth files
#   6. Commits and pushes

param(
    [string]$BackendPath  = ".\RedAndWhite-business",
    [string]$FrontendPath = ".\RedAndWhite-business-frontend",
    [string]$Branch       = "feature/identity-and-cart"
)

$ErrorActionPreference = "Stop"

function Write-Step($msg) { Write-Host "`n>> $msg" -ForegroundColor Cyan }
function Write-Ok($msg)   { Write-Host "   OK: $msg" -ForegroundColor Green }

# ─── BACKEND ────────────────────────────────────────────────────────────────
Write-Step "Backend: creating branch $Branch"
Push-Location $BackendPath

git checkout main
git pull
git checkout -b $Branch

# ── Identity Domain ──────────────────────────────────────────────────────────
$base = "src\Modules\Identity"
New-Item -ItemType Directory -Force "$base\RedAndWhite.Identity.Domain" | Out-Null
New-Item -ItemType Directory -Force "$base\RedAndWhite.Identity.Application\Commands" | Out-Null
New-Item -ItemType Directory -Force "$base\RedAndWhite.Identity.Application\Queries" | Out-Null
New-Item -ItemType Directory -Force "$base\RedAndWhite.Identity.Application\Dtos" | Out-Null
New-Item -ItemType Directory -Force "$base\RedAndWhite.Identity.Application\Abstractions" | Out-Null
New-Item -ItemType Directory -Force "$base\RedAndWhite.Identity.Infrastructure" | Out-Null
New-Item -ItemType Directory -Force "$base\RedAndWhite.Identity.Api\Controllers" | Out-Null

# AppUser.cs
@'
using Microsoft.AspNetCore.Identity;

namespace RedAndWhite.Identity.Domain;

public sealed class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? DniOrCuit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string FullName => $"{FirstName} {LastName}";
}
'@ | Set-Content "$base\RedAndWhite.Identity.Domain\AppUser.cs" -Encoding UTF8

# RefreshToken.cs
@'
namespace RedAndWhite.Identity.Domain;

public sealed class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}
'@ | Set-Content "$base\RedAndWhite.Identity.Domain\RefreshToken.cs" -Encoding UTF8

# Identity.Domain.csproj
@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="../../../Shared/RedAndWhite.Shared.Kernel/RedAndWhite.Shared.Kernel.csproj" />
  </ItemGroup>
</Project>
'@ | Set-Content "$base\RedAndWhite.Identity.Domain\RedAndWhite.Identity.Domain.csproj" -Encoding UTF8

Write-Ok "Identity.Domain"

# AuthDtos.cs
@'
namespace RedAndWhite.Identity.Application.Dtos;

public sealed record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiry,
    UserDto User);

public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName);
'@ | Set-Content "$base\RedAndWhite.Identity.Application\Dtos\AuthDtos.cs" -Encoding UTF8

# IJwtService.cs
@'
using RedAndWhite.Identity.Domain;

namespace RedAndWhite.Identity.Application.Abstractions;

public interface IJwtService
{
    string GenerateAccessToken(AppUser user, IList<string> roles);
    RefreshToken GenerateRefreshToken(Guid userId);
}
'@ | Set-Content "$base\RedAndWhite.Identity.Application\Abstractions\IJwtService.cs" -Encoding UTF8

# Register.cs
@'
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RedAndWhite.Identity.Application.Abstractions;
using RedAndWhite.Identity.Application.Dtos;
using RedAndWhite.Identity.Domain;
using RedAndWhite.Shared.Kernel;

namespace RedAndWhite.Identity.Application.Commands;

public sealed record RegisterCommand(
    string Email, string Password, string FirstName, string LastName
) : IRequest<Result<AuthResponseDto>>;

public sealed class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6)
            .WithMessage("La contrasena debe tener al menos 6 caracteres.");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}

internal sealed class RegisterHandler(UserManager<AppUser> userManager, IJwtService jwt)
    : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(RegisterCommand cmd, CancellationToken ct)
    {
        if (await userManager.FindByEmailAsync(cmd.Email) is not null)
            return Result<AuthResponseDto>.Failure("Ya existe una cuenta con ese email.");

        var user = new AppUser { UserName = cmd.Email, Email = cmd.Email, FirstName = cmd.FirstName, LastName = cmd.LastName };
        var result = await userManager.CreateAsync(user, cmd.Password);
        if (!result.Succeeded)
            return Result<AuthResponseDto>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, "customer");
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = jwt.GenerateAccessToken(user, roles);
        var refresh = jwt.GenerateRefreshToken(user.Id);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(
            accessToken, refresh.Token, DateTime.UtcNow.AddMinutes(15),
            new UserDto(user.Id, user.Email!, user.FirstName, user.LastName, user.FullName)));
    }
}
'@ | Set-Content "$base\RedAndWhite.Identity.Application\Commands\Register.cs" -Encoding UTF8

# Login.cs
@'
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RedAndWhite.Identity.Application.Abstractions;
using RedAndWhite.Identity.Application.Dtos;
using RedAndWhite.Identity.Domain;
using RedAndWhite.Identity.Infrastructure;
using RedAndWhite.Shared.Kernel;

namespace RedAndWhite.Identity.Application.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

internal sealed class LoginHandler(UserManager<AppUser> userManager, IJwtService jwt, IdentityDbContext db)
    : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(cmd.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, cmd.Password))
            return Result<AuthResponseDto>.Failure("Email o contrasena incorrectos.");

        var old = await db.RefreshTokens.Where(r => r.UserId == user.Id && !r.IsRevoked).ToListAsync(ct);
        old.ForEach(r => r.IsRevoked = true);

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = jwt.GenerateAccessToken(user, roles);
        var refresh = jwt.GenerateRefreshToken(user.Id);
        db.RefreshTokens.Add(refresh);
        await db.SaveChangesAsync(ct);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(
            accessToken, refresh.Token, DateTime.UtcNow.AddMinutes(15),
            new UserDto(user.Id, user.Email!, user.FirstName, user.LastName, user.FullName)));
    }
}
'@ | Set-Content "$base\RedAndWhite.Identity.Application\Commands\Login.cs" -Encoding UTF8

# RefreshToken.cs
@'
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RedAndWhite.Identity.Application.Abstractions;
using RedAndWhite.Identity.Application.Dtos;
using RedAndWhite.Identity.Domain;
using RedAndWhite.Identity.Infrastructure;
using RedAndWhite.Shared.Kernel;

namespace RedAndWhite.Identity.Application.Commands;

public sealed record RefreshTokenCommand(string Token) : IRequest<Result<AuthResponseDto>>;

internal sealed class RefreshTokenHandler(IdentityDbContext db, UserManager<AppUser> userManager, IJwtService jwt)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand cmd, CancellationToken ct)
    {
        var stored = await db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == cmd.Token, ct);
        if (stored is null || !stored.IsActive)
            return Result<AuthResponseDto>.Failure("Refresh token invalido o expirado.");

        var user = await userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null) return Result<AuthResponseDto>.Failure("Usuario no encontrado.");

        stored.IsRevoked = true;
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = jwt.GenerateAccessToken(user, roles);
        var newRefresh = jwt.GenerateRefreshToken(user.Id);
        db.RefreshTokens.Add(newRefresh);
        await db.SaveChangesAsync(ct);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(
            accessToken, newRefresh.Token, DateTime.UtcNow.AddMinutes(15),
            new UserDto(user.Id, user.Email!, user.FirstName, user.LastName, user.FullName)));
    }
}
'@ | Set-Content "$base\RedAndWhite.Identity.Application\Commands\RefreshToken.cs" -Encoding UTF8

# GetCurrentUser.cs
@'
using MediatR;
using Microsoft.AspNetCore.Identity;
using RedAndWhite.Identity.Application.Dtos;
using RedAndWhite.Identity.Domain;
using RedAndWhite.Shared.Kernel;

namespace RedAndWhite.Identity.Application.Queries;

public sealed record GetCurrentUserQuery(Guid UserId) : IRequest<Result<UserDto>>;

internal sealed class GetCurrentUserHandler(UserManager<AppUser> userManager)
    : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetCurrentUserQuery q, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(q.UserId.ToString());
        if (user is null) return Result<UserDto>.Failure("Usuario no encontrado.");
        return Result<UserDto>.Success(new UserDto(user.Id, user.Email!, user.FirstName, user.LastName, user.FullName));
    }
}
'@ | Set-Content "$base\RedAndWhite.Identity.Application\Queries\GetCurrentUser.cs" -Encoding UTF8

# Identity.Application.csproj
@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="MediatR" Version="12.2.0" />
    <PackageReference Include="FluentValidation" Version="11.8.0" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="../../../Shared/RedAndWhite.Shared.Kernel/RedAndWhite.Shared.Kernel.csproj" />
    <ProjectReference Include="../RedAndWhite.Identity.Domain/RedAndWhite.Identity.Domain.csproj" />
    <ProjectReference Include="../RedAndWhite.Identity.Infrastructure/RedAndWhite.Identity.Infrastructure.csproj" />
  </ItemGroup>
</Project>
'@ | Set-Content "$base\RedAndWhite.Identity.Application\RedAndWhite.Identity.Application.csproj" -Encoding UTF8

Write-Ok "Identity.Application"

# IdentityDbContext.cs
@'
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedAndWhite.Identity.Domain;

namespace RedAndWhite.Identity.Infrastructure;

public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    public const string Schema = "identity";
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.HasDefaultSchema(Schema);
        b.Entity<RefreshToken>(e => {
            e.ToTable("refresh_tokens");
            e.HasKey(x => x.Id);
            e.Property(x => x.Token).HasMaxLength(200).IsRequired();
            e.HasIndex(x => x.Token).IsUnique();
        });
        b.Entity<AppUser>().ToTable("users");
        b.Entity<IdentityRole<Guid>>().ToTable("roles");
        b.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
        b.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        b.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        b.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        b.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
    }
}
'@ | Set-Content "$base\RedAndWhite.Identity.Infrastructure\IdentityDbContext.cs" -Encoding UTF8

# JwtService.cs
@'
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RedAndWhite.Identity.Application.Abstractions;
using RedAndWhite.Identity.Domain;

namespace RedAndWhite.Identity.Infrastructure;

internal sealed class JwtService(IConfiguration config) : IJwtService
{
    public string GenerateAccessToken(AppUser user, IList<string> roles)
    {
        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!));
        var minutes = int.Parse(config["Jwt:AccessTokenMinutes"] ?? "15");
        var claims  = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
            new("firstName",                   user.FirstName),
            new("lastName",                    user.LastName),
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        var token = new JwtSecurityToken(
            issuer:             config["Jwt:Issuer"],
            audience:           config["Jwt:Audience"],
            claims:             claims,
            expires:            DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(Guid userId) => new()
    {
        UserId    = userId,
        Token     = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(config["Jwt:RefreshTokenDays"] ?? "7")),
    };
}
'@ | Set-Content "$base\RedAndWhite.Identity.Infrastructure\JwtService.cs" -Encoding UTF8

# IdentitySeeder.cs
@'
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Identity.Domain;

namespace RedAndWhite.Identity.Infrastructure;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        using var scope   = sp.CreateScope();
        var roleManager   = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager   = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var db            = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        await db.Database.MigrateAsync();
        foreach (var role in new[] { "customer", "admin" })
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        const string adminEmail = "admin@redandwhite.com.ar";
        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new AppUser { UserName = adminEmail, Email = adminEmail, FirstName = "Admin", LastName = "Red & White", EmailConfirmed = true };
            await userManager.CreateAsync(admin, "Admin1234!");
            await userManager.AddToRoleAsync(admin, "admin");
        }
    }
}
'@ | Set-Content "$base\RedAndWhite.Identity.Infrastructure\IdentitySeeder.cs" -Encoding UTF8

# Identity.Infrastructure.csproj
@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.0" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.3.2" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="../../../Shared/RedAndWhite.Shared.Infrastructure/RedAndWhite.Shared.Infrastructure.csproj" />
    <ProjectReference Include="../RedAndWhite.Identity.Domain/RedAndWhite.Identity.Domain.csproj" />
  </ItemGroup>
</Project>
'@ | Set-Content "$base\RedAndWhite.Identity.Infrastructure\RedAndWhite.Identity.Infrastructure.csproj" -Encoding UTF8

Write-Ok "Identity.Infrastructure"

# AuthController.cs
@'
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Identity.Application.Commands;
using RedAndWhite.Identity.Application.Dtos;
using RedAndWhite.Identity.Application.Queries;
using System.Security.Claims;

namespace RedAndWhite.Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(ISender mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new { error = result.Error });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest body, CancellationToken ct)
    {
        var result = await mediator.Send(new RefreshTokenCommand(body.Token), ct);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new { error = result.Error });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();
        var result = await mediator.Send(new GetCurrentUserQuery(userId), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    public sealed record RefreshTokenRequest(string Token);
}
'@ | Set-Content "$base\RedAndWhite.Identity.Api\Controllers\AuthController.cs" -Encoding UTF8

# IdentityModule.cs
@'
using System.Text;
using FluentValidation;
using FluentValidation.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RedAndWhite.Identity.Application.Abstractions;
using RedAndWhite.Identity.Application.Commands;
using RedAndWhite.Identity.Domain;
using RedAndWhite.Identity.Infrastructure;
using RedAndWhite.Shared.Kernel;

namespace RedAndWhite.Identity.Api;

public sealed class IdentityModule : IModule
{
    public string Name => "Identity";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var conn   = configuration.GetConnectionString("Postgres")!;
        var secret = configuration["Jwt:Secret"]!;

        services.AddDbContext<IdentityDbContext>(o =>
            o.UseNpgsql(conn, npg => npg.MigrationsHistoryTable("__ef_migrations_history", IdentityDbContext.Schema)));

        services.AddIdentity<AppUser, IdentityRole<Guid>>(o =>
        {
            o.Password.RequiredLength        = 6;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireUppercase       = false;
            o.Password.RequireDigit           = false;
            o.User.RequireUniqueEmail         = true;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = configuration["Jwt:Issuer"],
                ValidAudience            = configuration["Jwt:Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ClockSkew                = TimeSpan.Zero,
            };
        });

        services.AddScoped<IJwtService, JwtService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterCommand>());
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
    }

    public void RegisterEndpoints(object app) { }
}
'@ | Set-Content "$base\RedAndWhite.Identity.Api\IdentityModule.cs" -Encoding UTF8

# Identity.Api.csproj
@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="MediatR" Version="12.2.0" />
    <PackageReference Include="FluentValidation" Version="11.8.0" />
    <PackageReference Include="FluentValidation.DependencyInjection" Version="11.8.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.0" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="../../../Shared/RedAndWhite.Shared.Kernel/RedAndWhite.Shared.Kernel.csproj" />
    <ProjectReference Include="../RedAndWhite.Identity.Application/RedAndWhite.Identity.Application.csproj" />
    <ProjectReference Include="../RedAndWhite.Identity.Infrastructure/RedAndWhite.Identity.Infrastructure.csproj" />
  </ItemGroup>
</Project>
'@ | Set-Content "$base\RedAndWhite.Identity.Api\RedAndWhite.Identity.Api.csproj" -Encoding UTF8

Write-Ok "Identity.Api"

# ── Update Program.cs ────────────────────────────────────────────────────────
@'
using FluentValidation;
using MediatR;
using RedAndWhite.Api.Middleware;
using RedAndWhite.Catalog.Api;
using RedAndWhite.Identity.Api;
using RedAndWhite.Identity.Infrastructure;
using RedAndWhite.Shared.Infrastructure.Behaviors;
using RedAndWhite.Shared.Kernel;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration).WriteTo.Console());

IModule[] modules = [new CatalogModule(), new IdentityModule()];
foreach (var m in modules) m.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers()
    .AddApplicationPart(typeof(CatalogModule).Assembly)
    .AddApplicationPart(typeof(IdentityModule).Assembly);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization", Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer", BearerFormat = "JWT", In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {{
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
        }, []
    }});
});

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
    .WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
    .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

builder.Services.AddAuthorization(o =>
    o.AddPolicy("Admin", p => p.RequireAuthenticatedUser().RequireRole("admin")));

var app = builder.Build();

await IdentitySeeder.SeedAsync(app.Services);

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
foreach (var m in modules) m.RegisterEndpoints(app);
app.Run();
'@ | Set-Content "src\Api\Program.cs" -Encoding UTF8

Write-Ok "Program.cs updated"

# ── Update Api.csproj to reference Identity.Api ──────────────────────────────
@'
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="MediatR" Version="12.2.0" />
    <PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
    <PackageReference Include="Serilog.AspNetCore" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="../Shared/RedAndWhite.Shared.Kernel/RedAndWhite.Shared.Kernel.csproj" />
    <ProjectReference Include="../Shared/RedAndWhite.Shared.Infrastructure/RedAndWhite.Shared.Infrastructure.csproj" />
    <ProjectReference Include="../Modules/Catalog/RedAndWhite.Catalog.Api/RedAndWhite.Catalog.Api.csproj" />
    <ProjectReference Include="../Modules/Identity/RedAndWhite.Identity.Api/RedAndWhite.Identity.Api.csproj" />
  </ItemGroup>
</Project>
'@ | Set-Content "src\Api\RedAndWhite.Api.csproj" -Encoding UTF8

Write-Ok "RedAndWhite.Api.csproj updated"

# ── Add Identity projects to .sln ───────────────────────────────────────────
$slnAddition = @"

Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7558}") = "RedAndWhite.Identity.Domain", "src\Modules\Identity\RedAndWhite.Identity.Domain\RedAndWhite.Identity.Domain.csproj", "{EE000001-0000-0000-0000-000000000001}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7558}") = "RedAndWhite.Identity.Application", "src\Modules\Identity\RedAndWhite.Identity.Application\RedAndWhite.Identity.Application.csproj", "{EE000001-0000-0000-0000-000000000002}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7558}") = "RedAndWhite.Identity.Infrastructure", "src\Modules\Identity\RedAndWhite.Identity.Infrastructure\RedAndWhite.Identity.Infrastructure.csproj", "{EE000001-0000-0000-0000-000000000003}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7558}") = "RedAndWhite.Identity.Api", "src\Modules\Identity\RedAndWhite.Identity.Api\RedAndWhite.Identity.Api.csproj", "{EE000001-0000-0000-0000-000000000004}"
EndProject
"@

$sln = Get-Content "RedAndWhite.sln" -Raw
if ($sln -notmatch "RedAndWhite.Identity") {
    $sln = $sln -replace "Global", "$slnAddition`nGlobal"
    Set-Content "RedAndWhite.sln" $sln -Encoding UTF8
    Write-Ok "RedAndWhite.sln updated"
} else {
    Write-Ok "RedAndWhite.sln already has Identity entries"
}

# ── Commit & push backend ────────────────────────────────────────────────────
git add .
git commit -m "feat: add Identity module (JWT auth, register, login, refresh)"
git push origin $Branch

Write-Ok "Backend pushed to branch $Branch"
Pop-Location

# ─── FRONTEND ────────────────────────────────────────────────────────────────
Write-Step "Frontend: creating branch $Branch"
Push-Location $FrontendPath

git checkout main
git pull
git checkout -b $Branch

# auth-store.ts
New-Item -ItemType Directory -Force "lib" | Out-Null
@'
"use client";
import { create } from "zustand";
import { persist } from "zustand/middleware";

type User = { id: string; email: string; firstName: string; lastName: string; fullName: string };

type AuthStore = {
  user: User | null;
  accessToken: string | null;
  refreshToken: string | null;
  setAuth: (user: User, accessToken: string, refreshToken: string) => void;
  clearAuth: () => void;
  isLoggedIn: () => boolean;
};

export const useAuthStore = create<AuthStore>()(
  persist(
    (set, get) => ({
      user: null, accessToken: null, refreshToken: null,
      setAuth: (user, accessToken, refreshToken) => set({ user, accessToken, refreshToken }),
      clearAuth: () => set({ user: null, accessToken: null, refreshToken: null }),
      isLoggedIn: () => !!get().accessToken,
    }),
    { name: "rw-auth", version: 1 }
  )
);
'@ | Set-Content "lib\auth-store.ts" -Encoding UTF8

# AccountIcon.tsx
New-Item -ItemType Directory -Force "components\layout" | Out-Null
@'
"use client";
import Link from "next/link";
import { User, LogOut } from "lucide-react";
import { useAuthStore } from "@/lib/auth-store";

export default function AccountIcon() {
  const { user, isLoggedIn, clearAuth } = useAuthStore();
  if (isLoggedIn() && user) {
    return (
      <div className="flex items-center gap-3">
        <Link href="/mi-cuenta" className="hidden md:flex items-center gap-1.5 text-sm font-medium text-brand-text hover:text-brand-red transition">
          <User className="w-4 h-4" />
          <span>{user.firstName}</span>
        </Link>
        <button onClick={clearAuth} className="text-brand-muted hover:text-brand-red transition p-1" title="Cerrar sesion">
          <LogOut className="w-5 h-5" />
        </button>
      </div>
    );
  }
  return (
    <Link href="/ingresar" className="flex items-center gap-1.5 text-sm font-medium text-brand-text hover:text-brand-red transition">
      <User className="w-5 h-5" />
      <span className="hidden md:inline">Ingresar</span>
    </Link>
  );
}
'@ | Set-Content "components\layout\AccountIcon.tsx" -Encoding UTF8

# Navbar.tsx (updated)
@'
import Link from "next/link";
import { branding } from "@/lib/branding";
import CartIcon from "@/components/layout/CartIcon";
import SearchBar from "@/components/layout/SearchBar";
import AccountIcon from "@/components/layout/AccountIcon";

export default function Navbar() {
  return (
    <header className="sticky top-0 z-50 bg-white border-b border-gray-100 shadow-sm">
      <div className="max-w-7xl mx-auto px-4 h-16 flex items-center gap-4">
        <Link href="/" className="flex-shrink-0 flex items-center gap-2 font-bold text-brand-red text-xl">
          <span>{branding.name}</span>
        </Link>
        <div className="flex-1 max-w-xl"><SearchBar /></div>
        <nav className="hidden md:flex items-center gap-6 text-sm font-medium">
          <Link href="/productos" className="hover:text-brand-red transition">Productos</Link>
        </nav>
        <AccountIcon />
        <CartIcon />
      </div>
    </header>
  );
}
'@ | Set-Content "components\layout\Navbar.tsx" -Encoding UTF8

# Cart page
New-Item -ItemType Directory -Force "app\carrito" | Out-Null
@'
"use client";
import Link from "next/link";
import Image from "next/image";
import { ShoppingCart, Trash2, Plus, Minus, ArrowRight, ShoppingBag } from "lucide-react";
import { useCartStore } from "@/lib/cart-store";
import { formatPrice } from "@/lib/utils";

export default function CartPage() {
  const { items, updateQuantity, removeItem, subtotal } = useCartStore();

  if (items.length === 0) {
    return (
      <div className="min-h-[70vh] flex flex-col items-center justify-center text-center px-4">
        <div className="w-24 h-24 bg-brand-gray rounded-full flex items-center justify-center mb-6">
          <ShoppingBag className="w-12 h-12 text-brand-muted" />
        </div>
        <h1 className="text-2xl font-bold mb-2">Tu carrito esta vacio</h1>
        <p className="text-brand-muted mb-8 max-w-sm">Todavia no agregaste ningun producto.</p>
        <Link href="/productos" className="inline-flex items-center gap-2 bg-brand-red text-white font-semibold px-8 py-3 rounded-full hover:bg-brand-red-dark transition">
          Ver productos <ArrowRight className="w-4 h-4" />
        </Link>
      </div>
    );
  }

  const total = subtotal();

  return (
    <div className="max-w-6xl mx-auto px-4 py-10">
      <div className="flex items-center gap-3 mb-8">
        <ShoppingCart className="w-7 h-7 text-brand-red" />
        <h1 className="text-3xl font-bold">Tu carrito</h1>
        <span className="ml-2 bg-brand-red text-white text-sm font-bold px-2.5 py-0.5 rounded-full">
          {items.reduce((s, i) => s + i.quantity, 0)}
        </span>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2 space-y-4">
          {items.map((item) => (
            <div key={item.variantId} className="bg-white border border-gray-100 rounded-2xl p-4 flex gap-4 shadow-sm hover:shadow-md transition">
              <div className="w-20 h-20 flex-shrink-0 bg-brand-gray rounded-xl overflow-hidden">
                {item.imageUrl ? (
                  <Image src={item.imageUrl} alt={item.productName} width={80} height={80} className="object-contain w-full h-full p-2" />
                ) : (
                  <div className="w-full h-full flex items-center justify-center text-brand-muted text-xs">Sin foto</div>
                )}
              </div>
              <div className="flex-1 min-w-0">
                <h3 className="font-semibold text-brand-text leading-tight truncate">{item.productName}</h3>
                <p className="text-sm text-brand-muted mt-0.5">{item.variantLabel}</p>
                <div className="flex items-center justify-between mt-3">
                  <div className="flex items-center gap-2 bg-brand-gray rounded-full px-1 py-1">
                    <button onClick={() => updateQuantity(item.variantId, item.quantity - 1)} className="w-7 h-7 rounded-full flex items-center justify-center hover:bg-white transition"><Minus className="w-3 h-3" /></button>
                    <span className="w-6 text-center text-sm font-bold">{item.quantity}</span>
                    <button onClick={() => updateQuantity(item.variantId, item.quantity + 1)} className="w-7 h-7 rounded-full flex items-center justify-center hover:bg-white transition"><Plus className="w-3 h-3" /></button>
                  </div>
                  <div className="flex items-center gap-3">
                    <span className="font-bold text-brand-red">{formatPrice(item.unitPrice * item.quantity)}</span>
                    <button onClick={() => removeItem(item.variantId)} className="text-gray-300 hover:text-brand-red transition p-1"><Trash2 className="w-4 h-4" /></button>
                  </div>
                </div>
              </div>
            </div>
          ))}
          <Link href="/productos" className="inline-flex items-center gap-2 text-brand-red text-sm font-medium hover:underline mt-2">
            &larr; Seguir comprando
          </Link>
        </div>

        <div className="lg:col-span-1">
          <div className="bg-white border border-gray-100 rounded-2xl p-6 shadow-sm sticky top-24">
            <h2 className="text-lg font-bold mb-5 pb-4 border-b border-gray-100">Resumen del pedido</h2>
            <div className="space-y-3 text-sm">
              <div className="flex justify-between">
                <span className="text-brand-muted">Subtotal</span>
                <span className="font-medium">{formatPrice(subtotal())}</span>
              </div>
              <div className="flex justify-between">
                <span className="text-brand-muted">Envio</span>
                <span className="font-medium text-green-600">A coordinar</span>
              </div>
            </div>
            <div className="border-t border-gray-100 mt-4 pt-4 flex justify-between items-center">
              <span className="font-bold text-base">Total</span>
              <span className="font-bold text-2xl text-brand-red">{formatPrice(total)}</span>
            </div>
            <Link href="/checkout" className="mt-6 w-full flex items-center justify-center gap-2 bg-brand-red text-white font-bold py-4 rounded-full hover:bg-brand-red-dark transition text-base">
              Iniciar compra <ArrowRight className="w-5 h-5" />
            </Link>
            <div className="mt-5 space-y-2 text-xs text-brand-muted text-center">
              <p>Pago seguro con Mercado Pago</p>
              <p>Envios a todo Argentina</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
'@ | Set-Content "app\carrito\page.tsx" -Encoding UTF8

# Login page
New-Item -ItemType Directory -Force "app\ingresar" | Out-Null
@'
"use client";
import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { LogIn, Eye, EyeOff } from "lucide-react";
import { useAuthStore } from "@/lib/auth-store";
import { branding } from "@/lib/branding";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

export default function LoginPage() {
  const router = useRouter();
  const setAuth = useAuthStore((s) => s.setAuth);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPwd, setShowPwd] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      const res = await fetch(`${API_URL}/api/auth/login`, {
        method: "POST", headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });
      const data = await res.json();
      if (!res.ok) { setError(data.error ?? "Error al iniciar sesion."); return; }
      setAuth(data.user, data.accessToken, data.refreshToken);
      router.push("/mi-cuenta");
    } catch { setError("No se pudo conectar con el servidor."); }
    finally { setLoading(false); }
  };

  return (
    <div className="min-h-[80vh] flex items-center justify-center px-4">
      <div className="w-full max-w-md">
        <div className="bg-white rounded-2xl shadow-md border border-gray-100 overflow-hidden">
          <div className="bg-brand-red px-8 py-6 text-white text-center">
            <LogIn className="w-8 h-8 mx-auto mb-2 opacity-90" />
            <h1 className="text-2xl font-bold">Bienvenido de nuevo</h1>
            <p className="text-sm opacity-80 mt-1">{branding.name}</p>
          </div>
          <div className="px-8 py-8">
            {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-xl px-4 py-3 text-sm mb-5">{error}</div>}
            <form onSubmit={handleSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium mb-1.5">Email</label>
                <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required placeholder="tu@email.com" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-brand-red text-sm" />
              </div>
              <div>
                <label className="block text-sm font-medium mb-1.5">Contrasena</label>
                <div className="relative">
                  <input type={showPwd ? "text" : "password"} value={password} onChange={(e) => setPassword(e.target.value)} required placeholder="••••••••" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-brand-red text-sm pr-11" />
                  <button type="button" onClick={() => setShowPwd(!showPwd)} className="absolute right-3 top-1/2 -translate-y-1/2 text-brand-muted hover:text-brand-text">
                    {showPwd ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
              </div>
              <button type="submit" disabled={loading} className="w-full bg-brand-red text-white font-bold py-3.5 rounded-xl hover:bg-brand-red-dark transition disabled:opacity-60 text-base mt-2">
                {loading ? "Ingresando..." : "Ingresar"}
              </button>
            </form>
            <p className="text-center text-sm text-brand-muted mt-6">
              No tenes cuenta?{" "}
              <Link href="/registrarse" className="text-brand-red font-semibold hover:underline">Registrate gratis</Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
'@ | Set-Content "app\ingresar\page.tsx" -Encoding UTF8

# Register page
New-Item -ItemType Directory -Force "app\registrarse" | Out-Null
@'
"use client";
import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { UserPlus, Eye, EyeOff } from "lucide-react";
import { useAuthStore } from "@/lib/auth-store";
import { branding } from "@/lib/branding";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000";

export default function RegisterPage() {
  const router = useRouter();
  const setAuth = useAuthStore((s) => s.setAuth);
  const [form, setForm] = useState({ firstName: "", lastName: "", email: "", password: "" });
  const [showPwd, setShowPwd] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const set = (field: string) => (e: React.ChangeEvent<HTMLInputElement>) => setForm((f) => ({ ...f, [field]: e.target.value }));

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      const res = await fetch(`${API_URL}/api/auth/register`, {
        method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(form),
      });
      const data = await res.json();
      if (!res.ok) { setError(data.error ?? "Error al registrarse."); return; }
      setAuth(data.user, data.accessToken, data.refreshToken);
      router.push("/mi-cuenta");
    } catch { setError("No se pudo conectar con el servidor."); }
    finally { setLoading(false); }
  };

  return (
    <div className="min-h-[80vh] flex items-center justify-center px-4 py-10">
      <div className="w-full max-w-md">
        <div className="bg-white rounded-2xl shadow-md border border-gray-100 overflow-hidden">
          <div className="bg-brand-red px-8 py-6 text-white text-center">
            <UserPlus className="w-8 h-8 mx-auto mb-2 opacity-90" />
            <h1 className="text-2xl font-bold">Crear cuenta</h1>
            <p className="text-sm opacity-80 mt-1">{branding.name}</p>
          </div>
          <div className="px-8 py-8">
            {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-xl px-4 py-3 text-sm mb-5">{error}</div>}
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="block text-sm font-medium mb-1.5">Nombre</label>
                  <input type="text" value={form.firstName} onChange={set("firstName")} required placeholder="Juan" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-brand-red text-sm" />
                </div>
                <div>
                  <label className="block text-sm font-medium mb-1.5">Apellido</label>
                  <input type="text" value={form.lastName} onChange={set("lastName")} required placeholder="Perez" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-brand-red text-sm" />
                </div>
              </div>
              <div>
                <label className="block text-sm font-medium mb-1.5">Email</label>
                <input type="email" value={form.email} onChange={set("email")} required placeholder="tu@email.com" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-brand-red text-sm" />
              </div>
              <div>
                <label className="block text-sm font-medium mb-1.5">Contrasena <span className="text-brand-muted font-normal">(minimo 6 caracteres)</span></label>
                <div className="relative">
                  <input type={showPwd ? "text" : "password"} value={form.password} onChange={set("password")} required minLength={6} placeholder="••••••••" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-brand-red text-sm pr-11" />
                  <button type="button" onClick={() => setShowPwd(!showPwd)} className="absolute right-3 top-1/2 -translate-y-1/2 text-brand-muted hover:text-brand-text">
                    {showPwd ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                  </button>
                </div>
              </div>
              <button type="submit" disabled={loading} className="w-full bg-brand-red text-white font-bold py-3.5 rounded-xl hover:bg-brand-red-dark transition disabled:opacity-60 text-base mt-2">
                {loading ? "Creando cuenta..." : "Crear cuenta"}
              </button>
            </form>
            <p className="text-center text-sm text-brand-muted mt-6">
              Ya tenes cuenta?{" "}
              <Link href="/ingresar" className="text-brand-red font-semibold hover:underline">Ingresar</Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
'@ | Set-Content "app\registrarse\page.tsx" -Encoding UTF8

git add .
git commit -m "feat: cart page, login page, register page, auth store, AccountIcon"
git push origin $Branch

Write-Ok "Frontend pushed to branch $Branch"
Pop-Location

Write-Host "`n✅ Done!" -ForegroundColor Green
Write-Host "   Backend PR:  https://github.com/ipancaldo/RedAndWhite-business/compare/$Branch" -ForegroundColor Yellow
Write-Host "   Frontend PR: https://github.com/ipancaldo/RedAndWhite-business-frontend/compare/$Branch" -ForegroundColor Yellow
Write-Host ""
Write-Host "Next step (backend) — run the Identity migration:" -ForegroundColor Cyan
Write-Host "   cd RedAndWhite-business" -ForegroundColor White
Write-Host "   dotnet ef migrations add InitIdentity --project src/Modules/Identity/RedAndWhite.Identity.Infrastructure --startup-project src/Api" -ForegroundColor White
Write-Host "   dotnet ef database update --startup-project src/Api" -ForegroundColor White