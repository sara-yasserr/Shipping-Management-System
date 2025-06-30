using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Seeder
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(UnitOfWork unitOfWork)
        {
            
            await SeedUsersAndSellers(unitOfWork);
        }

       
        private static async Task SeedUsersAndSellers(UnitOfWork unitOfWork)
        {
            var userManager = unitOfWork.UserManager;

            // Check if user exists
            string email = "gehad@example.com";
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "gehaduser",
                    Email = email,
                    FirstName = "Gehad",
                    LastName = "Elbadry",
                    CreatedAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, "P@ssword123");
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // Add Seller
            if (!unitOfWork.SellerRepo.GetAll().Any(s => s.UserId == user.Id))
            {
                var city = unitOfWork.CityRepo.GetAll().First();

                var seller = new Seller
                {
                    StoreName = "Gehad Store",
                    Address = "Nasr City",
                    CancelledOrderPercentage = 0.05m,
                    CityId = city.Id,
                    UserId = user.Id
                };

                unitOfWork.SellerRepo.Add(seller);
                await unitOfWork.SaveAsync();
            }
        }
    }
}
