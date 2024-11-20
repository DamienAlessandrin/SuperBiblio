using Microsoft.EntityFrameworkCore;
using SuperBiblio.Data;
using SuperBiblio.Data.Repositories;
using SuperBiblio.Data.Repositories.Sql;
using System.Text.Json.Serialization;

namespace SuperBiblio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=data.db3"));

            builder.Services.AddScoped<IBookRepository, SqlBookRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}