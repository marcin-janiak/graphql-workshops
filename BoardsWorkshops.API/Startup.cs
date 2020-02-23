using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BoardsWorkshops.API.DataAccess;
using BoardsWorkshops.API.Graph;
using BoardsWorkshops.API.Graph.Cards;
using BoardsWorkshops.API.Graph.Lists;
using BoardsWorkshops.API.Graph.Users;
using BoardsWorkshops.API.Identity;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Interceptors;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

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
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddGraphQLSubscriptions();
            services.AddInMemorySubscriptionProvider();
            services.AddDataLoaderRegistry();

            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true,
                            ValidAudience = "audience",
                            ValidIssuer = "issuer",
                            RequireSignedTokens = false,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding
                                        .UTF8
                                        .GetBytes(
                                            "secretsecretsecret"
                                        )
                                )
                        };

                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                    }
                );
            
            services.AddAuthorization();
            services.AddHttpContextAccessor();

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<Query>()
                    .AddMutationType<MutationType>()
                    .AddSubscriptionType<Subscription>()
                    .AddType<MeMutationsType>()
                    .AddType<GetTokenMutationType>()
                    .AddType<TaskListType>()
                    .AddType<CardType>()
                    .AddType<UserType>()
                    .AddAuthorizeDirectiveType()
                    .Create()
            );


            services.AddCors(
                x =>
                    x.AddPolicy(
                        "allowAll",
                        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                    )
            );

            services.AddQueryRequestInterceptor(AuthenticationInterceptor());
        }

        private static OnCreateRequestAsync AuthenticationInterceptor()
        {
            return (context, builder, token) =>
            {
                var user = context.GetUser();
                if (context.GetUser().Identity.IsAuthenticated)
                {
                    builder.SetProperty(
                        "UserId",
                        Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier))
                    );
                }

                return Task.CompletedTask;
            };
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BoardsContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();


            app.UseCors("allowAll");

            app.UseAuthentication();

            app.UseWebSockets();
            app.UseGraphQL();
            app.UseVoyager();
            app.UsePlayground();
            app.UseGraphQLSubscriptions();

            context.Database.Migrate();
            context.Database.EnsureCreated();
        }
    }
}