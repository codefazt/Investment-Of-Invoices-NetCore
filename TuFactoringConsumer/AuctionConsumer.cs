using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using TuFactoringModels;
using TuFactoringModels.Validation;

namespace TuFactoringGraphql
{
    public class AuctionConsumer
    {
        private readonly string __notAuthorized = "You are not authorised to perform this action";
        private readonly GraphQLClient _client;

        public AuctionConsumer(GraphQLClient client)
        {
            _client = client;
        }

        #region Querys
        private string consultaCargarAuction()
        {
            return @"
                query($country:Int!){
                  auctions(country:$country, current: true){
                    list{
                      id
                      country
                      dated
                      created
                      opened
                      closed
                      payed
                      finalized
                      state
                    }
                  }
                }
            ";
        }

        private string consultaCargarPublicadas()
        {
            return @"query($country:Int!, $filter:Filter, $pagination: PaginationInput){
                      publications(input:{
                        country:$country
                        filter:$filter
                        pagination: $pagination
                      }){
                        list{
                          id
                          isOffered
                          country{
      	                    id  
                          }
                          currency{
                            iso_4217
                            symbol
                            digits
                          }
                          entity
                          {
                            id
                            person{
                              name
                            }
                            routing_number
                          }
                          invoice{
                            id
                            amount original_amount
                            country
                            supplier{
                              id
                              name
                              firstName
                              lastName
                            }
                            number
                            debtor{
                              id
                              name
                              firstName
                              lastName
                            }
                            expiration_date
                            term_days
                            currency{
                              symbol
                              iso_4217
                              digits
                            }        
                          }
                          discount
                          earnings
                          profitability
                          payable
                          rate
                          commission
                          receivable
                          bids{
                            id
                            factor{
                              id
                              name
                              lastName
                            }
                            discount
                          }
                          state
                        }
                      }
                    }";
        }

        private string consultaCargarPublicadasSupplier()
        {
            return @"query($country:Int!, $supplier: String, $filter:Filter, $pagination: PaginationInput){
                              publications(input:{
                                country:$country
	                            supplier:$supplier
                                filter: $filter
                                pagination: $pagination
                              }){
                                list{
                                  id
                                  country{
      	                            id  
                                  }
                                  currency{
                                    iso_4217
                                    symbol
                                    digits
                                  }
                                  entity
                                  {
                                    id
                                    person
                                    {
                                    name
                                    }
                                    routing_number
                                  }
                                  invoice{
                                    id
                                    country
                                    supplier{
                                      id
                                      name
                                      firstName
                                      lastName
                                    }
                                    debtor{
                                      id
                                      name
                                      firstName
                                      lastName
                                    }
                                    number
                                    expiration_date
                                    term_days
                                    amount original_amount
                                    currency{
                                      symbol
                                      iso_4217
                                      digits
                                    }        
                                  }
                                  discount
                                  earnings
                                  profitability
                                  payable
                                  rate
                                  commission
                                  receivable
                                  bids{
                                    id
                                    factor{
                                      id
                                    }
                                    discount
                                  }
                                  state
                                }
                              }
                            }";
        }

        public string consultaCargarPublishable()
        {
            return @"
                    query($supplier:String!,$state: String,$pagination: PaginationInput, $filter: Filter ){
                        releasable(input:{supplier:$supplier, state: $state, pagination: $pagination, filter: $filter}){
                          list{
                            id 
                          entity {id person{name} routing_number}
                          currency { id symbol iso_4217 digits}
                          discount
                          program { abbreviation }
                          commission
                          receivable
                          earnings
                          profitability
                          bids{
                              id
                          }
                          invoice{
                              id
                              number
                              amount original_amount
                              expiration_date
                              term_days
                              debtor { id name }
                          }
                            state
                          }
                    }
            }
            ";
        }

        private string consultaCargarWinner()
        {
            return @"query($country:Int!,$supplier:String!,$filter:Filter, $pagination:PaginationInput){
                      deals(input:{
                        country:$country
                        supplier:$supplier
                        filter:$filter
                        pagination:$pagination
                      }){
                        list{
                          id
                          country{
                            id
                          }
                          currency 
                          {
                            id
                            name
                            iso_4217
                            symbol
                            digits
                          }
                          invoice{
                            id
                            number
                            currency{
                              id
                              name
                              iso_4217
                              symbol
                              digits
                            }
                            amount
                            original_amount
                            supplier{id name}
                            debtor{ id name }
                            expiration_date
                            term_days
                          }
                          entity{
                            id
                            routing_number
                            person{
                              name
                            }        
                          }
                          discount
                          earnings
                          profitability
                          payable
                          rate
                          commission
                          receivable
                          bids{
                            id
                            factor{
                              id
                              name
                              lastName
        	                  discriminator
                            }
                          }
                        }
                      }
                    }";
        }

        #endregion

        #region Mutation
        private string mutationOffertPublish()
        {
            return @"mutation($country:Int!,$publication:String!,$factor:String,$offer:Float!){
                      offerPublication(input:{
                        country:$country
                        publication:$publication
                        factor:$factor
                        offer:$offer
                      }){
                        id
                        isOffered
                        currency{
                          symbol
                          iso_4217
                        }
                        invoice{
                          id
                          country
                          number
                          debtor{
                            name
                          }
                          expiration_date
                          term_days
                          amount original_amount
                        }
                        discount
                        earnings
                        profitability
                        payable
                        rate
                        commission
                        receivable
                        bids{
                          id
                          discount
                          factor{
                            id
                            name
                            lastName
                            discriminator
                          }
                        }
                      }
                    }";
        }

        private string mutationPostpone()
        {
            return @"mutation ($publication:String!, $country: Int!){
                        postponeInvoice(input:{publication:$publication, country: $country}){
                               id
                        }
                    }";
        }

        private string mutationPublish()
        {
            return @"mutation ($publication: String!, $country: Int!){
                      publishInvoice (input: {publication:$publication, country:$country}) {
                        id
                     }
                    }";
        }

        private string mutationRechazar()
        {
            return @" mutation($publication:String!, $country: Int!){
                        returnInvoice(input:{publication:$publication, country:$country}){ id }
                    }";
        }

        private string mutationSell()
        {
            return @"mutation ($publication: String!, $country: Int!, $bid: String){
                      sellInvoice (input: {publication: $publication, country:$country, bid: $bid}) {
                        id
                     }
                    }";
        }

        private string mutationAceptOffert()
        {
            return @"
                    mutation($bid:String!){
                      acceptOffer(bid:$bid){
                        id
                      }
                    }
                    ";
        }

        public string mutationCreateAuction()
        {
            return @"mutation ($country:Int!){
                      createAuction(country:$country){
                        id
                        opened
                        closed
                        dated
                        finalized payed state
                      }
                    }";
        }

        public string mutationOpenAuction()
        {
            return @"mutation ($country:String!){
                      openAuction(id:$country){
                        opened
                        state
                      }
                    }";
        }

        public string mutationCloseAuction()
        {
            return @"mutation ($country:String!){
                      closeAuction(id:$country){
                        closed
                        state
                      }
                    }";
        }

        public string mutationEndingAuction()
        {
            return @"mutation($country:String!){
                      finalizeAuction(id:$country){
                        state
                        finalized
                      }
                    }";
        }

        public string mutationConciliationAuction()
        {
            return @"mutation($country:Int!){
                      conciliationAuction(country:$country){
                        status
                        conciliation
                      }
                    }";
        }

        public string mutationPaymentsAuction()
        {
            return @"mutation($country: String!){
                      payAuction(id:$country){
                        state
                        payed
                      }
                    }";
        }

        #endregion

        #region Function Consultas
        public async Task<List<Auctions>> GetAuction(int idPais = 0, string token = "")
        {
            // graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            
            GraphQLRequest DataRegistroRequest;
            List<Auctions> resultInvoice = new List<Auctions>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaCargarAuction(),
                Variables = new { country = idPais }
            };


            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);
                
                if (graphQLResponse.Errors != null)
                {
                    if (graphQLResponse.Errors[0].Message == "Auction not found")
                    {
                        resultInvoice.Add(new Auctions() { Error = "Not Found" });
                        return resultInvoice;
                    }

                    resultInvoice.Add(new Auctions() { Error = graphQLResponse.Errors[0].Message });
                    return resultInvoice;
                }

                resultInvoice = graphQLResponse.GetDataFieldAs<AuctionsResponse>("auctions").List;
            }
            catch (Exception r)
            {

                resultInvoice.Add(new Auctions() { Error = r.Message });
                return resultInvoice;
            }
            return resultInvoice;

        }

        public async Task<List<Publications>> GetPublications(int idPais = 0, string rol = null, string idSupplier = null, filterInvoice filter = null, string token = "", Pagination pagination = null)
        {
            // graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            List<Publications> resultInvoice = new List<Publications>();
            GraphQLRequest DataRegistroRequest;

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (rol == null)
            {
                return resultInvoice;
            }

            if (rol == "FACTOR")
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaCargarPublicadas(),
                    Variables = new { country = idPais ,
                        pagination = pagination,
                        filter = filter == null ? null : new
                        {
                            program = "CONFIRMING",
                            confirmant = filter.Confirmant_id,
                            debtor = filter.Debtor_id,
                            number = filter.Number,
                            currency = filter.Currency_id,
                            expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            amount_from = filter.AmountFrom,
                            amount_to = filter.AmountTo,
                            isOffered = filter.IsOffered,
                            bidsStatus = "draft"
                        }
                    }
                };
            }
            else
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaCargarPublicadasSupplier(),
                    Variables = new { country = idPais, supplier = idSupplier,
                        pagination = pagination,
                        filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,
                        debtor = filter.Debtor_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo
                    }
                    }
                };
            }


            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null)
                {

                    if (graphQLResponse.Errors[0].Message == "Auction not found")
                    {
                        resultInvoice.Add(new Publications() { Errors = "Auction not created" });
                        return resultInvoice;
                    }
                    else if (graphQLResponse.Errors[0].Message == "Auction not open")
                    {
                        resultInvoice.Add(new Publications() { Errors = "Auction not opened" });
                        return resultInvoice;
                    }

                    resultInvoice.Add(new Publications() { Errors = graphQLResponse.Errors[0].Message });
                    return resultInvoice;
                }


                resultInvoice = graphQLResponse.GetDataFieldAs<PublicationsResponse>("publications").List;
            }
            catch (Exception)
            {
                return resultInvoice;
            }
            return resultInvoice;

        }

        public async Task<List<Publications>> GetPublishable(string owner = null,string state = null, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {
            List<Publications> result = new List<Publications>();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var consulta = new GraphQLRequest()
            {
                Query = consultaCargarPublishable(),
                Variables = new {supplier = owner, state = state, pagination, filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,
                        debtor = filter.Debtor_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        program = filter.Program
                    }
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(consulta);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Publications>() { new Publications() { Errors = __notAuthorized } };
                }

                result = graphQLResponse.GetDataFieldAs<PublicationsResponse>("releasable").List;
            }
            catch (Exception)
            {
                return result;
            }


            return result;
        }

        public async Task<List<Publications>> GetWinner(int idPais = 0, string owner = null, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {
            // graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            // graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);
            List<Publications> resultInvoice = new List<Publications>();
            GraphQLRequest DataRegistroRequest;

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaCargarWinner(),
                Variables = new { 
                    country = idPais, 
                    supplier = owner,
                    filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,
                        debtor = filter.Debtor_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo
                    },
                    pagination
                }
            };


            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Publications>() { new Publications() { Errors = __notAuthorized } };
                }

                resultInvoice = graphQLResponse.GetDataFieldAs<PublicationsResponse>("deals").List;
            }
            catch (Exception)
            {
                return resultInvoice;
            }
            return resultInvoice;
        }


        #endregion

        #region Function Mutaciones
        public async Task<Publications> OfferPublication(Offert oferta, string token)
        {
            Publications resultado = new Publications();
            // graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (!new OffertValidation().Validate(oferta).IsValid)
            {
                resultado.Error = new Exception("Invalid publication");
                return resultado;
            }

            var DataPost = new GraphQLRequest
            {
                Query = mutationOffertPublish(),
                Variables = new
                {
                    country = oferta.Country,
                    publication = oferta.Publication_id,
                    factor = oferta.Factor_id,
                    offer = oferta.Bid_amount
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataPost);

                if (graphQLResponse.Errors != null )
                {
                    if (graphQLResponse.Errors[0].Message == this.__notAuthorized)
                    {
                        return new Publications() { Errors = graphQLResponse.Errors[0].Message };

                    } 
                    else if(graphQLResponse.Errors[0].Message == "factor don't have accounts")
                    {
                        return new Publications() { Errors = "factor don't have accounts" };
                    } 
                    else if(graphQLResponse.Errors[0].Message == "Factor is banned")
                    {
                        return new Publications() { Errors = "Factor is banned" };
                    }


                    return new Publications() { Error = new Exception(graphQLResponse.Errors[0].Message)} ;
                }

                
                resultado = graphQLResponse.GetDataFieldAs<Publications>("offerPublication");

                return resultado;
            }
            catch (Exception e)
            {
                resultado.Error = e;
                return resultado;
            }

        }

        public async Task<Publications> PostponeInvoice(UpdateInvoice invoice, string token)
        {
            Publications resultOffert = new Publications();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (!new updateInvoicesValidation().Validate(invoice).IsValid)
            {
                resultOffert.Error = new Exception("Invalid publication");
                return resultOffert;
            }

            var DataPost = new GraphQLRequest
            {
                Query = mutationPostpone(),
                Variables = new { publication = invoice.Invoice_id, country = invoice.country }
            };
            try
            {
                var graphQLResponse = await this._client.PostAsync(DataPost);

                if (graphQLResponse.Errors != null)
                {
                    if (graphQLResponse.Errors[0].Message == this.__notAuthorized)
                    {
                        return new Publications() { Errors = graphQLResponse.Errors[0].Message };

                    }
                    return new Publications() { Error = new Exception(graphQLResponse.Errors[0].Message) };
                }

                resultOffert = graphQLResponse.GetDataFieldAs<Publications>("postponeInvoice");
            }
            catch (Exception e)
            {
                resultOffert.Error = e;
                return resultOffert;
            }

            return resultOffert;
        }

        public async Task<Publications> PublicationsInvoice(UpdateInvoice factura, string token)
        {
            Publications resultOffert = new Publications();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (!new updateInvoicesValidation().Validate(factura).IsValid)
            {
                resultOffert.Error = new Exception("Invalid publication");
                return resultOffert;
            }

            var DataOfertada = new GraphQLRequest
            {
                Query = mutationPublish(),
                Variables = new { publication = factura.Invoice_id, country = factura.country}
            };
            try
            {
                var graphQLResponse = await this._client.PostAsync(DataOfertada);

                if (graphQLResponse.Errors != null)
                {
                    if (graphQLResponse.Errors[0].Message == this.__notAuthorized)
                    {
                        return new Publications() { Errors = graphQLResponse.Errors[0].Message };

                    } else if(graphQLResponse.Errors[0].Message == "entity don't have accounts")
                    {
                        return new Publications() { Errors = graphQLResponse.Errors[0].Message };
                    }
                    else if (graphQLResponse.Errors[0].Message == "backOffice don't have accounts with entity")
                    {
                        return new Publications() { Errors = graphQLResponse.Errors[0].Message };
                    }

                    return new Publications() { Error = new Exception(graphQLResponse.Errors[0].Message) };
                }

                resultOffert = graphQLResponse.GetDataFieldAs<Publications>("publishInvoice");
            }
            catch (Exception e)
            {
                resultOffert.Error = e;
                return resultOffert;
            }

            return resultOffert;
        }

        public async Task<Publications> RejectOffer(UpdateInvoice oferta, string token)
        {
            Publications resultado = new Publications();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var consulta = new GraphQLRequest()
            {
                Query = mutationRechazar(),
                Variables = new { publication = oferta.Invoice_id, country = oferta.country }
            };

            try
            {
                var respuesta = await this._client.PostAsync(consulta);

                if (respuesta.Errors != null)
                {
                    if (respuesta.Errors[0].Message == this.__notAuthorized)
                    {
                        return new Publications() { Errors = respuesta.Errors[0].Message };

                    }
                    return new Publications() { Error = new Exception(respuesta.Errors[0].Message) };
                }

                resultado = respuesta.GetDataFieldAs<Publications>("returnInvoice");

            }
            catch (Exception e)
            {
                resultado.Error = e;
                return resultado;
            }

            return resultado;
        }

        public async Task<Publications> SellInvoice(UpdateInvoice oferta, string token)
        {
            Publications resultOffert = new Publications();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            // graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);

            if (!new updateInvoicesValidation().Validate(oferta).IsValid)
            {
                resultOffert.Error = new Exception("Invalid publication");
                return resultOffert;
            }

            var DataSell = new GraphQLRequest
            {
                Query = mutationSell(),
                Variables = new { publication = oferta.Invoice_id, country = oferta.country, bid = oferta.confirmant}
            };
            try
            {
                var respuesta = await this._client.PostAsync(DataSell);

                if (respuesta.Errors != null)
                {
                    if (respuesta.Errors[0].Message == this.__notAuthorized)
                    {
                        return new Publications() { Errors = respuesta.Errors[0].Message };

                    }
                    return new Publications() { Error = new Exception(respuesta.Errors[0].Message) };
                }

                resultOffert = respuesta.GetDataFieldAs<Publications>("sellInvoice");
            }
            catch (Exception e)
            {
                return new Publications() { Error = e };
            }

            return resultOffert;
        }

        public async Task<Publications> AceptOffert(UpdateInvoice invoice, string token)
        {
            Publications publication = new Publications();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (!new updateInvoicesValidation().Validate(invoice).IsValid)
            {
                publication.Error = new Exception("Invalid publication");
                return publication;
            }

            try
            {

                var data = new GraphQLRequest
                {
                    Query = mutationSell(),
                    Variables = new { 
                        publication = invoice.Publication_id,
                        country = invoice.country,
                        bid = invoice.Bid_id
                    }
                };

                var res = await this._client.PostAsync(data);

                if (res.Errors != null)
                {
                    if (res.Errors[0].Message == this.__notAuthorized)
                    {
                        return new Publications() { Errors = res.Errors[0].Message };

                    }
                    return new Publications() { Error = new Exception(res.Errors[0].Message) };
                }

                publication = res.GetDataFieldAs<Publications>("sellInvoice");
            }
            catch (Exception e)
            {
                publication.Error = e;
                return publication;
            }


            return publication;
        }

        public async Task<Publications> RejectOfferMarket(UpdateInvoice oferta, string token)
        {
            Publications resultado = new Publications();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            var consulta = new GraphQLRequest()
            {
                Query = mutationRechazar(),
                Variables = new { publication = oferta.Publication_id, country = oferta.country }
            };

            try
            {
                var respuesta = await this._client.PostAsync(consulta);

                if (respuesta.Errors != null)
                {
                    if (respuesta.Errors[0].Message == this.__notAuthorized)
                    {
                        return new Publications() { Errors = respuesta.Errors[0].Message };

                    }
                    return new Publications() { Error = new Exception(respuesta.Errors[0].Message) };
                }

                resultado = respuesta.GetDataFieldAs<Publications>("returnInvoice");

            }
            catch (Exception e)
            {
                resultado.Error = e;
                return resultado;
            }

            return resultado;
        }

        public async Task<Auctions> CreateAuction(string country, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (country == null) return new Auctions() { Error = "Country Null" };

            Auctions act;

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = mutationCreateAuction(),
                    Variables = new
                    {
                        country = country
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    return new Auctions() { Error = respuesta.Errors[0].Message }; ;
                }

                act = respuesta.GetDataFieldAs<Auctions>("createAuction");
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }


            return act;
        }

        public async Task<Auctions> OpenAuction(string country, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (country == null) return new Auctions() { Error = "Country Null" };

            Auctions act;
            
            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = mutationOpenAuction(),
                    Variables = new
                    {
                        country = country
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    return new Auctions() { Error = respuesta.Errors[0].Message }; ;
                }

                act = respuesta.GetDataFieldAs<Auctions>("openAuction");
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }


            return act;
        }

        public async Task<Auctions> CloseAuction(string country, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (country == null) return new Auctions() { Error = "Country Null" };

            Auctions act;
            

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = mutationCloseAuction(),
                    Variables = new
                    {
                        country = country
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    return new Auctions() { Error = respuesta.Errors[0].Message }; ;
                }

                act = respuesta.GetDataFieldAs<Auctions>("closeAuction");
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }


            return act;
        }
    
        public async Task<Auctions> EndingAuction(string country, string token)
        {
            if (country == null) return new Auctions() { Error = "Country Null" };

            Auctions act;

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = mutationEndingAuction(),
                    Variables = new
                    {
                        country = country
                    }
                };

                var respuesta = await this._client.PostAsync(data);


                if (respuesta.Errors != null)
                {
                    return new Auctions() { Error = respuesta.Errors[0].Message }; ;
                }

                act = respuesta.GetDataFieldAs<Auctions>("finalizeAuction");
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }
            
            return act;
        }

        public async Task<Auctions> ConciliationAuction(string country, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (country == null) return new Auctions() { Error = "Country Null" };

            Auctions act;
            
            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = mutationConciliationAuction(),
                    Variables = new
                    {
                        country = country
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return new Auctions() { Error = respuesta.Errors[0].Message }; ;
                }

                act = respuesta.GetDataFieldAs<Auctions>("conciliationAuction");
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }


            return act;
        }

        public async Task<Auctions> PaymentsAuction(string country, string token)
        {
            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            if (country == null) return new Auctions() { Error = "Country Null" };

            Auctions act;
            

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = mutationPaymentsAuction(),
                    Variables = new
                    {
                        country = country
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    return new Auctions() { Error = respuesta.Errors[0].Message }; ;
                }

                act = respuesta.GetDataFieldAs<Auctions>("payAuction");
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }


            return act;
        }

        #endregion

        #region Function Mutaciones Opciones "All"
        public async Task<List<Publications>> SellAllInvoices(List<UpdateInvoice> ofertas, string token)
        {
            List<Publications> resultOfferts = new List<Publications>();
            // graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            // graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            foreach (UpdateInvoice oferta in ofertas)
            {
                if (!new updateInvoicesValidation().Validate(oferta).IsValid)
                {
                    var result = new Publications()
                    {
                        Id = oferta.Invoice_id,
                        Error = new Exception("Invalid publication")
                    };
                    oferta.Invoice_id = null;
                    resultOfferts.Add(result);
                    continue;
                }

                var DataSell = new GraphQLRequest
                {
                    Query = mutationSell(),
                    Variables = new { publication = oferta.Publication_id, country = oferta.country, bid = oferta.Bid_id}
                };
                try
                {
                    var respuesta = await this._client.PostAsync(DataSell);

                    if (respuesta.Errors != null)
                    {
                        resultOfferts.Add(new Publications() { Errors = respuesta.Errors[0].Message, Error = new Exception(respuesta.Errors[0].Message), Id = oferta.Invoice_id });
                        continue;
                    }

                    var pub = respuesta.GetDataFieldAs<Publications>("sellInvoice");

                    resultOfferts.Add(pub);
                }
                catch (Exception e)
                {
                    resultOfferts.Add(new Publications() { Error = e, Id = oferta.Invoice_id });
                }
            }
            
            return resultOfferts;
        }

        public async Task<List<Publications>> RejectAllInvoices(List<UpdateInvoice> ofertas, string token)
        {
            List<Publications> resultOfferts = new List<Publications>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            foreach (UpdateInvoice oferta in ofertas)
            {
                
                var consulta = new GraphQLRequest()
                {
                    Query = mutationRechazar(),
                    Variables = new { publication = oferta.Invoice_id, country = oferta.country }
                };

                try
                {
                    var respuesta = await this._client.PostAsync(consulta);

                    if (respuesta.Data == null)
                    {
                        resultOfferts.Add(new Publications()
                        {
                            Errors = respuesta.Errors[0].Message,
                            Error = new Exception(respuesta.Errors[0].Message),
                            Id = oferta.Invoice_id
                        });
                        continue;
                    }

                    var data = respuesta.GetDataFieldAs<Publications>("returnInvoice");
                    resultOfferts.Add(data);
                }
                catch (Exception e)
                {
                    resultOfferts.Add(new Publications()
                    {
                        Error = e,
                        Id = oferta.Invoice_id
                    });
                    continue;
                }
            }

            return resultOfferts;
        }

        public async Task<List<Publications>> PublicationsAllInvoices(List<UpdateInvoice> ofertas, string token)
        {
            List<Publications> resultOfferts = new List<Publications>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            foreach (UpdateInvoice factura in ofertas)
            {
                if (!new updateInvoicesValidation().Validate(factura).IsValid)
                {
                    var p = new Publications() { Error = new Exception("Invalid publication"), Id = factura.Invoice_id };
                    resultOfferts.Add(p);
                    continue;
                }

                var DataOfertada = new GraphQLRequest
                {
                    Query = mutationPublish(),
                    Variables = new { publication = factura.Invoice_id, country = factura.country }
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataOfertada);

                    if (graphQLResponse.Errors != null)
                    {
                        resultOfferts.Add(new Publications()
                        {
                            Errors = graphQLResponse.Errors[0].Message,
                            Error = new Exception("Invalid publication"),
                            Id = factura.Invoice_id
                        });
                        continue;
                    }

                    var data = graphQLResponse.GetDataFieldAs<Publications>("publishInvoice");
                    resultOfferts.Add(data);
                }
                catch (Exception)
                {
                    var p = new Publications() { Error = new Exception("Invalid publication"), Id = factura.Invoice_id };
                    resultOfferts.Add(p);
                }
                
            }

            return resultOfferts;
        }

        public async Task<List<Publications>> PosponeAllInvoices(List<UpdateInvoice> ofertas, string token)
        {
            List<Publications> resultOfferts = new List<Publications>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            foreach (UpdateInvoice invoice in ofertas)
            {
                if (!new updateInvoicesValidation().Validate(invoice).IsValid)
                {
                    var p = new Publications() { Error = new Exception("Invalid publication"), Id = invoice.Invoice_id };
                    resultOfferts.Add(p);
                    continue;
                }

                var DataPost = new GraphQLRequest
                {
                    Query = mutationPostpone(),
                    Variables = new { publication = invoice.Invoice_id , country= invoice.country}
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataPost);

                    if (graphQLResponse.Errors != null)
                    {
                        resultOfferts.Add(new Publications()
                        {
                            Errors = graphQLResponse.Errors[0].Message,
                            Error = new Exception("Invalid publication"),
                            Id = invoice.Invoice_id
                        });
                        continue;
                    }

                    var data = graphQLResponse.GetDataFieldAs<Publications>("postponeInvoice");
                    resultOfferts.Add(data);
                }
                catch (Exception)
                {
                    var p = new Publications() { Error = new Exception("Invalid publication"), Id = invoice.Invoice_id };
                    resultOfferts.Add(p);
                }


            }

            return resultOfferts;
        }

        public async Task<List<Publications>> AceptOffertAll(List<UpdateInvoice> invoices,string token)
        {
            List<Publications> publication = new List<Publications>();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            foreach (var invoice in invoices)
            {
                if (!new updateInvoicesValidation().Validate(invoice).IsValid)
                {
                    publication.Add(new Publications()
                    {
                        Error = new Exception("Invalid publication")
                    });
                    return publication;
                }

                try
                {

                    var data = new GraphQLRequest
                    {
                        Query = mutationSell(),
                        Variables = new { 
                            publication = invoice.Publication_id,
                            country = invoice.country,
                            bid = invoice.Bid_id
                        }
                    };

                    var res = await this._client.PostAsync(data);

                    if (res.Errors != null)
                    {
                        publication.Add(new Publications()
                        {
                            Errors = res.Errors[0].Message,
                            Error = new Exception(res.Errors[0].Message)
                        });
                        return publication;
                    }

                    var pub = res.GetDataFieldAs<Publications>("sellInvoice");

                    publication.Add(pub);
                }
                catch (Exception e)
                {
                    publication.Add(new Publications()
                    {
                        Error = new Exception(e.Message)
                    });
                    return publication;
                }
            }


            return publication;
        }

        #endregion
    }
}
