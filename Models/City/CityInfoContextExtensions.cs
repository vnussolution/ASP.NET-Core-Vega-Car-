using System;
using System.Collections.Generic;
using Vega.Models.City;

namespace Vega.Models.Library {
    public static class CityInfoContextExtensions {
        public static void EnsureSeedCityDataForContext (this AppDbContext context) {

            // first, clear the database.  This ensures we can always start 
            // fresh with each demo.  Not advised for production environments, obviously :-)

            context.Cities.RemoveRange (context.Cities);
            context.SaveChanges ();

            var cities = new List<Models.City.City> () {
                new Models.City.City () {
                //  Id = 1,
                Name = "New York City",
                Description = "The one with that big park.",
                POIs = new List<POI> () {
                new POI () {
                //  Id = 1,
                Name = "Central Park",
                Description = "The most visited urban park in the United States."
                },
                new POI () {
                //   Id = 2,
                Name = "Empire State Building",
                Description = "A 102-story skyscraper located in Miwn Manhattan."
                },
                }
                },
                new Models.City.City () {
                //   Id = 2,
                Name = "Antwerp",
                Description = "The one with the cathedral that was never really finished.",
                POIs = new List<POI> () {
                new POI () {
                //  Id = 3,
                Name = "Cathedral of Our Lady",
                Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                },
                new POI () {
                //   Id = 4,
                Name = "Antwerp Central Station",
                Description = "The the finest example of railway architecture in Belgium."
                },
                }
                },
                new Models.City.City () {
                //    Id = 3,
                Name = "Paris",
                Description = "The one with that big tower.",
                POIs = new List<POI> () {
                new POI () {
                //  Id = 5,
                Name = "Eiffel Tower",
                Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                },
                new POI () {
                //    Id = 6,
                Name = "The Louvre",
                Description = "The world's largest museum."
                },
                }
                }
            };

            context.Cities.AddRange (cities);
            context.SaveChanges ();

        }
    }
}