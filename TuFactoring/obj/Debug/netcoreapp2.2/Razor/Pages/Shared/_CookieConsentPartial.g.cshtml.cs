#pragma checksum "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b6eb5447aa189dae5eba3c533c695b7b5047510f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Pages.Shared.Pages_Shared__CookieConsentPartial), @"mvc.1.0.view", @"/Pages/Shared/_CookieConsentPartial.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Pages/Shared/_CookieConsentPartial.cshtml", typeof(TuFactoring.Pages.Shared.Pages_Shared__CookieConsentPartial))]
namespace TuFactoring.Pages.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\_ViewImports.cshtml"
using TuFactoring;

#line default
#line hidden
#line 3 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\_ViewImports.cshtml"
using TuFactoring.Data;

#line default
#line hidden
#line 1 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml"
using Microsoft.AspNetCore.Http.Features;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b6eb5447aa189dae5eba3c533c695b7b5047510f", @"/Pages/Shared/_CookieConsentPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55741ea7499337265b012a75cbf5db5b74f16222", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Shared__CookieConsentPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(43, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml"
  
    var consentFeature = Context.Features.Get<ITrackingConsentFeature>();
    var showBanner = !consentFeature?.CanTrack ?? false;
    var cookieString = consentFeature?.CreateConsentCookie();

#line default
#line hidden
            BeginContext(248, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 9 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml"
 if (showBanner)
{

#line default
#line hidden
            BeginContext(271, 102, true);
            WriteLiteral("    <div id=\"cookieConsent\" class=\"alert text-center cookiealert fade show\" role=\"alert\">\r\n        <b>");
            EndContext();
            BeginContext(374, 30, false);
#line 12 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml"
      Write(Localizer.Text("weUseCookies"));

#line default
#line hidden
            EndContext();
            BeginContext(404, 204, true);
            WriteLiteral("</b><!--<a asp-page=\"/Privacy\" target=\"_blank\">Learn More</a>.-->\r\n\r\n        <button type=\"button\" class=\"btn btn-primary btn-sm acceptcookies\" data-dismiss=\"alert\" aria-label=\"Close\" data-cookie-string=\"");
            EndContext();
            BeginContext(609, 12, false);
#line 14 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml"
                                                                                                                                  Write(cookieString);

#line default
#line hidden
            EndContext();
            BeginContext(621, 16, true);
            WriteLiteral("\">\r\n            ");
            EndContext();
            BeginContext(638, 31, false);
#line 15 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml"
       Write(Localizer.Text("acceptCookies"));

#line default
#line hidden
            EndContext();
            BeginContext(669, 349, true);
            WriteLiteral(@"
        </button>
    </div>
    <script>
        (function () {
            var button = document.querySelector(""#cookieConsent button[data-cookie-string]"");
            button.addEventListener(""click"", function (event) {
                document.cookie = button.dataset.cookieString;
            }, false);
        })();
    </script>
");
            EndContext();
#line 26 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_CookieConsentPartial.cshtml"
}

#line default
#line hidden
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Utilities.CultureLocalizer Localizer { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
