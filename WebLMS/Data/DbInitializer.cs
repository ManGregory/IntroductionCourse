using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Data
{
    public class DbInitializer
    {
        public static void Initialize(LMSDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (context.Lectures.Any())
            {
                return;
            }

            SeedRoles(roleManager);

            SeedUsers(userManager);

            context.Lectures.Add(new Models.Lecture()
            {
                Subject = "Простые типы данных и операции с ними",
                Description = "Простые типы данных и операции с ними"
            });

            context.CodingHomeworks.Add(new Models.CodingHomework()
            {
                LectureId = context.Lectures.Single().Id,
                Subject = "Простые типы",
                Description = "Написать программу",
                TemplateCode = @"
using System;

namespace Lecture1
{
    class Program
    {
        public static bool IsTicketHappy(int ticketNum) 
        {
            return false;
        }
    }
}
                ",
                EntryType = "Lecture1.Program",
                EntryMethodName = "IsTicketHappy",
                CodingTestType = Models.CodingTestType.Method
            });

            context.CodingTests.Add(new Models.CodingTest()
            {
                HomeworkId = context.CodingHomeworks.Single().Id,
                Name = "1",
                InputParameters = "123456",
                ExpectedResult = "false"
            });

            context.CodingTests.Add(new Models.CodingTest()
            {
                HomeworkId = context.CodingHomeworks.Single().Id,
                Name = "2",
                InputParameters = "123123",
                ExpectedResult = "true"
            });

            context.SaveChanges();
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("student").Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "normal";
                user.Email = "user1@localhost";

                IdentityResult result = userManager.CreateAsync(user, "student").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                }
            }


            if (userManager.FindByNameAsync("admin").Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "user2";
                user.Email = "user2@localhost";

                IdentityResult result = userManager.CreateAsync(user, "admin").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "NormalUser";
                _ = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                _ = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
