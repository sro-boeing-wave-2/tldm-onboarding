using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Onboarding.Models;
using Onboarding.Contract;
using Onboarding.Services;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Onboarding
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.Use this method to add services to the container.

        // Added for docker support

        //public Startup(IConfiguration configuration, IHostingEnvironment environment)
        //{
        //    Configuration = configuration;
        //    Environment = environment;
        //}

        //public IConfiguration Configuration { get; }
        //public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //for docker 

            //var connection = @"Server=db;Database=OnboardingContext;User=sa;Password=YourStrongP@ssword;";

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            

            services.AddCors(o => o.AddPolicy("AppPolicy", builder =>
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowAnyOrigin()
                )
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //for docker 

            //if (Environment.IsEnvironment("Testing"))
            //{
            //    services.AddDbContext<OnboardingContext>(options =>
            //            options.UseSqlServer(Configuration.GetConnectionString("OnboardingContext")));
            //    services.AddDbContext<OnboardingContext>(options =>
            //        options.UseInMemoryDatabase("TestingDB"));
            //}
            //else
            //{
            //    services.AddDbContext<OnboardingContext>(options =>
            //       options.UseSqlServer(connection));
            //}

            services.AddAuthentication(
              options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(
              options =>
              {
                  options.SaveToken = true;
                  options.RefreshOnIssuerKeyNotFound = false;
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidAudience = "http://oec.com",
                      ValidIssuer = "http://oec.com",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"))
                  };
              }
              );

            services.AddDbContext<OnboardingContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("OnboardingContext")));

            services.AddTransient<IOnboardingService , OnboardService>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            { 
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //for docker 

            //var context = app.ApplicationServices.GetService<OnboardingContext>();
            //context.Database.Migrate();
            //app.UseCors("AllowSpecificOrigin");
            app.UseCors("AppPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
