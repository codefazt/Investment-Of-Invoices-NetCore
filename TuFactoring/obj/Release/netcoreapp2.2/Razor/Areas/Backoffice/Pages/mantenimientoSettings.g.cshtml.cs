#pragma checksum "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c1ac9260f89add3bc2e4c774bcedf8fe5017bb76"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(TuFactoring.Areas.Backoffice.Pages.Areas_Backoffice_Pages_mantenimientoSettings), @"mvc.1.0.razor-page", @"/Areas/Backoffice/Pages/mantenimientoSettings.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Areas/Backoffice/Pages/mantenimientoSettings.cshtml", typeof(TuFactoring.Areas.Backoffice.Pages.Areas_Backoffice_Pages_mantenimientoSettings), null)]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c1ac9260f89add3bc2e4c774bcedf8fe5017bb76", @"/Areas/Backoffice/Pages/mantenimientoSettings.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b4c32e74cd223d094aabc0adf938f9d9e0cac8ec", @"/Areas/Backoffice/Pages/_ViewImports.cshtml")]
    public class Areas_Backoffice_Pages_mantenimientoSettings : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/js/Operativo/mantenimientoSettings.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 3 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
   ViewData["Title"] = Localizer.Text("titlePageGestionSettings");
    Layout = "~/Pages/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(195, 31, true);
            WriteLiteral("\r\n<v-app id=\"app\" hidden>\r\n    ");
            EndContext();
            BeginContext(227, 23, false);
#line 8 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(250, 6, true);
            WriteLiteral("\r\n    ");
            EndContext();
            BeginContext(257, 49, false);
#line 9 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
Write(await Html.PartialAsync("_ModalLogoutPartial", 5));

#line default
#line hidden
            EndContext();
            BeginContext(306, 202, true);
            WriteLiteral("\r\n    <div class=\"d-sm-flex align-items-center justify-content-between mb-4\">\r\n        <h1 class=\"h3 mb-0 text-gray-800\">\r\n            <a href=\"#\" class=\"btn btn-success btn-circle\">\r\n                <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 508, "\"", 558, 2);
#line 13 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
WriteAttributeValue("", 516, Localizer.Text("iconSettings"), 516, 31, false);

#line default
#line hidden
            WriteAttributeValue(" ", 547, "text-white", 548, 11, true);
            EndWriteAttribute();
            BeginContext(559, 37, true);
            WriteLiteral("></i>\r\n            </a>\r\n            ");
            EndContext();
            BeginContext(597, 38, false);
#line 15 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
       Write(Localizer.Text("titleGestionSettings"));

#line default
#line hidden
            EndContext();
            BeginContext(635, 52, true);
            WriteLiteral("\r\n        </h1>\r\n        <span>\r\n            <v-btn ");
            EndContext();
            BeginContext(688, 90, true);
            WriteLiteral("@click=\"limpiar(); edit =true\" dark color=\"green\" class=\"float-right\">\r\n                <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 778, "\"", 812, 1);
#line 19 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
WriteAttributeValue("", 786, Localizer.Text("iconAdd"), 786, 26, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(813, 11, true);
            WriteLiteral("></i>&nbsp;");
            EndContext();
            BeginContext(825, 30, false);
#line 19 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                                           Write(Localizer.Text("buttonCreate"));

#line default
#line hidden
            EndContext();
            BeginContext(855, 1256, true);
            WriteLiteral(@"
            </v-btn>
        </span>
    </div>
    <p class=""mb-4"">
        Lorem ipsum dolor sit amet, consectetur adipisicing elit. Error quisquam sapiente, aliquam eos nisi cum similique ea rerum tempore distinctio asperiores cumque natus possimus, impedit, ratione mollitia animi at maiores, please visit the <a target=""_blank"" href=""https://www.chartjs.org/docs/latest/"">official Chart.js documentation</a>.
    </p>

    <div class=""card shadow mb-4"">
        <div class=""card-body"">
            <v-row>
                <v-col cols=""12"" md=""12"" lg=""12"" sm=""12"">
                    <v-data-table :headers=""header""
                                  :items=""settings""
                                  :loading=""loading""
                                  class=""elevation-1"">
                        <template v-slot:item.n=""props"">
                            {{settings.indexOf(props.item) + 1}}
                        </template>

                        <template v-slot:item.type_content=""pro");
            WriteLiteral("ps\">\r\n                            {{i18n.t(props.item.type_content)}}\r\n                        </template>\r\n                        <template v-slot:item.options=\"props\">\r\n                            <v-btn dark color=\"green\" small ");
            EndContext();
            BeginContext(2112, 95, true);
            WriteLiteral("@click=\"editar(settings.indexOf(props.item)); edit = true\">\r\n                                <i");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 2207, "\"", 2242, 1);
#line 44 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
WriteAttributeValue("", 2215, Localizer.Text("iconEdit"), 2215, 27, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2243, 11, true);
            WriteLiteral("></i>&nbsp;");
            EndContext();
            BeginContext(2255, 28, false);
#line 44 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                                                            Write(Localizer.Text("buttonEdit"));

#line default
#line hidden
            EndContext();
            BeginContext(2283, 327, true);
            WriteLiteral(@"
                            </v-btn>
                        </template>
                    </v-data-table>
                </v-col>
            </v-row>
        </div>
    </div>


    <v-dialog v-model=""edit"" max-width=""400"">
        <v-card>
            <v-card-title>
                <h4 class=""modal-title"">");
            EndContext();
            BeginContext(2611, 34, false);
#line 57 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                   Write(Localizer.Text("titleSettingData"));

#line default
#line hidden
            EndContext();
            BeginContext(2645, 74, true);
            WriteLiteral("</h4>\r\n                <v-spacer></v-spacer>\r\n                <v-btn icon ");
            EndContext();
            BeginContext(2720, 78, true);
            WriteLiteral("@click=\"edit = false\">\r\n                    <v-icon>\r\n                        ");
            EndContext();
            BeginContext(2799, 30, false);
#line 61 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                   Write(Localizer.Text("iconCloseMDI"));

#line default
#line hidden
            EndContext();
            BeginContext(2829, 307, true);
            WriteLiteral(@"
                    </v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>

                <v-row>
                    <v-col cols=""12"" md=""12"" sm=""12"" lg=""12"">
                        <v-col cols=""12"" md=""12"" sm=""12"" lg=""12"">
                            <label>");
            EndContext();
            BeginContext(3137, 35, false);
#line 70 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                              Write(Localizer.Text("textoAbbreviation"));

#line default
#line hidden
            EndContext();
            BeginContext(3172, 134, true);
            WriteLiteral("</label>\r\n                            <div class=\"input-group\" style=\"width:100%\">\r\n                                <input type=\"text\"");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 3306, "\"", 3362, 1);
#line 72 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
WriteAttributeValue("", 3320, Localizer.Text("placeholderAbbreviation"), 3320, 42, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3363, 245, true);
            WriteLiteral(" v-model=\"nuevo.abbreviation\" maxlength=\"255\" class=\"form-control\" />\r\n                            </div>\r\n                        </v-col>\r\n\r\n                        <v-col cols=\"12\" md=\"12\" sm=\"12\" lg=\"12\">\r\n                            <label>");
            EndContext();
            BeginContext(3609, 34, false);
#line 77 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                              Write(Localizer.Text("textoDescription"));

#line default
#line hidden
            EndContext();
            BeginContext(3643, 134, true);
            WriteLiteral("</label>\r\n                            <div class=\"input-group\" style=\"width:100%\">\r\n                                <input type=\"text\"");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 3777, "\"", 3832, 1);
#line 79 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
WriteAttributeValue("", 3791, Localizer.Text("placeholderDescription"), 3791, 41, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3833, 499, true);
            WriteLiteral(@" v-model=""nuevo.description"" maxlength=""255"" class=""form-control"" />
                            </div>
                        </v-col>
                    </v-col>

                    <v-col cols=""12"" md=""12"" sm=""12"" lg=""12"">
                        <v-expansion-panels>
                            <v-expansion-panel>
                                <v-expansion-panel-header v-slot=""{ open }"">
                                    <v-col>
                                        <span>");
            EndContext();
            BeginContext(4333, 34, false);
#line 89 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                         Write(Localizer.Text("textoTypeContent"));

#line default
#line hidden
            EndContext();
            BeginContext(4367, 1287, true);
            WriteLiteral(@"</span>
                                    </v-col>
                                    <v-col>
                                        <v-fade-transition leave-absolute>
                                            <span v-if=""!open"">{{i18n.t(nuevo.type_content)}}</span>
                                        </v-fade-transition>
                                    </v-col>
                                </v-expansion-panel-header>
                                <v-expansion-panel-content>
                                    <div class=""form-check"" v-for=""(item,index) in types_content"">
                                        <v-divider v-if=""index != 0""></v-divider>
                                        <v-radio-group v-model=""nuevo.type_content"">
                                            <v-radio small color=""#5867dd"" :value=""item"" :label=""i18n.t(item)""></v-radio>
                                        </v-radio-group>
                                    </div>
                      ");
            WriteLiteral(@"          </v-expansion-panel-content>
                            </v-expansion-panel>
                        </v-expansion-panels>
                    </v-col>

                    <v-col cols=""12"" md=""12"" sm=""12"" lg=""12"">
                        <label>");
            EndContext();
            BeginContext(5655, 30, false);
#line 110 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                          Write(Localizer.Text("textoContent"));

#line default
#line hidden
            EndContext();
            BeginContext(5685, 117, true);
            WriteLiteral("</label>\r\n                        <div class=\"input-group\" style=\"width:100%\">\r\n                            <textarea");
            EndContext();
            BeginWriteAttribute("placeholder", " placeholder=\"", 5802, "\"", 5853, 1);
#line 112 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
WriteAttributeValue("", 5816, Localizer.Text("placeholderContent"), 5816, 37, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5854, 303, true);
            WriteLiteral(@" v-model=""nuevo.content"" maxlength=""255"" class=""form-control""> </textarea>
                        </div>
                    </v-col>
                </v-row>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color=""green"" dark ");
            EndContext();
            BeginContext(6158, 80, true);
            WriteLiteral("@click=\"procesoSetting(); edit = false\">\r\n                    {{indice == -1 ? \'");
            EndContext();
            BeginContext(6239, 30, false);
#line 120 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                 Write(Localizer.Text("buttonCreate"));

#line default
#line hidden
            EndContext();
            BeginContext(6269, 5, true);
            WriteLiteral("\' : \'");
            EndContext();
            BeginContext(6275, 30, false);
#line 120 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                                                     Write(Localizer.Text("buttonUpdate"));

#line default
#line hidden
            EndContext();
            BeginContext(6305, 71, true);
            WriteLiteral("\'}}\r\n                </v-btn>\r\n                <v-btn color=\"red\" dark ");
            EndContext();
            BeginContext(6377, 44, true);
            WriteLiteral("@click=\"edit = false\">\r\n                    ");
            EndContext();
            BeginContext(6422, 29, false);
#line 123 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
               Write(Localizer.Text("buttonClose"));

#line default
#line hidden
            EndContext();
            BeginContext(6451, 244, true);
            WriteLiteral("\r\n                </v-btn>\r\n            </v-card-actions>\r\n        </v-card>\r\n    </v-dialog>\r\n    <!--\r\n    <v-dialog v-model=\"dialogBloq\" max-width=\"400\">\r\n        <v-card>\r\n            <v-card-title>\r\n                <h4 class=\"modal-title\">");
            EndContext();
            BeginContext(6696, 35, false);
#line 132 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                   Write(Localizer.Text("titleSystemTuFact"));

#line default
#line hidden
            EndContext();
            BeginContext(6731, 74, true);
            WriteLiteral("</h4>\r\n                <v-spacer></v-spacer>\r\n                <v-btn icon ");
            EndContext();
            BeginContext(6806, 95, true);
            WriteLiteral("@click=\"bloquear = false; indice = -1\">\r\n                    <v-icon>\r\n                        ");
            EndContext();
            BeginContext(6902, 30, false);
#line 136 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                   Write(Localizer.Text("iconCloseMDI"));

#line default
#line hidden
            EndContext();
            BeginContext(6932, 135, true);
            WriteLiteral("\r\n                    </v-icon>\r\n                </v-btn>\r\n            </v-card-title>\r\n            <v-card-text>\r\n                <p> ");
            EndContext();
            BeginContext(7068, 33, false);
#line 141 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
               Write(Localizer.Text("textoSecureRole"));

#line default
#line hidden
            EndContext();
            BeginContext(7101, 204, true);
            WriteLiteral(" <strong>{{indice == -1 ? \'\': roles[indice].name}}</strong></p>\r\n            </v-card-text>\r\n            <v-card-actions>\r\n                <v-spacer></v-spacer>\r\n                <v-btn dark color=\"green\" ");
            EndContext();
            BeginContext(7306, 46, true);
            WriteLiteral("@click=\"blockRol(indice); dialogBloq = false\">");
            EndContext();
            BeginContext(7353, 29, false);
#line 145 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                                                                    Write(Localizer.Text("buttonAcept"));

#line default
#line hidden
            EndContext();
            BeginContext(7382, 50, true);
            WriteLiteral("</v-btn>\r\n                <v-btn dark color=\"red\" ");
            EndContext();
            BeginContext(7433, 28, true);
            WriteLiteral("@click=\"dialogBloq = false\">");
            EndContext();
            BeginContext(7462, 29, false);
#line 146 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
                                                                Write(Localizer.Text("buttonClose"));

#line default
#line hidden
            EndContext();
            BeginContext(7491, 98, true);
            WriteLiteral("</v-btn>\r\n            </v-card-actions>\r\n        </v-card>\r\n    </v-dialog>\r\n    -->\r\n</v-app>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(7606, 8, true);
                WriteLiteral("\r\n\r\n    ");
                EndContext();
                BeginContext(7614, 89, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c1ac9260f89add3bc2e4c774bcedf8fe5017bb7622159", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 155 "C:\Users\truco\Desktop\tufactoring-frontend\TuFactoring\Areas\Backoffice\Pages\mantenimientoSettings.cshtml"
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
                BeginContext(7703, 2, true);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TuFactoring.Areas.Backoffice.Pages.mantenimientoSettingsModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Backoffice.Pages.mantenimientoSettingsModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TuFactoring.Areas.Backoffice.Pages.mantenimientoSettingsModel>)PageContext?.ViewData;
        public TuFactoring.Areas.Backoffice.Pages.mantenimientoSettingsModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
