#pragma checksum "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "969fd9397f6b0f4ab727829a8419f8dfcc800a38"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Areas.Confirmant.Pages.Areas_Confirmant_Pages_ConsultaCesion), @"mvc.1.0.razor-page", @"/Areas/Confirmant/Pages/ConsultaCesion.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Areas/Confirmant/Pages/ConsultaCesion.cshtml", typeof(TuFactoring.Areas.Confirmant.Pages.Areas_Confirmant_Pages_ConsultaCesion), null)]
namespace TuFactoring.Areas.Confirmant.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"969fd9397f6b0f4ab727829a8419f8dfcc800a38", @"/Areas/Confirmant/Pages/ConsultaCesion.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a91367a1958076cc8ce472bcf1aad08953ca148a", @"/Areas/Confirmant/Pages/_ViewImports.cshtml")]
    public class Areas_Confirmant_Pages_ConsultaCesion : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/Bancos/consultaCesionConfirmant.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
  

    ViewData["Title"] = Localizer.Text("titleQueriesCesion");
    Layout = "~/Pages/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(189, 39, true);
            WriteLiteral("\r\n<v-app id=\"appConsultasCesion\">\r\n    ");
            EndContext();
            BeginContext(229, 23, false);
#line 10 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(252, 186, true);
            WriteLiteral("\r\n\r\n    <div class=\"modal fade in fa fa-spinner\" v-if=\"cargando\" role=\"dialog\">\r\n        <div class=\"modal-dialog text-center\">\r\n\r\n            <h2 style=\"color:#000\"><span id=\"cargando\">");
            EndContext();
            BeginContext(439, 22, false);
#line 15 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
                                                  Write(Localizer.Text("load"));

#line default
#line hidden
            EndContext();
            BeginContext(461, 97, true);
            WriteLiteral("</span></h2>\r\n        </div>\r\n    </div>\r\n\r\n    <div id=\"contenido\" hidden class=\"row\">\r\n        ");
            EndContext();
            BeginContext(559, 49, false);
#line 20 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
   Write(await Html.PartialAsync("_ModalLogoutPartial", 5));

#line default
#line hidden
            EndContext();
            BeginContext(608, 380, true);
            WriteLiteral(@"
        <div class=""col-sm-12"">
            <div class=""d-sm-flex align-items-center justify-content-between mb-4"">
                <h1 class=""h3 mb-0 text-gray-800"">
                    <a href=""#"" style=""color:#fff !important"" class=""btn btn-success btn-circle"">
                        <i class=""far fa-file-alt""></i>
                    </a>
                    &nbsp;");
            EndContext();
            BeginContext(989, 36, false);
#line 27 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
                     Write(Localizer.Text("titleQueriesCesion"));

#line default
#line hidden
            EndContext();
            BeginContext(1025, 91, true);
            WriteLiteral("\r\n                </h1>\r\n            </div>\r\n            <p class=\"mb-4\">\r\n                ");
            EndContext();
            BeginContext(1117, 43, false);
#line 31 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
           Write(Localizer.Text("textInformationCesionBank"));

#line default
#line hidden
            EndContext();
            BeginContext(1160, 265, true);
            WriteLiteral(@"
            </p>

            <div class=""card shadow mb-4"">
                <div class=""card-header py-3 d-flex flex-row align-items-center justify-content-between"">
                    <h4 class=""m-0 font-weight-bold text-primary"">
                        ");
            EndContext();
            BeginContext(1426, 44, false);
#line 37 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
                   Write(Localizer.Text("textContratoCesionDerechos"));

#line default
#line hidden
            EndContext();
            BeginContext(1470, 3853, true);
            WriteLiteral(@"
                    </h4>
                </div>
                <div class=""card-body"">
                    <div class=""row"">
                        <div class=""col-sm-12"">
                            <!------------------------------------------- Tabla de Consultas ---------------------------------------------->
                            <v-data-table :headers=""headerInvoices""
                                          :items=""facturas""
                                          :items-per-page=""10""
                                          :mobile-breakpoint=""widthTelefono""
                                          :options.sync=""options""
                                          :loading=""loading""
                                          class=""elevation-1"">
                                <template v-slot:item.n=""{ item }"">
                                    <div class=""text-center"">
                                        {{facturas.indexOf(item) + 1}}
                               ");
            WriteLiteral(@"     </div>
                                </template>

                                <template v-slot:item.debtor=""props"">
                                    <div class=""text-left"" v-if=""tamanoTlf()"">
                                        {{ props.item.invoice.debtor.name }}
                                    </div>
                                    <div class=""text-right"" v-else>
                                        <p class=""text-sm-right"">{{ props.item.invoice.debtor.name }}</p>
                                    </div>
                                </template>

                                <template v-slot:item.supplier=""props"">
                                    <div class=""text-left"" v-if=""tamanoTlf()"">
                                        {{ props.item.invoice.supplier.name }}
                                    </div>
                                    <div class=""text-right"" v-else>
                                        <p class=""text-sm-right"">{{ props.item");
            WriteLiteral(@".invoice.supplier.name }}</p>
                                    </div>
                                </template>

                                <template v-slot:item.expiration_date=""props"">
                                    <div class=""text-center"">
                                        {{ backEndDateFormat(props.item.invoice.expiration_date) }}
                                        <v-chip x-small
                                                label>
                                            {{props.item.invoice.term_days}}
                                        </v-chip>
                                    </div>
                                </template>
                                <template v-slot:item.changelogs=""props"">
                                    <div class=""text-center"">
                                        {{ backEndDateFormat(props.item.changelogs[0].changedAt) }}
                                    </div>
                                </template>
            WriteLiteral(@"
                                <template v-slot:item.amount=""props"">
                                    <div class=""text-right"">{{ props.item.currency.symbol }} {{ formatoMonedaInput(props.item.invoice.amount,lang,digits) }} <span class=""text-xs badge bg-gray-200"">{{ props.item.currency.iso_4217 }}</span></div>
                                </template>
                                <template v-slot:item.status=""props"">
                                    <div class=""text-center"">{{ estado_factura(props.item.state) }}</div>
                                </template>

                                <template v-slot:item.options=""props"">
                                    <button type=""button"" v-on:click=""getFile(props.item)"" class=""btn btn-sm btn-success""");
            EndContext();
            BeginWriteAttribute("title", " title=\"", 5323, "\"", 5373, 1);
#line 97 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
WriteAttributeValue("", 5331, Localizer.Text("textoVerDocumentoCesion"), 5331, 42, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5374, 271, true);
            WriteLiteral(@"><i class=""fa fa-eye""></i></button>
                                </template>
                            </v-data-table>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</v-app>
");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(5662, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(5668, 89, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "969fd9397f6b0f4ab727829a8419f8dfcc800a3812244", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 109 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Confirmant\Pages\ConsultaCesion.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(5757, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoring.Areas.Confirmant.Pages.ConsultaCesionModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Confirmant.Pages.ConsultaCesionModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Confirmant.Pages.ConsultaCesionModel>)PageContext?.ViewData;
        public TuFactoring.Areas.Confirmant.Pages.ConsultaCesionModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591