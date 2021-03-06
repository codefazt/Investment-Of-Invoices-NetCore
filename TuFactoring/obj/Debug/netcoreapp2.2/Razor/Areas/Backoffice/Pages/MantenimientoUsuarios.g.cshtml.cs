#pragma checksum "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\MantenimientoUsuarios.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fcf97d51237edcb184c5f0f6000a770d5be966f8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Areas.Backoffice.Pages.Areas_Backoffice_Pages_MantenimientoUsuarios), @"mvc.1.0.razor-page", @"/Areas/Backoffice/Pages/MantenimientoUsuarios.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Areas/Backoffice/Pages/MantenimientoUsuarios.cshtml", typeof(TuFactoring.Areas.Backoffice.Pages.Areas_Backoffice_Pages_MantenimientoUsuarios), null)]
namespace TuFactoring.Areas.Backoffice.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fcf97d51237edcb184c5f0f6000a770d5be966f8", @"/Areas/Backoffice/Pages/MantenimientoUsuarios.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b4c32e74cd223d094aabc0adf938f9d9e0cac8ec", @"/Areas/Backoffice/Pages/_ViewImports.cshtml")]
    public class Areas_Backoffice_Pages_MantenimientoUsuarios : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
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
#line 3 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\MantenimientoUsuarios.cshtml"
  
    ViewData["Title"] = "Gestión de Usuarios";
    Layout = "~/Pages/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(174, 31, true);
            WriteLiteral("\r\n<v-app id=\"app\" hidden>\r\n    ");
            EndContext();
            BeginContext(206, 23, false);
#line 9 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\MantenimientoUsuarios.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(229, 33, true);
            WriteLiteral("\r\n\r\n    <input hidden id=\"People\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 262, "\"", 285, 1);
#line 11 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\MantenimientoUsuarios.cshtml"
WriteAttributeValue("", 270, ViewData["id"], 270, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(286, 387, true);
            WriteLiteral(@" />


    <div class=""d-sm-flex align-items-center justify-content-between mb-4"">
        <h1 class=""h3 mb-0 text-gray-800"">
            <a href=""#"" class=""btn btn-success btn-circle"">
                <i class=""fa fa-user text-white""></i>
            </a>
            Gestión de Usuarios
        </h1>
        <span>
            <v-btn dark color=""green"" class=""float-right""  ");
            EndContext();
            BeginContext(674, 2081, true);
            WriteLiteral(@"@click=""limpiar(); invitar = true"">
                <i class=""fa fa-plus""></i>&nbsp;Invitar
            </v-btn>
        </span>
    </div>
    <p class=""mb-4"">
        Lorem ipsum dolor sit amet, consectetur adipisicing elit. Error quisquam sapiente, aliquam eos nisi cum similique ea rerum tempore distinctio asperiores cumque natus possimus, impedit, ratione mollitia animi at maiores, please visit the <a target=""_blank"" href=""https://www.chartjs.org/docs/latest/"">official Chart.js documentation</a>.
    </p>

    <div class=""card shadow mb-4"">
        <div class=""card-body"">
            <v-row>
                <v-col md=""12"" sm=""12"" lg=""12"" cols=""12"">
                    <v-data-table :headers=""header""
                                  :items=""personal""
                                  class=""elevation-1"">
                        <template v-slot:item.n=""props"">
                            {{personal.indexOf(props.item)+1}}
                        </template>
                        <temp");
            WriteLiteral(@"late v-slot:item.name=""props"">
                            <div class=""text-left"">
                                {{props.item.name}}
                            </div>
                        </template>
                        <template v-slot:item.email=""props"">
                            <div class=""text-left"">
                                {{props.item.email}}
                            </div>
                        </template>
                        <template v-slot:item.foto=""props"">
                            <div class=""text-center justify-content-center"">
                                <v-img :src=""props.item.foto"" max-width=""5rem""></v-img>
                            </div>
                        </template>
                        <template v-slot:item.created_at=""props"">
                            {{backEndDateFormat(props.item.created_at)}}
                        </template>
                        <template v-slot:item.opciones=""props"">
                           ");
            WriteLiteral(" <v-btn color=\"green\" small dark ");
            EndContext();
            BeginContext(2756, 273, true);
            WriteLiteral(@"@click=""editar(personal.indexOf(props.item)); invitar = true"">
                                <i class=""fa fa-pencil-square-o""></i>&nbsp;Editar
                            </v-btn>
                            <v-btn :color=""props.item.status ? 'red':'blue'"" small dark ");
            EndContext();
            BeginContext(3030, 639, true);
            WriteLiteral(@"@click=""indice = personal.indexOf(props.item); bloquear = true"">
                                <i class=""fa fa-ban""></i>&nbsp;{{ props.item.status ? 'Bloquear ' : 'Desbloquear '}}
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
                <h4 class=""modal-title"">Datos del Usuario</h4>
                <v-spacer></v-spacer>
                <v-btn icon ");
            EndContext();
            BeginContext(3670, 2398, true);
            WriteLiteral(@"@click=""invitar = false;"">
                    <v-icon>mdi-close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <div class=""row"">
                    <div class=""col-md-12 col-lg-12 col-sm-12 col-xs-12"" style=""padding:0"">
                        <div class=""col-md-12 col-lg-12 col-sm-12 col-xs-12"">
                            <label>Nombre</label>
                            <div class=""input-group"" style=""width:100%"">
                                <input type=""text"" placeholder=""Introduzca el nombre  "" v-model=""nuevo.name"" maxlength=""255"" class=""form-control"" />
                            </div>
                        </div>
                        <div class=""col-md-12 col-lg-12 col-sm-12 col-xs-12"">
                            <label>Email</label>
                            <div class=""input-group"" style=""width:100%"">
                                <input type=""text"" placeholder=""Introduzca el email  "" v-model=""nuevo.email"" maxl");
            WriteLiteral(@"ength=""255"" class=""form-control"" />
                            </div>
                        </div>
                    </div>
                    <div class=""col-md-12 col-lg-12 col-sm-12"">
                        <v-expansion-panels>
                            <v-expansion-panel>
                                <v-expansion-panel-header v-slot=""{ open }"">
                                    <v-col>
                                        <span>Roles</span>
                                    </v-col>

                                </v-expansion-panel-header>
                                <v-expansion-panel-content>
                                    <div v-for=""(item,index) in roles"" class=""form-check"">
                                        <v-divider v-if=""index != 0""></v-divider>
                                        <v-checkbox v-model=""nuevo.roles_id"" :value=""item.id"" :label=""item.name"" color=""#5867dd"" hide-details></v-checkbox>
                                    </div>
  ");
            WriteLiteral(@"                              </v-expansion-panel-content>
                            </v-expansion-panel>
                        </v-expansion-panels>
                    </div>

                </div>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color=""green"" dark ");
            EndContext();
            BeginContext(6069, 136, true);
            WriteLiteral("@click=\"procesoUsuario(); invitar = false\">{{indice == -1 ? \'Crear \' : \'Actualizar \'}}</v-btn>\r\n                <v-btn color=\"red\" dark ");
            EndContext();
            BeginContext(6206, 379, true);
            WriteLiteral(@"@click=""invitar = false"">Cerrar</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>

    <v-dialog v-model=""bloquear"" max-width=""400"" transition=""dialog-bottom-transition"">
        <v-card>
            <v-card-title>
                <h4 class=""modal-title"">Sistema Tu Factoring</h4>
                <v-spacer></v-spacer>
                <v-btn icon ");
            EndContext();
            BeginContext(6586, 492, true);
            WriteLiteral(@"@click=""bloquear = false; indice = -1"">
                    <v-icon>
                        mdi-close
                    </v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <p>Estas seguro de realizar esta acción al usuario <strong>{{indice == -1 ? '': personal[indice].name}}</strong></p>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn dark color=""green"" ");
            EndContext();
            BeginContext(7079, 106, true);
            WriteLiteral("@click=\"blockPersonal(indice); bloquear = false\">Aceptar</v-btn>\r\n                <v-btn dark color=\"red\" ");
            EndContext();
            BeginContext(7186, 121, true);
            WriteLiteral("@click=\"bloquear = false\">Cerrar</v-btn>\r\n            </v-card-actions>\r\n        </v-card>\r\n    </v-dialog>\r\n</v-app>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(7324, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(7330, 89, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fcf97d51237edcb184c5f0f6000a770d5be966f813194", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 151 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\MantenimientoUsuarios.cshtml"
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
                BeginContext(7419, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoring.Areas.Admin.Pages.MantenimientoUsuariosModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Admin.Pages.MantenimientoUsuariosModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Admin.Pages.MantenimientoUsuariosModel>)PageContext?.ViewData;
        public TuFactoring.Areas.Admin.Pages.MantenimientoUsuariosModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
