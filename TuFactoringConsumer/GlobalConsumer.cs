using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringGraphql
{
    public class GlobalConsumer
    {
        private readonly string __notAuthorized = "You are not authorised to perform this action";
        private readonly GraphQLClient _client;

        public GlobalConsumer(GraphQLClient client)
        {
            _client = client;
        }

        #region Consultas
        private string crearConsultaInicial(int idPais = 0, string tipo = null)
        {
            return @"query catalog { countries " + (idPais != 0 ? "( id : " + idPais + " )" : "")
                + @" {
						id
                        name
                        callingCode
                        currencies { id name symbol digits iso_4217}
                        charges { id name abbreviation status }
                        identifications { id name discriminator abbreviation default prefix
                            prefixes{ id abbreviation }
                        }
                        banks { id name routing_number } 
                    } }";
        }

        private string crearConsultaInicial1(int idPais = 0, string tipo = null)
        {
            return @"query catalog { countries " + (idPais != 0 ? "( id : " + idPais + " )" : "")
                + @" {
						id
                        name
                        callingCode
                        subdivisions { id name status
                            cities { id abbreviation name }
                        }
                        currencies { id name symbol digits iso_4217}
                        identifications { id name discriminator abbreviation default prefix
                            prefixes{ id abbreviation }
                        }
                        occupations { id abbreviation name}
                        purposes { id abbreviation name }
                        banks { id name routing_number } 
                        invoice_types { id name abbreviation }
                        charges {id name abbreviation }
                    } }";
        }

        private string crearConsultaInicialInvoices()
        {
            return @"query ($idPais: Int){
                        countries(input:{id:$idPais}){
                          list{
                            id
                            calling_code
                            programs{
                                abbreviation
                            }
                            charges {
                                id
                                name
                                abbreviation
                            }
                            currencies{
                              id
                              name
                              symbol
                              iso_4217
                              digits
                            }
                            currency{
                                id
                                iso_4217
                                name
                                symbol
                            }
                            identifications{
                              name
                              discriminator
                              prefix
                              abbreviation
                              regexp
                              prefixes{ abbreviation }
                            }
                            settings{
                                content
                                abbreviation
                                mask_edit
                            }
                            }
                          }
                        }";
        }

        private string crearConsultaInicialCurrency()
        {
            return @"query ($idPais: Int){
                          countries(input:{id:$idPais}){
                            list{
                              id
                              currencies{
                                id
                                symbol
                                iso_4217
                                name
                              }
                            programs{
                                abbreviation
                            }
                              charges {
                                id
                                name
                                abbreviation
                            }
                          }
                        }}";
        }
        private string crearConsultaInicialOnlyCurrency()
        {
            return @"query ($idPais: Int){
                          countries(input:{id:$idPais}){
                            list{
                                id
                                currencies{
                                    id
                                    symbol
                                    iso_4217
                                    name
                                }
                          }
                        }}";
        }
        #endregion

        #region getDocumentByCountry
        public async Task<Country> GetDocumentByCountry(int countryId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($input: CountryInput!) {
                    countries(input: $input) {
                       list{
                          id
                         identifications { 
      	                    id name discriminator regexp digits abbreviation default prefix mask_edit status
                            prefixes{ 
                              id abbreviation status
                            }
    	                  }
                       }
                    }
                }",
                Variables = new { input = new { id = countryId} }
            };

            try
            {
                var response = await _client.PostAsync(query);
                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<CountryResponse>("countries").List;
                return countries[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Country> GetDocumentByCountry2(int countryId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($id: Int!) {
                    countries(id: $id) {
                        id
                        name
                        callingCode
                        subdivisions { id name status
                            cities { id abbreviation name }
                        }
                        currencies { id name symbol }
                        identifications { id name discriminator regexp digits abbreviation default prefix
                            prefixes{ id abbreviation }
                        }
                        occupations { id abbreviation name}
                        purposes { id abbreviation name }
                        banks { id name }
                    }
                }",
                Variables = new { id = countryId }
            };

            try
            {
                var response = await _client.PostAsync(query);
                var jsonD = JsonConvert.SerializeObject(response);
                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<List<Country>>("countries");
                return countries[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region dataCountry
        public async Task<Country> GetDataCountry(int countryId)
        {
            var query = new GraphQLRequest
            {
                Query = crearConsultaInicial(countryId),
                Variables = new { id = countryId }
            };

            try
            {
                var response = await _client.PostAsync(query);

                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<List<Country>>("countries");
                return countries[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Country> GetDataCountry1(int countryId)
        {
            var query = new GraphQLRequest
            {
                Query = crearConsultaInicial1(countryId),
                Variables = new { id = countryId }
            };

            try
            {
                var response = await _client.PostAsync(query);

                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<List<Country>>("countries");
                return countries[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region dataCountryInvoices
        public async Task<Country> GetDataCountryInvoices(int countryId, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = crearConsultaInicialInvoices(),
                Variables = new { idPais = countryId }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                    return new Country() { Errors = response.Errors[0].Message};
                }

                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<CountryResponse>("countries").List;
                return countries[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region GetDataCountryCurrency
        public async Task<Country> GetDataCountryCurrency(int countryId, string token, bool onlyCurrency)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Variables = new { idPais = countryId }
            };


            if (onlyCurrency)
                query.Query = crearConsultaInicialOnlyCurrency();
            else
                query.Query = crearConsultaInicialCurrency();
            

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                    return new Country() { Errors = response.Errors[0].Message };
                }

                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<CountryResponse>("countries").List;
                return countries[0];
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        // Consulta para Detalle de Clientes del Confirmant
        #region Consulta para Detalle de Clientes del Confirmant

        public async Task<Country> ConsultaClienteConfirmantCountry(int countryId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($id: Int!) {
                    countries(id: $id) {
                        identifications { id discriminator abbreviation default prefix
                            prefixes{ id abbreviation }
                        }
                        occupations { id abbreviation }
                        purposes { id abbreviation }
                    }
                }",
                Variables = new { id = countryId }
            };

            try
            {
                var response = await _client.PostAsync(query);
                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<List<Country>>("countries");
                return countries[0];
            }
            catch (Exception)
            {
                return new Country();
            }
        }
        #endregion

        // Consulta para registros y para perfiles
        #region ConsultasCountry

        public async Task<Country> ConsultasCountry(int countryId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($id: Int!) {
                    countries(id: $id) {
                        id
                        name
                        callingCode
                        currencies { id name symbol }
                        identifications { id name discriminator regexp digits abbreviation default prefix
                            prefixes{ id abbreviation }
                        }
                        occupations { id abbreviation name}
                        purposes { id abbreviation name }
                        banks { id name }
                    }
                }",
                Variables = new { id = countryId }
            };

            try
            {
                var response = await _client.PostAsync(query);
                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<List<Country>>("countries");
                return countries[0];
            }
            catch (Exception)
            {
                return new Country();
            }
        }
        #endregion
        #region Consulta Estates and Cities
        public async Task<Country> ConsultaCities(int countryId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($id: Int!) {
                    countries(id: $id) {
                        id
                        subdivisions { id name status
                            cities { id abbreviation name }
                        }
                    }
                }",
                Variables = new { id = countryId }
            };

            try
            {
                var response = await _client.PostAsync(query);
                List<Country> countries = new List<Country>();

                countries = response.GetDataFieldAs<List<Country>>("countries");
                return countries[0];
            }
            catch (Exception)
            {
                return new Country();
            }
        }
        #endregion

        //Lista de Bancos
        #region ConsultaBanksAsync
        public async Task<Country> ConsultaBanksAsync(int id, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = @"query($id: Int) 
                { 
                    countries ( input:{id : $id} ){
                        list{ entities { id person{name} status routing_number} settings{abbreviation content} }
                    } 
                }",
                Variables = new { id = id }
            };

            try
            {
                var response = await _client.PostAsync(query);
                var Banks = response.GetDataFieldAs<CountryResponse>("countries").List;
                return Banks[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Country();
            }
        }

        public async Task<Country> GetCountryEntities(int id, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = @"query($id: Int) 
                { 
                    countries ( input:{id : $id} ){
                        list{ 
                            entities 
                            { id 
                            person{ id name} 
                            status 
                            related
                            routing_number
                            }  
                            }
                    } 
                }",
                Variables = new { id = id }
            };

            try
            {

                var graphQLResponse = await this._client.PostAsync(query);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Country { Errors = __notAuthorized };
                }

                var Banks = graphQLResponse.GetDataFieldAs<CountryResponse>("countries").List;
                return Banks[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Country();
            }
        }
        #endregion

        //-------------------- Nuevas Consultas --------------------------------------------------

        // Consulta para registros y para perfiles
        #region ConsultasCountry

        public async Task<ListCountry> ConsultasCountryTF(ParamCountry country)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($country: CountryInput!) {
                      countries(input: $country){
                        list{
                          calling_code
                          settings{
                            abbreviation
                            content
                            mask_edit
                          }
                          identifications{
                            id
                            discriminator
                            prefix
                            regexp
                            mask_edit
                            abbreviation
                            digits
                            default
                            name
                            prefixes{
                              id
                              name
                              identification
                              abbreviation
                              status
                            }
                          }
                          categories{
                            id
                            name
                            discriminator
                          }
                          allies{
                            id
                            routing_number
                            person{
                              company
                            }
                          }
                          entities{
                            id
                            routing_number
                            person{
                              company
                              name
                              id
                            }
                          }
                          currency{
                            id
                            iso_4217
                            name
                            symbol
                          }
                          currencies { id name symbol iso_4217 }
                          regions{
                            id
                            name
                          }
                        }
                      }
                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);
                Countries countries = new Countries();

                countries = response.GetDataFieldAs<Countries>("countries");
                return countries.List[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<ListCountry> ConsultasAsociadosTF(ParamCountry country)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($country: CountryInput!) {
                      countries(input: $country){
                        list{
                          calling_code
                          settings{
                            abbreviation
                            content
                            mask_edit
                          }
                          identifications{
                            id
                            discriminator
                            prefix
                            regexp
                            mask_edit
                            abbreviation
                            digits
                            default
                            name
                            prefixes{
                              id
                              name
                              identification
                              abbreviation
                              status
                            }
                          }
                        }
                      }
                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);
                Countries countries = new Countries();

                countries = response.GetDataFieldAs<Countries>("countries");
                return countries.List[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<ListCountry> ConsultaEstatesTF(ParamCountry country)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($country: CountryInput!) {
                      countries(input: $country){
                        list{
                          regions{
                            id
                            name				            
                          }
                        }
                      }
                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);
                Countries countries = new Countries();

                countries = response.GetDataFieldAs<Countries>("countries");
                return countries.List[0];
            }
            catch (Exception)
            {
                return new ListCountry();
            }
        }
        public async Task<ListCountry> ConsultaCitiesTF(ParamCountry country)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($country: CountryInput!) {
                      countries(input: $country){
                        list{
                          regions{
                            id
                            name
                            code
                            country
				            cities{
                              id
                              calling_code
                              name
                            }
                          }
                        }
                      }
                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);
                Countries countries = new Countries();

                countries = response.GetDataFieldAs<Countries>("countries");
                return countries.List[0];
            }
            catch (Exception)
            {
                return new ListCountry();
            }
        }
        public async Task<ListCountry> ConsultaIdentificationsAndCitiesTF(ParamCountry country)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($country: CountryInput!) {
                      countries(input: $country){
                        list{
                        currency{
                            id
                            iso_4217
                            name
                            symbol
                        }
                         currencies{
                            id
                            name
                            iso_4217
      	                    digits
                            symbol
                         }
                         identifications { 
      	                    id name discriminator regexp digits abbreviation default prefix
                            prefixes{ 
                              id abbreviation 
                            }
    	                  }
                          regions{
                            id
                            name
                            code
                            country
				            cities{
                              id
                              calling_code
                              name
                            }
                          }
                          entities{
                            id
                            person{
                              company
                              name
                              id
                            }
                          }
                        }
                      }
                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);
                Countries countries = new Countries();

                countries = response.GetDataFieldAs<Countries>("countries");
                return countries.List[0];
            }
            catch (Exception)
            {
                return new ListCountry();
            }
        }
        public async Task<ListCountry> ConsultaRegiosAndCitiesTF(ParamCountry country)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($country: CountryInput!) {
                      countries(input: $country){
                        list{
                          calling_code
                          settings{
                            abbreviation
                            content
                          }
                          identifications{
                            id
                            discriminator
                            prefix
                            regexp
                            abbreviation
                            digits
                            default
                            prefixes{
                              id
                              identification
                              abbreviation
                              status
                            }
                          }
                          categories{
                            id
                            name
                            discriminator
                          }
                          allies{
                            id
                            routing_number
                            person{
                              company
                            }
                          }
                          entities{
                            id
                            person{
                              company
                              name
                              id
                            }
                          }
                          currencies { id name symbol iso_4217 }
                          regions{
                            id
                            name
                            code
                            country
				            cities{
                              id
                              calling_code
                              name
                            }
                          }
                        }
                      }
                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);
                Countries countries = new Countries();

                countries = response.GetDataFieldAs<Countries>("countries");
                return countries.List[0];
            }
            catch (Exception)
            {
                return new ListCountry();
            }
        }

        public async Task<ListCountry> ConsultaBanksTF(ParamCountry country)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query country($country: CountryInput!) {
                      countries(input: $country){
                        list{
                        currency{
                            id
                            iso_4217
                            name
                            symbol
                        }
                          currencies { id name symbol iso_4217 }
                          entities{
                            id
                            routing_number
                            person{
                              name
                            }
                          }
                        }
                      }
                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);
                Countries countries = new Countries();

                countries = response.GetDataFieldAs<Countries>("countries");
                return countries.List[0];
            }
            catch (Exception)
            {
                return new ListCountry();
            }
        }
        #endregion


        #region Groups y Programs
        public async Task<List<Country>> ConsultaCountryPrograms(int country, string token)
        {
            List<Country> resp = new List<Country>();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = @"query ($country:Int){
                            countries(input:{id:$country}){
                              list{
                                programs{
                                  id
                                  name
                                }
                                currencies{
                                  id name
                                }
                              }
                            }
                        }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                    resp.Add(new Country() { Errors = response.Errors[0].Message });
                    return resp;
                }

                resp = response.GetDataFieldAs<CountryResponse>("countries").List;
                
                return resp;
            }
            catch (Exception e)
            {
                resp.Add(new Country() { Errors =e.Message });
                return resp;
            }
        }
        #endregion


        #region Settings
        public async Task<List<Setting>> GetSettings(int country,string token)
        {
            List<Setting> resp = new List<Setting>();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = @"query ($country: Int!){
                                  settings(input:{country:$country}){
                                    list{
                                      id
                                      abbreviation
                                      content
                                      description
                                      type_content
                                      status
                                    }
                                  }
                                }",
                Variables = new { country }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                    return new List<Setting>() { new Setting() { Errors = response.Errors[0].Message } };
                }

                resp = response.GetDataFieldAs<SettingResponse>("settings").List;
                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
        }

        public async Task<Setting> CreateSetting(Setting setting, string token)
        {
            Setting resp = new Setting();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = @"mutation ($input: SettingInput!){
                          createSetting(input:$input){
                            id
                          }
                        }",
                Variables = new { input = setting }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                   return new Setting() { Errors = response.Errors[0].Message };
                }

                 resp = response.GetDataFieldAs<Setting>("createSetting");
                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
        }

        public async Task<Setting> UpdateSetting(Setting setting, string token)
        {
            Setting resp = new Setting();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = @"mutation ($input: SettingInput!){
                          updateSetting(input:$input){
                            id
                          }
                        }",
                Variables = new { input = setting }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                    return new Setting() { Errors = response.Errors[0].Message };
                }

                resp = response.GetDataFieldAs<Setting>("updateSetting");
                return resp;
            }
            catch (Exception)
            {
                return resp;
            }
        }

        #endregion

    }
}
