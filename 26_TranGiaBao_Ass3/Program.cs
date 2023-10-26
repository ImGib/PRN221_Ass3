using _26_TranGiaBao_Ass3.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace _26_TranGiaBao_Ass3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SignalRContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SignalRContext") ?? throw new InvalidOperationException("Connection string 'SignalRContext' not found.")));
            builder.Services.AddSignalR();
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddJsonOptions(
                options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
        endpoints.MapHub<SignalrServer>("/signalrServer"));

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}