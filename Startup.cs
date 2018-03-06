using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vega.Models.PieShop;

namespace Vega {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<AppDbContext> (options => options.UseSqlServer (Configuration.GetConnectionString ("DefaultConnection")));

            // setup validation for registering new account
            services.Configure<IdentityOptions> (options => {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = false;
            });

            //add identity to service
            services.AddIdentity<IdentityUser, IdentityRole> ().AddEntityFrameworkStores<AppDbContext> ();

            // trasient: whenever IPieRepository is requested, new instance of MockPieRepository is returned everytime
            services.AddTransient<IPieRepository, PieRepository> ();
            services.AddTransient<IFeedbackRepository, FeedbackRepository> ();
            // single: whenever IPieRepository is requested, only one instance of MockPieRepository is returned everytime           
            //  services.AddSingleton<IPieRepository, MockPieRepository> ();

            // scoped: whenever IPieRepository is requested, the same instance of MockPieRepository is returned  
            // but when the request goes out of scope, the instance is removed, and next request a new instance is going to be returned          
            // services.AddScoped<IPieRepository, MockPieRepository> ();

            services.AddMvc ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage (); // show nice error page
                app.UseStatusCodePages (); // show status code info
                app.UseWebpackDevMiddleware (new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            } else {
                app.UseExceptionHandler ("/Home/Error");
            }

            app.UseStaticFiles (); // use file under wwwroot folder

            // enable authentication, need to put before mvcdot
            app.UseAuthentication ();

            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute (
                    name: "spa-fallback",
                    defaults : new { controller = "Home", action = "PieIndex" });
            });
        }
    }
}