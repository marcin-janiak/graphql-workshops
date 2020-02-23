using BoardsWorkshops.API.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BoardsWorkshops.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BoardsContext>(
                cfg => cfg
                    .UseNpgsql(
                        Configuration.GetConnectionString(
                            "DefaultConnection"
                        )
                    ),
                ServiceLifetime.Transient
            );

            services.AddCors(
                x =>
                    x.AddPolicy(
                        "allowAll",
                        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                    )
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BoardsContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("allowAll");

            context.Database.Migrate();
            context.Database.EnsureCreated();
        }
    }
}