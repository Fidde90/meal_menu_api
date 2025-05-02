
using meal_menu_api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers().AddJsonOptions(options =>{options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;});
            builder.Services.AddDbContext<DbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<AppUser>(x => {
                x.SignIn.RequireConfirmedAccount = false;
                x.Password.RequiredLength = 3;
                x.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DbContext>();

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

            // Configure the HTTP request pipeline

            app.UseCors("Meal_menu_client");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
