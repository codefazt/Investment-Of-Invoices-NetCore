#pragma checksum "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\logout.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "323d2c08e155edda8d0283afd8c5a269bc48aabf"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Pages.Pages_logout), @"mvc.1.0.razor-page", @"/Pages/logout.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/logout.cshtml", typeof(TuFactoring.Pages.Pages_logout), null)]
namespace TuFactoring.Pages
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"323d2c08e155edda8d0283afd8c5a269bc48aabf", @"/Pages/logout.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55741ea7499337265b012a75cbf5db5b74f16222", @"/Pages/_ViewImports.cshtml")]
    public class Pages_logout : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\logout.cshtml"
  
    ViewData["Title"] = "logout";

#line default
#line hidden
            BeginContext(87, 21, true);
            WriteLiteral("\r\n<h1>logout</h1>\r\n\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoring.Pages.logoutModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Pages.logoutModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Pages.logoutModel>)PageContext?.ViewData;
        public TuFactoring.Pages.logoutModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
