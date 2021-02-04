using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;
using AccountRespond = TuFactoringModels.nuevaVersion.AccountRespond;

namespace TuFactoringGraphql
{
    public class PeopleConsumer
    {
        private readonly string __notAuthorized = "You are not authorised to perform this action";
        private readonly GraphQLClient _client;

        public PeopleConsumer(GraphQLClient client)
        {
            _client = client;
        }

        #region Querys Consultas Generales
        private string consultaProveedoresDebtor()
        {
            return @"
                query($idPersona:String!){
                 listSupplierToDebtor(debtor_id:$idPersona){
                     list{
                            id
                            name
                            documents{
                                prefix
                                display_number
                                identification
                                number display_number
                            }
                        }
                    }}";
        }

        private string queryDebtorForSupplier()
        {
            return @"
                query($idPersona:String!){
                 listClientsToSupplier(supplier_id:$idPersona){
                     list{
                            id
                            name
                            documents{
                                prefix
                                display_number
                                identification
                                number display_number
                            }
                        }
                    }}";
        }

        private string consultaDebtorProveedores()
        {
            return @"
                query($idPersona:String!){
                  listClientsToSupplier(owner_id:$idPersona){
                     id
                     name
                  }
                }";
        }
        private string consultaDebtorConfirmant()
        {
            return @"
                query($idPersona:String!){
                  listClientsToConfirmant(bank_id:$idPersona){
                     id
                     name
                  }
                }";
        }
        private string consultaSupplierConfirmant()
        {
            return @"
                query($idPersona:String!){
                  listSupplierToConfirmant(bank_id:$idPersona){
                     id
                     name
                  }
                }
            ";
        }
        private string consultaSupplierFactor()
        {
            return @"
                query($idPersona:String!){
                  listSupplierToFactor(factor_id:$idPersona){
                     id
                     name
                  }
                }";
        }
        #endregion

        #region Querys Consultas Clientes para el Banco
        private string consultaClientesConfirmant()
        {
            return @"
            query($listaClientes: ListClientsInput!){
	            listClientsToConfirmant(input:$listaClientes){
                    list{
                      id
                      name
                      phones{
                          id
                          number
                      }
                      emails{
                        id
                        address
                      }
                      quotas{
                        available
                        usage
                        abbreviation
                        currency
                      }
                      documents{
                          number
                          prefix
                        identification
                      }
                    }
                }
            }
            ";        
        }

        private string queryIsFintech()
        {
            return @"query($country:Int!, $filter:Filter){
                       people(input:{
                        country:$country
                        filter:$filter
                        }){
                        list{
                          entities{
                            is_fintech
                          }  
                        }
                      }
                    }";
        }

        private string consultaAccountsPeople()
        {
            return @"query($person:PeopleInput!){
                       people(input:$person){
                        list{
                            accounts{
                                id status
                                entity{
                                    id
                                    person{ name }
                                    routing_number
                                }
                                accountNumber
                                accountType
                                currency
                                default
                            }
                        }
                      }
                    }";
        }

        private string consultaDetalleClientesConfirmant()
        {
            return @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                        id
                        country
                        discriminator
                        company
                        category
                        documents{
                            id
                            number
                            prefix
                            identification
                        }
                        firstName
                        lastName
                        addresses{
                            id
                            label
                            line1
                            line2
                            region
                            city
                            country
                        }
                        phones{
                            id
                            label
                            country
                            number
                        }
                        emails{
                            id
                            label
                            address
                        }
                        accounts{
                            id
                            entity{
                                id
                                routing_number
                            }
                            accountNumber
                            accountType
                            currency
                            default
                        }
                        contacts{
                            id
                            name
                            identification
                            label
                            prefix
                            phoneNumber
                            email
                            documentNumber
                        }
                    }
                  }
                }
            ";
        }


        private string listaNombresEmailsConfirmant()
        {
            return @"
                query($idConfirmant: String!){
                listClientsToConfirmant(bank_id: $idConfirmant){
                    id
                    name
                    emails{
                        id
                        address
                    }
                }
            }
            ";
        }
        #endregion

        #region Querys Consultas Usuarios para BackOffice
        private string consultaUserBackOffice()
        {
            return @"
                query($country: Int!, $filter:FilterPeople, $pagination : PaginationInput){
                listPeople(country: $country, filter: $filter, pagination: $pagination){
                    id
                    name
                    lastName
                    discriminator
                    status
                    phones{
                        code
                        number
                    }
                    emails{
                        address
                    }
                    confirmings{
                        amountRisk
                    }
                    documents{
                        number
                        prefix
                      identification
                    }
                }
            }
            ";
        }
        private string consultaDetalleUserBackOffice()
        {
            return @"
                query($id: String!){
                consultaPeople(id: $id){
                    id
                    name
                    addresses{
                      id
                      building
                      street
                      cityId
                    }
                    phones{
                      code
                      number
                    }
                    accounts{
                      nameOnAccount
                      accountNumber
                      accountType
                    }
                    contacts{
                      name
                      last_name
                      category
                      identificationId
                      prefixId
                      number
                      phoneNumber
                      email
                    }
                }
            }
            ";
        }
        #endregion

        #region Get Suppliers for Debtor O Get Debtor for Suppliers
        public async Task<List<People>> GetSuppliers(string idPais, string idCliente, string token)
        {
            GraphQLRequest DataRegistroRequest;

            List<People> lista = new List<People>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaProveedoresDebtor(),
                Variables = new { idPersona = idCliente }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Data == null || graphQLResponse.Data.length == 0)
                {
                    return lista;
                }

                lista = graphQLResponse.GetDataFieldAs<PeopleResponse>("listSupplierToDebtor").List;
            }
            catch (Exception e)
            {
                lista.Add(new People() { Errors = e.Message });
            }

            return lista;
        }

        public async Task<List<People>> GetDebtors(string idCliente, string token)
        {
            GraphQLRequest DataRegistroRequest;

            List<People> lista = new List<People>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = queryDebtorForSupplier(),
                Variables = new { idPersona = idCliente }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Data == null || graphQLResponse.Data.length == 0)
                {
                    return lista;
                }

                lista = graphQLResponse.GetDataFieldAs<PeopleResponse>("listClientsToSupplier").List;
            }
            catch (Exception e)
            {
                lista.Add(new People() { Errors = e.Message });
            }

            return lista;
        }

        public async Task<List<ResponseProveedores>> GetDeptors(string idPersona, string token)
        {
            if (token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            GraphQLRequest DataRegistroRequest;

            List<ResponseProveedores> lista = new List<ResponseProveedores>();

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaDebtorProveedores(),
                Variables = new { idPersona }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Data == null || graphQLResponse.Data.length == 0)
                {
                    return lista;
                }

                lista = graphQLResponse.GetDataFieldAs<List<ResponseProveedores>>("listClientsToSupplier");
            }
            catch (Exception e)
            {
                lista.Add(new ResponseProveedores() { Errors = e.Message });
            }

            return lista;
        }
        #endregion

        #region Get Suppliers and Debtor for Confirmant
        public async Task<List<ResponseProveedores>> GetDeptorsForConfirmant(string idPersona, string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            GraphQLRequest DataRegistroRequest;

            List<ResponseProveedores> lista = new List<ResponseProveedores>();

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaDebtorConfirmant(),
                Variables = new { idPersona }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Data == null || graphQLResponse.Data.length == 0)
                {
                    return lista;
                }

                lista = graphQLResponse.GetDataFieldAs<List<ResponseProveedores>>("listClientsToConfirmant");
            }
            catch (Exception e)
            {
                lista.Add(new ResponseProveedores() { Errors = e.Message });
            }

            return lista;
        }
        public async Task<List<ResponseProveedores>> GetSupplierForConfirmant(string idPersona, string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            GraphQLRequest DataRegistroRequest;

            List<ResponseProveedores> lista = new List<ResponseProveedores>();

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaSupplierConfirmant(),
                Variables = new { idPersona }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Data == null || graphQLResponse.Data.length == 0)
                {
                    return lista;
                }

                lista = graphQLResponse.GetDataFieldAs<List<ResponseProveedores>>("listSupplierToConfirmant");
            }
            catch (Exception e)
            {
                lista.Add(new ResponseProveedores() { Errors = e.Message });
            }

            return lista;
        }
        #endregion

        #region Get Suppliers for Factor
        public async Task<List<ResponseProveedores>> GetSupplierForFactor(string idPersona, string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            GraphQLRequest DataRegistroRequest;

            List<ResponseProveedores> lista = new List<ResponseProveedores>();

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaSupplierFactor(),
                Variables = new { idPersona }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Data == null || graphQLResponse.Data.length == 0)
                {
                    return lista;
                }

                lista = graphQLResponse.GetDataFieldAs<List<ResponseProveedores>>("listSupplierToFactor");
            }
            catch (Exception e)
            {
                lista.Add(new ResponseProveedores() { Errors = e.Message });
            }

            return lista;
        }
        #endregion


        //-------------------- Nuevas Consultas --------------------------------------------------
        #region Consultas Registros
        public async Task<Prospecto> RegisterById(ParamProspecto registro, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                        id
                        country
                        discriminator
                        company
                        category
                        documents{
                            id
                            number
                            prefix
                            identification
                        }
                        entities{
                            routing_number
                        }
                            identities{
                            participant
                        }
                        firstName
                        lastName
                        addresses{
                            id
                            label
                            line1
                            line2
                            region
                            city
                            country
                            zipCode
                        }
                        phones{
                            id
                            label
                            country
                            number
                        }
                        emails{
                            id
                            label
                            address
                        }
                        accounts{
                            id
                            entity{
                                id
                                routing_number
                            }
                            accountNumber
                            accountType
                            currency
                            default
                            status
                        }
                        contacts{
                            id
                            name
                            identification
                            label
                            prefix
                            phoneNumber
                            email
                            documentNumber
                        }
                        agreements{
                            id
                            entity
                            participant
                            accepted
                            acceptedAt
                            status
                            abbreviation
                            createdAt
                        }
                    }
                  }
                }",
                Variables = new { person = registro }
            };

            try
            {
                if (token != null && token !="")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospecto() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Prospectos>("people");
                return person.List[0];
            }
            catch (Exception) { return null; }
        }
        public async Task<Prospecto> RegisterById(ParamProspecto registro)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                        id
                        country
                        discriminator
                        company
                        category
                        documents{
                            id
                            number
                            prefix
                            identification
                        }
                        entities{
                            routing_number
                        }
                            identities{
                            participant
                        }
                        firstName
                        lastName
                        addresses{
                            id
                            label
                            line1
                            line2
                            region
                            city
                            country
                            zipCode
                        }
                        phones{
                            id
                            label
                            country
                            number
                        }
                        emails{
                            id
                            label
                            address
                        }
                        accounts{
                            id
                            entity{
                                id
                                routing_number
                            }
                            accountNumber
                            accountType
                            currency
                            default
                            status
                        }
                        contacts{
                            id
                            name
                            identification
                            label
                            prefix
                            phoneNumber
                            email
                            documentNumber
                        }
                        agreements{
                            id
                            entity
                            participant
                            accepted
                            acceptedAt
                            status
                            abbreviation
                            createdAt
                        }
                    }
                  }
                }",
                Variables = new { person = registro }
            };

            try
            {
                var graphQLResponse = await _client.PostAsync(query);
                var jsonD = JsonConvert.SerializeObject(graphQLResponse);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospecto() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Prospectos>("people");
                return person.List[0];
            }
            catch (Exception) { return null; }
        }
        public async Task<Prospecto> ConsultaAsociados(ParamProspecto registro)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                        id
                        country
                        discriminator
                        company
                        documents{
                            id
                            number
                            prefix
                            identification
                        }
                        customers{
                            id
                            identification
                            prefix
                            company
                            name
                            number
                            email
                            state
                            invitedAt
                            invited
                            phone_number
                            person{
                                id
                                documents{
                                    number
                                }
                            }
                        }
                        suppliers{
                            id
                            identification
                            prefix
                            company
                            name
                            number
                            email
                            state
                            invitedAt
                            invited
                            phone_number
                            person{
                                id
                                documents{
                                    number
                                }
                            }
                        }
                        identities{
                            participant
                        }
                    }
                  }
                }",
                Variables = new { person = registro }
            };

            try
            {
                var graphQLResponse = await _client.PostAsync(query);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospecto() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Prospectos>("people");
                return person.List[0];
            }
            catch (Exception) { return null; }
        }
        public async Task<ListDocuments> RegisterByDocument(ParamProspecto registro)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($document:DocumentInput!){
                  documents(input:$document){
                    list{
                      id
                      person
                      identification
                      prefix
                      number
                      display_number
                    }
                  }
                }",
                Variables = new { document = registro.Document }
            };

            try
            {

                var graphQLResponse = await _client.PostAsync(query);
                var doc = graphQLResponse.GetDataFieldAs<Documents>("documents");
                return doc.List[0];

            }
            catch (Exception) { return null; }
        }
        public async Task<Prospecto> ConsultaEmails(string email, int? country = null)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($email:String!, $country:Int!){
                  emailVerify(email:$email, country:$country){
                    id
                    name
                  }
                }",
                Variables = new { email, country }
            };

            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                var usuario = graphQLResponse.GetDataFieldAs<Prospecto>("emailVerify");
                return usuario;

            }
            catch (Exception) { return new Prospecto { Name = graphQLResponse.Errors[0].Message }; }
        }

        public async Task<AccountRespond> ConsultaAccount(string entity, string accountNumber, string token = null)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($ID:String!, $accountNumbery:String!){
                  emailVerify(id:$ID, account_number:$accountNumbery){
                    id
                  }
                }",
                Variables = new { entity, accountNumber }
            };

            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                var usuario = graphQLResponse.GetDataFieldAs<AccountRespond>("emailVerify");
                return usuario;

            }
            catch (Exception) { return new AccountRespond { Errors = graphQLResponse.Errors[0].Message }; }
        }
        #endregion
        #region RegistrarDatosAsync
        public async Task<string> RegisterDebtorTF(Persons registro)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation ($person: RegisterInput!){
                  createDebtor(input:$person){
                    id
                  }
                }",
                Variables = new { person = registro }
            };

            var jsonD = JsonConvert.SerializeObject(registro);
            var graphQLResponse = await _client.PostAsync(query);

            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return __notAuthorized;
                }
                var person = graphQLResponse.GetDataFieldAs<Prospecto>("createDebtor");
                return "success: " + person.Id;

            }
            catch (Exception) { return graphQLResponse.Errors[0].Message; } //errores.Errors[0].ToString()
        }
        public async Task<string> RegisterSupplierTF(Persons registro)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation ($person: RegisterInput!){
                  createSupplier(input:$person){
                    id
                  }
                }",
                Variables = new { person = registro }
            };

            var jsonD = JsonConvert.SerializeObject(registro);
            var graphQLResponse = await _client.PostAsync(query);

            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return __notAuthorized;
                }
                var person = graphQLResponse.GetDataFieldAs<Prospecto>("createSupplier");
                return "success: " + person.Id;

            }
            catch (Exception) { return graphQLResponse.Errors[0].Message; } //errores.Errors[0].ToString()
        }
        public async Task<string> RegisterFactorTF(Persons registro)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation ($person: RegisterInput!){
                  createFactor(input:$person){
                    id
                  }
                }",
                Variables = new { person = registro }
            };
            var jsonD = JsonConvert.SerializeObject(registro);
            var graphQLResponse = await _client.PostAsync(query);

            try
            {

                var person = graphQLResponse.GetDataFieldAs<Prospecto>("createFactor");
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return __notAuthorized;
                }
                return "success: " + person.Id;

            }
            catch (Exception) { return graphQLResponse.Errors[0].Message; } //errores.Errors[0].ToString()
        }
        public async Task<string> RegisterConfirmantTF(Persons registro, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation ($person: RegisterInput!){
                  createEntity(input:$person){
                    id
                  }
                }",
                Variables = new { person = registro }
            };
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var jsonD = JsonConvert.SerializeObject(registro);
            var graphQLResponse = await _client.PostAsync(query);

            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return __notAuthorized;
                }
                var person = graphQLResponse.GetDataFieldAs<Prospecto>("createEntity");
                return "success: " + person.Id;
            }
            catch (Exception) { return graphQLResponse.Errors[0].Message; } //errores.Errors[0].ToString()
        }

        public async Task<Prospecto> UpdateProfileTF(Profiles perfil, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($perfil: PersonInput!){
                  updatePerson(input:$perfil){
                    id
                    country
                    discriminator
                    company
                    category
                    documents{
                        id
                        number
                        prefix
                        identification
                    }
                    firstName
                    lastName
                    addresses{
                        id
                        label
                        line1
                        line2
                        region
                        city
                        country
                    }
                    phones{
                        id
                        label
                        country
                        number
                    }
                    emails{
                        id
                        label
                        address
                    }
                    accounts{
                        id
                        entity{
                            id
                            routing_number
                        }
                        accountNumber
                        accountType
                        currency
                        default
                        status
                    }
                    contacts{
                        id
                        name
                        identification
                        label
                        prefix
                        phoneNumber
                        email
                        documentNumber
                    }
                    agreements{
                        id
                        entity
                        participant
                        accepted
                        acceptedAt
                        status
                        abbreviation
                        createdAt
                    }
                  }
                }",
                Variables = new { perfil }
            };

            var jsonD = JsonConvert.SerializeObject(perfil);
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var graphQLResponse = await _client.PostAsync(query);
            var a = 2;
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospecto() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Prospecto>("updatePerson");
                return person;

            }
            catch (Exception) { return new Prospecto() { Error = graphQLResponse.Errors[0].Message }; }
        }
        public async Task<Prospecto> UpdateAsociadoTF(Profiles perfil, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($perfil: PersonInput!){
                  updatePerson(input:$perfil){
                    id
                    country
                    discriminator
                    company
                    category
                    documents{
                        id
                        number
                        prefix
                        identification
                    }                  
                    customers{
                        id
                        identification
                        prefix
                        company
                        name
                        number
                        email
                        state
                        invitedAt
                        phone_number
                        invited
                        person{
                            id
                            documents{
                                number
                            }
                        }
                    }
                    suppliers{
                        id
                        identification
                        prefix
                        company
                        name
                        number
                        email
                        state
                        invitedAt
                        invited
                        phone_number
                        person{
                            id
                            documents{
                                number
                            }
                        }
                    }                    
                    agreements{
                        id
                        entity
                        participant
                        accepted
                        acceptedAt
                        status
                        abbreviation
                        createdAt
                    }
                  }
                }",
                Variables = new { perfil }
            };
            var jsonD = JsonConvert.SerializeObject(perfil);

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospecto() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Prospecto>("updatePerson");
                return person;

            }
            catch (Exception) { return null; }
        }
        #endregion
        #region Mutaciones de Invitacion rechazo Cancelar y Toggle Principaes Clientes y Proveedores
        public async Task<Guest> AcceptInvitation(ParamGuests guest, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($guest: InvitationInput!){
                  acceptInvitation(input:$guest){
                    id
                  }
                }",
                Variables = new { guest }
            };

            var jsonD = JsonConvert.SerializeObject(guest);
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Guest() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Guest>("acceptInvitation");
                return person;
            }
            catch (Exception) { return new Guest { Error = graphQLResponse.Errors[0].Message }; }
        }
        public async Task<Guest> CancelInvitation(ParamGuests guest, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($guest: InvitationInput!){
                  cancelInvitation(input:$guest){
                    id
                  }
                }",
                Variables = new { guest }
            };

            var jsonD = JsonConvert.SerializeObject(guest);
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }
            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Guest() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Guest>("cancelInvitation");
                return person;

            }
            catch (Exception) { return new Guest { Error = graphQLResponse.Errors[0].Message }; }
        }
        public async Task<Guest> ToggleInvitation(ParamGuests guest, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($guest: InvitationInput!){
                  toggleInvitation(input:$guest){
                    id
                  }
                }",
                Variables = new { guest }
            };

            var jsonD = JsonConvert.SerializeObject(guest);
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Guest() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Guest>("toggleInvitation");
                return person;

            }
            catch (Exception) { return new Guest { Error = graphQLResponse.Errors[0].Message }; }
        }
        public async Task<Guest> RejectInvitation(ParamGuests guest, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($guest: InvitationInput!){
                  rejectInvitation(input:$guest){
                    id
                  }
                }",
                Variables = new { guest }
            };
            var jsonD = JsonConvert.SerializeObject(guest);
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Guest() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Guest>("rejectInvitation");
                return person;

            }
            catch (Exception) { return new Guest { Error = graphQLResponse.Errors[0].Message }; }
        }
        public async Task<Guest> SendInvitation(ParamGuests guest, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($guest: InvitationInput!){
                  sendInvitation(input:$guest){
                    id
                  }
                }",
                Variables = new { guest }
            };
            var jsonD = JsonConvert.SerializeObject(guest);
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Guest() { Error = __notAuthorized };
                }
                var person = graphQLResponse.GetDataFieldAs<Guest>("sendInvitation");
                return person;

            }
            catch (Exception) { return new Guest { Error = graphQLResponse.Errors[0].Message }; }
        }
        #endregion

        //Termino y Condiciones
        #region Consulta Contratos Terminos y Contrato Marco
        public async Task<Prospecto> ConsultaContratoAsync(ParamProspecto person, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                        id
                        discriminator
                        agreements{
                            id
                            acceptedAt
                            participant
                            abbreviation
                            entity
                            accepted
                            status
                        }
                    }
                  }
                }",
                Variables = new { person }
            };
            //var jsonD = JsonConvert.SerializeObject(registro);
            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                var people = graphQLResponse.GetDataFieldAs<Prospectos>("people");
                return people.List[0];

            }
            catch (Exception) { return null; }
        }

        public async Task<Agreements> MutacionContratoAsync(AcceptanceAgreements contrato, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"mutation($contrato: AgreementInput!){
                    acceptanceAgreement(input:$contrato){
                        id
                    }
                }",
                Variables = new { contrato }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                return graphQLResponse.GetDataFieldAs<Agreements>("acceptanceAgreement");

            }
            catch (Exception) {  return null; }
        }
        #endregion

        //Vista Verificar Datos
        #region ConsultaDatosParaVerificarAsync
        public async Task<Verification> ConsultaDatosParaVerificarAsync(string confirmant, filterInvoice filter, Pagination pagination, string token)
        {
            //$confirmant: String!, $filter: Filter, $pagination: PaginationInput
            var query = new GraphQLRequest
            {
                Query = @"query($confirmant: String!, $filter: Filter) {
                      verificable(input: {
                    confirmant:$confirmant
                    filter: $filter
                    #pagination: $pagination
                    }){
                        list{
                            id
                            person{
                                id
                                discriminator
                                documents{
                                    identification
                                    prefix
                                    number
                                }
                                company
                                firstName
                                name
                                addresses{
                                    line1
                                    line2
                                    region
                                    city
                                }
                                identities{
                                    participant
                                    state
                                }
                                agreements{
                                    participant
                                }
                                contacts{
                                    label
                                    documentNumber
                                    identification
                                    prefix
                                    name
                                    email
                                    phoneNumber
                                }
                            }
                          accepted
                          acceptedAt
                          createdAt
                        }
                    }
                }",
                Variables = new { confirmant, filter }
            };
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var graphQLResponse = await _client.PostAsync(query);
            
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Verification() { Error = __notAuthorized };
                }
                return graphQLResponse.GetDataFieldAs<Verification>("verificable");

            }
            catch (Exception)
            {
                return new Verification();
            }
        }
        #endregion
        #region MutacionDatosParaVerificarAsync
        public async Task<string> MutacionDatosParaVerificarAsync(string id, ApproveVerification verification, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                    mutation($id :ID! ,$verification :VerificationInput!){
                        approveVerification(id: $id, input: $verification){
                            id
                        }
                    }",
                Variables = new { id, verification }
            };
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }
            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return  __notAuthorized;
                }
                var respuestaVerficar = graphQLResponse.GetDataFieldAs<ListVerification>("approveVerification");
                return "success:" + respuestaVerficar.Id;

            }
            catch (Exception)
            {
                //return "Error";
                return graphQLResponse.Errors[0].Message;
            }
        }
        #endregion

        //Vista Segmentar
        #region Consulta DatosParaSegmentar y MutacionSegmentacion
        public async Task<Prospectos> ConsultaDatosParaSegmentarAsync(string confirmant, filterInvoice filter, Pagination pagination, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"query($confirmant: String!, $filter: Filter) {
                      segmentable(input: {
                    confirmant:$confirmant
                    filter: $filter
                    #pagination: $pagination
                    }){
                        list{
                            createdAt
                            id
                            discriminator
                            documents{
                                identification
                                prefix
                                number
                            }
                            company
                            firstName
                            name
                            addresses{
                                line1
                                line2
                                region
                                city
                            }
                            identities{
                                participant
                                state
                            }
                            executives{
                                id
                                name
                            }
                            contacts{
                                label
                                documentNumber
                                identification
                                prefix
                                name
                                email
                                phoneNumber
                            }
                            
                        }
                    }
                }",
                Variables = new { confirmant, filter }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospectos() { Error = __notAuthorized };
                }
                return graphQLResponse.GetDataFieldAs<Prospectos>("segmentable");

            }
            catch (Exception) { return new Prospectos(); }
        }
        public async Task<string> MutacionSegmentacionAsync(string id, SegmentPerson segmentado, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($id:ID!,$segmentado:SegmentInput!){
                  segmentPerson(id: $id, input: $segmentado){
                    id
                  }
                }",
                Variables = new { id, segmentado }
            };

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return __notAuthorized;
                }
                var respuesta = graphQLResponse.GetDataFieldAs<ListVerification>("segmentPerson");
                return "success:" + respuesta.Id;

            }
            catch (Exception) { return graphQLResponse.Errors[0].Message; }
        }
        #endregion

        //Vista Limite de Cuenta
        #region  Consulta de DatosEjecutivoCuentas y Mutacion
        public async Task<Prospectos> ConsultaDatosEjecutivoCuentasAsync(ParamCreditLimit limiteCredito, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"query($limiteCredito :  PortfolioInput!) {
                      portfolio(input:$limiteCredito){
                        list{
                            createdAt
                            id
                            discriminator
                            documents{
                                identification
                                prefix
                                number
                            }
                            company
                            firstName
                            name
                            addresses{
                                line1
                                line2
                                region
                                city
                            }
                            identities{
                                participant
                                state
                            }
                            quotas{
                                id
                                usage
                                available
                                currency
                                abbreviation
                            }
                            contacts{
                                label
                                documentNumber
                                identification
                                prefix
                                name
                                email
                                phoneNumber
                            }
                            
                        }
                    }
                }",
                Variables = new { limiteCredito }
            };

            var jsonD = JsonConvert.SerializeObject(limiteCredito);
            var json2 = JsonConvert.SerializeObject(query);
            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospectos() { Error = __notAuthorized };
                }
                return graphQLResponse.GetDataFieldAs<Prospectos>("portfolio");

            }
            catch (Exception) { return new Prospectos(); }
        }

        public async Task<string> MutacionLimiteCuentaAsync(AllocateQuota limiteCredito, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                    mutation($limiteCredito:PortfolioInput!){
                      allocateQuota(input:$limiteCredito){
                        id
                      }
                    }",
                Variables = new { limiteCredito }
            };
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var jsonD = JsonConvert.SerializeObject(limiteCredito);
            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return  __notAuthorized;
                }
                var person = graphQLResponse.GetDataFieldAs<Prospecto>("allocateQuota");
                return "success:" + person.Id;
            }
            catch (Exception) { return graphQLResponse.Errors[0].Message; }
        }
        #endregion

        //Consulta Clientes del Banco y Consulta de Detalle
        # region Consulta Clientes del Banco
        public async Task<Prospectos> GetConsultaClientConfirmant(ParamClienteOFConfirmant listaClientes, string token)
        {
            listaClientes.Participant = "DEBTOR";
            var query = new GraphQLRequest
            {
                Query = @"
                query($listaClientes: ListClientsInput!){
	                listClientsToConfirmant(input:$listaClientes){
                        list{
                          id
                          name
                          discriminator
                          company
                          phones{
                              id
                              number
                          }
                          emails{
                            id
                            address
                          }
                          documents{
                              number
                              prefix
                            identification
                          }
                          contacts{
                            id
                            name
                            identification
                            label
                            prefix
                            phoneNumber
                            email
                            documentNumber
                          }
                       }
                    }
                }",
                Variables = new { listaClientes }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospectos() { Error = __notAuthorized };
                }
                var people = graphQLResponse.GetDataFieldAs<Prospectos>("listClientsToConfirmant");
                return people;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<Prospecto> GetDetalleClientConfirmant(ParamProspecto person, string token)
        {

            var query = new GraphQLRequest
            {
                Query = @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                        id
                        country
                        discriminator
                        company
                        category
                        documents{
                            id
                            number
                            prefix
                            identification
                        }
                        firstName
                        lastName
                        addresses{
                            id
                            label
                            line1
                            line2
                            region
                            city
                            country
                        }
                        phones{
                            id
                            label
                            country
                            number
                        }
                        quotas{
                            entity { id }
                            available
                            usage
                            abbreviation
                            currency
                        }
                        emails{
                            id
                            label
                            address
                        }
                        accounts{
                            id
                            entity{
                                id
                                routing_number
                            }
                            accountNumber
                            accountType
                            currency
                            default
                            status
                        }
                        contacts{
                            id
                            name
                            identification
                            label
                            prefix
                            phoneNumber
                            email
                            documentNumber
                        }
                        agreements{
                            acceptedAt
                            participant
                            entity
                            accepted
                        }
                    }
                  }
                }",
                Variables = new { person }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new Prospecto() { Error = __notAuthorized };
                }
                var people = graphQLResponse.GetDataFieldAs<Prospectos>("people");
                return people.List[0];
            }
            catch (Exception) { return null; }
        }

        public async Task<Prospecto> GetDetailQuotas(ParamProspecto person, string token)
        {

            var query = new GraphQLRequest
            {
                Query = @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                      id
                      quotas  {
                        entity { id person { name } }
                        abbreviation
                        currency
                        available
                        usage
                      }
                    }
                  }
                }",
                Variables = new { person }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                var people = graphQLResponse.GetDataFieldAs<Prospectos>("people");
                return people.List[0];
            }
            catch (Exception) { return null; }
        }
        public async Task<Prospectos> GetConsultaFinaciamientoConfirmant(ParamClienteOFConfirmant listaClientes, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($bank_id: String!, $filter:Filter, $pagination : PaginationInput){
	                queryFinancing(input:{bank_id:$bank_id, filter: $filter, pagination : $pagination}){
                        list{
                          id
                          name
                          discriminator
                          company
                          phones{
                              id
                              number
                          }
                          emails{
                            id
                            address
                          }
                          documents{
                              number
                              prefix
                            identification
                          }
                          quotas{
                            available
                            usage
                            abbreviation
                            currency
                          }
                          accountants{
                            peopleID
                            name
                            count
                            sum
                          }
                          contacts{
                            id
                            name
                            identification
                            label
                            prefix
                            phoneNumber
                            email
                            documentNumber
                          }
                       }
                    }
                }",
                Variables = new
                {
                    bank_id = listaClientes.Bank_id,
                    participant = listaClientes.Participant,
                    filter = listaClientes.Filter == null ? null : new
                    {
                        debtor = listaClientes.Filter.Debtor,
                        currency = listaClientes.Filter.Currency_id,
                        expiration_from = listaClientes.Filter.ExpirationFrom.HasValue ? listaClientes.Filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = listaClientes.Filter.ExpirationTo.HasValue ? listaClientes.Filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        abbreviation = "CREDIT",
                        invoiceStatusNot = "overdue",
                    },
                    pagination = listaClientes.Pagination
                }
            };
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var jsonD = JsonConvert.SerializeObject(listaClientes);
            var graphQLResponse = await _client.PostAsync(query);
            try
            {
                
                var people = graphQLResponse.GetDataFieldAs<Prospectos>("queryFinancing");
                return people;
                //return new Prospectos() { List = new List<Prospecto>() { new Prospecto() { Error = __notAuthorized } } };
            }
            catch (Exception)
            {
                return new Prospectos() { List = new List<Prospecto>() { new Prospecto() { Error = graphQLResponse.Errors[0].Message } } };
            }
        }
        #endregion

        //Consulta de Usuarios para el BackOffice
        # region Consulta de Usuarios para el BackOffice
        public async Task<Prospectos> GetConsultaUsersBackOffice(ParamProspecto person, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($person:PeopleInput!){
                  people(input:$person){
                    list{
                        id
                        state
                        country
                        discriminator
                        company
                        name
                        category
                        documents{
                            id
                            number
                            prefix
                            identification
                        }
                        firstName
                        lastName
                        addresses{
                            id
                            label
                            line1
                            line2
                            region
                            city
                            country
                        }
                        phones{
                            id
                            label
                            country
                            number
                        }
                        emails{
                            id
                            label
                            address
                        }
                        accounts{
                            id
                            entity{
                                id
                                routing_number
                            }
                            accountNumber
                            accountType
                            currency
                            default
                        }
                        contacts{
                            id
                            name
                            identification
                            label
                            prefix
                            phoneNumber
                            email
                            documentNumber
                        }
                        agreements{
                            acceptedAt
                            participant
                            entity
                            accepted
                        }
                    }
                  }
                }",
                Variables = new { person }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                return graphQLResponse.GetDataFieldAs<Prospectos>("people");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<ClienteOfConfirmant> GetDetalleUsersBackOffice(string token, string user = null)
        {
            ClienteOfConfirmant people = new ClienteOfConfirmant();
            GraphQLRequest DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaDetalleUserBackOffice(),
                Variables = new { id = user }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(DataRegistroRequest);
                people = graphQLResponse.GetDataFieldAs<ClienteOfConfirmant>("consultaPeople");
            }
            catch (Exception) { return new ClienteOfConfirmant(); }

            return people;
        }
        #endregion

        //Verify if IsFintech

        public async Task<List<Prospecto>> IsFintech(int country, string token)
        {
            filterInvoice filter = new filterInvoice();

            var query = new GraphQLRequest
            {
                Query = this.queryIsFintech(),
                Variables = new { 
                    country,
                    filter = filter == null ? null : new
                    {
                        participant = "CONFIRMANT"
                    },
                }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);

                if (graphQLResponse.Errors != null)
                {
                    return new List<Prospecto>() { new Prospecto() { Error = graphQLResponse.Errors[0].Message } };
                }

                var entityResponse = graphQLResponse.GetDataFieldAs<Prospectos>("people").List;

                return entityResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<AccountRespond>> GetConsultaAccountsPeople(ParamProspecto person, string token)
        {
            var query = new GraphQLRequest
            {
                Query = this.consultaAccountsPeople(),
                Variables = new { person }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                
                if(graphQLResponse.Errors != null)
                {
                    return new List<AccountRespond>() { new AccountRespond() { Errors = graphQLResponse.Errors[0].Message } };
                }

                var accounts = graphQLResponse.GetDataFieldAs<Prospectos>("people").List;

                return accounts[0].Accounts;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User> Entity(string person)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query ($person: ID!){
                  entity(person_id:$person){
                    id
                  }
                }",
                Variables = new { person = person }
            };

            try
            {

                var graphQLResponse = await _client.PostAsync(query);
                return graphQLResponse.GetDataFieldAs<User>("entity");

            }
            catch (Exception) { return new User(); }
        }

        public async Task<Entity> MakeEntityAllied(Entity entity, int idCountry, string token)
        {

            Entity newAllied = new Entity();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var query = new GraphQLRequest
            {
                Query = @"mutation($country:Int!,$id:String!){
                          relateEntity(input:{
                            country:$country
                            id:$id
                          }){
                            id
                            country
                            person{
                              name
                            }
                            related
                          }
                        }",
                Variables = 
                new { country = idCountry,
                      id = entity.Id,
                }
            };

            try
            {
                var graphQLResponse = await _client.PostAsync(query);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    newAllied.Errors = __notAuthorized;

                    return newAllied;
                }
                newAllied = graphQLResponse.GetDataFieldAs<Entity>("relateEntity");
                return newAllied;

            }
            catch (Exception e)
            {
                newAllied.Errors = e.Message;   
                return newAllied;
            }
        }

        public async Task<Dashboard> GetDashboard(string topic, int currency, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query ($topic: String, $currency: Int) {
                    dashboard (topic: $topic, currency: $currency) {
                        count
                        list {
                            participant
                            content
                            abbreviation
                            description
                            value
                            min
                            max
                            ratio
                            count
                            url
                            icon
                            currency{
                                id
                                name
                                iso_4217
                                symbol
                                digits
                            }
                            items {
                                participant
                                content
                                abbreviation
                                description
                                value
                                min
                                max
                                ratio
                                ratio_count
                                count
                                url
                                icon
                            }
                        }
                    }
                }",
                Variables = new { topic, currency }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                var dashboard = graphQLResponse.GetDataFieldAs<Dashboard>("dashboard");
                return dashboard;
            }
            catch (Exception) { return null; }
        }
    }
}
