using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using TuFactoring.Data;
using TuFactoring.CustomProviders;
using TuFactoring.Services;
using TuFactoring.Utilities;
using GraphQL.Client;
using TuFactoringGraphql;
using System.Reflection;
using TuFactoring.Resources;
using System.Globalization;
using TuFactoringModels;
using TuFactoring.WebSocket;

namespace TuFactoring
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
            services.ConfigureApplicationCookie(ops =>
            {
                InitialConfigurations.SetTimeSesion(10);
                // Cookie settings
                ops.Cookie.HttpOnly = false;
                ops.ExpireTimeSpan = TimeSpan.FromMinutes(InitialConfigurations.GetTimeSesion());

                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                ops.LoginPath = $"/{CultureInfo.CurrentCulture.Name}/Identity/Account/Login";

                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                ops.AccessDeniedPath = $"/{CultureInfo.CurrentCulture.Name}/Identity/Account/Logout";

                ops.SlidingExpiration = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<CultureLocalizer>();
            services.ConfigureRequestLocalization();

            services.AddSession();
            services.AddMemoryCache();
            //Configuracion de las politicas y los roles
            services.AddAuthorization(config => {
                #region Profile
                config.AddPolicy("PolicyProfile", policy => policy.RequireRole("OFFICER","SEGMENTATION","EXECUTIVE","TREASURER","GUEST", "PAYER", "OPERATIONSTREASURER", "ACCOUNTANT", "ADMINISTRATOR", "LEGAL", "MANAGER"));
                config.AddPolicy("PolicyProfileChange", policy => policy.RequireRole("ADMINISTRATOR", "LEGAL", "MANAGER"));
                config.AddPolicy("PolicyContracts", policy => policy.RequireRole("LEGAL"));
                config.AddPolicy("PolicySecurity", policy => policy.RequireRole("ADMINISTRATOR", "LEGAL", "MANAGER"));
                #endregion

                #region Debtor
                config.AddPolicy("RequiredDebtor", policy => policy.RequireRole("DEBTOR"));
                config.AddPolicy("PolicyLoadInvoiceDebtor", policy => policy.RequireRole("MANAGER","PAYER"));
                config.AddPolicy("PolicyEditInvoiceDebtor", policy => policy.RequireRole("MANAGER", "PAYER"));
                config.AddPolicy("PolicyPostedDebtor", policy => policy.RequireRole("MANAGER", "TREASURER"));
                config.AddPolicy("PolicyFinancingDebtor", policy => policy.RequireRole("MANAGER", "TREASURER"));
                config.AddPolicy("PolicyConfirmingDebtor", policy => policy.RequireRole("MANAGER", "TREASURER"));
                #endregion

                #region Supplier
                config.AddPolicy("RequiredSupplier", policy => policy.RequireRole("SUPPLIER"));
                    #region Publication Page
                    config.AddPolicy("PolicyPagePublishedSupplier", policy => policy.RequireRole("MANAGER", "TREASURER", "PAYER"));
                    config.AddPolicy("PolicyAcceptOfferBankSupplier", policy => policy.RequireRole("MANAGER", "TREASURER"));
                    config.AddPolicy("PolicyPublishedInvoiceSupplier", policy => policy.RequireRole("MANAGER", "TREASURER", "PAYER"));
                    #endregion
                config.AddPolicy("PolicyOverdueInvoiceSupplier", policy => policy.RequireRole("MANAGER", "TREASURER", "PAYER"));
                config.AddPolicy("PolicyLoadInvoiceSupplier", policy => policy.RequireRole("MANAGER", "PAYER"));
                config.AddPolicy("PolicyEditInvoiceSupplier", policy => policy.RequireRole("MANAGER", "PAYER"));
                config.AddPolicy("PolicyPostedSupplier", policy => policy.RequireRole("MANAGER", "TREASURER"));
                config.AddPolicy("PolicyAuctionSupplier", policy => policy.RequireRole("LEGAL","MANAGER", "TREASURER", "PAYER", "GUEST"));
                config.AddPolicy("PolicyCloseAuctionSupplier", policy => policy.RequireRole("MANAGER", "TREASURER"));
                #endregion

                #region Confirmant
                config.AddPolicy("RequiredConfirmant", policy => policy.RequireRole("CONFIRMANT"));
                config.AddPolicy("PolicyRegisterDebtorConfirmant", policy => policy.RequireRole("ADMINISTRATOR", "LEGAL", "MANAGER","EXECUTIVE"));
                config.AddPolicy("PolicyVerifyConfirmant", policy => policy.RequireRole("MANAGER", "OFFICER"));
                config.AddPolicy("PolicyReviewInvoice", policy => policy.RequireRole("MANAGER", "TREASURER"));
                config.AddPolicy("PolicyAssignExecutiveConfirmant", policy => policy.RequireRole("MANAGER","SEGMENTATION"));
                config.AddPolicy("PolicyAssignRiskConfirmant", policy => policy.RequireRole("MANAGER","EXECUTIVE"));
                config.AddPolicy("PolicyConfirmInvoiceConfirmant", policy => policy.RequireRole("MANAGER","EXECUTIVE"));
                config.AddPolicy("PolicyBuyInvoiceConfirmant", policy => policy.RequireRole("MANAGER","TREASURER"));
                config.AddPolicy("PolicyPaidInvoiceConfirmant", policy => policy.RequireRole("MANAGER", "OPERATIONSTREASURER"));
                config.AddPolicy("PolicyPaidConciliedConfirmant", policy => policy.RequireRole("MANAGER", "OPERATIONSTREASURER"));
                config.AddPolicy("PolicyPaidInvoiceOverdueConfirmant", policy => policy.RequireRole("MANAGER", "OPERATIONSTREASURER"));
                #endregion

                #region Factor
                config.AddPolicy("RequiredFactor", policy => policy.RequireRole("FACTOR"));
                config.AddPolicy("PolicyAuctionFactor", policy => policy.RequireRole("TREASURER","DPERSON"));
                config.AddPolicy("PolicyPaidFactor", policy => policy.RequireRole("TREASURER","DPERSON"));
                #endregion

                #region Backoffice
                config.AddPolicy("RequiredBackoffice", policy => policy.RequireRole("BACKOFFICE"));
                #endregion
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddViewLocalization(o => o.ResourcesPath = "Resources")
                .AddModelBindingMessagesLocalizer(services)

                // Option A: use this for localization with shared resource
                .AddDataAnnotationsLocalization(o => {
                    var type = typeof(ViewResource);
                    var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
                    var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var localizer = factory.Create("ViewResource", assemblyName.Name);
                    o.DataAnnotationLocalizerProvider = (t, f) => localizer;
                })

                // Option B: use this for localization by view specific resource
                //.AddDataAnnotationsLocalization() 
                .AddRazorPagesOptions(o => {
                    o.Conventions.Add(new CultureTemplateRouteModelConvention());
                    o.AllowAreas = true;

                    #region Profile
                    o.Conventions.AuthorizeAreaFolder("Profile", "/");
                    o.Conventions.AuthorizeAreaPage("Profile", "/ActualizarEmpresa", "PolicyProfile");
                    o.Conventions.AuthorizeAreaPage("Profile", "/MantenimientoUsuarios", "PolicySecurity");
                    #endregion

                    #region Debtor
                    o.Conventions.AuthorizeAreaFolder("Debtor", "/","RequiredDebtor");
                    o.Conventions.AuthorizeAreaPage("Debtor", "/CargaMasiva", "PolicyLoadInvoiceDebtor");
                    o.Conventions.AuthorizeAreaPage("Debtor", "/CargaManual", "PolicyEditInvoiceDebtor");
                    o.Conventions.AuthorizeAreaPage("Debtor", "/PostularFacturas", "PolicyPostedDebtor");
                    o.Conventions.AuthorizeAreaPage("Debtor", "/FinanciamientoFacturas", "PolicyFinancingDebtor");
                    o.Conventions.AuthorizeAreaPage("Debtor", "/ConfirmarFacturas", "PolicyConfirmingDebtor");
                    #endregion

                    #region Confirmant
                    o.Conventions.AuthorizeAreaFolder("Confirmant","/", "RequiredConfirmant"); 
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/Account/Register", "PolicyRegisterDebtorConfirmant");
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/VerificarDatos", "PolicyVerifyConfirmant");
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/Segmentacion", "PolicyAssignExecutiveConfirmant"); 
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/EjecutivoCuentas", "PolicyAssignRiskConfirmant"); 
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/ConfirmarFacturas", "PolicyConfirmInvoiceConfirmant"); 
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/CompraFactura", "PolicyBuyInvoiceConfirmant"); 
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/PagoFacturas", "PolicyPaidInvoiceConfirmant");
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/FacturasConciliadas", "PolicyPaidConciliedConfirmant");
                    o.Conventions.AuthorizeAreaPage("Confirmant", "/FacturasVencidas", "PolicyPaidInvoiceOverdueConfirmant");
                    #endregion

                    #region Supplier
                    o.Conventions.AuthorizeAreaFolder("Supplier", "/", "RequiredSupplier"); 
                    o.Conventions.AuthorizeAreaPage("Supplier", "/PublicarFacturas", "PolicyPagePublishedSupplier"); 
                    o.Conventions.AuthorizeAreaPage("Supplier", "/FacturasVencimiento", "PolicyOverdueInvoiceSupplier"); 
                    o.Conventions.AuthorizeAreaPage("Supplier", "/MercadoFacturas", "PolicyAuctionSupplier");
                    o.Conventions.AuthorizeAreaPage("Supplier", "/CierreMercado", "PolicyCloseAuctionSupplier");
                    #endregion

                    #region Factor
                    o.Conventions.AuthorizeAreaFolder("Factor", "/", "RequiredFactor");
                    o.Conventions.AuthorizeAreaPage("Factor", "/MercadoFacturas", "PolicyAuctionFactor");
                    o.Conventions.AuthorizeAreaPage("Factor", "/PagoFacturas", "PolicyPaidFactor");
                    #endregion

                    o.Conventions.AuthorizeAreaFolder("Backoffice", "/", "RequiredBackoffice");
                });

            // Add identity types
            services.AddIdentity<User, Role>(
                config => {
                    //config.SignIn.RequireConfirmedEmail = true;
                    //config.User.RequireUniqueEmail = true;
                    // Password requirements
                    config.Password.RequiredLength = 8;
                    config.Password.RequireDigit = true;
                    config.Password.RequireUppercase = true;
                    config.Password.RequireLowercase = true;
                    config.Password.RequireNonAlphanumeric = true;
                }).AddDefaultUI(UIFramework.Bootstrap4)
                .AddDefaultTokenProviders()
                .AddClaimsPrincipalFactory<CustomClaimsStore>();

            // Identity Services
            services.AddTransient<IUserStore<User>, CustomUserStore>();
            services.AddTransient<IRoleStore<Role>, CustomRoleStore>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IAuctionService, AuctionService>();
            services.AddSingleton<IGlobalService, GlobalService>();
            services.AddSingleton<IPeopleService, PeopleService>();
            services.AddSingleton<IInvoiceService, InvoiceService>();
            services.AddSingleton<IPaymentService, PaymentService>();

            // Services
            services.AddSingleton(t => new GraphQLClient(Configuration["GraphQlEndpoint"]));
            services.AddSingleton<GlobalConsumer>();
            services.AddSingleton<AuthConsumer>();
            services.AddSingleton<PeopleConsumer>();
            services.AddSingleton<InvoiceConsumer>();
            services.AddSingleton<AuctionConsumer>();
            services.AddSingleton<PaymentConsumer>();

            services.AddSignalR();
            services.AddProgressiveWebApp();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();

            app.UseSignalR(routes =>
            {
                routes.MapHub<SocketSubastas>("/wsSubastas");
                routes.MapHub<SocketUser>("/wsUser");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}");
            });
        }
    }
}
