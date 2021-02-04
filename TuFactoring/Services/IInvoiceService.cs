using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Services
{
    public interface IInvoiceService
    {
        #region CRUD
        Task<List<Invoices>> GetInvoices(string idPais, string idCliente,string tipo, filterInvoice filter = null, Pagination pag = null,string token = "");

        Task<List<Invoices>> GetInvoicesForDebtorEdition(ParametersDebtorEdition parameters, string tipo, filterInvoice filter = null, Pagination pagination = null, string token = "", string order = null, string group = null);

        Task<List<Invoices>> GetInvoicesSupplier(ParametersDebtorEdition parameters, string tipo, filterInvoice filter = null, Pagination pagination = null, string token = "", string order = null, string group = null);

        Task<Invoices> CreateInvoice(Invoices invoice, string token = "");

        Task<List<Invoices>> CreateInvoices(List<Invoices> invoices, string token = "");

        Task<Invoices> DeleteInvoice(UpdateInvoice invoice, string token = "");
        Task<Invoices> UpdateInvoice(Invoices invoice, string token = "");
        #endregion

        #region Postulates Method
        Task<List<Entity>> GetConfirmants(string debtorId, string token = "");

        Task<List<Entity>> GetConfirmantsSupplier(string supplierId, string token = "");

        Task<List<Invoices>> GetPostulates(string debtorId, filterInvoice filter = null, Pagination pagination = null, string token = "");

        Task<List<Invoices>> GetPostulatesSupplier(string debtorId, filterInvoice filter = null, Pagination pagination = null, string token = "");

        Task<List<Invoices>> PostulateInvoices(List<Invoices> invoices, string token = "");

        #endregion


        #region Financing Method
        Task<List<Invoices>> FinancingInvoices(List<Invoices> invoices, string token = "");

        Task<List<Financiable>> GetFinanciables(string idClient, filterInvoice filter = null, Pagination pagination = null, string token = "");

        Task<ListAccountantsInvoices> GetFinanciablesBankInvoices(ParamAccountantsInvoices param, filterInvoice filter = null, string token = "");
        #endregion


        #region Bank Method
        Task<List<Publications>> GetCandidates( string owner, filterInvoice filter = null, Pagination pagination = null, string token = "", string participant = null);

        Task<List<Publications>> GetCandidatesReview(string owner, filterInvoice filter = null, Pagination pagination = null, string token = "");

        Task<List<Publications>> GetConfirmed( string owner, filterInvoice filter = null, Pagination pagination = null, string token = "");

        Task<List<Invoices>> OffertInvoices(List<OffertInvoice> invoices, string token = "");

        Task<List<Invoices>> ConfirmInvoices(List<UpdateInvoice> invoices, string token = "");
        Task<List<Invoices>> ReviewInvoice(List<UpdateInvoice> invoices, string token = "");
        Task<List<Invoices>> RefuseInvoice(List<UpdateInvoice> invoices, string token = "");
        #endregion


        #region Invoices Expiration
        Task<List<Invoices>> GetPostponed(string owner, filterInvoice filter = null, Pagination pagination = null, string token = "");
        #endregion

        #region ConsultaInvoiceGenerales
        Task<List<Invoices>> GetConsultaInvoices(string user = null, string participant = null, filterInvoice filter = null, Pagination pagination = null, string token = "", string order = null, string group = null, bool changeStatus = false);
        Task<Invoices> GetDetalleConsultaInvoices(string id = null, string token = "");
        #endregion

        #region Deductions
        Task<Response> CreateDeduction(Charges deduction, string token = "");
        Task<Response> DeleteDeduction(Charges deduction, string token = "");

        Task<Response> UpdateDeduction(Charges deduction, string token = "");
        #endregion

        #region Consultas
        Task<List<Invoices>> GetInvoicesConsultas(string owner, string participant, filterInvoice filter = null, Pagination pagination = null);

        Task<List<Publications>> GetPublicationsSessionsFactor(string owner, int country, filterInvoice filter = null, Pagination pagination = null, string token = null);
        Task<List<Publications>>GetPublicationsSessions(string owner, int country, filterInvoice filter = null, Pagination pagination = null, string token = null);
        #endregion

    }
}
