using System.Text.Json;
using Backend.Infrastructure.Database;
using Backend.Models;

namespace Backend.Seeders
{
    public static class LocationSeeding
    {
        public static void Seed(AppDbContext dbContext)
        {
            if (dbContext.Locations.Any())
                return;

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "regions.json");
            var json = File.ReadAllText(jsonPath);
            var regions = JsonSerializer.Deserialize<List<RegionSeedModel>>(json);
            if (regions == null) return;

            var locations = new List<Location>();
            foreach (var region in regions)
            {
                foreach (var city in region.kota)
                {
                    locations.Add(new Location
                    {
                        Id = Guid.NewGuid(),
                        Province = region.provinsi,
                        City = city
                    });
                }
            }
            dbContext.Locations.AddRange(locations);
            dbContext.SaveChanges();
        }

        private class RegionSeedModel
        {
            public string provinsi { get; set; }
            public List<string> kota { get; set; }
        }
    }
}
