using DataAccess.DBContexts;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.Services
{
    public class DataSeeder
    {
        private readonly BreakfastContext _context;

        public DataSeeder(BreakfastContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            await SeedAdmin(configuration);
            await SeedSampleData();
        }

        private async Task SeedAdmin(IConfiguration configuration)
        {
            var username = configuration["Admin:Username"];
            var password = configuration["Admin:Password"];

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return;

            if (await _context.User.AnyAsync(u => u.Username == username)) return;

            _context.User.Add(new Users
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Email = configuration["Admin:Email"] ?? string.Empty,
                Role = "Admin"
            });

            await _context.SaveChangesAsync();
        }

        private async Task SeedSampleData()
        {
            if (await _context.Breakfasts.AnyAsync()) return;

            var omelette = new Breakfast
            {
                Name = "املت گوجه",
                Description = "صبحانه‌ی ساده و گرم ایرانی",
                Price = 120000,
                Recipe = "۱. گوجه را خرد کنید و بپزید تا آبش کم شود.\n۲. تخم‌مرغ را بریزید و هم بزنید.",
                Ingredients =
                {
                    new BreakfastIngredient { Name = "تخم‌مرغ", Amount = 2, Unit = "عدد", Calories = 156, Price = 30000 },
                    new BreakfastIngredient { Name = "گوجه‌فرنگی", Amount = 2, Unit = "عدد", Calories = 44, Price = 15000 },
                    new BreakfastIngredient { Name = "روغن زیتون", Amount = 1, Unit = "قاشق", Calories = 119, Price = 10000 }
                },
                HealthInfos =
                {
                    new BreakfastHealthInfo { Condition = "دیابت", IsSuitable = true, Note = "داده‌ی نمونه" },
                    new BreakfastHealthInfo { Condition = "کلسترول بالا", IsSuitable = false, Note = "داده‌ی نمونه" }
                }
            };

            var oatmeal = new Breakfast
            {
                Name = "فرنی جو دوسر",
                Description = "صبحانه‌ی سبک و سیرکننده",
                Price = 90000,
                Recipe = "۱. شیر را گرم کنید.\n۲. جو را اضافه کنید و ۵ دقیقه بپزید.\n۳. با موز سرو کنید.",
                Ingredients =
                {
                    new BreakfastIngredient { Name = "جو دوسر", Amount = 50, Unit = "گرم", Calories = 190, Price = 12000 },
                    new BreakfastIngredient { Name = "شیر کم‌چرب", Amount = 1, Unit = "لیوان", Calories = 100, Price = 15000 },
                    new BreakfastIngredient { Name = "موز", Amount = 1, Unit = "عدد", Calories = 105, Price = 20000 }
                },
                HealthInfos =
                {
                    new BreakfastHealthInfo { Condition = "دیابت", IsSuitable = false, Note = "داده‌ی نمونه" },
                    new BreakfastHealthInfo { Condition = "کلسترول بالا", IsSuitable = true, Note = "داده‌ی نمونه" }
                }
            };

            var cafe = new Restaurant { Name = "کافه صبح", Type = "Cafe", Address = "تهران، خیابان ولیعصر", Phone = "02100000000" };
            cafe.RestaurantBreakfasts.Add(new RestaurantBreakfast { Breakfast = omelette, Price = 150000 });
            cafe.RestaurantBreakfasts.Add(new RestaurantBreakfast { Breakfast = oatmeal, Price = 110000 });

            var restaurant = new Restaurant { Name = "رستوران سنتی بهار", Type = "Restaurant", Address = "تهران، میدان تجریش", Phone = "02111111111" };
            restaurant.RestaurantBreakfasts.Add(new RestaurantBreakfast { Breakfast = omelette, Price = 170000 });

            _context.Restaurants.AddRange(cafe, restaurant);

            await _context.SaveChangesAsync();
        }
    }
}
