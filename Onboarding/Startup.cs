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

namespace Onboarding
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime.Use this method to add services to the container.

        // Added for docker support

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //for docker 

           var connection = @"Server=db;Database=OnboardingContext;User=sa;Password=YourStrongP@ssword;";

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

            if (Environment.IsEnvironment("Testing"))
            {
                services.AddDbContext<OnboardingContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("OnboardingContext")));
                services.AddDbContext<OnboardingContext>(options =>
                    options.UseInMemoryDatabase("TestingDB"));
            }
            else
            {
                services.AddDbContext<OnboardingContext>(options =>
                   options.UseSqlServer(connection));
            }



            services.AddDbContext<OnboardingContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("OnboardingContext")));
            //services.AddS
            services.AddTransient<IOnboardingService , OnboardService>();
            services.AddSingleton<IJWTTokenService, JWTTokenService>();
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

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/api/onboarding/login" || context.Request.Path == "/api/onboarding/create/workspace" || context.Request.Path == "/api/onboarding/create/workspace/email" || context.Request.Path == "/api/onboarding/workspacedetails" || context.Request.Path == "/api/onboarding/verify" || context.Request.Path == "/api/onboarding/invite/verify")
            //    {
            //        await next();
            //    }
            //    Chilkat.Jwt jwt = new Chilkat.Jwt();

            //    using (var client = new ConsulClient())
            //    {

            //        var getPair = await client.KV.Get("secretkey");
            //        string token = context.Request.Headers["Authorization"];
            //        if (token != null)
            //        {
            //            var x = token.Replace("Bearer ", "");

            //            Chilkat.Rsa rsaPublicKey = new Chilkat.Rsa();
            //            rsaPublicKey.ImportPublicKey(Encoding.UTF8.GetString(getPair.Response.Value));
            //            var isTokenVerified = jwt.VerifyJwtPk(x, rsaPublicKey.ExportPublicKeyObj());
            //            if (isTokenVerified)
            //            {
            //                await next();
            //            }
            //        }
            //    }
            //});

            // app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //for docker 

            var context = app.ApplicationServices.GetService<OnboardingContext>();
            context.Database.Migrate();
            //app.UseCors("AllowSpecificOrigin");
            app.UseCors("AppPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
