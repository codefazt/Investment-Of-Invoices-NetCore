#pragma checksum "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_NavigationPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "716cdc789cf669a9ddc91d2f54036e5c4fbeb2aa"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Pages.Shared.Pages_Shared__NavigationPartial), @"mvc.1.0.view", @"/Pages/Shared/_NavigationPartial.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Pages/Shared/_NavigationPartial.cshtml", typeof(TuFactoring.Pages.Shared.Pages_Shared__NavigationPartial))]
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
#line 1 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\_ViewImports.cshtml"
using TuFactoring;

#line default
#line hidden
#line 3 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\_ViewImports.cshtml"
using TuFactoring.Data;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"716cdc789cf669a9ddc91d2f54036e5c4fbeb2aa", @"/Pages/Shared/_NavigationPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55741ea7499337265b012a75cbf5db5b74f16222", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Shared__NavigationPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_LoginPartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("name", "_LanguagePartial", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 185, true);
            WriteLiteral("<nav class=\"navbar navbar-expand-lg navbar-light fixed-top\" id=\"mainNav\">\r\n    <div class=\"container\">\r\n        <a class=\"navbar-brand js-scroll-trigger\" href=\"#page-top\">\r\n            ");
            EndContext();
            BeginContext(186, 26, false);
#line 4 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_NavigationPartial.cshtml"
       Write(Localizer.Text("branding"));

#line default
#line hidden
            EndContext();
            BeginContext(212, 532, true);
            WriteLiteral(@"
        </a>
        <button class=""navbar-toggler navbar-toggler-right"" type=""button"" data-toggle=""collapse"" data-target=""#navbarResponsive"" aria-controls=""navbarResponsive"" aria-expanded=""false"" aria-label=""Toggle navigation"">
            <i class=""fas fa-bars""></i>
        </button>
        <div class=""collapse navbar-collapse"" id=""navbarResponsive"">
            <ul class=""navbar-nav ml-auto"">
                <li class=""nav-item d-lg-none"">
                    <a class=""nav-link js-scroll-trigger"" href=""#page-top"">");
            EndContext();
            BeginContext(745, 24, false);
#line 12 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_NavigationPartial.cshtml"
                                                                      Write(Localizer.Text("Inicio"));

#line default
#line hidden
            EndContext();
            BeginContext(769, 143, true);
            WriteLiteral("</a>\r\n                </li>\r\n                <li class=\"nav-item\">\r\n                    <a class=\"nav-link js-scroll-trigger\" href=\"#about_us\">");
            EndContext();
            BeginContext(913, 31, false);
#line 15 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_NavigationPartial.cshtml"
                                                                      Write(Localizer.Text("Quienes somos"));

#line default
#line hidden
            EndContext();
            BeginContext(944, 151, true);
            WriteLiteral("</a>\r\n                </li>\r\n                <li class=\"nav-item\">\r\n                    <a class=\"nav-link js-scroll-trigger\" href=\"#how_does_it_work\">");
            EndContext();
            BeginContext(1096, 31, false);
#line 18 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_NavigationPartial.cshtml"
                                                                              Write(Localizer.Text("Como funciona"));

#line default
#line hidden
            EndContext();
            BeginContext(1127, 143, true);
            WriteLiteral("</a>\r\n                </li>\r\n                <li class=\"nav-item\">\r\n                    <a class=\"nav-link js-scroll-trigger\" href=\"#services\">");
            EndContext();
            BeginContext(1271, 27, false);
#line 21 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\_NavigationPartial.cshtml"
                                                                      Write(Localizer.Text("Servicios"));

#line default
#line hidden
            EndContext();
            BeginContext(1298, 62, true);
            WriteLiteral("</a>\r\n                </li>\r\n            </ul>\r\n\r\n            ");
            EndContext();
            BeginContext(1360, 32, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "716cdc789cf669a9ddc91d2f54036e5c4fbeb2aa7470", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1392, 65, true);
            WriteLiteral("\r\n\r\n            <ul class=\"navbar-nav ml-auto\">\r\n                ");
            EndContext();
            BeginContext(1457, 35, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("partial", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "716cdc789cf669a9ddc91d2f54036e5c4fbeb2aa8796", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.PartialTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_PartialTagHelper.Name = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1492, 57, true);
            WriteLiteral("\r\n            </ul>\r\n        </div>\r\n    </div>\r\n</nav>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
