using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server;
using Simplify.Feature.Identity;
using Simplify.SeedWork;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreHandlers;

namespace Simplify.IdentityApp
{
    public static class ServiceCollectionExtensions
    {
        public static ISimplifyBuilder AddIdentity(this ISimplifyBuilder builder)
        {
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = $".{builder.Options.Id}.auth";
            });

            builder.Services.AddAuthentication()
                    .AddCookie();

            builder.Services.AddSimplifyIdentityStores();

            // DefaultXmlRepository
            // builder.Services.Configure<KeyManagementOptions>((c, options) => options.XmlRepository = c.GetRequiredService<IXmlRepository>());

            builder.Services.AddDataProtection()
                .SetApplicationName(builder.Options.Slug);

            builder.Services.AddIdentity<User, Role>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            //builder.Services.AddSingletonAs<TokenStoreInitializer>()
            //    .AsSelf();

            //builder.Services.ConfigureOptions<DefaultKeyStore>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            builder.Services.Configure<OpenIddictServerOptions>(options =>
            {
                options.Issuer = new Uri($"{builder.Options.BaseUrl}/identity");
            });

            //builder.Services
            //    .AddOpenIddict()
            //    .AddCore(builder =>
            //    {
            //        //builder.Services.AddSingletonAs<IdentityServerConfiguration.Scopes>()
            //        //    .As<IOpenIddictScopeStore<ImmutableScope>>();

            //        //builder.Services.AddSingletonAs<DynamicApplicationStore>()
            //        //    .As<IOpenIddictApplicationStore<ImmutableApplication>>();

            //        //builder.ReplaceApplicationManager(typeof(ApplicationManager<>));
            //    })
            //    .AddServer(builder =>
            //    {
            //        builder.RemoveEventHandler(ValidateTransportSecurityRequirement.Descriptor);

            //        //builder.AddEventHandler<ProcessSignInContext>(builder =>
            //        //{
            //        //    builder.UseSingletonHandler<AlwaysAddTokenHandler>()
            //        //        .SetOrder(AttachTokenParameters.Descriptor.Order + 1);
            //        //});

            //        builder
            //            .SetAuthorizationEndpointUris("/connect/authorize")
            //            .SetIntrospectionEndpointUris("/connect/introspect")
            //            .SetLogoutEndpointUris("/connect/logout")
            //            .SetTokenEndpointUris("/connect/token")
            //            .SetUserinfoEndpointUris("/connect/userinfo");

            //        builder.DisableAccessTokenEncryption();

            //        builder.RegisterScopes(
            //            Scopes.Email,
            //            Scopes.Profile,
            //            Scopes.Roles);

            //        builder.SetAccessTokenLifetime(TimeSpan.FromDays(30));

            //        builder.AllowClientCredentialsFlow();
            //        builder.AllowImplicitFlow();
            //        builder.AllowAuthorizationCodeFlow();

            //        builder.UseAspNetCore()
            //            .DisableTransportSecurityRequirement()
            //            .EnableAuthorizationEndpointPassthrough()
            //            .EnableLogoutEndpointPassthrough()
            //            .EnableStatusCodePagesIntegration()
            //            .EnableTokenEndpointPassthrough()
            //            .EnableUserinfoEndpointPassthrough();
            //    })
            //    .AddValidation(options =>
            //    {
            //        options.UseLocalServer();
            //        options.UseAspNetCore();
            //    });



            return builder;
        }
    }
}
