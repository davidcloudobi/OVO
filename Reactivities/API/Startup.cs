using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Middleware;
using Application.Activities;
using Application.Interfaces;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            //################################### ConnectionString  ##################################################

            services.AddDbContext<DataContext> (opt => {
                opt.UseSqlite (Configuration.GetConnectionString ("DefaultConnection"));
            });

            //################################### CLOSE ConnectionString  ##################################################


            //################################### CorsPolicy FRONTEND ##################################################

            services.AddCors (opt => {
                opt.AddPolicy ("CorsPolicy", policy => {
                    policy.AllowAnyHeader ().AllowAnyMethod ().WithOrigins ("http://localhost:3000");
                });
            });

            //###################################CLOSE  CorsPolicy FRONTEND ##################################################

            //################################### EntityFrameworkStores ##################################################



            //var builder = services.AddIdentityCore<AppUser>();
            //var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            //identityBuilder.AddEntityFrameworkStores<DataContext>();
            //identityBuilder.AddSignInManager<SignInManager<AppUser>>();

            //################################### EntityFrameworkStores ##################################################

            services.AddDefaultIdentity<AppUser>()
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager<SignInManager<AppUser>>();

            //################################### CLOSE EntityFrameworkStores ##################################################


            //################################### Authentication ##################################################

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

                              //#### DEPENDENCE INJECTION FOR JWT GENERATOR Authentication---- and---- TO GET USERNAME FROM TOKEN ####

                                   services.AddScoped<IJwtGenerator, JwtGenerator>();
                                   services.AddScoped<IUserAccessor, UserAccessor>();
                                 

            //################################### CLOSE Authentication ##################################################



            //services.AddIdentity<AppUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DataContext>();

            //################################### Mediator ##################################################

            services.AddMediatR(typeof(List.Handler).Assembly);

            //################################### CLOSE Mediator ##################################################

            services.AddControllers ();


            //################################### Model Validator ##################################################

            services.AddMvc(opt =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    opt.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddFluentValidation(cfg =>
                    cfg.RegisterValidatorsFromAssemblyContaining<Create>()
                    )
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            //################################### CLOSE Model Validator ##################################################
        }
         
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment ()) {
               // app.UseDeveloperExceptionPage ();
            }

            //app.UseHttpsRedirection();

            app.UseRouting ();
            //####### Authentication ######

            app.UseAuthentication();

            //####### CLOSE Authentication ######

            app.UseAuthorization();

            app.UseCors ("CorsPolicy");

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}