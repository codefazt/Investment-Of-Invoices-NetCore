#pragma checksum "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0abfb3dc97f11dd3308af68b8ce73bcf22e42569"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Pages.Shared.Components.BackofficeWidgets.Pages_Shared_Components_BackofficeWidgets_Default), @"mvc.1.0.view", @"/Pages/Shared/Components/BackofficeWidgets/Default.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Pages/Shared/Components/BackofficeWidgets/Default.cshtml", typeof(TuFactoring.Pages.Shared.Components.BackofficeWidgets.Pages_Shared_Components_BackofficeWidgets_Default))]
namespace TuFactoring.Pages.Shared.Components.BackofficeWidgets
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0abfb3dc97f11dd3308af68b8ce73bcf22e42569", @"/Pages/Shared/Components/BackofficeWidgets/Default.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55741ea7499337265b012a75cbf5db5b74f16222", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Shared_Components_BackofficeWidgets_Default : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TuFactoringModels.nuevaVersion.Dashboard>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(49, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
 if (Model.Count > 0)
{

#line default
#line hidden
            BeginContext(77, 23, true);
            WriteLiteral("    <div class=\"row\">\r\n");
            EndContext();
#line 6 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
         foreach (var Widget in Model.list)
        {

#line default
#line hidden
            BeginContext(156, 34, true);
            WriteLiteral("            <!-- Content Row -->\r\n");
            EndContext();
#line 9 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
             if (Widget.Content == "COUNTER")
            {

#line default
#line hidden
            BeginContext(252, 421, true);
            WriteLiteral(@"                <div class=""col-xl-3 col-md-3 mb-4"">
                    <div class=""card border-left-warning shadow h-100 py-2"">
                        <div class=""card-body"">
                            <div class=""row no-gutters align-items-center"">
                                <div class=""col mr-2"">
                                    <div class=""text-xs font-weight-bold text-warning text-uppercase mb-1"">");
            EndContext();
            BeginContext(674, 35, false);
#line 16 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                                                                                      Write(Localizer.Text(Widget.Abbreviation));

#line default
#line hidden
            EndContext();
            BeginContext(709, 96, true);
            WriteLiteral("</div>\r\n                                    <div class=\"h5 mb-0 font-weight-bold text-gray-800\">");
            EndContext();
            BeginContext(806, 12, false);
#line 17 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                                                                   Write(Widget.Count);

#line default
#line hidden
            EndContext();
            BeginContext(818, 142, true);
            WriteLiteral("</div>\r\n                                </div>\r\n                                <div class=\"col-auto\">\r\n                                    <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 960, "\"", 980, 1);
#line 20 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
WriteAttributeValue("", 968, Widget.Icon, 968, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(981, 167, true);
            WriteLiteral("></i>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n");
            EndContext();
#line 26 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
            }

#line default
#line hidden
#line 27 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
             if (Widget.Content == "AMOUNT")
            {

#line default
#line hidden
            BeginContext(1224, 421, true);
            WriteLiteral(@"                <div class=""col-xl-3 col-md-3 mb-4"">
                    <div class=""card border-left-success shadow h-100 py-2"">
                        <div class=""card-body"">
                            <div class=""row no-gutters align-items-center"">
                                <div class=""col mr-2"">
                                    <div class=""text-xs font-weight-bold text-success text-uppercase mb-1"">");
            EndContext();
            BeginContext(1646, 35, false);
#line 34 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                                                                                      Write(Localizer.Text(Widget.Abbreviation));

#line default
#line hidden
            EndContext();
            BeginContext(1681, 138, true);
            WriteLiteral("</div>\r\n                                    <div class=\"h5 mb-0 font-weight-bold text-gray-800\">\r\n                                        ");
            EndContext();
            BeginContext(1820, 43, false);
#line 36 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                   Write(String.Format("{0:#,##0.00}", Widget.Value));

#line default
#line hidden
            EndContext();
            BeginContext(1863, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 37 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                         if (Widget.Count > 0)
                                        {

#line default
#line hidden
            BeginContext(1972, 89, true);
            WriteLiteral("                                            <span class=\"badge badge-pill badge-success\">");
            EndContext();
            BeginContext(2062, 12, false);
#line 39 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                                                                    Write(Widget.Count);

#line default
#line hidden
            EndContext();
            BeginContext(2074, 9, true);
            WriteLiteral("</span>\r\n");
            EndContext();
#line 40 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                        }

#line default
#line hidden
            BeginContext(2126, 178, true);
            WriteLiteral("                                    </div>\r\n                                </div>\r\n                                <div class=\"col-auto\">\r\n                                    <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 2304, "\"", 2324, 1);
#line 44 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
WriteAttributeValue("", 2312, Widget.Icon, 2312, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2325, 167, true);
            WriteLiteral("></i>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n");
            EndContext();
#line 50 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
            }

#line default
#line hidden
#line 51 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
             if (Widget.Content == "ACTIVITY")
            {

#line default
#line hidden
            BeginContext(2570, 421, true);
            WriteLiteral(@"                <div class=""col-xl-3 col-md-3 mb-4"">
                    <div class=""card border-left-primary shadow h-100 py-2"">
                        <div class=""card-body"">
                            <div class=""row no-gutters align-items-center"">
                                <div class=""col mr-2"">
                                    <div class=""text-xs font-weight-bold text-primary text-uppercase mb-1"">");
            EndContext();
            BeginContext(2992, 35, false);
#line 58 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                                                                                      Write(Localizer.Text(Widget.Abbreviation));

#line default
#line hidden
            EndContext();
            BeginContext(3027, 96, true);
            WriteLiteral("</div>\r\n                                    <div class=\"h5 mb-0 font-weight-bold text-gray-800\">");
            EndContext();
            BeginContext(3124, 12, false);
#line 59 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
                                                                                   Write(Widget.Count);

#line default
#line hidden
            EndContext();
            BeginContext(3136, 142, true);
            WriteLiteral("</div>\r\n                                </div>\r\n                                <div class=\"col-auto\">\r\n                                    <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 3278, "\"", 3298, 1);
#line 62 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
WriteAttributeValue("", 3286, Widget.Icon, 3286, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3299, 167, true);
            WriteLiteral("></i>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n");
            EndContext();
#line 68 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
            }

#line default
#line hidden
#line 68 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
             
        }

#line default
#line hidden
            BeginContext(3492, 12, true);
            WriteLiteral("    </div>\r\n");
            EndContext();
#line 71 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Pages\Shared\Components\BackofficeWidgets\Default.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoringModels.nuevaVersion.Dashboard> Html { get; private set; }
    }
}
#pragma warning restore 1591
