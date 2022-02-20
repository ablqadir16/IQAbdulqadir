using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data.SeedData
{
    public class SeedData
    {
        //Seed Data
        public static async Task SeedAsync(IQADbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                //Seeding User Data from users.json mock data file
                if (!context.Users.Any())
                {
                    var usersData = File.ReadAllText(@"../Infrastructure/Data/SeedData/users.json");
                    var users = JsonSerializer.Deserialize<List<User>>(usersData);

                    foreach (var item in users)
                    {

                        context.Users.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<IQADbContext>();
                logger.LogError(ex.Message);
            }
        }
    }
}
