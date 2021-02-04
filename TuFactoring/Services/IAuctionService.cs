using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;

namespace TuFactoring.Services
{
    public interface IAuctionService
    {
        #region consultas
        Task<List<Publications>> GetPublications(int country, string participant, string owner = null, filterInvoice filter = null, string token = "", Pagination pagination = null);
        Task<List<Auctions>> GetAuction(int country, string token = "");
        Task<List<Publications>> GetPublishable(string owner, string type, filterInvoice filter = null, Pagination pagination = null, string token = "");

        Task<List<Publications>> GetWinner(int idPais, string owner, filterInvoice filter = null, Pagination pagination = null, string token = "");

        #endregion

        #region Mutaciones
        Task<Publications> OfferPublication(Offert offert, string token = "");
        Task<Publications> PublicationsInvoice(UpdateInvoice invoice, string token = "");
        Task<Publications> SellInvoice(UpdateInvoice invoice, string token = "");
        Task<Publications> RejectOffer(UpdateInvoice invoice, string token = "");
        Task<Publications> PostponeInvoice(UpdateInvoice invoice, string token = "");
        Task<Publications> AceptOffert(UpdateInvoice invoice, string token = "");
        Task<Publications> RejectOfferMarket(UpdateInvoice invoice, string token = ""); 

        Task<Auctions> CreateAuction(string country,string token = "");
        Task<Auctions> OpenAuction(string country, string token = "");
        Task<Auctions> CloseAuctionAsync(string country, string token = "");

        Task<Auctions> EndingAuction(string country,string token = "");
        Task<Auctions> ConciliationAuction(string country, string token = "");
        Task<Auctions> PaymentsAuction(string country, string token = "");

        #endregion


        #region mutacion para opciones "ALL"
        Task<List<Publications>> SellAllInvoices(List<UpdateInvoice> ofertas, string token = "");

        Task<List<Publications>> RejectAllInvoices(List<UpdateInvoice> ofertas, string token = "");

        Task<List<Publications>> PublicationsAllInvoices(List<UpdateInvoice> ofertas, string token = "");

        Task<List<Publications>> PosponeAllInvoices(List<UpdateInvoice> ofertas, string token = "");

        Task<List<Publications>> AceptOffertAll(List<UpdateInvoice> invoices, string token = "");
        #endregion
    }
}
