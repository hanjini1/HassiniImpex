using Core.Entites.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
  public class AppIdentityDbContextSeed
  {
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
      if (!userManager.Users.Any())
      {
        var user = new AppUser
        {
          DisplayName = "Suyog",
          Email = "Suyog@test.com",
          UserName = "Suyog@test.com",
          Address = new Address
          {
            FirstName = "Suyog",
            LastName = "Harish",
            Street = "3rd Cross RIFCO",
            City = "Bangalore",
            State = "KA",
            Zipcode = "560049"
          }
        };

        await userManager.CreateAsync(user, "Suyog@123456");
      }
    }
  }
}