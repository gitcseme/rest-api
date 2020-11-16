using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TwitterBook2.Data;
using Microsoft.AspNetCore.Mvc;
using TwitterBook2.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using TwitterBook2.Services;
using TwitterBook2.Authorization;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FluentValidation.AspNetCore;
using TwitterBook2.Filters;
using TwitterBook2.Extensions;

namespace TwitterBook2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddCors();

            services.AddControllers();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TagViewer",
                    builder => builder.RequireClaim("tags.view", "true"));

                options.AddPolicy("MustWorkForTourBD", policy =>
                {
                    policy.AddRequirements(new WorksForCompanyRequirement("tourBD.com"));
                });
            });

            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();

            // JWT settings
            services.AddJwt(Configuration);
            // Swagger settings
            services.AddSwagger();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IIdentityService, IdentityService>();

            services
                .AddMvc(options => 
                {
                    options.Filters.Add<ValidationFilter>();
                })
                .AddFluentValidation(mvcConf => mvcConf.RegisterValidatorsFromAssemblyContaining<Startup>()); // Fluent validation
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(options => { options.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(c => c
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
