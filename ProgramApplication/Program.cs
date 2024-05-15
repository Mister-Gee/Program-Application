using ProgramApplication.Config;
using Infrastructure.Extensions;
using ProgramApplication.Mapper;

namespace ProgramApplication
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.SetupCosmosDb(Configuration);
            builder.Services.AddAutoMapper(typeof(MapperProfiles));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.EnsureCosmosDbIsCreated();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
