using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using Common.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Common.Data
{
    public class Seed
    {
        public static async Task SeedUser(DataContext context)
        {
            if(await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Common/Database/TestData/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password"));
                user.PasswordSalt = hmac.Key;

                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }
}