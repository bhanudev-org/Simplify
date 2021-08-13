using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Simplify.Feature.Identity;
using Simplify.IdentityApp.App;
using Simplify.SeedWork;
using Simplify.Storage.MongoDb;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreHandlers;

namespace Simplify.IdentityApp;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if(await manager.FindByClientIdAsync("console") is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "console",
                ClientSecret = Guid.NewGuid().ToString(),
                DisplayName = "My console application",
                Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials
                }
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public static class ConfigurationServiceExtensions
{
    public static IServiceCollection Configure<T>(this IServiceCollection services, Action<IServiceProvider, T> configure) where T : class
    {
        services.AddSingleton<IConfigureOptions<T>>(c => new ConfigureOptions<T>(o => configure(c, o)));

        return services;
    }

    public static IServiceCollection Configure<T>(this IServiceCollection services, IConfiguration config, string path) where T : class
    {
        services.AddOptions<T>().Bind(config.GetSection(path));

        return services;
    }
}


public static class ServiceCollectionExtensions
{
    public static ISimplifyBuilder AddIdentity(this ISimplifyBuilder builder)
    {
        builder.Services.Configure<CookieTempDataProviderOptions>(options =>
        {
            options.Cookie.Name = $".{builder.Options.Id}.temp";
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddAntiforgery(options =>
        {
            options.Cookie.Name = $".{builder.Options.Id}.af";
            options.FormFieldName = $"{builder.Options.Id}_af";
            options.HeaderName = "X-CSRF-TOKEN";
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.HttpOnly = true;
            options.Cookie.Expiration = TimeSpan.FromDays(1);
        });

        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = $".{builder.Options.Id}.session";
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromHours(1);
            options.Cookie.HttpOnly = false;
            options.Cookie.Expiration = TimeSpan.FromHours(1);
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = $".{builder.Options.Id}.id";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.SameSite = SameSiteMode.Strict;

            options.ClaimsIssuer = $"{builder.Options.Id}-issuer";
            options.ExpireTimeSpan = TimeSpan.FromDays(1);
            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            options.SlidingExpiration = true;
        });

        builder.Services
                .AddAuthentication()
                .AddCookie();

        builder.Services
                .AddDataProtection(options => options.ApplicationDiscriminator = builder.Options.Id)
                .SetApplicationName(builder.Options.Slug);

        builder.Services.AddSimplifyIdentityStores();

        builder.Services.AddSingleton<IXmlRepository, DataProtectionXmlRepository>();

        builder.Services.Configure<KeyManagementOptions>((c, options) =>
        {
            options.XmlEncryptor = new NullXmlEncryptor();
            options.XmlRepository = c.GetRequiredService<IXmlRepository>();
        });

        builder.Services
                .AddIdentity<User, Role>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

        //builder.Services.AddSingletonAs<TokenStoreInitializer>()
        //    .AsSelf();

        //builder.Services.ConfigureOptions<DefaultKeyStore>();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4;

            options.User.RequireUniqueEmail = true;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
        });

        builder.Services.Configure<OpenIddictServerOptions>(options =>
        {
            options.Issuer = new Uri($"{builder.Options.BaseUrl}/identity");
        });

        builder.Services.AddHostedService<Worker>();

        var sp = builder.Services.BuildServiceProvider();

        builder.Services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseMongoDb(builder =>
                {
                    builder.SetApplicationsCollectionName("oiApps");
                    builder.SetAuthorizationsCollectionName("oiAuthorizations");
                    builder.SetScopesCollectionName("oiScopes");
                    builder.SetTokensCollectionName("oiTokens");
                    builder.UseDatabase(sp.GetRequiredService<IMongoDbContext>().Database);
                });

                //builder.Services.AddSingletonAs<IdentityServerConfiguration.Scopes>()
                //    .As<IOpenIddictScopeStore<ImmutableScope>>();

                //builder.Services.AddSingletonAs<DynamicApplicationStore>()
                //    .As<IOpenIddictApplicationStore<ImmutableApplication>>();

                //builder.ReplaceApplicationManager(typeof(ApplicationManager<>));
            })
            .AddServer(builder =>
            {
                builder.RemoveEventHandler(ValidateTransportSecurityRequirement.Descriptor);

                //builder.AddEventHandler<ProcessSignInContext>(builder =>
                //{
                //    builder.UseSingletonHandler<AlwaysAddTokenHandler>()
                //        .SetOrder(AttachTokenParameters.Descriptor.Order + 1);
                //});

                builder
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetIntrospectionEndpointUris("/connect/introspect")
                    .SetLogoutEndpointUris("/connect/logout")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo");

                builder.DisableAccessTokenEncryption();

                builder.RequireProofKeyForCodeExchange();

                builder.AddDevelopmentEncryptionCertificate()
                  .AddDevelopmentSigningCertificate();

                builder.RegisterScopes(
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.Roles);

                builder.SetAccessTokenLifetime(TimeSpan.FromDays(30));

                builder.AllowClientCredentialsFlow();
                builder.AllowAuthorizationCodeFlow();

                builder.UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableStatusCodePagesIntegration()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                // options.UseLocalServer();
                options.UseAspNetCore();
            });

        return builder;
    }
}
