using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Heimdall.Gateway.Domain;
using Heimdall.Gateway.ServiceDiscovery;

namespace Heimdall.Gateway.API
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }  // { get { return _hostingEnvironment; } }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
        //        .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);

        //    Configuration = builder.Build();
        //    _hostingEnvironment = env;
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure Options
            services.AddOptions();
            services.Configure<GatewayOptions>(Configuration.GetSection("GatewayConfig"));

            var discoveryOptions = Configuration.GetSection("ServiceLocation").Get<ServiceLocationOptions>();           

            // Register dependencies
            services.AddDomainDependencies();
            services.AddServiceDiscoveryDependencies(discoveryOptions);

            // Find and register module addins
            var rootFolder = HostingEnvironment.ContentRootPath;
            var modulesFolder = Path.Combine(rootFolder, Configuration["GatewayOptions:ModulesPath"]);
            services.LoadModules(ModuleManager.FindModules(modulesFolder));

            services.AddMvc();

            // Configure Authentication             
            string authority = Configuration["IdentityServer:AuthorityUrl"];
            var scopes = Configuration.GetSection("IdentityServer:AllowedScopes").Get<List<string>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityServerAuthentication(cnf =>
            {
                cnf.Authority = authority;
                cnf.AllowedScopes = scopes;
                cnf.RequireHttpsMetadata = false;                
                }
            );

            services.AddResponseCaching();

            // Configure Swagger for API testing
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "CL Service Gateway API",
                    Version = "v1"
                });

                // Set the comments path for the Swagger JSON and UI
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Heimdall.Gateway.API.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                // options return a full error + stack response
                //options =>
                //{
                //    options.Run(
                //        async context =>
                //        {
                //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //            context.Response.ContentType = "text/html";
                //            var ex = context.Features.Get<IExceptionHandlerFeature>();
                //            if (ex != null)
                //            {
                //                var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                //                await context.Response.WriteAsync(err).ConfigureAwait(false);
                //            }
                //        });
                //}
                );
            }

            app.UseAuthentication();
            app.UseResponseCaching();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            // Placed here before app.UseMVC() so that Swagger can map custom attribute based routes
            // as our DefaultController route captures everything else preventing Swagger from servicing
            // it's expected "/swagger/" route.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CL Service Gateway API V1");
            });

            // One Route to rule them all
            app.UseMvc(routes => {
                routes.MapRoute(name: "default", template: "{*url}", defaults: new { controller = "Default", action = "Handler" });
            });
        }
    }
}
