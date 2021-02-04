using GraphQL.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringGraphql;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Services
{
    public class InvoiceService :IInvoiceService
    {

        private readonly InvoiceConsumer _consumer;
        private readonly IConfiguration _configuration;

        public InvoiceService(IConfiguration configuration)
        {
            _configuration = configuration;
            _consumer = new InvoiceConsumer(new GraphQLClient(_configuration["InvoiceEndpoint"]));
        }

        #region CRUD
        public async Task<List<Invoices>> GetInvoicesForDebtorEdition(ParametersDebtorEdition parameters, string tipo, filterInvoice filter = null, Pagination pagination = null, string token = "", string order = null, string group = null) 
        { 
            try
            {
                return await this._consumer.GetInvoicesForDebtorEdition(parameters, tipo, filter, pagination, token, order, group);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<List<Invoices>> GetInvoices(string idPais,string idCliente, string tipo, filterInvoice filter = null, Pagination pag = null, string token = "")
        {
            try
            {
                return await this._consumer.GetInvoices(idPais,idCliente, tipo, filter, pag, token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<List<Invoices>> GetInvoicesSupplier(ParametersDebtorEdition parameters, string tipo, filterInvoice filter = null, Pagination pagination = null, string token = "", string order = null, string group = null)
        {
            try
            {
                return await this._consumer.GetInvoicesSupplier(parameters, tipo, filter, pagination, token, order, group);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<Invoices> CreateInvoice(Invoices invoice, string token = "")
        {
            try
            {
                return await this._consumer.CreateInvoice(invoice, token);
            }
            catch (Exception e)
            {
                return new Invoices() { Errors = e.Message };
            }
        }

        public async Task<List<Invoices>> CreateInvoices(List<Invoices> invoices, string token = "")
        {
            try
            {
                return await this._consumer.CreateInvoices(invoices,token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<Invoices> DeleteInvoice(UpdateInvoice invoice, string token = "")
        {
            try
            {
                return await this._consumer.DeleteInvoice(invoice, token);
            }
            catch (Exception e)
            {
                return new Invoices() { Errors = e.Message };
            }
        }

        public async Task<Invoices> UpdateInvoice(Invoices invoice, string token = "")
        {
            try
            {
                return await this._consumer.UpdateInvoice(invoice,token);
            }
            catch (Exception e)
            {
                return new Invoices() { Error = e };
            }
        }
        #endregion


        #region Postulates Method
        public async Task<List<Entity>> GetConfirmants(string debtorId, string token = "")
        {
            try
            {
                return await this._consumer.GetConfirmants(debtorId, token);
            }
            catch (Exception)
            {
                return new List<Entity>();
            }
        }

        public async Task<List<Entity>> GetConfirmantsSupplier(string supplierId, string token = "")
        {
            try
            {
                return await this._consumer.GetConfirmantsSupplier(supplierId, token);
            }
            catch (Exception)
            {
                return new List<Entity>();
            }
        }

        public async Task<List<Invoices>> GetPostulates(string debtorId, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetPostulates(debtorId, filter,pagination, token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

        }

        public async Task<List<Invoices>> GetPostulatesSupplier(string supplierId, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetPostulatesSupplier(supplierId, filter, pagination, token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

        }


        public async Task<List<Invoices>> PostulateInvoices(List<Invoices> invoices, string token = "")
        {
            try
            {
                return await this._consumer.PostulateInvoices(invoices, token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }

        }

        #endregion

        #region Financing Method

        public async Task<List<Invoices>> FinancingInvoices(List<Invoices> invoices, string token = "")
        {
            try
            {
                return await this._consumer.FinancingInvoices(invoices, token);
            }catch(Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<List<Financiable>> GetFinanciables(string idClient, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetFinanciables(idClient, filter,pagination, token);
            }
            catch (Exception)
            {
                return new List<Financiable>();
            }
        }

        public async Task<ListAccountantsInvoices> GetFinanciablesBankInvoices(ParamAccountantsInvoices param, filterInvoice filter = null, string token = "")
        {
            try
            {
                return await this._consumer.GetFinanciablesBankInvoices(param, filter, token);
            }
            catch (Exception)
            {
                return new ListAccountantsInvoices();
            }
        }
        #endregion

        #region Bank Methods
        public async Task<List<Publications>> GetCandidates(string owner, filterInvoice filter = null, Pagination pagination = null, string token = "", string participant = null)
        {
            try
            {
                return await this._consumer.GetCandidates(owner,filter,pagination,token,participant);
            }catch(Exception)
            {
                return new List<Publications>();
            }
        }
        public async Task<List<Publications>> GetCandidatesReview(string owner, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetCandidatesReview(owner, filter, pagination, token);
            }
            catch (Exception)
            {
                return new List<Publications>();
            }
        }

        public async Task<List<Publications>> GetConfirmed(string owner, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetConfirmed(owner,filter,pagination,token);
            }
            catch (Exception)
            {
                return new List<Publications>();
            }
        }

        public async Task<List<Invoices>> OffertInvoices(List<OffertInvoice> invoices, string token = "")
        {
            try
            {
                return await this._consumer.OffertInvoices(invoices,token);
            }catch(Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<List<Invoices>> ConfirmInvoices(List<UpdateInvoice> invoices, string token = "")
        {
            try
            {
                return await this._consumer.ConfirmInvoices(invoices, token);
            }catch(Exception)
            {
                return new List<Invoices>();
            }
        }
        public async Task<List<Invoices>> ReviewInvoice(List<UpdateInvoice> invoices, string token = "")
        {
            try
            {
                return await this._consumer.ReviewInvoice(invoices, token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }
        public async Task<List<Invoices>> RefuseInvoice(List<UpdateInvoice> invoices, string token = "")
        {
            try
            {
                return await this._consumer.RefuseInvoice(invoices, token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }
        #endregion

        #region Invoices Expiration
        public async Task<List<Invoices>> GetPostponed(string owner, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetPostponed(owner, filter, pagination,token);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }
        #endregion

        #region ConsultaInvoiceGenerales
        public async Task<List<Invoices>> GetConsultaInvoices(string user = null, string participant = null, filterInvoice filter = null, Pagination pagination = null, string token = "", string order = null, string group = null, bool changeStatus = false)
        {
            try
            {
                return await this._consumer.GetConsultaInvoices(user, participant, filter, pagination,token,order,group, changeStatus);
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<Invoices> GetDetalleConsultaInvoices(string id = null, string token = "")
        {
            try
            {
                return await this._consumer.GetDetalleConsultaInvoices(id,token);
            }
            catch (Exception)
            {
                return new Invoices();
            }
        }
        #endregion
        
        #region Deductions
        public async Task<Response> CreateDeduction(Charges deduction, string token = "")
        {
            try
            {
                return await this._consumer.CreateDeduction(deduction, token);
            }
            catch (Exception e)
            {
                return new Response() { Error = e.Message };
            }
        }


        public async Task<Response> DeleteDeduction(Charges deduction, string token = "")
        {
            try
            {
                return await this._consumer.DeleteDeduction(deduction,token);
            }
            catch (Exception e)
            {
                return new Response() { Error = e.Message };
            }
        }

        public async Task<Response> UpdateDeduction(Charges deduction, string token = "")
        {
            try
            {
                return await this._consumer.UpdateDeduction(deduction, token);
            }
            catch (Exception e)
            {
                return new Response() { Error = e.Message };
            }
        }
        #endregion
      
        #region Consultas
        public async Task<List<Invoices>> GetInvoicesConsultas(string owner, string participant, filterInvoice filter = null, Pagination pagination = null)
        {
            try
            {
                return null;
            }
            catch (Exception)
            {
                return new List<Invoices>();
            }
        }

        public async Task<List<Publications>> GetPublicationsSessionsFactor(string owner, int country, filterInvoice filter = null, Pagination pagination = null, string token = null)
        {
            try
            {
                return await this._consumer.GetPublicationsSessionsFactor(owner, country, filter, pagination, token);
            }
            catch (Exception)
            {
                return new List<Publications>();
            }
        }

        public async Task<List<Publications>> GetPublicationsSessions(string owner, int country, filterInvoice filter = null, Pagination pagination = null,string token = null)
        {
            try
            {
                return await this._consumer.GetPublicationsSessions(owner,country,filter,pagination,token);
            }
            catch (Exception)
            {
                return new List<Publications>();
            }
        }
        #endregion

    }
}
