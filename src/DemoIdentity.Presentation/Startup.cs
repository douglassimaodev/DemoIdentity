using DemoIdentity.IdentityIsolated.ContextConfiguration;
using DemoIdentity.IdentityIsolated.Entities;
using DemoIdentity.IdentityIsolated.Repository;
using DemoIdentity.IdentityIsolated.Repository.Interfaces;
using DemoIdentity.Presentation.Data;
using DemoIdentity.Presentation.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoIdentity.Presentation
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.

                //Here comes the change:
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("DemoIdentity.Presentation")));
            //Quanto se tem mais de um projeto tem que especificar o nome onde quer salvar as migrations

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<CustomSignInManager>();
            //CustomSignInManager para forçar a applicacao a se relogar 
            //recarregando as clains customizadas

            #region Dependency Injection

            services.AddTransient<CustomSignInManager>();
            services.AddScoped<ISecurityRepository, SecurityRepository>();

            #endregion

            //Nao add nenhum valor as minhas clains, so valido por ter elas ou nao
            const string defaultClaimValue = "_";
            // api user claim policy
            services.AddAuthorization(options =>
            {           

                options.AddPolicy(PermissionHelper.Create, policy => policy.RequireClaim(PermissionHelper.Create, defaultClaimValue));
                options.AddPolicy(PermissionHelper.Read, policy => policy.RequireClaim(PermissionHelper.Read, defaultClaimValue));
                options.AddPolicy(PermissionHelper.Update, policy => policy.RequireClaim(PermissionHelper.Update, defaultClaimValue));
                options.AddPolicy(PermissionHelper.Delete, policy => policy.RequireClaim(PermissionHelper.Delete, defaultClaimValue));
                options.AddPolicy(PermissionHelper.PodeVerBotaoGlobal, policy => policy.RequireClaim(PermissionHelper.PodeVerBotaoGlobal, defaultClaimValue));
                options.AddPolicy(PermissionHelper.PodeVerAdminPage, policy => policy.RequireClaim(PermissionHelper.PodeVerAdminPage, defaultClaimValue));

            });

            var serviceProvider = services.BuildServiceProvider();

            //here is where you set you accessor
            var accessor = serviceProvider.GetService<IHttpContextAccessor>();
            PermissionHelper.SetHttpContextAccessor(accessor);

            //Da um Nome ao "ant falsidade"
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-Token");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
                IApplicationBuilder app, 
                IHostingEnvironment env,
                ApplicationDbContext applicationDbContext,
                UserManager<ApplicationUser> userManager,
                RoleManager<ApplicationRole> roleManager
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();

            //Executa o migration no banco caso nao tenha sido rodado
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var appContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                appContext.Database.Migrate();               
            }

            // Alimenta o banco de dados com informacoes
            DbSeeder.DbInit(applicationDbContext, userManager, roleManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
