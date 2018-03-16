using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vega.Models;
using Vega.Models.Library;
using Vega.Models.PieShop;

namespace Vega {
    public class Program {
        public static void Main (string[] args) {
            var host = BuildWebHost (args);
            //seeding
            using (var scope = host.Services.CreateScope ()) {
                var services = scope.ServiceProvider;
                try {
                    var context = services.GetRequiredService<AppDbContext> ();

                    //populate dummy data for Pie
                    DbInitializer.Seed (context);

                    //populate the dummy data for library
                    context.EnsureSeedLibraryDataForContext ();
                    context.EnsureSeedCityDataForContext ();
                } catch (Exception) {
                    //we could log this in a real-world situation
                }
            }

            host.Run ();
        }

        public static IWebHost BuildWebHost (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseStartup<Startup> ()
            .UseUrls ("http://localhost:1028/")
            .Build ();
    }
}