using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;
using Microsoft.Extensions.Options;

namespace OzSapkaTShirt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ApplicationContext? context;
            UserManager<ApplicationUser>? userManager;
            RoleManager<IdentityRole>? roleManager;
            ApplicationUser applicationUser;
            IdentityRole role;

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationContext") ?? throw new InvalidOperationException("Connection string 'ApplicationContext' not found.")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole >(options => { options.SignIn.RequireConfirmedAccount = false; options.Password.RequireNonAlphanumeric = false; })
                .AddEntityFrameworkStores<ApplicationContext>();


            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
                        app.UseAuthentication();;

            app.UseAuthorization();

            app.MapControllerRoute(
              name: "areas",
              pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            context = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetService<ApplicationContext>();

            if (context != null)
            {
                context.Database.Migrate();
                userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
                roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole>>();
                if (roleManager != null)
                {
                    if (roleManager.FindByNameAsync("Administrator").Result == null)
                    {
                        role = new IdentityRole("Administrator");
                        roleManager.CreateAsync(role).Wait();

                        if (userManager != null)
                        {
                            applicationUser = new ApplicationUser();
                            applicationUser.UserName = "Administrator";
                            applicationUser.Name = "abcd";
                            applicationUser.SurName = "abcd";
                            applicationUser.Address = "abcd";
                            applicationUser.Email = "abcd";
                            applicationUser.PhoneNumber = "1234";
                            applicationUser.Corporate = true;
                            applicationUser.Gender = 1;
                            applicationUser.PassWord = "1234";
                            applicationUser.ConfirmPassWord = "1234";


                            //add other required properties for applicationuser
                            userManager.CreateAsync(applicationUser, "Abcd1234").Wait();
                            userManager.AddToRoleAsync(applicationUser, "Administrator").Wait();
                        }
                    }
                }
            }
    
            


            app.Run();
        }
    }
}