using MessengerApp.BLL.Interfaces;
using MessengerApp.BLL.Services;
using MessengerApp.Bots.Interfaces;
using MessengerApp.Bots.Services;
using MessengerApp.DAL;
using MessengerApp.DAL.EF;
using MessengerApp.DAL.Intrefaces;
using MessengerApp.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MessengerApp
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
            services.AddControllers();

            var connectionString = Configuration.GetConnectionString("PostgreConnection");
            services.AddDbContext<MessengerAppContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUsersRepository, DatabaseUsersRepository>();
            services.AddScoped<IChatsRepository, DatabaseChatsRepository>();
            services.AddScoped<IMessagesRepository, DatabaseMessagesRepository>();
            services.AddScoped<IChatEventsRepository, DatabaseChatEventsRepository>();
            services.AddScoped<IBotsRepository, DatabaseBotsRepository>();

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            services.AddSingleton<IMessagesForBotsManager, MessagesForBotsManager>();
            services.AddSingleton<IBackgroundMessagesDataQueue>(ctx =>
            {
                if (!int.TryParse(Configuration["QueueCapacity"], out var queueCapacity))
                    queueCapacity = 100;
                return new BackgroundMessagesDataQueue(queueCapacity);
            });
            services.AddHostedService<QueuedHostedService>(servProvider =>
            {
                if (!int.TryParse(Configuration["MaxBotsThreadsCount"], out var maxBotsThreadsCount))
                    maxBotsThreadsCount = 5;
                return new QueuedHostedService(servProvider.GetRequiredService<IBackgroundMessagesDataQueue>(), servProvider, maxBotsThreadsCount, Configuration["PathToBotsAPI"], Configuration["BackUrlToBots"]);
            });
            

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
          
            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
