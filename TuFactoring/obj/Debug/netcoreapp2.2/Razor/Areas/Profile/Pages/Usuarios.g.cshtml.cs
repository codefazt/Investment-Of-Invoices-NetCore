#pragma checksum "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3fd9daa017d8c9e3b515f2253390ab41202b5486"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Areas.Profile.Asociados.Areas_Profile_Pages_Usuarios), @"mvc.1.0.razor-page", @"/Areas/Profile/Pages/Usuarios.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Areas/Profile/Pages/Usuarios.cshtml", typeof(TuFactoring.Areas.Profile.Asociados.Areas_Profile_Pages_Usuarios), null)]
namespace TuFactoring.Areas.Profile.Asociados
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 6 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
using System.Globalization;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3fd9daa017d8c9e3b515f2253390ab41202b5486", @"/Areas/Profile/Pages/Usuarios.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a0d876ac07db4560a63de31f86fcc7a6c15cfdef", @"/Areas/Profile/Pages/_ViewImports.cshtml")]
    public class Areas_Profile_Pages_Usuarios : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/Operativo/vueUsers.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 7 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
  
    ViewData["Title"] = "Usuarios";

    switch (@Model.TipoParticipante)
    {
        case "CONFIRMANT":
            ViewData["Sidebar"] = "dark";
            break;
        case "DEBTOR":
            ViewData["Sidebar"] = "primary";
            break;
        case "FACTOR":
            ViewData["Sidebar"] = "warning";
            break;
        case "SUPPLIER":
            ViewData["Sidebar"] = "purple";
            break;
        default:
            ViewData["Sidebar"] = "navy";
            break;
    }

    Layout = "~/Pages/Shared/_Layout.cshtml";
    var culture = System.Globalization.CultureInfo.CurrentCulture.Name;

#line default
#line hidden
            BeginContext(902, 57, true);
            WriteLiteral("\r\n<v-app id=\"appUsers\">\r\n    <div class=\"mt-5\">\r\n        ");
            EndContext();
            BeginContext(960, 23, false);
#line 35 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
   Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(983, 41, true);
            WriteLiteral("\r\n        <input hidden id=\"contenidoRaw\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1024, "\"", 1047, 1);
#line 36 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
WriteAttributeValue("", 1032, Model.UserJson, 1032, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1048, 542, true);
            WriteLiteral(@" />

        <div class=""modal fade in fa fa-spinner"" v-if=""cargando"" role=""dialog"">
            <div class=""modal-dialog text-center"">

                <h2 style=""color:#000""><span id=""cargando"">Cargando...</span></h2>
            </div>
        </div>

        <div id=""contenido"" hidden>

            <div class=""col-lg-12"">
                <div class=""card shadow mb-4"">
                    <div class=""card-header py-3"">
                        <h6 class=""m-0 font-weight-bold text-primary"">
                            <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 1590, "\"", 1645, 2);
#line 51 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
WriteAttributeValue("", 1598, Localizer.Text("iconCommercialData"), 1598, 37, false);

#line default
#line hidden
            WriteAttributeValue(" ", 1635, "logoColor", 1636, 10, true);
            EndWriteAttribute();
            BeginContext(1646, 48, true);
            WriteLiteral("></i> &nbsp;\r\n                            <span>");
            EndContext();
            BeginContext(1695, 35, false);
#line 52 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
                             Write(Localizer.Text("textoDatosUsuario"));

#line default
#line hidden
            EndContext();
            BeginContext(1730, 263, true);
            WriteLiteral(@"</span>

                        </h6>
                    </div>
                    <div class=""card-body"">
                        <div class=""row"">

                            <div class=""from-group col-sm-12"">
                                <label>");
            EndContext();
            BeginContext(1994, 34, false);
#line 60 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
                                  Write(Localizer.Text("textonameSurname"));

#line default
#line hidden
            EndContext();
            BeginContext(2028, 147, true);
            WriteLiteral("</label>\r\n                                <input maxlength=\"255\" v-on:blur=\"validarNombres\" v-model=\"user.name\" :class=\"[errorName,\'form-control\']\"");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 2175, "\"", 2224, 1);
#line 61 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
WriteAttributeValue("", 2189, Localizer.Text("textonameSurname"), 2189, 35, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2225, 242, true);
            WriteLiteral(">\r\n                                <span class=\"text-danger\">{{ errorNameText }}</span>\r\n                            </div>\r\n\r\n                            <div class=\"from-group col-sm-12\">\r\n                                <label for=\"email\">");
            EndContext();
            BeginContext(2468, 28, false);
#line 66 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
                                              Write(Localizer.Text("textoEmail"));

#line default
#line hidden
            EndContext();
            BeginContext(2496, 457, true);
            WriteLiteral(@"</label>
                                <div class=""input-group"">
                                    <div class=""input-group-prepend"">
                                        <span class=""input-group-text""><i class=""fa fa-envelope""></i></span>
                                    </div>
                                    <input onPaste=""return false"" maxlength=""60"" v-on:blur=""validarEmail"" v-model=""user.email"" :class=""['form-control',errorEmail]""");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 2953, "\"", 3002, 1);
#line 71 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
WriteAttributeValue("", 2967, Localizer.Text("placeholderEmail"), 2967, 35, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3003, 270, true);
            WriteLiteral(@">
                                </div>
                                <span class=""text-danger"">{{errorEmailText}}</span>
                            </div>

                            <div  class=""from-group col-sm-12"">
                                <label>");
            EndContext();
            BeginContext(3274, 28, false);
#line 77 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
                                  Write(Localizer.Text("textoRoles"));

#line default
#line hidden
            EndContext();
            BeginContext(3302, 165, true);
            WriteLiteral("</label>\r\n                                <input v-for=\"rol in user.roles\" disabled maxlength=\"255\" v-on:blur=\"\" v-model=\"rol.name\" :class=\"[\'form-control\', \'mb-5\']\"");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 3467, "\"", 3510, 1);
#line 78 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
WriteAttributeValue("", 3481, Localizer.Text("textoRoles"), 3481, 29, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3511, 571, true);
            WriteLiteral(@">
                            </div>

                            <div class=""row"">
                                <div class=""col-md-4""></div>
                                <div class=""col-md-4""><button :disabled=""habilitarBoton"" v-on:click=""actualizarUser"" class=""btn btn-primary btn-block""><i class=""fa fa-edit""></i> Actualizar</button></div>
                                <div class=""col-md-4""></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            ");
            EndContext();
            BeginContext(4083, 58, false);
#line 91 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
       Write(await Html.PartialAsync("_ModalLogoutPartial", Model.NRol));

#line default
#line hidden
            EndContext();
            BeginContext(4141, 42, true);
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n</v-app>\r\n\r\n");
            EndContext();
            DefineSection("Scripts", async() => {
                BeginContext(4206, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(4212, 76, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3fd9daa017d8c9e3b515f2253390ab41202b548612170", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 98 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\Usuarios.cshtml"
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
                BeginContext(4288, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoring.Areas.Profile.Pages.UsuariosModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Profile.Pages.UsuariosModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Profile.Pages.UsuariosModel>)PageContext?.ViewData;
        public TuFactoring.Areas.Profile.Pages.UsuariosModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
