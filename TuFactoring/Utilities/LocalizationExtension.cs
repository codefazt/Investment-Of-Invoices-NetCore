using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;

namespace TuFactoring.Utilities
{
    public static class LocalizationExtension
    {
        /// <summary>
        /// localize request according to {culture} route value.
        /// define supported cultures list, 
        /// define default culture for fallback,
        /// customize culture info e.g.: dat time format, digit shape,...
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRequestLocalization(this IServiceCollection services)
        {
            var cultures = new CultureInfo[]
            {
                new CultureInfo("en"),
                new CultureInfo("es-AR"),
                new CultureInfo("es-VE"),
                new CultureInfo("es-DO"),
            };

            services.Configure<RequestLocalizationOptions>(ops =>
            {
                ops.DefaultRequestCulture = new RequestCulture("es-AR");
                ops.SupportedCultures = cultures.OrderBy(x => x.EnglishName).ToList();
                ops.SupportedUICultures = cultures.OrderBy(x => x.EnglishName).ToList();

                // add RouteValueRequestCultureProvider to the beginning of the providers list. 
                ops.RequestCultureProviders.Insert(0,
                    new RouteValueRequestCultureProvider(cultures));
            });
        }
    }
}
