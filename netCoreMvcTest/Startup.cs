using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using netCoreMvcTest.Data;
using netCoreMvcTest.Ioc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using netCoreMvcTest.Email;
using netCoreMvcTest.Email.Templates;

namespace netCoreMvcTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            IocContainer.Configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //add send grid email sender
            services.AddSendGridEmailSender();

            //add general emailtemplate sender
            services.AddEmailTemplateSender();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // options.UseMySQL(Configuration["ConnectionString:DefaultConnection"]);
                options.UseMySql(IocContainer.Configuration["ConnectionString:DefaultConnection"]);
            });

            //add identity add coockie based authentication
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    //adds user store and role store from this context
                    //consumed by usermanager and rolemanager
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    //provider that generates unique passwords hashes for this like
                    //forgot password, phone numbrt etc..
                    .AddDefaultTokenProviders();

            //Auth policy and schemes
            //https://docs.microsoft.com/ru-ru/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-2.2&tabs=aspnetcore2x
            //add Jwt authentication
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = IocContainer.Configuration["Jwt:Issuer"],
                        ValidAudience = IocContainer.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IocContainer.Configuration["Jwt:SecretKey"]))
                    };

                    
                });
            //roles base authorization
            //https://stackoverflow.com/questions/42036810/asp-net-core-jwt-mapping-role-claims-to-claimsidentity

            //password policy
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = true;

            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //alter application coockie info
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(15);
            });


            services.AddMvc(options =>
            {
               
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            IocContainer.Provider1 = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
             IocContainer.Provider = app.ApplicationServices;
            //IocContainer.Provider = serviceProvider;
            app.UseAuthentication();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "aboutPage",
                    template: "more",
                    defaults: new { controller = "about", action = "TellMeMore" });
            });

           var context = serviceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated();
        }
    }
}
