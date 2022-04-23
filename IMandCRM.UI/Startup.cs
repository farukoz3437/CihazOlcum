using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;
using IMandCRM.UI.EmailServices;
using IMandCRM.UI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //identity ayarlarý
      
           
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(@"Data Source=LAPTOP-Q5KQP3V5\SQLEXPRESS;Initial Catalog=Measurement;persist security info=True;user id=qbot;Password=123;"));
            


            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = true; //parola içinde sayýsal deðer olmalýdýr
                options.Password.RequireLowercase = true; // parola içinde küçük harf olmak zorunda
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8; // parola min. kaç karakter olacaðý
                options.Password.RequireNonAlphanumeric = true; // parola içinde bir karakter olmalýdýr

                //Locaout
                options.Lockout.MaxFailedAccessAttempts = 5; //parolanýn max 5 defa yanlýþ girebilir
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                /*options.User.AllowedUserNameCharacters = "";*///user içinde olmasýný istediðiniz karakterler
                options.User.RequireUniqueEmail = true; // ayný mail adresinden iki kullanýcý olamaz
                options.SignIn.RequireConfirmedEmail = true; ; //kullanýcýya onay maili gönderilmesi
                options.SignIn.RequireConfirmedPhoneNumber = false;


            });

            //cookie ayarlarý

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true; //cookie nin yaþam süresini belirler
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".rubusoft.security.cookie",
                    SameSite = SameSiteMode.Strict
                };

            });

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Dependency Injection

            services.AddScoped<IMeasurementDal, MeasurementDal>();
            services.AddScoped<IMeasurementService, MeasurementService>();

            services.AddScoped<IAspNetUserDal, AspNetUserDal>();
            services.AddScoped<IAspNetUserService, AspNetUserService>();

            services.AddScoped<IArrayDesignDal, ArrayDesignDal>();
            services.AddScoped<IArrayDesignService, ArrayDesignService>();

            services.AddScoped<IArrayMeasurementDal, ArrayMeasurementDal>();
            services.AddScoped<IArrayMeasurementService, ArrayMeasurementService>();

            //email injection
            services.AddScoped<IEmailSender, EmailSender>(i => new EmailSender(
                Configuration["EmailSender:Host"],
                Configuration.GetValue<int>("EmailSender:Port"),
                Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                Configuration["EmailSender:UserName"],
                Configuration["EmailSender:Password"]
                ));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("es"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // Localized UI strings.
                SupportedUICultures = supportedCultures
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "/",
                    defaults: new { controller = "Account", action = "Login" });

                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "DeleteUser", pattern: "{controller=Account}/{action=UserDelete}/{id?}");

                endpoints.MapControllerRoute(name: "MeasurementDetail", pattern: "{controller=Measurement}/{action=MeasurementDetail}/{idKod?}");

                endpoints.MapControllerRoute(name: "MeasurementEdit", pattern: "{controller=Measurement}/{action=MeasurementEdit}/{idKod?}/{userId?}");

                endpoints.MapControllerRoute(name: "MeasurementDelete", pattern: "{controller=Measurement}/{action=MeasurementDelete}/{idKod?}/{userId?}");

                endpoints.MapControllerRoute(name: "DownloadZip", pattern: "{controller=Measurement}/{action=DownloadZip}/{idKod?}");

                endpoints.MapControllerRoute(name: "ArrayMeasurements", pattern: "{controller=ArrayMeasurement}/{action=ArrayMeasurements}/{arrayDesignIdKod?}");

                endpoints.MapControllerRoute(name: "ArrayMeasurementEdit", pattern: "{controller=ArrayMeasurement}/{action=ArrayMeasurementEdit}/{idKod?}/{userId?}");

                endpoints.MapControllerRoute(name: "ArrayMeasurementDetail", pattern: "{controller=ArrayMeasurement}/{action=ArrayMeasurementDetail}/{idKod?}");

                endpoints.MapControllerRoute(name: "ArrayMeasurementLocation", pattern: "{controller=ArrayMeasurement}/{action=Location}/{arrayIdKod?}");

                endpoints.MapControllerRoute(name: "DownloadZip", pattern: "{controller=ArrayMeasurement}/{action=DownloadZip}/{idKod?}");

                endpoints.MapControllerRoute(name: "ArrayDesignDelete", pattern: "{controller=Arraydesign}/{action=ArrayDesignDelete}/{idKod?}/{userId?}");
            });
            SeedIdentity.Seed(userManager, roleManager, configuration).Wait();
        }
    }
}
