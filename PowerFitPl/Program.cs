using Microsoft.EntityFrameworkCore;
using PowerFitBLL.Mapping;
using PowerFitBLL.Services.Classes;
using PowerFitBLL.Services.Interfaces;
using PowerFitDAL.Data.Contexts;
using PowerFitDAL.Data.DataSeeding;
using PowerFitDAL.Repositories.Classes;
using PowerFitDAL.Repositories.Interfaces;
using PowerFitDAL.UnitOfWork;

namespace PowerFitPl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<PowerFitDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                    throw new InvalidOperationException(message: "No Connection string was found"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles())); 

            var app = builder.Build();

            #region Migrate Database & Data Seeding

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PowerFitDbContext>();

            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if(pendingMigrations?.Any() ?? false)
            {
                dbContext.Database.Migrate();
            }

            PowerFitDbContextSeeding.SeedData(dbContext);

            #endregion



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
