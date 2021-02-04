using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Services
{
    public interface IGlobalService
    {
        Task<Country> GetDocumentByCountry(int countryId);
        Task<Country> GetDocumentByCountry2(int countryId);

        Task<Country> GetDataCountry(int countryId);
        Task<Country> GetDataCountry1(int countryId);

        Task<Country> GetDataCountryCurrency(int countryId, string token = "", bool onlyCurrency = false);

        Task<Country> GetDataCountryInvoices(int countryId, string token = ""); 

        //Consulta para registro y actualizar perfil
        Task<Country> ConsultasCountry(int countryId);
        Task<Country> ConsultaCities(int countryId);
        //Consulta para la lista de bancos
        Task<Country> ConsultaBanksAsync(int id,string token = "");

        Task<Country> GetCountryEntities(int id, string token = "");

        //Consulta Clientes Banco
        Task<Country> ConsultaClienteConfirmantCountry(int countryId);

        #region Groups y Programs

        Task<List<Country>> ConsultaCountryPrograms(int country, string token);
        #endregion

        #region Setting
        Task<List<Setting>> GetSettings(int country, string token = "");

        Task<Setting> CreateSetting(Setting setting, string token = "");

        Task<Setting> UpdateSetting(Setting setting, string token = "");

        #endregion
        // --------------------------------- Nuevas Consultas ----------------------
        Task<ListCountry> ConsultasCountryTF(ParamCountry country);
        Task<ListCountry> ConsultasAsociadosTF(ParamCountry country);
        Task<ListCountry> ConsultaEstatesTF(ParamCountry country);
        Task<ListCountry> ConsultaCitiesTF(ParamCountry country);
        Task<ListCountry> ConsultaIdentificationsAndCitiesTF(ParamCountry country);
        Task<ListCountry> ConsultaRegiosAndCitiesTF(ParamCountry country);
        Task<ListCountry> ConsultaBanksTF(ParamCountry country);


    }
}
