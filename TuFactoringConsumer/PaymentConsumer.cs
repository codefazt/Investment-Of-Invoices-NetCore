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
    public class PaymentConsumer
    {
        private readonly string __notAuthorized = "You are not authorised to perform this action";
        private readonly GraphQLClient _client;

        public PaymentConsumer(GraphQLClient client)
        {
            _client = client;
        }

        //Lista de Bancos Asociados
        #region ConsultaBanksAsociadosAsync
        public async Task<AlliedAccount> ConsultaBanksAsociadosAsync(string idPais)
        {
            var query = new GraphQLRequest
            {
                Query = @"query($idPais: String!){
                    bancosaliados(idPais:$idPais){
                        titular
                        documentoidentidad
                        cuentas{
                            idbanco
                            nombre
                            cuenta
                            currencysymbol
                            currencyiso
                        }
                    }
                }",
                Variables = new { idPais = idPais }
            };

            try
            {
                var response = await _client.PostAsync(query);
                return response.GetDataFieldAs<AlliedAccount>("bancosaliados");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new AlliedAccount();
            }
        }
        #endregion

        //Vista Pago de Facturas
        #region ConsultaFacturasAsync
        public async Task<List<Receipts>> ConsultaFacturasAsync(string payer, string country,string abbreviation, string state, string payed_date, string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var query = new GraphQLRequest
            {
                Query = @"query($payer: String, $country: Int!,$abbreviation: String, $state: String, $payed_date: Time){
                    receipts(input:{payer: $payer, country:$country, abbreviation: $abbreviation, state: $state, payed_date: $payed_date}){
                       list{
                          id
                          state
                          method
                          receipt_date
                         payments{
                            entity{
                              person{
                                name
                              }
                            }
                            amount
    		                payment_date
                            number
                            account_number
                            state
                          } 
                          currency{
                            id
                            iso_4217
                            digits
                            symbol
                          }
                          entity{
                            id routing_number
                            person{
                              id
                              name
                              lastName
                            }
                          }
                          receiver{
                            name
                            documents{
                              id
                              display_number
                            }
                          }
                          receiving_account{
                            id
                            accountType
                            accountNumber
                          }
                          payer{
                            id
                            name
                          }
                          paying_account{
                            id
                            accountNumber
                          }
                          paid
                          processing
                          unpaid
                          commission
                          amount
                          program { abbreviation name currency }
                          publications{
                            payable receivable discount commission earnings
                            program { abbreviation name currency }
                            invoice{
                                request_financing
                                number
                                expiration_date
                                term_days
                                amount
                                original_amount
                                debtor{
                                    name
                                }
                                supplier{
                                  name
                                  documents{
                                    display_number
                                  }
                                }
                          }
                        } 
                      }
                    }
                }",
                Variables = new { payer, country, abbreviation, state, payed_date }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if(response.Errors != null)
                {
                    return new List<Receipts>() { new Receipts() { Errors = response.Errors[0].Message } };
                }

               var data = response.GetDataFieldAs<ReceiptsResponse>("receipts").List;

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Receipts>();
            }
        }

        public async Task<List<Receipts>> ConsultaFacturasAsyncFactor(string payer, string country, string abbreviation, string state, string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var query = new GraphQLRequest
            {
                Query = @"query($payer: String, $country: Int!,$abbreviation: String, $state: String){
                    receipts(input:{payer: $payer, country:$country, abbreviation: $abbreviation, state: $state}){
                       list{
                          id
                          state
                          method
                          receipt_date
                         payments{
                            entity{
                              person{
                                name
                              }
                            }
                            amount
    		                payment_date
                            number
                            account_number
                            state
                          } 
                          currency{
                            id
                            iso_4217
                            digits
                            symbol
                          }
                          entity{
                            id routing_number
                            person{
                              id
                              name
                              lastName
                            }
                          }
                          receiver{
                            name
                            documents{
                              id
                              display_number
                            }
                          }
                          receiving_account{
                            id
                            accountType
                            accountNumber
                          }
                          payer{
                            id
                            name
                          }
                          paying_account{
                            id
                            accountNumber
                          }
                          paid
                          processing
                          unpaid
                          commission
                          amount
                          program { abbreviation name }
                          publications{
                            payable receivable discount commission earnings
                            invoice{
                                request_financing
                                number
                                expiration_date
                                term_days
                                amount original_amount
                                debtor{
                                    name
                                }
                                supplier{
                                  name
                                  documents{
                                    display_number
                                  }
                                }
                          }
                        } 
                      }
                    }
                }",
                Variables = new { payer, country, abbreviation, state}
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                    return new List<Receipts>() { new Receipts() { Errors = response.Errors[0].Message } };
                }

                var data = response.GetDataFieldAs<ReceiptsResponse>("receipts").List;

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Receipts>();
            }
        }
        #endregion
        #region ConsultaDetallesFacturasAsync
        public async Task<List<Receipts>> ConsultaDetallesFacturasAsync(string id, string factorId = null, string confirmantId = null)
        {
            var query = new GraphQLRequest
            {
                Query = @"query($userId : String!){
                        
                    FacturasInversionista(id: $id, factor:$factorId, confirmant: $confirmantId){
                        invoices{
                            id
                            suppliername
                            number
                            amount
                        }
                                 
                    }

                }",
                Variables = new { id, factorId, confirmantId }
            };

            try
            {
                var response = await _client.PostAsync(query);
                return response.GetDataFieldAs<List<Receipts>>("FacturasInversionista");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Receipts>();
            }
        }
        #endregion
        #region MutacionPagoFacturasAsync
        public async Task<Receipts> MutacionPagoFacturasAsync(Payment pago, string token)
        {
            Receipts respuestaPago = new Receipts();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var query = new GraphQLRequest
            {
                Query = @"mutation($pago:PaymentInput!){
                      paymentReceipt (input:$pago){id state }
                    }",
                Variables = new { pago }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null && response.Errors[0].Message == "account not valid")
                {
                    respuestaPago.Errors = response.Errors[0].Message;
                    return respuestaPago;
                }

                respuestaPago = response.GetDataFieldAs<Receipts>("paymentReceipt");

                return respuestaPago;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                respuestaPago.Errors = e.Message;
                return respuestaPago;
            }
        }
        #endregion
        //Servicios para Aplicacion de Pagos
        #region AplicacionPagos
        public async Task<List<Receipts>> GetAplicationPays(string country, string entity, string token, string abbreviation)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var query = new GraphQLRequest
            {
                Query = @"query($country: Int!, $entity: String, $abbreviation: String){
                    receipts(input:{country:$country, entity: $entity, abbreviation: $abbreviation}){
                       list{
                          id
                          receipt_date
                          method
                          state
                          method
                          currency{
                            id
                            iso_4217
                            digits
                            symbol
                          }
                          abbreviation
                          entity{
                            id
                            person{
                              name
                              lastName
                            }
                          }
                          receiver{
                            name
                            documents{
                              id
                              display_number
                            }
                          }
                          receiving_account{
                            id
                            accountType
                            accountNumber
                          }
                          payer{
                            discriminator
                            name
                            firstName
                            lastName
                          }
                          paying_account{
                            id
                            accountNumber
                            accountType
                          }
                          amount
                        program { abbreviation name }
                        publications{
                          payable
                          discount
                          earnings
                          receivable
                          commission
                          invoice{
                            issued_date
                            expiration_date
                            term_days
                            original_amount
                            amount
                            number
                            request_financing
                            debtor{
                              name
                            }
                            supplier{
                              name
                              documents{
                                display_number
                              }
                            }
                          }
                          }
                        }                        
                    }
                }",
                Variables = new { country, entity, abbreviation }
            };

            try
            {
                var response = await _client.PostAsync(query);
                var data = response.GetDataFieldAs<ReceiptsResponse>("receipts").List;
                
                if (response.Errors != null)
                {
                    if (response.Errors[0].Message == __notAuthorized)
                    {
                        return new List<Receipts>() { new Receipts() { Errors = response.Errors[0].Message } };
                    }

                }

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Receipts>();
            }
        }


        public async Task<Receipts> MutationPayReceipt(Payment pago, string token)
        {

            Receipts respuestaPago = new Receipts();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var query = new GraphQLRequest
            {
                Query = @"mutation($receipt:String){
                          paymentReceipt(input:{
                            receipt:$receipt
                            })
                            {id state}
                        }",
                Variables = new 
                { 
                    receipt = pago.Receipt 
                }
            };

            try
            {
                var response = await _client.PostAsync(query);
                if(response.Errors != null)
                {
                    if(response.Errors[0].Message == __notAuthorized)
                    {
                        respuestaPago.Errors = __notAuthorized;
                        return respuestaPago;
                    }
                }
                respuestaPago = response.GetDataFieldAs<Receipts>("paymentReceipt");

                return respuestaPago;
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                respuestaPago.Errors = "Error";
                return respuestaPago;
            }
        }
        #endregion
        //Vista Consolidación
        #region ConsultaConciliacions y Mutacion Async
        public async Task<ListPayments> ConsultaConciliacionsAsync(ParamPayments conciliar, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($conciliar : PaymentsInput!){
                  payments(input:$conciliar){
                    list{
                      id
                      receipt{
                        payer{
                          name
                        }
                        receiving_account{
                          id
                          accountNumber
                          entity{
                            person{
                              name
                            }
                            routing_number
                          }
                        }
                      }
                      entity{
                        id
                        person{
                          name
                        }
                      }
                      account_number
                      amount
                      number
                      state
                      currency{
                        id
                        iso_4217
                        symbol
                        digits
                      }
                      payment_date
                    }
                  }
                }",
                Variables = new { conciliar }
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
                    return new ListPayments() { Error = __notAuthorized };
                }
                return graphQLResponse.GetDataFieldAs<ListPayments>("payments");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<PaymentsNotConciliated> ConsultaMovementsAsync(ParamConciliarMovements param, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query($movement : MovementsInput!){
                  paymentsNotConciliated(input:$movement){
                    list{
                      id
                      movement_dated
                      entity{
                        id
                        routing_number
                        person{
                          name
                        }
                      }
                      account_number
                      amount
                      number
                      
                      currency{
                        id
                        iso_4217
                        symbol
                        digits
                      }
                    }
                  }
                }",
                Variables = new { movement =param }
            };

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                var graphQLResponse = await _client.PostAsync(query);
                var jsonD = JsonConvert.SerializeObject(graphQLResponse);
                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new PaymentsNotConciliated() { Error = __notAuthorized  };
                }
                return graphQLResponse.GetDataFieldAs<PaymentsNotConciliated>("paymentsNotConciliated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<string> GuardarArchivoAsync(ParamConciliarArchivo data, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($data: SaveFilePaymentsInput!){
                  saveFilePayments(input: $data){
                    msg
                  }
                }",
                Variables = new { data }
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
                var mensaje = graphQLResponse.GetDataFieldAs<Response>("saveFilePayments");
                return mensaje.Msg;
            }
            catch (Exception e)
            {
                return graphQLResponse.Errors[0].Message;
            }
        }
        public async Task<TuFactoringModels.nuevaVersion.Payments> ConciliarUsuarioAsync(string conciliar, string movement, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($conciliar: ID! $movement: ID){
                  confirmPayment(input: { id: $conciliar, movement: $movement }){
                    id
                    state
                  }
                }",
                Variables = new { conciliar, movement }
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
                    return new TuFactoringModels.nuevaVersion.Payments() { Error = __notAuthorized };
                }
                return graphQLResponse.GetDataFieldAs<TuFactoringModels.nuevaVersion.Payments>("confirmPayment");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TuFactoringModels.nuevaVersion.Payments() { Error = graphQLResponse.Errors[0].Message };
            }
        }
        public async Task<TuFactoringModels.nuevaVersion.Payments> BloquearUsuarioAsync(string conciliar, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($conciliar: ID!){
                  unconfirmPayment(input: { id: $conciliar }){
                    id
                  }
                }",
                Variables = new { conciliar }
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
                    return new TuFactoringModels.nuevaVersion.Payments() { Error = __notAuthorized };
                }
                return graphQLResponse.GetDataFieldAs<TuFactoringModels.nuevaVersion.Payments>("unconfirmPayment");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TuFactoringModels.nuevaVersion.Payments() { Error = graphQLResponse.Errors[0].Message };
            }
        }
        public async Task<TuFactoringModels.nuevaVersion.Payments> MovementAsync(string movement, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation($movement: ID!){
                  unconfirmMovement(input: { id: $movement }){
                    id
                  }
                }",
                Variables = new { movement }
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
                    return new TuFactoringModels.nuevaVersion.Payments() { Error = __notAuthorized };
                }
                return graphQLResponse.GetDataFieldAs<TuFactoringModels.nuevaVersion.Payments>("unconfirmMovement");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TuFactoringModels.nuevaVersion.Payments() { Error = graphQLResponse.Errors[0].Message };
            }
        }
        #endregion

        #region Consultas de Pagos 
        //Receipts Backoffice
        public async Task<List<Receipts>> ReceiptsQueryBackoffice(string state = null, string country = null, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            GraphQLRequest QueryReceiptBackoffice;

            QueryReceiptBackoffice = new GraphQLRequest
            {
                Query = @"query($state: String, $country:Int!){
                    receipts(input:{state:$state, country:$country}){
                       list{
                          id
                          receipt_date
                          state
                          payments {
                            currency{
                                name
                                iso_4217
                                symbol
                                digits
                            }
                            account_number
                            entity{
                                routing_number
                                person{
                                    name
                                }
                            }
                            payment_date
                            amount
                            number
                            state
                          }
                          program { abbreviation name currency }
                          publications{
                            earnings
                            payable
                            receivable discount commission
                            invoice{
                                request_financing
                                number
                                expiration_date
                                term_days
                                original_amount
                                amount
                                debtor{
                                    name
                                }
                                supplier{
                                  name
                                }
                            }
                          }
                          currency{
                            id
                            iso_4217
                            digits
                            symbol
                          }
                          entity{ 
                            routing_number
                            person{
                              name
                            }
                          }
                          receiver{
                            name
                          }
                          receiving_account{
                            accountNumber
                          }
                          payer{
                            name
                          }
                          paying_account{
                            accountNumber
                          }
                          paid
                          amount
                        } 
                      }
                    }",
                Variables = new 
                { 
                  state,
                  country,
                  filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,
                        supplier = filter.Supplier_id,
                        debtor = filter.Debtor_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        financied = filter.Financied,
                        invoiceStatus = filter.InvoiceStatus
                    },
                    pagination
                }
            };

            try
            {
                var response = await _client.PostAsync(QueryReceiptBackoffice);

                if (response.Errors != null)
                {
                    if(response.Errors[0].Message == __notAuthorized)
                    {
                        return new List<Receipts> { new Receipts() { Errors = response.Errors[0].Message } };
                    }

                    return new List<Receipts>() { new Receipts() { Errors = response.Errors[0].Message } };
                }

                var data = response.GetDataFieldAs<ReceiptsResponse>("receipts").List;

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Receipts>();
            }
        }

        //Receipts Confirmant
        public async Task<List<Receipts>> ReceiptsQueryConfirmant(ParamConsultaPeyments param, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            GraphQLRequest QueryReceiptConfirmant;

            QueryReceiptConfirmant = new GraphQLRequest
            {
                Query = @"query($payer: String, $entity: String, $abbreviation: String, $state: String, $country:Int!, $method: String, $stateInvoice: String, $stateInvoiceNot: String, $consult: Boolean, $filter:Filter, $pagination : PaginationInput){
                    receipts(input:{payer: $payer, entity: $entity, abbreviation:$abbreviation, state:$state, country:$country, method: $method, stateInvoice: $stateInvoice, stateInvoiceNot: $stateInvoiceNot, consult: $consult, filter: $filter, pagination : $pagination}){
                       list{
                          id
                          receipt_date
                          state
                          payments {
                            currency{
                                name
                                iso_4217
                                symbol
                                digits
                            }
                            account_number
                            entity{
                                routing_number
                                person{
                                    name
                                }
                            }
                            payment_date
                            amount
                            number
                            state
                          }
                          program { abbreviation name currency }
                          publications{
                            earnings
                            payable
                            receivable discount commission
                            invoice{
                                request_financing
                                number
                                expiration_date
                                term_days
                                original_amount
                                amount
                                debtor{
                                    name
                                }
                                supplier{
                                  name
                                }
                          }
                          }
                          currency{
                            id
                            iso_4217
                            digits
                            symbol
                          }
                          entity{ 
                            routing_number
                            person{
                              name
                            }
                          }
                          receiver{
                            name
                          }
                          receiving_account{
                            accountNumber
                          }
                          payer{
                            name
                          }
                          paying_account{
                            accountNumber
                          }
                          paid
                          amount
                        } 
                      }
                    }",
                Variables = new
                {
                    param.Payer,
                    param.Entity,
                    param.Abbreviation,
                    param.State,
                    param.Country,
                    param.Method,
                    param.StateInvoice,
                    param.StateInvoiceNot,
                    param.Consult,

                    filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,
                        supplier = filter.Supplier_id,
                        debtor = filter.Debtor_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        financied = filter.Financied,
                        invoiceStatus = filter.InvoiceStatus,
                        program = filter.Program
                    },
                    pagination
                }
            };

            var jsonD = JsonConvert.SerializeObject(param);
            try
            {
                var response = await _client.PostAsync(QueryReceiptConfirmant);

                if (response.Errors != null)
                {
                    if (response.Errors[0].Message == __notAuthorized)
                    {
                        return new List<Receipts> { new Receipts() { Errors = response.Errors[0].Message } };
                    }

                    return new List<Receipts>() { new Receipts() { Errors = response.Errors[0].Message } };
                }

                var data = response.GetDataFieldAs<ReceiptsResponse>("receipts").List;

                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Receipts>();
            }
        }
        #endregion
    }
}
