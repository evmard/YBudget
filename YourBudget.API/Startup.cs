using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;

using YourBudget.API.Models;
using YourBudget.API.Services;
using YourBudget.API.Utils;
using YourBudget.API.Utils.Auth;

namespace YourBudget.API
{
    public class Startup
    {
        private const string ApiTitle = "Your budget API";
        private const string DebugCors = "local_debug_cors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStr = Configuration.GetConnectionString("BudgetDb");
            services.AddDbContext<BudgetContext>(options => options.UseSqlServer(connectionStr));            

            services.AddScoped<Localizer>();
            services.AddTransient<UserService>();
            services.AddTransient<BudgetService>();

            #region Auth
            string issuer = Configuration.GetValue<string>("Issuer");
            string audience = Configuration.GetValue<string>("Audience");
            string key = Configuration.GetValue<string>("Key");
            var jwtOptions = new AuthJWTOptions(issuer, audience, key, 10);
            services.AddSingleton(jwtOptions);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtOptions.Issuer,

                            ValidateAudience = true,
                            ValidAudience = jwtOptions.Audience,

                            ValidateLifetime = true,

                            IssuerSigningKey = jwtOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });
            #endregion

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddMvcOptions(config => config.EnableEndpointRouting = false); 

            services.AddSwaggerDocument(config =>
            {
                config.Title = ApiTitle;
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
                config.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("JWT token", new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Name = "Authorization",
                        Description = "Copy 'Bearer ' + valid JWT token into field",
                        TokenUrl = "/api/User/Token"

                    }));
            });

            services.AddCors(option =>
            {
                option.AddPolicy(DebugCors, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(DebugCors);
            }
            else
            {                
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseAuthentication();
            app.UseMvc();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
        }
    }
}
