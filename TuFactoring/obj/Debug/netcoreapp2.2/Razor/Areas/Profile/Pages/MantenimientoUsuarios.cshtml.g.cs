#pragma checksum "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6bc3d04b90d369caffc0f5b6471ce7ddad8ead52"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Areas.Profile.Pages.Areas_Profile_Pages_MantenimientoUsuarios), @"mvc.1.0.razor-page", @"/Areas/Profile/Pages/MantenimientoUsuarios.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Areas/Profile/Pages/MantenimientoUsuarios.cshtml", typeof(TuFactoring.Areas.Profile.Pages.Areas_Profile_Pages_MantenimientoUsuarios), null)]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6bc3d04b90d369caffc0f5b6471ce7ddad8ead52", @"/Areas/Profile/Pages/MantenimientoUsuarios.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a0d876ac07db4560a63de31f86fcc7a6c15cfdef", @"/Areas/Profile/Pages/_ViewImports.cshtml")]
    public class Areas_Profile_Pages_MantenimientoUsuarios : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/Operativo/mantenimientoUsuarios.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 3 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
  
    ViewData["Title"] = Localizer.Text("titleGestionUsuarios");

    switch (@Model.Participant)
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

#line default
#line hidden
            BeginContext(693, 31, true);
            WriteLiteral("\r\n<v-app id=\"app\" hidden>\r\n    ");
            EndContext();
            BeginContext(725, 23, false);
#line 30 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(748, 8, true);
            WriteLiteral("\r\n\r\n    ");
            EndContext();
            BeginContext(757, 49, false);
#line 32 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
Write(await Html.PartialAsync("_ModalLogoutPartial", 1));

#line default
#line hidden
            EndContext();
            BeginContext(806, 202, true);
            WriteLiteral("\r\n    <div class=\"d-sm-flex align-items-center justify-content-between mb-4\">\r\n        <h1 class=\"h3 mb-0 text-gray-800\">\r\n            <a href=\"#\" class=\"btn btn-success btn-circle\">\r\n                <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 1008, "\"", 1054, 2);
#line 36 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
WriteAttributeValue("", 1016, Localizer.Text("iconUser"), 1016, 27, false);

#line default
#line hidden
            WriteAttributeValue(" ", 1043, "text-white", 1044, 11, true);
            EndWriteAttribute();
            BeginContext(1055, 37, true);
            WriteLiteral("></i>\r\n            </a>\r\n            ");
            EndContext();
            BeginContext(1093, 37, false);
#line 38 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
       Write(Localizer.Text("titleUserManagement"));

#line default
#line hidden
            EndContext();
            BeginContext(1130, 91, true);
            WriteLiteral("\r\n        </h1>\r\n        <span>\r\n            <v-btn dark color=\"green\" class=\"float-right\" ");
            EndContext();
            BeginContext(1222, 55, true);
            WriteLiteral("@click=\"limpiar(); invitar = true\">\r\n                <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 1277, "\"", 1311, 1);
#line 42 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
WriteAttributeValue("", 1285, Localizer.Text("iconAdd"), 1285, 26, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1312, 11, true);
            WriteLiteral("></i>&nbsp;");
            EndContext();
            BeginContext(1324, 30, false);
#line 42 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                           Write(Localizer.Text("buttonInvite"));

#line default
#line hidden
            EndContext();
            BeginContext(1354, 83, true);
            WriteLiteral("\r\n            </v-btn>\r\n        </span>\r\n    </div>\r\n    <p class=\"mb-4\">\r\n        ");
            EndContext();
            BeginContext(1438, 56, false);
#line 47 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
   Write(Localizer.Text("textoMantenimiento" + Model.Participant));

#line default
#line hidden
            EndContext();
            BeginContext(1494, 33, true);
            WriteLiteral("\r\n        <br /> <br />\r\n        ");
            EndContext();
            BeginContext(1528, 28, false);
#line 49 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
   Write(Localizer.Text("textoPress"));

#line default
#line hidden
            EndContext();
            BeginContext(1556, 13, true);
            WriteLiteral(" <a href=\"#\" ");
            EndContext();
            BeginContext(1570, 28, true);
            WriteLiteral("@click=\"dialogAyuda = true\">");
            EndContext();
            BeginContext(1599, 27, false);
#line 49 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                          Write(Localizer.Text("textoHere"));

#line default
#line hidden
            EndContext();
            BeginContext(1626, 5, true);
            WriteLiteral("</a> ");
            EndContext();
            BeginContext(1632, 38, false);
#line 49 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                                                           Write(Localizer.Text("textoMoreInformation"));

#line default
#line hidden
            EndContext();
            BeginContext(1670, 1688, true);
            WriteLiteral(@"
    </p>

    <div class=""card shadow mb-4"">
        <div class=""card-body"">
            <v-row>
                <v-col md=""12"" sm=""12"" lg=""12"" cols=""12"">
                    <v-data-table :headers=""header""
                                  :mobile-breakpoint=""widthTelefono""
                                  :loading=""loading""
                                  :items=""personal""
                                  class=""elevation-1"">
                        <template v-slot:item.n=""props"">
                            {{personal.indexOf(props.item)+1}}
                        </template>
                        <template v-slot:item.name=""props"">
                            <div class=""text-left"">
                                {{props.item.name}}
                            </div>
                        </template>
                        <template v-slot:item.email=""props"">
                            <div class=""text-left"">
                                {{props.item.email}}
       ");
            WriteLiteral(@"                     </div>
                        </template>
                        <template v-slot:item.foto=""props"">
                            <div class=""text-center justify-content-center"">
                                <v-img :src=""props.item.foto"" max-width=""5rem""></v-img>
                            </div>
                        </template>
                        <template v-slot:item.created_at=""props"">
                            {{backEndDateFormat(props.item.createdAt)}}
                        </template>
                        <template v-slot:item.opciones=""props"">
                            <v-btn color=""green"" small ");
            EndContext();
            BeginContext(3359, 138, true);
            WriteLiteral("@click=\"editar(personal.indexOf(props.item)); invitar = true\" class=\"text-white\" :disabled=\"enviando\">\r\n                                <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 3497, "\"", 3532, 1);
#line 84 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
WriteAttributeValue("", 3505, Localizer.Text("iconEdit"), 3505, 27, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3533, 11, true);
            WriteLiteral("></i>&nbsp;");
            EndContext();
            BeginContext(3545, 28, false);
#line 84 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                            Write(Localizer.Text("buttonEdit"));

#line default
#line hidden
            EndContext();
            BeginContext(3573, 173, true);
            WriteLiteral("\r\n                            </v-btn>\r\n                            <v-btn :color=\"props.item.state == \'active\'? \'blue\':\'red\'\" small class=\"text-white\" :disabled=\"enviando\" ");
            EndContext();
            BeginContext(3747, 136, true);
            WriteLiteral("@click=\"indice = personal.indexOf(props.item); bloquear = true\" v-if=\"props.item.state != \'invite\'\">\r\n                                <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 3883, "\"", 3920, 1);
#line 87 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
WriteAttributeValue("", 3891, Localizer.Text("iconCancel"), 3891, 29, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3921, 46, true);
            WriteLiteral("></i>&nbsp;{{ props.item.state == \'active\' ? \'");
            EndContext();
            BeginContext(3968, 29, false);
#line 87 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                                                                 Write(Localizer.Text("buttonBlock"));

#line default
#line hidden
            EndContext();
            BeginContext(3997, 6, true);
            WriteLiteral(" \' : \'");
            EndContext();
            BeginContext(4004, 30, false);
#line 87 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                                                                                                     Write(Localizer.Text("buttonUnlock"));

#line default
#line hidden
            EndContext();
            BeginContext(4034, 370, true);
            WriteLiteral(@" '}}
                            </v-btn>
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>
        </div>
    </div>

    <v-dialog v-model=""invitar"" max-width=""400"" transition=""dialog-bottom-transition"">
        <v-card>
            <v-card-title>
                <h4 class=""modal-title"">");
            EndContext();
            BeginContext(4405, 31, false);
#line 99 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                   Write(Localizer.Text("titleUserData"));

#line default
#line hidden
            EndContext();
            BeginContext(4436, 74, true);
            WriteLiteral("</h4>\r\n                <v-spacer></v-spacer>\r\n                <v-btn icon ");
            EndContext();
            BeginContext(4511, 56, true);
            WriteLiteral("@click=\"invitar = false;\">\r\n                    <v-icon>");
            EndContext();
            BeginContext(4568, 30, false);
#line 102 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("iconCloseMDI"));

#line default
#line hidden
            EndContext();
            BeginContext(4598, 335, true);
            WriteLiteral(@"</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <div class=""row"">
                    <div class=""col-md-12 col-lg-12 col-sm-12 col-xs-12"" style=""padding:0"">
                        <div class=""col-md-12 col-lg-12 col-sm-12 col-xs-12"">
                            <label>");
            EndContext();
            BeginContext(4934, 30, false);
#line 109 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                              Write(Localizer.Text("textoOneName"));

#line default
#line hidden
            EndContext();
            BeginContext(4964, 134, true);
            WriteLiteral("</label>\r\n                            <div class=\"input-group\" style=\"width:100%\">\r\n                                <input type=\"text\"");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 5098, "\"", 5146, 1);
#line 111 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
WriteAttributeValue("", 5112, Localizer.Text("placeholderName"), 5112, 34, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5147, 245, true);
            WriteLiteral(" v-model=\"nuevo.name\" maxlength=\"255\" class=\"form-control\" />\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"col-md-12 col-lg-12 col-sm-12 col-xs-12\">\r\n                            <label>");
            EndContext();
            BeginContext(5393, 28, false);
#line 115 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                              Write(Localizer.Text("textoEmail"));

#line default
#line hidden
            EndContext();
            BeginContext(5421, 134, true);
            WriteLiteral("</label>\r\n                            <div class=\"input-group\" style=\"width:100%\">\r\n                                <input type=\"text\"");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 5555, "\"", 5604, 1);
#line 117 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
WriteAttributeValue("", 5569, Localizer.Text("placeholderEmail"), 5569, 35, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5605, 489, true);
            WriteLiteral(@" v-model=""nuevo.email"" maxlength=""255"" class=""form-control"" />
                            </div>
                        </div>
                    </div>
                    <div class=""col-md-12 col-lg-12 col-sm-12"">
                        <v-expansion-panels>
                            <v-expansion-panel>
                                <v-expansion-panel-header v-slot=""{ open }"">
                                    <v-col>
                                        <span>");
            EndContext();
            BeginContext(6095, 28, false);
#line 126 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                         Write(Localizer.Text("textoRoles"));

#line default
#line hidden
            EndContext();
            BeginContext(6123, 902, true);
            WriteLiteral(@"</span>
                                    </v-col>

                                </v-expansion-panel-header>
                                <v-expansion-panel-content>
                                    <div v-for=""(item,index) in roles"" class=""form-check"">
                                        <v-divider v-if=""index != 0""></v-divider>
                                        <v-checkbox v-model=""nuevo.roles_id"" :value=""item.id"" :label=""item.name"" color=""#5867dd"" hide-details></v-checkbox>
                                    </div>
                                </v-expansion-panel-content>
                            </v-expansion-panel>
                        </v-expansion-panels>
                    </div>

                </div>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color=""green"" ");
            EndContext();
            BeginContext(7026, 86, true);
            WriteLiteral("@click=\"procesoUsuario(); \" :disabled=\"enviando\" class=\"text-white\">{{indice == -1 ? \'");
            EndContext();
            BeginContext(7113, 30, false);
#line 144 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                                                                       Write(Localizer.Text("buttonCreate"));

#line default
#line hidden
            EndContext();
            BeginContext(7143, 5, true);
            WriteLiteral("\' : \'");
            EndContext();
            BeginContext(7149, 30, false);
#line 144 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                                                                                                           Write(Localizer.Text("buttonUpdate"));

#line default
#line hidden
            EndContext();
            BeginContext(7179, 48, true);
            WriteLiteral("\'}}</v-btn>\r\n                <v-btn color=\"red\" ");
            EndContext();
            BeginContext(7228, 44, true);
            WriteLiteral("@click=\"invitar = false\" class=\"text-white\">");
            EndContext();
            BeginContext(7273, 29, false);
#line 145 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                           Write(Localizer.Text("buttonClose"));

#line default
#line hidden
            EndContext();
            BeginContext(7302, 254, true);
            WriteLiteral("</v-btn>\r\n            </v-card-actions>\r\n        </v-card>\r\n    </v-dialog>\r\n\r\n    <v-dialog v-model=\"bloquear\" max-width=\"400\" transition=\"dialog-bottom-transition\">\r\n        <v-card>\r\n            <v-card-title>\r\n                <h4 class=\"modal-title\">");
            EndContext();
            BeginContext(7557, 35, false);
#line 153 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                   Write(Localizer.Text("titleSystemTuFact"));

#line default
#line hidden
            EndContext();
            BeginContext(7592, 74, true);
            WriteLiteral("</h4>\r\n                <v-spacer></v-spacer>\r\n                <v-btn icon ");
            EndContext();
            BeginContext(7667, 95, true);
            WriteLiteral("@click=\"bloquear = false; indice = -1\">\r\n                    <v-icon>\r\n                        ");
            EndContext();
            BeginContext(7763, 30, false);
#line 157 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                   Write(Localizer.Text("iconCloseMDI"));

#line default
#line hidden
            EndContext();
            BeginContext(7793, 135, true);
            WriteLiteral("\r\n                    </v-icon>\r\n                </v-btn>\r\n            </v-card-title>\r\n            <v-card-text>\r\n                <p> ");
            EndContext();
            BeginContext(7929, 33, false);
#line 162 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
               Write(Localizer.Text("textoSecureUser"));

#line default
#line hidden
            EndContext();
            BeginContext(7962, 202, true);
            WriteLiteral(" <strong>{{indice == -1 ? \'\': personal[indice].name}}</strong></p>\r\n            </v-card-text>\r\n            <v-card-actions>\r\n                <v-spacer></v-spacer>\r\n                <v-btn color=\"green\" ");
            EndContext();
            BeginContext(8165, 72, true);
            WriteLiteral("@click=\"blockPersonal(indice);\" class=\"text-white\" :disabled=\"enviando\">");
            EndContext();
            BeginContext(8238, 29, false);
#line 166 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                                                         Write(Localizer.Text("buttonAcept"));

#line default
#line hidden
            EndContext();
            BeginContext(8267, 45, true);
            WriteLiteral("</v-btn>\r\n                <v-btn color=\"red\" ");
            EndContext();
            BeginContext(8313, 45, true);
            WriteLiteral("@click=\"bloquear = false\" class=\"text-white\">");
            EndContext();
            BeginContext(8359, 29, false);
#line 167 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                                                                            Write(Localizer.Text("buttonClose"));

#line default
#line hidden
            EndContext();
            BeginContext(8388, 284, true);
            WriteLiteral(@"</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>

    <v-dialog v-model=""dialogAyuda"" max-width=""500"" transition=""dialog-bottom-transition"">
        <v-card>
            <v-card-title>
                <v-spacer></v-spacer>
                <v-btn icon ");
            EndContext();
            BeginContext(8673, 59, true);
            WriteLiteral("@click=\"dialogAyuda = false\">\r\n                    <v-icon>");
            EndContext();
            BeginContext(8733, 30, false);
#line 177 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("iconCloseMDI"));

#line default
#line hidden
            EndContext();
            BeginContext(8763, 248, true);
            WriteLiteral("</v-icon>\r\n                </v-btn>\r\n            </v-card-title>\r\n            <v-card-text>\r\n                <v-container>\r\n                    <v-row>\r\n                        <v-col cols=\"12\" sm=\"12\" lg=\"12\" md=\"12\">\r\n                            ");
            EndContext();
            BeginContext(9012, 44, false);
#line 184 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation1"));

#line default
#line hidden
            EndContext();
            BeginContext(9056, 38, true);
            WriteLiteral("<br />\r\n\r\n                            ");
            EndContext();
            BeginContext(9095, 44, false);
#line 186 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation2"));

#line default
#line hidden
            EndContext();
            BeginContext(9139, 36, true);
            WriteLiteral("<br />\r\n                            ");
            EndContext();
            BeginContext(9176, 44, false);
#line 187 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation3"));

#line default
#line hidden
            EndContext();
            BeginContext(9220, 42, true);
            WriteLiteral("<br /><br />\r\n                            ");
            EndContext();
            BeginContext(9263, 44, false);
#line 188 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation4"));

#line default
#line hidden
            EndContext();
            BeginContext(9307, 38, true);
            WriteLiteral("<br />\r\n\r\n                            ");
            EndContext();
            BeginContext(9346, 44, false);
#line 190 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation5"));

#line default
#line hidden
            EndContext();
            BeginContext(9390, 44, true);
            WriteLiteral("<br /><br />\r\n\r\n                            ");
            EndContext();
            BeginContext(9435, 44, false);
#line 192 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation6"));

#line default
#line hidden
            EndContext();
            BeginContext(9479, 36, true);
            WriteLiteral("<br />\r\n                            ");
            EndContext();
            BeginContext(9516, 44, false);
#line 193 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation7"));

#line default
#line hidden
            EndContext();
            BeginContext(9560, 42, true);
            WriteLiteral("<br /><br />\r\n                            ");
            EndContext();
            BeginContext(9603, 44, false);
#line 194 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
                       Write(Localizer.Text("textoSeguridadInformation8"));

#line default
#line hidden
            EndContext();
            BeginContext(9647, 178, true);
            WriteLiteral("\r\n\r\n                        </v-col>\r\n                    </v-row>\r\n                </v-container>\r\n            </v-card-text>\r\n        </v-card>\r\n    </v-dialog>\r\n\r\n</v-app>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(9842, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(9848, 89, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6bc3d04b90d369caffc0f5b6471ce7ddad8ead5230368", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 206 "C:\Users\mgiaccusys\Desktop\tufactoring-frontend\TuFactoring\Areas\Profile\Pages\MantenimientoUsuarios.cshtml"
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
                BeginContext(9937, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoring.Areas.Profile.Pages.MantenimientoUsuariosModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Profile.Pages.MantenimientoUsuariosModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Profile.Pages.MantenimientoUsuariosModel>)PageContext?.ViewData;
        public TuFactoring.Areas.Profile.Pages.MantenimientoUsuariosModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
