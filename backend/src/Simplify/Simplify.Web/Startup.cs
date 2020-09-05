using System.IO.Compression;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Serilog;
using Simplify.Infrastructure;
using Simplify.SeedWork;
using Simplify.Storage.MongoDb;
using Simplify.Web.Data;
using WebMarkupMin.AspNetCore3;
using WebMarkupMin.Core;

namespace Simplify.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Generic Options

            services.Configure<RouteOptions>(options =>
            {
                options.AppendTrailingSlash = false;
                options.LowercaseUrls = true;
                options.SuppressCheckForUnhandledSecurityMetadata = false;
            });

            services.Configure<StaticFileOptions>(options =>
            {
                options.HttpsCompression = HttpsCompressionMode.Compress;
                options.OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append(HeaderNames.CacheControl, "public, max-age=315360");
                };
            });

            services.Configure<CookieTempDataProviderOptions>(options => options.Cookie.Name = ".sy.temp");
            services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });
            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"application/xhtml+xml", "application/atom+xml", "image/svg+xml"});
            });

            services.AddWebMarkupMin(options =>
                {
                    options.AllowCompressionInDevelopmentEnvironment = true;
                    options.AllowMinificationInDevelopmentEnvironment = true;
                    options.DisablePoweredByHttpHeaders = true;
                })
                .AddHtmlMinification(options =>
                {
                    var settings = options.MinificationSettings;
                    settings.RemoveRedundantAttributes = true;
                    settings.WhitespaceMinificationMode = WhitespaceMinificationMode.Aggressive;
                    settings.RemoveHtmlComments = true;
                    settings.RemoveHtmlCommentsFromScriptsAndStyles = true;
                })
                .AddHttpCompression();

            #endregion

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSimplify(Configuration)
                .AddCommands(SimplifyWebHelper.Assembly, SimplifyInfraHelper.Assembly)
                .AddMongoDB()
                .AddInfra();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();
            app.UseResponseCompression();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}