using DevExpress.ExpressApp.Mobile;
using DevExpress.ExpressApp.Spa.AspNetCore;
using DevExpress.ExpressApp.Spa.AspNetCore.Mvc;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using SpaDemo.Spa;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace MainDemo.Spa {
    public class Startup {
        private static PhysicalFileProvider physicalFileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        private Lazy<bool> enableAllOrigins;

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
            enableAllOrigins = new Lazy<bool>(() => Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAllOrigins"]));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors(options => {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:2065").AllowCredentials().AllowAnyMethod().AllowAnyHeader());

                options.AddPolicy("DevelopmentCorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowCredentials().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddMvc(options => {
                //options.Filters.Add<AntiforgeryCookieResultFilter>();
            }).AddJsonOptions(s => {
                s.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                s.SerializerSettings.ContractResolver = new TypesInfoContractResolver();
                // Ignored cause not working in react UI
                //s.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
            //services.AddDevExpressControls();

            services.AddSingleton<IFileProvider>(physicalFileProvider);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Application factory
            services.AddScoped<ISpaApplicationProvider, SpaApplicationProvider>();
            services.AddScoped<ISpaApplicationFactory, SpaApplicationFactory<SpaDemoSpaApplication>>();
            //


            services.Configure<RouteOptions>(routeOptions => {
                routeOptions.ConstraintMap.Add("detailView", typeof(DetailViewRouteConstraint));
                routeOptions.ConstraintMap.Add("listView", typeof(ListViewRouteConstraint));
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            // Uncomment if you need to show unhandled exceptions in UI
            //if(env.IsDevelopment()) {
            //    app.UseExceptionHandlerWithDetails(options => {
            //        options.CorsPolicyName = "DevelopmentCorsPolicy";
            //    });
            //}
            app.UseUserFriendlyExceptionHandler(options => {
                options.CorsPolicyName = env.IsDevelopment() ? "DevelopmentCorsPolicy" : "AllowSpecificOrigin";
            });

            if(enableAllOrigins.Value || env.IsDevelopment()) {
                app.UseCors("DevelopmentCorsPolicy");
            }
            else {
                app.UseCors("AllowSpecificOrigin");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dist", "static")),
                RequestPath = "/static"
            });

            app.UseMiddleware<RequestCultureMiddleware>();

            app.UseMvc(routes => {
                routes.MapSpaFallbackRoute("react-fallback",
                    new { controller = "IndexPage", action = "Index" });
            });

            HttpContextValueManager.HttpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            ValueManager.ValueManagerType = typeof(HttpContextValueManager<>).GetGenericTypeDefinition();
        }
    }
}
