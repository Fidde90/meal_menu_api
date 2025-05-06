using meal_menu_api.Entities;
using Microsoft.EntityFrameworkCore;
using meal_menu_api.Config;
using System.Text.Json.Serialization;
using meal_menu_api.Filters;
using meal_menu_api.Database.Seeders;
using meal_menu_api.Database.Context;


namespace meal_menu_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("meal_menu_db");

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            builder.Services.RegisterRepositories(builder.Configuration);
            builder.Services.RegisterServices(builder.Configuration);
            builder.Services.RegisterJwt(builder.Configuration);
            builder.Services.AddControllers(options => { options.Filters.Add<UseApiKeyAttribute>();})
                .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; });

            builder.Services.AddDefaultIdentity<AppUser>(x => {
                x.SignIn.RequireConfirmedAccount = false;
                x.Password.RequiredLength = 3;
                x.User.RequireUniqueEmail = true;
                x.Password.RequireDigit = false;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<DataContext>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Meal_menu_client", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                DbSeeder.SeedUnits(context);
            }

            // Configure the HTTP request pipeline

            app.UseCors("Meal_menu_client");
            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Meal menu v1"));
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
