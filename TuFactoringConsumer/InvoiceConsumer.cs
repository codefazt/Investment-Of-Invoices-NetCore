using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;
using TuFactoringModels.Validation;

namespace TuFactoringGraphql
{
    public class InvoiceConsumer
    {
        private readonly string __notAuthorized = "You are not authorised to perform this action";
        private readonly GraphQLClient _client;

        public InvoiceConsumer(GraphQLClient client)
        {
            _client = client;
        }

        #region Querys 

        private string consultaPublicationsSessions()
        {
            return @"query($supplier:String, $factor:String, $country:Int!, $filter:Filter, $pagination:PaginationInput){
                      publicationsSessions(input:{
                        supplier:$supplier
                        factor:$factor
                        country:$country
                        filter:$filter
                        pagination:$pagination
                      }){
                        list{
                          id
                          changelogs{
                            from
                            to
                            note
                            changedAt
                            changedBy
                          }
                          currency{
                            id
                            symbol
                            iso_4217
                          }
                          bids{
                            state
                            factor{
                              name
                            }
                          }
                          entity{
                            routing_number
                            person{
                              name
                            }
                          }
                          invoice{ 
                            id
                            amount
                            number
                            debtor{
                              name
                            }
                            supplier{
                                name
                            }
                            expiration_date
                            term_days
                          }
                          state
                        }
                      }
                    }";
        }

        private string consultaCargarFactura()
        {
            return @"query{
                      invoices{
                        id
                        invoice_type_id
                        invoice_type {name}
                        number
                        issued_date
                        expiration_date
                        expiration_term
                        original_amount
                        amount
                        currency_id
                        currency { name symbol digits }
                        supplier_id
                        supplier
                        charges{
                            id
                            amount
                            number
                            charge_type_id
                        }
                        status
                      }
                    }";
        }
        private string queryInvoiceEditionDebtor()
        {
            return @"query($debtor: String, $country: Int, $pagination: PaginationInput, $filter: Filter, $order: String, $group: String){
                        invoices(input:{
                            debtor: $debtor,
                            country: $country,
                            pagination: $pagination,
                            filter: $filter,
                            order: $order,
                            group: $group
                            }){       
                            list{
                             id participant
                            number
                            issued_date
                            expiration_date
                            term_days
                            original_amount
                            amount
                            currency { id name iso_4217 symbol  digits}
                            supplier { id name }
                            charges{
                                id
                                amount
                                number
                                charge_type_id
                            }
                            publications { program { abbreviation } state}
                            state
                    }
                }
            }";
        }

        private string queryInvoiceEditionSupplier()
        {
            return @"query($supplier: String, $country: Int, $pagination: PaginationInput, $filter: Filter, $order: String, $group: String){
                        invoices(input:{
                            supplier: $supplier,
                            country: $country,
                            pagination: $pagination,
                            filter: $filter,
                            order: $order,
                            group: $group
                            }){       
                            list{
                             id
                            number
                            issued_date
                            expiration_date
                            term_days
                            original_amount
                            amount
                            currency { id name iso_4217 symbol  digits}
                            supplier { id name }
                            debtor { id name }
                            charges{
                                id
                                amount
                                number
                                charge_type_id
                            }
                            state
                    }
                }
            }";
        }

        private string consultaCargarFacturaDebtor()
        {
            return @"query ($debtor:String, $filter:Filter, $pagination : PaginationInput, $country: Int) {
                      invoices (debtor:$debtor, filter: $filter, pagination : $pagination, country: $country){
                        id
                        number
                        issued_date
                        expiration_date
                        term_days
                        original_amount
                        amount
                        currency { iso_4217 symbol  digits}
                        supplier { id name }
                        charges{
                            id
                            amount
                            number
                            charge_type_id
                        }
                        state
                      }
                    }";
        }
        
        private string consultaCargarFacturaSupplier()
        {
            return @"query ($supplier:String){
                      invoices (supplier:$supplier){
                        id
                        debtor { name }
                        number
                        expiration_date
                        expiration_term
                        amount
                        status
                      }
                    }";
        }

        private string consultaConfirmantes()
        {
            return @"
                    query ($debtor:String!){
                      listConfirmantToDebtor(debtor_id:$debtor){
                        list{
                          id
                          person{
                            name
                          }
                          is_fintech
                        }
                      }
                    }
                ";
        }

        private string consultaConfirmantesSupplier()
        {
            return @"
                    query ($supplier:String!){
                      listConfirmantToSupplier(supplier_id:$supplier){
                        list{
                          id
                          person{
                            name
                          }
                        }
                      }
                    }
                ";
        }

        private string consultaCargarPostuladas()
        {
            return @"query ($debtor: String!, $filter:Filter,$pagination: PaginationInput){
                      postulable(input:{
                        debtor:$debtor,
                        pagination: $pagination, filter:$filter
                      }){
                        list{
     	                    id
                          number
                          expiration_date
                          term_days
                          amount original_amount
                          request_financing
                          currency{ id symbol iso_4217 digits}
                          supplier { id name}
                          debtor { id name} 
                        }
                      }
                    }";
        }

        private string consultaCargarFinanciable()
        {
            return @"query($debtor:String!,$pagination: PaginationInput, $filter : Filter){
                      financiable(input:{debtor:$debtor, pagination: $pagination ,filter : $filter}){
                        list{
                          id
                          number
                          expiration_date
                          term_days
                          currency { id iso_4217 symbol digits}
                          amount
                          original_amount
                          supplier { name}
                          request_financing
                          publications { entity { routing_number person {name} } program { abbreviation } } 
                        }
                      }
                    }";
        }

        private string consultaFinanciableBank()
        {
            return @"query($country:Int!,$confirmant:String, $filter : Filter){
                      accountantsInvoices(input:{country:$country, confirmant:$confirmant, filter : $filter}){
                        list{
                          peopleID
                          count
                          name
                          sum
                        }
                      }
                    }";
        }

        private string consultaCargarCandidatas()
        {
            return @"query ($confirmant:String!, $filter:Filter, $pagination: PaginationInput){ 
                confirmable ( input:{ confirmant : $confirmant, filter: $filter , pagination: $pagination})
                    {
                       list{ 
                            id  
                            currency { id symbol iso_4217 digits}
                            invoice{
                                id
                                number
                                expiration_date
                                term_days
                                amount
                                request_financing
                                supplier { id name}
                                debtor { id name}
                            }
                        }
                      }
                    }";
        }

        private string consultaCargarCandidatasDebtor()
        {
            return @"query ($confirmant:String!, $filter:Filter, $pagination: PaginationInput){ 
                confirmable ( input:{ confirmant : $confirmant, filter: $filter , pagination: $pagination})
                    {
                       list{ 
                            id  
                            currency { id symbol iso_4217 digits}
                            entity {id routing_number}
                            invoice{
                                id
                                currency { id symbol iso_4217 digits }
                                number
                                expiration_date
                                term_days
                                original_amount
                                amount
                                issued_date
                                request_financing
                                supplier { id name }
                                charges{
                                    id
                                    amount
                                    number
                                    charge_type_id
                                }
                            }
                        }
                      }
                    }";
        }
        private string consultaCargarCandidatasReview()
        {
            return @"query ($confirmant:String!, $filter:Filter, $pagination: PaginationInput){ 
                reviewable ( input:{ confirmant : $confirmant, filter: $filter , pagination: $pagination})
                    {
                       list{ 
                            id  
                            currency { id symbol iso_4217 digits}
                            invoice{
                                id
                                number
                                expiration_date
                                term_days
                                amount
                                request_financing
                                supplier { id name}
                                debtor { id name}
                            }
                        }
                      }
                    }";
        }
        //Consulta de Disponibles para la compra del Banco
        private string queryConfirmed()
        {
            return @"query($confirmant: String!, $filter: Filter, $pagination: PaginationInput){
                    available(input: {
                    confirmant:$confirmant
                    filter: $filter
                    pagination: $pagination
                    }){
                        list{
                            id
                            currency { id symbol iso_4217 digits}
                            program { abbreviation }
                            invoice { 
                                number
                                expiration_date
                                term_days
                                request_financing
                                amount original_amount
                                supplier{ id name }
                                debtor { id name }
                        }
                    }
                }
            }";
        }
        private string consultaCargarConfirmadas()
        {
            return @"query ($confirmant:String!, $filter:CandidatesFilter){ 
                      confirmed(confirmant:$confirmant, filter: $filter){
                        id
                        number
                        expiration_date
                        expiration_term
                        amount
                        request_financing
                        currency { symbol iso_4217 digits}
                        supplier_id
                        supplier { name}
                        debtor_id
                        debtor { name}
                            }
                    }";
        }

        private string consultaPospuestas()
        {
            return @"query($supplier:String!, $pagination: PaginationInput, $filter : Filter){
                        postponed(input:{supplier:$supplier, filter : $filter, pagination: $pagination}){
                            list{
                              id
                              debtor { name}
                              number 
                              request_financing
                              expiration_date term_days currency{ id symbol iso_4217 digits} amount 
                              publications{id state entity{ id person{name} routing_number}}
                            }
                        }
                    }";
        }

        // Consulta para invoices para la Empresa
        private string consultaDebtor()
        {
            return @"
                query ($idDebtor: String, $filter:Filter, $pagination : PaginationInput, $order: String, $group: String){
                    invoices(input:{debtor:$idDebtor, filter: $filter, pagination : $pagination, order:$order, group:$group}){
                        list{
                          id participant
                          country
                          number
                          request_financing
                          supplier{
                              id
                              name
                              lastName
                          }
                          currency{
                            name
                            symbol
                            iso_4217
                          }
                            publications{ state program{ abbreviation } }
                          amount original_amount
                          expiration_date
                          term_days
                          state
                        }
                    }
                }
            ";
        }

        // Consulta para invoices para la Proveedor
        private string consultaSupplier()
        {
            return @"
                query ($idSupplier: String, $filter:Filter, $pagination : PaginationInput, $order: String, $group: String){
                      invoices(input:{supplier:$idSupplier, filter: $filter, pagination : $pagination, order:$order, group:$group}){
                          list{
                            id participant
                          country
                          number
                          publications{
                            state program{ abbreviation } 
                          }
                          debtor{
                            id
                            name
                            lastName
                          }
                          currency{
                            name
                            symbol
                            iso_4217
                          }
                          amount original_amount
                          expiration_date
                          term_days
                          state
                          }
                      }
                  }
            ";
        }

        // Consulta para invoices para la Inversonista
        private string consultaFactor()
        {
            return @"
               query($idFactor: String, $filter:Filter, $pagination : PaginationInput, $order: String, $group: String){
                    invoices(input:{factor:$idFactor, filter: $filter, pagination : $pagination, order:$order, group:$group}){
                       list{
                         id
                        country
                        number
                        supplier{
                            id
                            name
                            lastName
                        }
                        publications{
                            state program{ abbreviation } 
                            entity{
                                id
                                person{
                                  name
                                  lastName
                                }
                                routing_number
                            }
                        }
                        currency{
                          name
                          symbol
                          iso_4217
                        }
                        amount original_amount
                        expiration_date
                        term_days
                        state
                      }
                    }
                }
            ";
        }
        
        private string consultaConfirmant()
        {
            return @"
                 query($idConfirmant: String, $filter:Filter, $pagination : PaginationInput, $order: String, $group: String){
                    invoices(input:{confirmant:$idConfirmant, filter: $filter, pagination : $pagination, order:$order, group:$group}){
                        list{
                          id participant
                          number
                          publications{
                            state program{ abbreviation } 
                          }
                          supplier{
                              name
                          }
                          debtor{
                              id
                              name
                          }
                          request_financing
                          currency{
                            id
                            name
                            symbol
                            iso_4217
                          }
                          amount original_amount
                          expiration_date
                          term_days
                          state
                        }
                    }
                }
            ";
        }

        // Consulta para invoices para el BackOffice
        private string consultaBackkoffice()
        {
            return @"
                query($country: Int, $filter:Filter, $pagination : PaginationInput, $order: String, $group: String){
                    invoices(input:{country:$country, filter: $filter, pagination : $pagination, order:$order, group:$group}){
                        list{
                          id
                          number
                          supplier{
                              name
                          }
                          debtor{
                              name
                          }
                          request_financing
                          currency{
                            name
                            symbol
                            iso_4217
                          }
                          amount original_amount
                          expiration_date
                          term_days
                          state publications{ state program{ abbreviation } }
                        }
                    }
                }
            ";
        }

        private string detalleFacturasVistaConsulta()
        {
            return @"
                    query($id: ID){
                        invoices(input:{
                            id:$id
                        }){
                            list{
                                state
                                supplier{
                                    emails{
                                        address
                                    }
                                    name
                                }
                                publications{ createdAt
                                    published_at
                                    receivable
                                    commission
                                    payable
                                    profitability
                                    earnings
                                    discount program{ abbreviation }
                                    receipts{ paid processing commission method receipt_date
                                        payer{
                                            name
                                        } 
                                        paying_account{
                                            accountNumber
                                        }
                                        receiver{
                                            name
                                        }
                                        receiving_account{
                                            accountNumber
                                        }
                                        payments{
                                            id number payment_date
                                        }
                                    }
                                    bids{ state
                                        factor{
                                            id
                                            name
                                            emails{
                                                address
                                            }
                                        }
                                    }
                                    entity{ routing_number
                                        person{
                                            name
                                            emails{
                                                address
                                            }
                                        }
                                    }
                                    state
                                }
                                debtor{
                                    name
                                    emails{
                                        address
                                    }
                                }
                                expiration_date
                                term_days
                                state
                                request_financing
                                amount
                                number
                                issued_date
                                original_amount
                                currency{
                                    digits
                                    name
                                    symbol
                                    iso_4217
                                }
                                changelogs{
                                    to
                                    changedAt
                                }
                            }
                        }
                    }
                ";
        }
        #endregion

        #region Mutaciones
        private string mutacionCargaFactura()
        {
            return @"mutation ($nuevaFactura: InvoiceInput!) {
                      createInvoice( input: $nuevaFactura) {
                        id
                        number
                        issued_date
                        expiration_date
                        term_days
                        original_amount
                        amount
                        currency { id name symbol digits iso_4217}
                        supplier { id name }
                        charges{
                            id
                            amount
                            number
                            charge_type_id
                        }
                        state
                      }
                      
                    }";
        }

        //Nueva mutación de Creación de Deducciones
        private string mutationCreateCharges()
        {
            return @"mutation($deducciones: NewChargeInput!){
                        createCharge(input: $deducciones){
                            id
                }
            }";
        }
        
        private string mutationCharges()
        {
            return @"mutation ($deducciones: NewChargeInput!){
                      createCharge(input: $deducciones) {
                        id
                      }
                    }";
        }
        //Nueva mutación para la Eliminación de Facturas
        private string mutationDeleteInvoice()
        {
            return @"mutation($id: String!, $debtor: String!){
                    deleteInvoice(id: $id,debtor: $debtor){
                }
            }";
        }
        private string mutationEliminarFactura()
        {
            return @"mutation($id:ID!, $debtor: String!){
                      deleteInvoice(
                                id:$id,
                                debtor:$debtor
                        ){
                        id
                    }
                    }";
        }
        //Nueva mutación de Actualización de Facturas
        private string mutationUpdateInvoice() {

            return @"mutation($id: String!, $supplier: String!, $debtor: String!, 
                                $number: String!, $issued_date: Time!, $expiration_date: Time!, 
                                $currency: Int!, $original_amount: Float!){
                        updateInvoice(input:{
                                id: $id,
                                supplier: $supplier,
                                debtor: $debtor,
                                number: $number,
                                issued_date: $issued_date,
                                expiration_date: $expiration_date,
                                currency: $currency,
                                original_amount: $original_amount
                            }){
                            id
                            amount
                    }
                }
            }";

        }
        //Actualizar la Factura
        private string mutationUpdateFactura()
        {
            return @"mutation($id:String!, $debtor:String!, $original_amount:Float!, 
                        $expiration_date:Time!, $issued_date:Time!,
                        $supplier:String!, $currency:Int!, 
                        $number:String!
		                    ){
                      updateInvoice(
                        input:{
     	                      id: $id,
    	                      debtor: $debtor,
                              original_amount:$original_amount,
                              expiration_date: $expiration_date,
                              issued_date:$issued_date,
                              supplier: $supplier,
                              currency: $currency,
                              number: $number
                        }
                      ){
                        id
                        amount
                      }
                    }";
        }
        //Nueva Mutación de Eliminación de Deducciones
        private string mutationEliminarDeduccion()
        {
            return @"mutation($id:String!){
                  deleteCharge(id: $id){
                        id
                    }
                }";
        }
        //Nueva Mutación de Actualización de Deducciones
        private string mutationUpdateCharge()
        {
            return @"mutation($id:String!,$invoice_id:String!,$charge_type_id:Int!,$currency_id:Int!,
                        $number:String!,$amount:Float!){
                        updateCharge(input :{
                          id:$id,
                          invoice_id:$invoice_id,
                          charge_type_id:$charge_type_id,
                          currency_id:$currency_id,
                          number:$number,
                          amount:$amount
                        }){
                    id
                }
            }";
        }
        private string mutationUpdateDedducion()
        {
            return @"mutation($charge:UpdateCharge!){
                      updateCharge(input:$charge)
                    }";
        }
        //Nueva mutación Postulación de Facturas
        private string mutationPostulateInvoice()
        {
            return @"mutation($newPostulation: PostulateInput!){
                    postulateInvoice(input: $newPostulation) {
                            invoice{
                              id
                            }                       
                        }
                    }";
        }

        private string mutationPostulacion()
        {
            return @"mutation  ($invoice:String!,$confirmant:String!) {
                      postulateInvoice (invoice: $invoice,confirmant:$confirmant) {
                        id
                      }
                    }";
        }

        private string mutationFinanciar()
        {
            return @"mutation($factura: ID!) {
                    requestFinancing( id: $factura) {
                      id
                      request_financing
                    }
                  }";
        }

        private string mutationOffert()
        {
            return @"mutation($country: Int!, $confirmant: String, 
  			                    $publication: String!, $offer: Float!){
                      offerInvoice(input:{
                        country:$country
                        confirmant:$confirmant
                        publication:$publication
                        offer:$offer
                        }){
		                    id		   
                            }

                        }";
        }
        //Nueva mutación para la Confirmación de Facturas
        private string mutationConfirmInvoice()
        {
            return @" mutation($confirm: ConfirmInput!){
                            confirmInvoice(input: $confirm){
                                id
                        }   
                    }";
        }
        private string mutationReviewInvoice()
        {
            return @" mutation($confirm: ReviewInput!){
                            reviewInvoice(input: $confirm){
                                id
                        }   
                    }";
        }
        private string mutationRefuseInvoice()
        {
            return @" mutation($confirm: ReturnInput!){
                            returnInvoice(input: $confirm){
                                id
                        }   
                    }";
        }

        #endregion

        #region Funciones Consultas
        public async Task<List<Invoices>> GetInvoicesForDebtorEdition(ParametersDebtorEdition parameters, string tipo = null, filterInvoice filter = null, Pagination pagination = null, string token = null, string order = null, string group = null)
        {
            GraphQLRequest DataRegistroRequest;
            List<Invoices> data = new List<Invoices>();


            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (tipo == null)
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaCargarFactura()
                };
            }
            else
            {
                if (parameters.Id == null)
                {
                    return new List<Invoices>();
                }

                if (tipo == "Carga Manual" || tipo == "Postular")
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = queryInvoiceEditionDebtor(),
                        Variables = new
                        {
                            debtor = parameters.Id,
                            country = parameters.Country,
                            pagination = pagination,
                            order = order,
                            group = group,
                            filter = filter == null ? null : new
                            {
                                programsAllInvoiceStatus = "confirming",
                                invoiceStatusProgramNot = "draft,posted",
                                publicationStatusNot = "rejected",
                                supplier = filter.Supplier_id,
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
                }
                else if (tipo == "Facturas Vencimiento")
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = consultaCargarFacturaSupplier(),
                        Variables = new { supplier = parameters.Id }
                    };
                }
                else
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = consultaCargarFactura(),
                    };
                }

            }

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Invoices>() { new Invoices() { Errors = __notAuthorized } };
                }

                data = graphQLResponse.GetDataFieldAs<InvoicesResponse>("invoices").List;
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

            return data;
        }

        public async Task<List<Invoices>> GetInvoices(string pais = null, string id = null, string tipo = null, filterInvoice filter = null, Pagination pagination = null, string token = null, string order = null, string group = null)
        {
            GraphQLRequest DataRegistroRequest;
            List<Invoices> data = new List<Invoices>();


            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (tipo == null)
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaCargarFactura()
                };
            }
            else
            {
                if (id == null)
                {
                    return new List<Invoices>();
                }

                if (tipo == "Carga Manual" || tipo == "Postular")
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = queryInvoiceEditionDebtor(),
                        Variables = new { debtor = id,
                            country = pais,
                            pagination = pagination,
                            order = order,
                            group = group,
                            filter = filter == null ? null : new
                            {
                                supplier = filter.Supplier_id,
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
                else if (tipo == "Facturas Vencimiento")
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = consultaCargarFacturaSupplier(),
                        Variables = new { supplier = id }
                    };
                }
                else
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = consultaCargarFactura(),
                    };
                }

            }

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Invoices>() { new Invoices() { Errors = __notAuthorized} };
                }

                data = graphQLResponse.GetDataFieldAs<InvoicesResponse>("invoices").List;
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

            return data;
        }

        public async Task<List<Invoices>> GetInvoicesSupplier(ParametersDebtorEdition parameters, string tipo = null, filterInvoice filter = null, Pagination pagination = null, string token = null, string order = null, string group = null)
        {
            GraphQLRequest DataRegistroRequest;
            List<Invoices> data = new List<Invoices>();


            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (tipo == null)
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaCargarFactura()
                };
            }
            else
            {
                if (parameters.Id == null)
                {
                    return new List<Invoices>();
                }

                if (tipo == "Edicion Facturas" || tipo == "Postular")
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = queryInvoiceEditionSupplier(),
                        Variables = new
                        {
                            supplier = parameters.Id,
                            country = parameters.Country,
                            pagination = pagination,
                            order = order,
                            group = group,
                            filter = filter == null ? null : new
                            {
                                participant = "SUPPLIER",
                                publicationStatusNot = "rejected",
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
                else if (tipo == "Facturas Vencimiento")
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = consultaCargarFacturaSupplier(),
                        Variables = new { supplier = parameters.Id }
                    };
                }
                else
                {
                    DataRegistroRequest = new GraphQLRequest
                    {
                        Query = consultaCargarFactura(),
                    };
                }

            }

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Invoices>() { new Invoices() { Errors = __notAuthorized } };
                }

                data = graphQLResponse.GetDataFieldAs<InvoicesResponse>("invoices").List;
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

            return data;
        }

        public async Task<List<Entity>> GetConfirmants(string id = null, string token = "")
        {
            GraphQLRequest DataRegistroRequest;
            List<Entity> confirmantes = new List<Entity>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            } 

            try
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaConfirmantes(), 
                    Variables = new { debtor = id }
                };

                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Entity>() { new Entity() { Errors = __notAuthorized } };
                }

                confirmantes = graphQLResponse.GetDataFieldAs<EntityResponse> ("listConfirmantToDebtor").List;
            }
            catch (Exception)
            {
                return confirmantes;
            }


            return confirmantes;
        }

        public async Task<List<Entity>> GetConfirmantsSupplier(string id = null, string token = "")
        {
            GraphQLRequest DataRegistroRequest;
            List<Entity> confirmantes = new List<Entity>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaConfirmantesSupplier(),
                    Variables = new { supplier = id }
                };

                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Entity>() { new Entity() { Errors = __notAuthorized } };
                }

                confirmantes = graphQLResponse.GetDataFieldAs<EntityResponse>("listConfirmantToSupplier").List;
            }
            catch (Exception)
            {
                return confirmantes;
            }


            return confirmantes;
        }

        public async Task<List<Invoices>> GetPostulates(string idCliente = null, filterInvoice filter = null,Pagination pagination = null, string token = "")
        {
            List<Invoices> postulables = new List<Invoices>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaCargarPostuladas(),
                Variables = new { debtor = idCliente,
                    pagination,
                    filter = filter == null ? null : new
                    {
                        supplier = filter.Supplier_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo
                    },
                }
            };
            
            try
            {
                
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Invoices>() { new Invoices() { Errors = __notAuthorized } };
                }

                postulables = graphQLResponse.GetDataFieldAs<InvoicesResponse>("postulable").List;

                return postulables;
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

        }

        public async Task<List<Invoices>> GetPostulatesSupplier(string idCliente = null, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            List<Invoices> postulables = new List<Invoices>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaCargarPostuladas(),
                Variables = new
                {
                    debtor = idCliente,
                    pagination,
                    filter = filter == null ? null : new
                    {
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
                }
            };

            try
            {

                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Invoices>() { new Invoices() { Errors = __notAuthorized } };
                }

                postulables = graphQLResponse.GetDataFieldAs<InvoicesResponse>("postulable").List;

                return postulables;
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

        }

        public async Task<List<Publications>> GetConfirmed(string id = null, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {
            List<Publications> resultInvoices = new List<Publications>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = queryConfirmed(),
                Variables = new
                {
                    confirmant = id,
                    pagination,
                    filter = filter == null ? null : new
                    {
                        debtor = filter.Debtor_id,
                        supplier = filter.Supplier_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        financied = filter.Financied,
                        program = filter.Program
                    }
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Publications>() { new Publications() { Errors = __notAuthorized } };
                }

                resultInvoices = graphQLResponse.GetDataFieldAs<PublicationsResponse>("available").List;
            }
            catch (Exception)
            {
                return resultInvoices;
            }
            return resultInvoices;
        }

        public async Task<List<Publications>> GetCandidates(string id = null, filterInvoice filter = null, Pagination pagination = null, string token = null, string participant = null)
        {
            List<Publications> resultInvoice = new List<Publications>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Variables = new {
                    confirmant = id,
                    pagination,
                    filter = filter == null ? null : new
                    {
                        debtor = filter.Debtor_id,
                        supplier = filter.Supplier_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        financied = filter.Financied
                    }
                }
            };

            if (participant == null || participant == "CONFIRMANT")
                DataRegistroRequest.Query = consultaCargarCandidatas();
            else
                DataRegistroRequest.Query = consultaCargarCandidatasDebtor();


            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Publications>() { new Publications() { Errors = __notAuthorized } };
                }

                resultInvoice = graphQLResponse.GetDataFieldAs<PublicationsResponse>("confirmable").List;


            }
            catch (Exception e)
            {
                resultInvoice.Add(new Publications() { Error = e });
                return resultInvoice;
            }
            return resultInvoice;
        }
        public async Task<List<Publications>> GetCandidatesReview(string id = null, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {
            List<Publications> resultInvoice = new List<Publications>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaCargarCandidatasReview(),
                Variables = new
                {
                    confirmant = id,
                    pagination,
                    filter = filter == null ? null : new
                    {
                        debtor = filter.Debtor_id,
                        supplier = filter.Supplier_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        financied = filter.Financied
                    }
                }
            };
            try
            {
                var jsonD = JsonConvert.SerializeObject(DataRegistroRequest);
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Publications>() { new Publications() { Errors = __notAuthorized } };
                }

                resultInvoice = graphQLResponse.GetDataFieldAs<PublicationsResponse>("reviewable").List;


            }
            catch (Exception e)
            {
                resultInvoice.Add(new Publications() { Error = e });
                return resultInvoice;
            }
            return resultInvoice;
        }



        public async Task<List<Financiable>> GetFinanciables(string idClient, filterInvoice filter = null,Pagination pagination = null, string token = "")
        {
            GraphQLRequest DataRegistroRequest;
            List<Financiable> data = new List<Financiable>();


            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaCargarFinanciable(),
                Variables = new
                {
                    debtor = idClient,
                    pagination,
                    filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,
                        supplier = filter.Supplier_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        financied = filter.Financied,
                        program = filter.Program
                    }
                }
            };


            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Financiable>() { new Financiable() { Errors = __notAuthorized } };
                }
            

                data = graphQLResponse.GetDataFieldAs<FinanciableResponse>("financiable").List;
            }
            catch (Exception)
            {
                return new List<Financiable>();
            }

            return data;
        }

        public async Task<List<Invoices>> GetPostponed(string idCliente = null, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {

            List<Invoices> lista = new List<Invoices>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaPospuestas(),
                Variables = new { supplier = idCliente,
                pagination,
                    filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,//Aqui se esta pasando el nombre del banco, debido a la cantidad de cambios repentinos no se cambio el nombre de la variable
                        debtor = filter.Debtor_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                    }
                }
            };


            try
            {

                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Invoices>() { new Invoices() { Errors = __notAuthorized } };
                }

                lista = graphQLResponse.GetDataFieldAs<InvoicesResponse>("postponed").List;
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

            return lista;

        }

        //Consulta Sumatoria Facturas Financiadas Consulta Financiamiento Banco
        public async Task<ListAccountantsInvoices> GetFinanciablesBankInvoices(ParamAccountantsInvoices param, filterInvoice filter = null, string token = "")
        {
            GraphQLRequest DataRegistroRequest;
            //List<AccountantsInvoices> data = new List<AccountantsInvoices>();

            filter.Financied = true;
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaFinanciableBank(),
                Variables = new
                {
                    country = param.Country,
                    confirmant = param.Confirmant,
                    filter = filter == null ? null : new
                    {
                        confirmant = filter.Confirmant_id,
                        supplier = filter.Supplier_id,
                        number = filter.Number,
                        currency = filter.Currency_id,
                        expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                        amount_from = filter.AmountFrom,
                        amount_to = filter.AmountTo,
                        financied = filter.Financied
                    }
                }
            };


            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new ListAccountantsInvoices() {  Error = __notAuthorized };
                }

                var data = graphQLResponse.GetDataFieldAs<ListAccountantsInvoices>("accountantsInvoices");
                return data;
            }
            catch (Exception e)
            {
                var error = e.Message;
                return new ListAccountantsInvoices();
            }

        }

        // Consulta para las invoices
        public async Task<List<Invoices>> GetConsultaInvoices(string user = null, string participant = null, filterInvoice filter = null, Pagination pagination = null, string token = "", string order = null, string group = null, bool changeStatus = false)
        {
            GraphQLRequest DataRegistroRequest;
            List<Invoices> data = new List<Invoices>();
            Utils utilsObject = new Utils();
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (filter.InvoiceStatus != null)
            {
                utilsObject = Utils.getStatus(filter.InvoiceStatus);
                filter.InvoiceStatus = utilsObject.Value;
            }

            if(utilsObject.ChangeFrom == "confirmedForClient" && filter.Program != "CONFIRMING")
            {
                filter.Program = "DIRECT";
            } else if(filter.Program == "CONFIRMING" && utilsObject.ChangeFrom == "confirmedForClient")
            {
                filter.Number = "x";
            }

            if (participant == "DEBTOR")
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaDebtor(),
                    Variables = new
                    {
                        idDebtor = user,
                        order = order,
                        group = group,
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
                            invoiceStatus = filter.InvoiceStatus == "draft" ? "draft" : null,
                            publicationStatus = filter.InvoiceStatus != "draft" ? filter.InvoiceStatus : null,
                            program = filter.Program,
                           // changeStatus = filter.ChangeStatus,
                           // change_from = filter.ChangeFrom.HasValue ? filter.ChangeFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                           // change_to = filter.ChangeTo.HasValue ? filter.ChangeTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null
                        },
                        pagination
                    }
                };
            } 
            else if (participant == "SUPPLIER")
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaSupplier(),
                    Variables = new
                    {
                        idSupplier = user,
                        pagination,
                        order = order,
                        group = group,
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
                            publicationStatus = filter.InvoiceStatus,
                            program = filter.Program,
                        },
                    }
                };
            }
            else if (participant == "FACTOR")
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaFactor(),
                    Variables = new
                    {
                        idFactor = user,
                        order = order,
                        group = group,
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
                            publicationStatus = filter.InvoiceStatus,
                            program = filter.Program,
                        },
                        pagination
                    }
                };
            }
            else if (participant == "BACKOFFICE")
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaBackkoffice(),
                    Variables = new
                    {
                        country = int.Parse(user),
                        order = order,
                        group = group,
                        filter = filter == null ? null : new
                        {
                            confirmant = filter.Confirmant_id,
                            supplier = filter.Supplier_id,
                            debtor = filter.Debtor_id,
                            factor = filter.Factor_id,
                            number = filter.Number,
                            currency = filter.Currency_id,
                            expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            amount_from = filter.AmountFrom,
                            amount_to = filter.AmountTo,
                            financied = filter.Financied,
                            invoiceStatus = filter.InvoiceStatus == "draft" ? "draft" : null,
                            publicationStatus = filter.InvoiceStatus != "draft" ? filter.InvoiceStatus : null,
                            program = filter.Program,
                        },
                        pagination
                    }
                };

            }
            else
            {
                DataRegistroRequest = new GraphQLRequest
                {
                    Query = consultaConfirmant(),
                    Variables = new
                    {
                        idConfirmant = user,
                        order = order,
                        group = group,
                        filter = filter == null ? null : new
                        {
                            confirmant = filter.Confirmant_id,
                            supplier = filter.Supplier_id,
                            factor = filter.Factor_id,
                            debtor = filter.Debtor_id,
                            debtorId = filter.DebtorId,
                            number = filter.Number,
                            currency = filter.Currency_id,
                            expiration_from = filter.ExpirationFrom.HasValue ? filter.ExpirationFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            expiration_to = filter.ExpirationTo.HasValue ? filter.ExpirationTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            issued_from = filter.IssuedFrom.HasValue ? filter.IssuedFrom.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            issued_to = filter.IssuedTo.HasValue ? filter.IssuedTo.Value.ToString("yyyy-MM-dd") + "T00:00:00Z" : null,
                            amount_from = filter.AmountFrom,
                            amount_to = filter.AmountTo,
                            financied = filter.Financied,
                            invoiceStatus = filter.InvoiceStatus == "draft" ? "draft" : null,
                            publicationStatus = filter.InvoiceStatus != "draft" ? filter.InvoiceStatus : null,
                            program = filter.Program,
                        },
                        pagination
                    }
                };

            }

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    return new List<Invoices>() { new Invoices() { Errors = __notAuthorized } };
                }

                data = graphQLResponse.GetDataFieldAs<InvoicesResponse>("invoices").List;

                if (changeStatus)
                {
                    data.ForEach(x => {
                        Utils.setStatus(x, participant, utilsObject.ChangeFrom);

                        if (x.Publications != null)
                        {
                            x.Publications.ForEach(e => {
                                e.Participant = x.Participant;
                                Utils.setStatus(e, participant, utilsObject.ChangeFrom);
                            });
                        }
                    });
                }
                
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

            return data;
        }

        // Consulta del Detalle de las invoices
        public async Task<Invoices> GetDetalleConsultaInvoices(string id = null, string token = "")
        {
            GraphQLRequest DataRegistroRequest;
            List<Invoices> data = new List<Invoices>();


            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = detalleFacturasVistaConsulta(),
                Variables = new { id }
            };

            
            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);
                if (graphQLResponse.Errors != null)
                {
                    return new Invoices() { Errors = graphQLResponse.Errors[0].Message };
                    
                }
                data = graphQLResponse.GetDataFieldAs<InvoicesResponse>("invoices").List;
            }
            catch (Exception e)
            {
                return new Invoices() { Errors = e.Message }; ;
            }

            return data[0];
        }
        //
        public async Task<List<Publications>> GetPublicationsSessionsFactor(string owner, int country, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {
            GraphQLRequest DataRegistroRequest;
            List<Publications> data = new List<Publications>();


            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaPublicationsSessions(),
                Variables = new
                {
                    factor = owner,
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
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);
                if (graphQLResponse.Errors != null)
                {
                    data.Add(new Publications() { Errors = graphQLResponse.Errors[0].Message });
                    return data;

                }
                data = graphQLResponse.GetDataFieldAs<PublicationsResponse>("publicationsSessions").List;
            }
            catch (Exception)
            {
                return data;
            }

            return data;
        }
        //
        public async Task<List<Publications>> GetPublicationsSessions(string owner , int country, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {
            GraphQLRequest DataRegistroRequest;
            List<Publications> data = new List<Publications>();


            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            DataRegistroRequest = new GraphQLRequest
            {
                Query = consultaPublicationsSessions(),
                Variables = new
                {
                    supplier = owner,
                    country = country,
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
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);
                if (graphQLResponse.Errors != null)
                {
                    data.Add(new Publications() { Errors = graphQLResponse.Errors[0].Message });
                    return data;

                }
                data = graphQLResponse.GetDataFieldAs <PublicationsResponse>("publicationsSessions").List;
            }
            catch (Exception)
            {
                return data;
            }

            return data;
        }
        #endregion

        #region Funciones Mutaciones

        #region Carga de Facturas
        public async Task<Invoices> CreateInvoice(Invoices factura, string token)
        {
            Invoices resultInvoice = new Invoices();
            invoicesValidation validator = new invoicesValidation();
            Charges resultCharge = new Charges();
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }


            if (!validator.Validate(factura).IsValid)
            {
                resultInvoice.Errors = "Invalid invoice";
                return resultInvoice;
            }
            
            resultInvoice.Number = factura.Number;
            factura.Request_financing = false;
            if (factura.Id == null)
            {
                CargaFactura newInvoice = new CargaFactura
                {
                    Country = factura.Country_id.Value,
                    Supplier = factura.Supplier_id,
                    Debtor = factura.Debtor_id,
                    Number = factura.Number,
                    Issued_date = factura.Issued_date.Trim() + "T00:00:00Z",
                    Expiration_date = factura.Expiration_date.Trim() + "T00:00:00Z",
                    Currency = factura.Currency_id,
                    Original_amount = factura.Original_amount,
                    Amount = 0,
                    Request_financing = factura.Request_financing
                };
                var DataCargaFactura = new GraphQLRequest
                {
                    Query = mutacionCargaFactura(),
                    Variables = new { nuevaFactura = newInvoice }
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataCargaFactura);
                    
                    if (graphQLResponse.Errors != null)
                    {
                        if (graphQLResponse.Errors[0].Message == __notAuthorized)
                        {
                            resultInvoice.Errors = __notAuthorized;
                        }
                        else if (graphQLResponse.Errors[0].Message == "invoice error created")
                        {
                            resultInvoice.Errors = "invoice found";
                        }
                        else if(graphQLResponse.Errors[0].Message == "field [terms_days] not valid")
                        {
                            resultInvoice.Errors = "term days not valid";
                        }
                        else if (graphQLResponse.Errors[0].Message == "supplier has not accepted invitation")
                        {
                            resultInvoice.Errors = "supplier has not accepted invitation";
                        }
                        else if (graphQLResponse.Errors[0].Message == "Supplier don't have accounts")
                        {
                            resultInvoice.Errors = "Supplier don't have accounts";
                        }
                        else if (graphQLResponse.Errors[0].Message == "Debtor don't have accounts")
                        {
                            resultInvoice.Errors = "Debtor don't have accounts";
                        }
                        else
                        {
                            resultInvoice.Errors = graphQLResponse.Errors[0].Message;
                        }

                        return resultInvoice;
                    }

                    resultInvoice = graphQLResponse.GetDataFieldAs<Invoices>("createInvoice");
                    resultInvoice.Supplier_id = factura.Supplier_id;
                    resultInvoice.Charges = factura.Charges;
                }
                catch (Exception E) { resultInvoice.Errors = E.Message; return resultInvoice; }

            }
            else
            {
                resultInvoice = factura;
            }


            if (resultInvoice.Charges.Count != 0)
            {   
                for (var i = 0; i < resultInvoice.Charges.Count; i++)
                {
                    CargaDeducciones newCharge = new CargaDeducciones
                    {
                        Invoice_id = resultInvoice.Id,
                        Charge_type_id = resultInvoice.Charges[i].Charge_type_id,
                        Currency_id = resultInvoice.Currency.Id,
                        Number = resultInvoice.Charges[i].Number,
                        Amount = resultInvoice.Charges[i].Amount
                    };
                    var DataCharge = new GraphQLRequest
                    {
                        Query = mutationCreateCharges(),
                        Variables = new { deducciones = newCharge }
                    };
                    try
                    {
                        var graphQLResponse = await this._client.PostAsync(DataCharge);

                        if (graphQLResponse.Data == null)
                        {
                            resultInvoice.Charges.Remove(resultInvoice.Charges[i]);
                            resultInvoice.Errors = "Error al cargar deducciones";
                            continue;
                        }
                        resultInvoice.Amount -= resultInvoice.Charges[i].Amount;
                        resultInvoice.Charges[i].Id = graphQLResponse.GetDataFieldAs<Charges>("createCharge").Id;
                    }
                    catch (Exception E)
                    {
                        resultInvoice.Errors = E.Message;
                    }
                }
            }
            return resultInvoice;
        }

        public async Task<List<Invoices>> CreateInvoices(List<Invoices> facturas, string token)
        {
            List<Invoices> lista = new List<Invoices>();
            Invoices resultInvoice = new Invoices();
            var i = 0;
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            foreach (var factura in facturas)
            {
                i++;
                factura.Request_financing = false;
                if (!new invoicesValidation().Validate(factura).IsValid)
                {
                    lista.Add(new Invoices()
                    {
                        Number = factura.Number,
                        Id = "" + i,
                        Errors = "Invalid invoice"
                    });
                    continue;
                }

                if (factura.Id == null)
                {
                    CargaFactura newInvoice = new CargaFactura
                    {
                        Country = factura.Country_id.Value,
                        Supplier = factura.Supplier_id,
                        Debtor = factura.Debtor_id,
                        Number = factura.Number,
                        Issued_date = factura.Issued_date.Trim() + "T00:00:00Z",
                        Expiration_date = factura.Expiration_date.Trim() + "T00:00:00Z",
                        Currency = factura.Currency_id,
                        Amount = factura.Amount,
                        Original_amount = factura.Original_amount,
                        Request_financing = factura.Request_financing
                    };
                    var DataCargaFactura = new GraphQLRequest
                    {
                        Query = mutacionCargaFactura(),
                        Variables = new { nuevaFactura = newInvoice }
                    };
                    try
                    {
                        var graphQLResponse = await this._client.PostAsync(DataCargaFactura);

                        if (graphQLResponse.Errors != null)
                        {
                            if (graphQLResponse.Errors[0].Message == __notAuthorized)
                            {
                                resultInvoice.Errors = __notAuthorized;
                            }

                            var error = graphQLResponse.Errors[0].Message;

                            if (error == "invoice error created")
                                error = "invoice found";

                            lista.Add(new Invoices()
                            {
                                Number = newInvoice.Number,
                                Id = "" + i,
                                Errors = error
                            });
                            continue;
                        }

                        resultInvoice = graphQLResponse.GetDataFieldAs<Invoices>("createInvoice");
                        resultInvoice.Supplier_id = factura.Supplier_id;
                        lista.Add(resultInvoice);
                    }
                    catch (Exception E)
                    {
                        lista.Add(new Invoices() { Number = newInvoice.Number, Id = "" + i, Errors = E.Message });

                    }

                }
                else
                {
                    lista.Add(factura);
                }


            }
            return lista;
        }
        #endregion

        #region Edicion de Facturas
        public async Task<Invoices> DeleteInvoice(UpdateInvoice factura, string token)
        {
            Invoices resultInvoices = new Invoices();
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (!new updateInvoicesValidation().Validate(factura).IsValid)
            {
                resultInvoices.Errors = "Invalid data";
                return resultInvoices;
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = mutationEliminarFactura(),
                Variables = new { 
                    id = factura.Invoice_id,
                    debtor = factura.Debtor_id
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    resultInvoices.Errors = __notAuthorized;
                }

                var result = graphQLResponse.GetDataFieldAs<Invoices>("deleteInvoice");

                if (result.Errors != null)
                {
                    resultInvoices.Errors = graphQLResponse.Errors[0].Message;
                }
            }
            catch (Exception e)
            {
                resultInvoices.Errors = e.Message;
            }
            return resultInvoices;
        }

        public async Task<Invoices> UpdateInvoice(Invoices factura, string token)
        {
            Invoices resultInvoices = new Invoices();
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (!new invoicesValidation().Validate(factura).IsValid)
            {
                resultInvoices.Errors = "Invalid data";
                return resultInvoices;
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = mutationUpdateFactura(),
                Variables = new {
                    id = factura.Id,
                    supplier = factura.Supplier_id,
                    debtor = factura.Debtor_id,
                    number = factura.Number,
                    issued_date = factura.Issued_date.Trim() + "T00:00:00Z",
                    expiration_date = factura.Expiration_date.Trim() + "T00:00:00Z",
                    currency = factura.Currency_id,
                    original_amount = factura.Original_amount,
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);


                if (graphQLResponse.Errors != null) 
                {
                   if (graphQLResponse.Errors[0].Message == __notAuthorized)
                    {
                        resultInvoices.Errors = __notAuthorized;
                    }
                    else if (graphQLResponse.Errors[0].Message == "pq: duplicate key value violates unique constraint \"uq_invoice_index\"")
                    { 
                        resultInvoices.Errors = "invoice already exist";
                    }
                    else
                    {
                        resultInvoices.Errors = graphQLResponse.Errors[0].Message;
                    }

                    return resultInvoices;
                }
                //
                resultInvoices = graphQLResponse.GetDataFieldAs<Invoices>("updateInvoice");

            }
            catch (Exception e)
            {
                resultInvoices.Error = e;
            }
            return resultInvoices;
        }

        public async Task<Response> CreateDeduction(Charges charge, string token)
        {
            Response resultCharge = new Response();
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            CargaDeducciones newCharge = new CargaDeducciones
            {
                Charge_type_id = charge.Charge_type_id,
                Invoice_id = charge.Invoice_id,
                Currency_id = charge.Currency_id,
                Number = charge.Number,
                Amount = charge.Amount
            };

            var data = new GraphQLRequest
            {
                Query = mutationCreateCharges(),
                Variables = new
                {
                    deducciones = newCharge
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(data);


                if (graphQLResponse.Errors != null)
                {
                    if (graphQLResponse.Errors[0].Message == __notAuthorized)
                    {
                        resultCharge.Error = __notAuthorized;
                    }
                    else if (graphQLResponse.Errors[0].Message == "Invalid amount by reducing deductions")
                    {
                        resultCharge.Error = "Deducciones superan monto";
                    }
                    else
                    {
                        resultCharge.Error = graphQLResponse.Errors[0].Message;
                        return resultCharge;
                    }

                    return resultCharge;
                }

                resultCharge.Id = graphQLResponse.GetDataFieldAs<Charges>("createCharge").Id;

            }
            catch (Exception e)
            {
                resultCharge.Error = e.Message;
            }
            return resultCharge;
        }

        public async Task<Response> DeleteDeduction(Charges deduction, string token)
        {
            Response resultCharge = new Response();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = mutationEliminarDeduccion(),
                Variables = new
                {
                    id = deduction.Id
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);
                //


                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    resultCharge.Error = __notAuthorized;
                } 
                else if (graphQLResponse.Errors != null)
                {
                    return new Response() { Error = graphQLResponse.Errors[0].Message };
                }

                var res = graphQLResponse.GetDataFieldAs<Charges>("deleteCharge");


            }
            catch (Exception e)
            {
                resultCharge.Error = e.Message;
            }
            return resultCharge;
        }

        public async Task<Response> UpdateDeduction(Charges charge, string token)
        {
            Response resultCharge = new Response();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            var DataRegistroRequest = new GraphQLRequest
            {
                Query = mutationUpdateCharge(),
                Variables = new
                {
                    id = charge.Id,
                    invoice_id = charge.Invoice_id,
                    charge_type_id = charge.Charge_type_id,
                    currency_id = charge.Currency_id,
                    number = charge.Number,
                    amount = charge.Amount
                }
            };

            try
            {
                var graphQLResponse = await this._client.PostAsync(DataRegistroRequest);

                if (graphQLResponse.Errors != null && graphQLResponse.Errors[0].Message == __notAuthorized)
                {
                    resultCharge.Error = __notAuthorized;
                }
                else if (graphQLResponse.Errors != null)
                {
                    return new Response() { Error = graphQLResponse.Errors[0].Message };
                }

                var res = graphQLResponse.GetDataFieldAs<Charges>("updateCharge");

            }
            catch (Exception e)
            {
                resultCharge.Error = e.Message;
            }
            return resultCharge;
        }
        #endregion

        public async Task<List<Invoices>> PostulateInvoices(List<Invoices> facturas, string token)
        {
            List<Invoices> lista = new List<Invoices>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            foreach (var postulada in facturas)
            {
                Invoices resultPostulation = new Invoices();

                PostulationInvoice newPostulation = new PostulationInvoice
                {
                    Country = postulada.Country_id,
                    Invoice = postulada.Id,
                    Confirmant = postulada.Publication.Confirmant.Id,
                    Financiated = false
                };

                if (!new postulationInvoicesValidation().Validate(newPostulation).IsValid)
                {
                    resultPostulation.Errors = "Invalid invoice";
                    lista.Add(resultPostulation);
                    continue;
                }

                var DataPostulada = new GraphQLRequest
                {
                    Query = mutationPostulateInvoice(),
                    Variables = new { newPostulation = newPostulation }
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataPostulada);

                    if (graphQLResponse.Errors != null)
                    {
                        resultPostulation.Errors = graphQLResponse.Errors[0].Message; 
                        resultPostulation.Number = postulada.Number;
                        resultPostulation.Name = postulada.Currency.Iso_4217;
                        lista.Add(resultPostulation);
                        continue;
                    }

                    resultPostulation = graphQLResponse.GetDataFieldAs<Publications>("postulateInvoice").Invoice;
                }catch (Exception e) {
                    resultPostulation.Errors = e.Message;
                }

                var financio = true;
                if ((bool)postulada.Request_financing && resultPostulation.Errors == null)
                {
                    var res = await FinancingInvoice(new UpdateInvoice { Invoice_id = postulada.Id },token);

                    if (res.Errors != null)
                        financio = false;
                }

                if (!financio)
                {
                    resultPostulation.Errors = "Error al financiar";
                    resultPostulation.Id = postulada.Id;
                    resultPostulation.Number = postulada.Number;
                }

                lista.Add(resultPostulation);
            }
            return lista;
        }

        public async Task<Invoices> FinancingInvoice(UpdateInvoice factura, string token)
        {
            Invoices resultPostulation = new Invoices();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (!new updateInvoicesValidation().Validate(factura).IsValid)
            {
                resultPostulation.Errors = "Invalid invoice";
                return resultPostulation;
            }

            var DataPostulada = new GraphQLRequest
            {
                Query = mutationFinanciar(),
                Variables = new { factura = factura.Invoice_id}
            };
            try
            {
                var graphQLResponse = await this._client.PostAsync(DataPostulada);

                if (graphQLResponse.Errors != null)
                {
                    return new Invoices() { Errors = graphQLResponse.Errors[0].Message };
                }

                resultPostulation = graphQLResponse.GetDataFieldAs<Invoices>("requestFinancing");
            }
            catch (Exception e) { resultPostulation.Errors = e.Message; }
            return resultPostulation;
        }

        public async Task<List<Invoices>> FinancingInvoices(List<Invoices> facturas, string token)
        {
            List<Invoices> retorno = new List<Invoices>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            foreach (var item in facturas)
            {

                if (!new updateInvoicesValidation().Validate(new UpdateInvoice() { Invoice_id = item.Id }).IsValid)
                {
                    retorno.Add(new Invoices() { Errors = "Invalid invoice", Id = item.Id });
                    continue;
                }

                var DataPostulada = new GraphQLRequest
                {
                    Query = mutationFinanciar(),
                    Variables = new { factura = item.Id } 
                };

                var graphQLResponse = await this._client.PostAsync(DataPostulada);

                try
                {

                    if (graphQLResponse.Data == null)
                    {
                        retorno.Add(new Invoices() { Errors = graphQLResponse.Errors[0].Message, Id = item.Id });
                        continue;
                    }

                    retorno.Add(graphQLResponse.GetDataFieldAs<Invoices>("requestFinancing"));

                }
                catch (Exception e)
                {
                    retorno.Add(new Invoices() { Errors = e.Message, Id = item.Id });
                }
            }

            return retorno;
        }
        
        public async Task<List<Invoices>> OffertInvoices(List<OffertInvoice> ofertas,string token)
        {
            List<Invoices> resultOfferting = new List<Invoices>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }


            foreach (var item in ofertas)
            {
                if (!new OffertInvoiceValidation().Validate(item).IsValid)
                {
                    resultOfferting.Add(new Invoices()
                    {
                        Id = item.Publication_id,
                        Errors = "Invalid publication"
                    });

                    continue;
                }

                var DataOfertada = new GraphQLRequest
                {
                    Query = mutationOffert(),
                    Variables = new 
                    { 
                        confirmant = item.confirmant, 
                        country = item.country,
                        offer = item.Bid_amount,
                        publication = item.Publication_id 
                    }
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataOfertada);

                    if (graphQLResponse.Errors != null)
                    {
                        resultOfferting.Add(new Invoices() { Id = item.Publication_id, Errors = graphQLResponse.Errors[0].Message });
                        continue;
                    }

                    resultOfferting.Add(graphQLResponse.GetDataFieldAs<Invoices>("offerInvoice"));
                }
                catch (Exception e)
                {
                    resultOfferting.Add(new Invoices()
                    {
                        Id = item.Publication_id,
                        Errors = e.Message
                    });
                }
            }

            return resultOfferting;
        }
        
        public async Task<List<Invoices>> ConfirmInvoices(List<UpdateInvoice> confirmar, string token)
        {
            List<Invoices> resultConfirming = new List<Invoices>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            foreach (var item in confirmar)
            {
                if (!new updateInvoicesValidation().Validate(item).IsValid)
                {
                    resultConfirming.Add(new Invoices()
                    {
                        Id = item.Invoice_id,
                        Errors = "Invalida invoice"
                    });

                    continue;
                }

                var DataPostulada = new GraphQLRequest
                {
                    Query = mutationConfirmInvoice(),
                    Variables = new { confirm = new { confirmant = item.confirmant, country = item.country, publication = item.Invoice_id} }
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataPostulada);

                    if (graphQLResponse.Errors != null)
                    {
                        resultConfirming.Add(new Invoices() { Id = item.Invoice_id, Errors = graphQLResponse.Errors[0].Message });
                        continue;
                    }

                    resultConfirming.Add(graphQLResponse.GetDataFieldAs<Invoices>("confirmInvoice"));
                }
                catch (Exception e)
                {
                    resultConfirming.Add(new Invoices()
                    {
                        Id = item.Invoice_id,
                        Errors = e.Message
                    });
                }
            }

            return resultConfirming;
        }

        public async Task<List<Invoices>> ReviewInvoice(List<UpdateInvoice> review, string token)
        {
            List<Invoices> resultConfirming = new List<Invoices>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            foreach (var item in review)
            {
                if (!new updateInvoicesValidation().Validate(item).IsValid)
                {
                    resultConfirming.Add(new Invoices()
                    {
                        Id = item.Invoice_id,
                        Errors = "Invalida invoice"
                    });

                    continue;
                }

                var DataPostulada = new GraphQLRequest
                {
                    Query = mutationReviewInvoice(),
                    Variables = new { confirm = new { confirmant = item.confirmant, country = item.country, publication = item.Invoice_id } }
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataPostulada);

                    if (graphQLResponse.Errors != null)
                    {
                        resultConfirming.Add(new Invoices() { Id = item.Invoice_id, Errors = graphQLResponse.Errors[0].Message });
                        continue;
                    }

                    resultConfirming.Add(graphQLResponse.GetDataFieldAs<Invoices>("reviewInvoice"));
                }
                catch (Exception e)
                {
                    resultConfirming.Add(new Invoices()
                    {
                        Id = item.Invoice_id,
                        Errors = e.Message
                    });
                }
            }

            return resultConfirming;
        }

        public async Task<List<Invoices>> RefuseInvoice(List<UpdateInvoice> rechazar, string token)
        {
            List<Invoices> resultConfirming = new List<Invoices>();
            //graphQLClient.DefaultRequestHeaders.Add("Grant-Access", access);
            //graphQLClient.DefaultRequestHeaders.Add("Authorization", authorized);

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            foreach (var item in rechazar)
            {
                if (!new updateInvoicesValidation().Validate(item).IsValid)
                {
                    resultConfirming.Add(new Invoices()
                    {
                        Id = item.Invoice_id,
                        Errors = "Invalida invoice"
                    });

                    continue;
                }

                var DataPostulada = new GraphQLRequest
                {
                    Query = mutationRefuseInvoice(),
                    Variables = new { confirm = new { country = item.country, publication = item.Invoice_id } }
                };
                try
                {
                    var graphQLResponse = await this._client.PostAsync(DataPostulada);

                    if (graphQLResponse.Errors != null)
                    {
                        resultConfirming.Add(new Invoices() { Id = item.Invoice_id, Errors = graphQLResponse.Errors[0].Message });
                        continue;
                    }

                    resultConfirming.Add(graphQLResponse.GetDataFieldAs<Invoices>("returnInvoice"));
                }
                catch (Exception e)
                {
                    resultConfirming.Add(new Invoices()
                    {
                        Id = item.Invoice_id,
                        Errors = e.Message
                    });
                }
            }

            return resultConfirming;
        }
        #endregion


    }
}
