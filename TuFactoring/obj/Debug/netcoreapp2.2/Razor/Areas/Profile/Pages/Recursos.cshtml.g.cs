#pragma checksum "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Recursos.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "605b02f6e4f2d6b8f56ef2fce7b1d4e62ce3354e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Areas.Profile.Pages.Areas_Profile_Pages_Recursos), @"mvc.1.0.razor-page", @"/Areas/Profile/Pages/Recursos.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Areas/Profile/Pages/Recursos.cshtml", typeof(TuFactoring.Areas.Profile.Pages.Areas_Profile_Pages_Recursos), null)]
namespace TuFactoring.Areas.Profile.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"605b02f6e4f2d6b8f56ef2fce7b1d4e62ce3354e", @"/Areas/Profile/Pages/Recursos.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a0d876ac07db4560a63de31f86fcc7a6c15cfdef", @"/Areas/Profile/Pages/_ViewImports.cshtml")]
    public class Areas_Profile_Pages_Recursos : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(68, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
            BeginContext(73, 13, false);
#line 7 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Recursos.cshtml"
Write(Model.version);

#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoring.Areas.Profile.Pages.RecursosModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Profile.Pages.RecursosModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Profile.Pages.RecursosModel>)PageContext?.ViewData;
        public TuFactoring.Areas.Profile.Pages.RecursosModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
