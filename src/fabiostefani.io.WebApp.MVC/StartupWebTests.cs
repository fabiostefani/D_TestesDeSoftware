using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using fabiostefani.io.WebApp.MVC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using fabiostefani.io.Catalogo.Data;
using fabiostefani.io.Vendas.Data;
using fabiostefani.io.Catalogo.Application.AutoMapper;
using Microsoft.AspNetCore.Http;
using MediatR;
using fabiostefani.io.WebApp.MVC.Setup;
using Microsoft.AspNetCore.Mvc;

namespace fabiostefani.io.WebApp.MVC
{
    public class StartupWebTests
    {
        public StartupWebTests(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;                 
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<CatalogoContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<VendasContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
               //.AddDefaultUI(UIFramework.Bootstrap4)
               .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpContextAccessor();

            // services.AddSwaggerGen(c =>
            // {
            //    var security = new Dictionary<string, IEnumerable<string>>
            //    {
            //        {"Bearer", new string[] { }}
            //    };

            //    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //    {
            //        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
            //        Name = "Authorization",
            //        In = "header",
            //        Type = "apiKey"
            //    });

            //    c.AddSecurityRequirement(security);

            //    c.SwaggerDoc("v1", new Info
            //    {
            //        Version = "v1",
            //        Title = "desenvolvedor.io API",
            //        Description = "desenvolvedor.io  API",
            //        TermsOfService = "Nenhum",
            //        Contact = new Contact { Name = "desenvolvedor.io", Email = "email@desenvolvedor.io", Url = "http://desenvolvedor.io" },
            //        License = new License { Name = "MIT", Url = "http://desenvolvedor.io/licensa" }
            //    });
            // });
            
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();

            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
            services.AddMediatR(typeof(Startup));
            services.RegisterServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
             if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            // app.UseSwagger();
            // app.UseSwaggerUI(s =>
            // {
            //    s.SwaggerEndpoint("/swagger/v1/swagger.json", "desenvolvedor.io API v1.0");
            // });    

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Vitrine}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
            });
        }
    }
}
