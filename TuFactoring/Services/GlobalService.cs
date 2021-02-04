using GraphQL.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringGraphql;
using TuFactoringModels;
using TuFactoring.CustomProviders;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Services
{
    public class GlobalService : IGlobalService
    {
        private readonly GlobalConsumer _consumer;
        private readonly IConfiguration _configuration;

        public GlobalService(IConfiguration configuration)
        {
            _configuration = configuration;
            _consumer = new GlobalConsumer(new GraphQLClient(_configuration["GlobalEndpoint"]));
        }

        public async Task<Country> GetDocumentByCountry(int countryId)
        {
            try
            {

                return await _consumer.GetDocumentByCountry(countryId);

            }
            catch
            {
                return null;
                throw;
            }
        }
        public async Task<Country> GetDocumentByCountry2(int countryId)
        {
            try
            {

                return await _consumer.GetDocumentByCountry2(countryId);

            }
            catch
            {
                return null;
                throw;
            }
        }

        public async Task<Country> GetDataCountry(int countryId)
        {
            try
            {
                return await _consumer.GetDataCountry(countryId);
            }
            catch
            {
                return null;
                throw;
            }
        }
        public async Task<Country> GetDataCountry1(int countryId)
        {
            try
            {
                return await _consumer.GetDataCountry1(countryId);
            }
            catch
            {
                return null;
                throw;
            }
        }

        public async Task<Country> GetDataCountryInvoices(int countryId, string token = null)
        {
            try
            {
                return await _consumer.GetDataCountryInvoices(countryId, token);
            }
            catch
            {
                return null;
                throw;
            }
        }

        public async Task<Country> GetDataCountryCurrency(int countryId, string token = "", bool onlyCurrency = false)
        {
            try
            {
                return await _consumer.GetDataCountryCurrency(countryId, token, onlyCurrency);
            }
            catch
            {
                return null;
                throw;
            }
        }

        //Consulta de Datos Iniciales Registros
        public async Task<Country> ConsultasCountry(int countryId)
        {
            try
            {
                return await _consumer.ConsultasCountry(countryId);
            }
            catch
            {
                return null;
            }
        }
        public async Task<Country> ConsultaCities(int countryId)
        {
            try
            {
                return await _consumer.ConsultaCities(countryId);
            }
            catch
            {
                return null;
            }
        }

        #region Groups y Programs
        public async Task<List<Country>> ConsultaCountryPrograms(int country, string token)
        {
            try
            {

                return await _consumer.ConsultaCountryPrograms(country, token);

            }
            catch
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Setting
        public async Task<List<Setting>> GetSettings(int countryId, string token = "")
        {
            try
            {

                return await _consumer.GetSettings(countryId,token);

            }
            catch
            {
                return null;
                throw;
            }
        }

        public async Task<Setting> CreateSetting(Setting setting, string token = "")
        {
            try
            {

                return await _consumer.CreateSetting(setting, token);

            }
            catch
            {
                return null;
                throw;
            }
        }

        public async Task<Setting> UpdateSetting(Setting setting, string token = "")
        {
            try
            {

                return await _consumer.UpdateSetting(setting, token);

            }
            catch
            {
                return null;
                throw;
            }
        }

        #endregion

        //Consulta de Bancos
        public async Task<Country> ConsultaBanksAsync(int id, string token = "")
        {
            try
            {

                return await _consumer.ConsultaBanksAsync(id, token);

            }
            catch
            {
                return null;
                throw;
            }
        }

        public async Task<Country> GetCountryEntities(int id, string token = "")
        {
            try
            {

                return await _consumer.GetCountryEntities(id, token);

            }
            catch
            {
                return null;
                throw;
            }
        }

        //Consulta Clientes Banco
        public async Task<Country> ConsultaClienteConfirmantCountry(int countryId)
        {
            try { return await _consumer.ConsultaClienteConfirmantCountry(countryId); }
            catch { return null; }
        }





        // --------------------------------- Nuevas Consultas ----------------------
        public async Task<ListCountry> ConsultasCountryTF(ParamCountry country)
        {
            try { return await _consumer.ConsultasCountryTF(country); }
            catch { return null; }
        }
        public async Task<ListCountry> ConsultasAsociadosTF(ParamCountry country)
        {
            try { return await _consumer.ConsultasAsociadosTF(country); }
            catch { return null; }
        }
        public async Task<ListCountry> ConsultaEstatesTF(ParamCountry country)
        {
            try { return await _consumer.ConsultaEstatesTF(country); }
            catch { return null; }
        }
        public async Task<ListCountry> ConsultaCitiesTF(ParamCountry country)
        {
            try { return await _consumer.ConsultaCitiesTF(country); }
            catch { return null; }
        }
        public async Task<ListCountry> ConsultaIdentificationsAndCitiesTF(ParamCountry country)
        {
            try { return await _consumer.ConsultaIdentificationsAndCitiesTF(country); }
            catch { return null; }
        }
        public async Task<ListCountry> ConsultaRegiosAndCitiesTF(ParamCountry country)
        {
            try { return await _consumer.ConsultaRegiosAndCitiesTF(country); }
            catch { return null; }
        }

        public async Task<ListCountry> ConsultaBanksTF(ParamCountry country)
        {
            try { return await _consumer.ConsultaBanksTF(country); }
            catch { return null; }
        }
    }
}
