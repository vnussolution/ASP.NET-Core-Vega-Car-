using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using Vega.Helpers;
using Vega.Models;
using Vega.Models.City;
using Vega.Models.City.Dto;
using Vega.Models.Library;
using Vega.Models.Library.Dto;
using Vega.Models.PieShop;
using Vega.Services.City;
using Vega.Services.Library;

namespace Vega {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            //setup up to use SQL server
            services.AddDbContext<AppDbContext> (options => options.UseSqlServer (Configuration.GetConnectionString ("DefaultConnection")));

            //to use SqlLite
            //services.AddDbContext<AppDbContext> (options => options.UseSqlite ("Data Source=MvcEmployee.db"));

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

            services.AddScoped<ILibraryRepository, LibraryRepository> ();

            services.AddScoped<ICityInfoRepository, CityInfoRepository> ();

            //Adding Mail service
#if !DEBUG
            services.AddTransient<IMailService, LocalMailService> ();
#else
            services.AddTransient<IMailService, CloudMailService> ();
#endif

            services.AddMvc (setupAction => {

                    // enable xml format for api
                    setupAction.ReturnHttpNotAcceptable = true;
                    setupAction.OutputFormatters.Add (new XmlDataContractSerializerOutputFormatter ());
                    setupAction.InputFormatters.Add (new XmlDataContractSerializerInputFormatter ());
                })

                //enable XML support input and output
                .AddMvcOptions (o => o.OutputFormatters.Add (new XmlDataContractSerializerOutputFormatter ()))

                //By default mvc returns camel casing JSON, do this to enable PASCAL casing
                .AddJsonOptions (o => {
                    if (o.SerializerSettings.ContractResolver != null) {
                        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                        castedResolver.NamingStrategy = null;
                    }
                });;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AppDbContext context) {

            //logging
            loggerFactory.AddConsole ();
            loggerFactory.AddDebug (LogLevel.Error);
            // use Nlog service , 3rd party https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2
            loggerFactory.AddNLog ();

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage (); // show nice error page
                app.UseStatusCodePages (); // show status code info
                app.UseWebpackDevMiddleware (new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            } else {
                // handel Error page MVC
                //app.UseExceptionHandler ("/Home/Error");

                //Handle  API error : 500 
                app.UseExceptionHandler (appBuilder => {
                    appBuilder.Run (async contextBuilder => {
                        contextBuilder.Response.StatusCode = 500;
                        await contextBuilder.Response.WriteAsync ("An unexpected fault happened. Try again later.");
                    });
                });

            }

            app.UseStaticFiles (); // use file under wwwroot folder

            // enable authentication, need to put before mvcdot
            app.UseAuthentication ();

            //Setup Automapper
            AutoMapper.Mapper.Initialize (cfg => {

                //setup AutoMapper for Author                
                cfg.CreateMap<Author, AuthorDto> ()
                    .ForMember (dest => dest.Name, opt => opt.MapFrom (src =>
                        $"{src.FirstName} {src.LastName}"))
                    .ForMember (dest => dest.Age, opt => opt.MapFrom (src =>
                        src.DateOfBirth.GetCurrentAge ()));

                //setup AutoMapper for Book
                cfg.CreateMap<Book, BookDto> ();

                //setup AutoMapper for Author creation POST
                cfg.CreateMap<AuthorForCreationDto, Author> ();

                //setup AutoMapper for Book
                cfg.CreateMap<BookForCreationDto, Book> ();

                //setup AutoMapper for City
                cfg.CreateMap<City, CityWithoutPoiDto> ();
                cfg.CreateMap<City, CityDto> ();
                cfg.CreateMap<POI, PointOfInterestDto> ();
                // cfg.CreateMap<ICollection<POI>, ICollection<PointOfInterestDto>> ();
                cfg.CreateMap<PointOfInterestCreationDto, POI> ();
                cfg.CreateMap<PointOfInterestUpdateDto, POI> ();
                cfg.CreateMap<POI, PointOfInterestUpdateDto> ();

            });

            //show status code to webpage if it fails 400, 500
            app.UseStatusCodePages ();

            // setup mvc route
            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=PieIndex}/{id?}");

                routes.MapSpaFallbackRoute (
                    name: "spa-fallback",
                    defaults : new { controller = "Home", action = "PieIndex" });
            });
        }
    }
}