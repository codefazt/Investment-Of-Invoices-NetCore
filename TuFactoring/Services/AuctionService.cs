using GraphQL.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringGraphql;
using TuFactoringModels;

namespace TuFactoring.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly AuctionConsumer _consumer;
        private readonly IConfiguration _configuration;

        public AuctionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _consumer = new AuctionConsumer(new GraphQLClient(_configuration["AuctionEndpoint"]));
        }

        #region Consultas
        public async Task<List<Auctions>> GetAuction(int country, string token = "")
        {
            try
            {
                return await this._consumer.GetAuction(country,token);
            }catch(Exception)
            {
                return new List<Auctions>();
            }
        }

        public async Task<List<Publications>> GetPublications(int country, string participant, string owner = null, filterInvoice filter = null, string token = "",Pagination pagination = null)
        {
            try
            {
                return await this._consumer.GetPublications(country, participant, owner, filter, token, pagination);
            }
            catch (Exception)
            {
                return new List<Publications>();
            }
        }

        public async Task<List<Publications>> GetPublishable( string owner,string type, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetPublishable(owner,type, filter, pagination,token);
            }
            catch (Exception)
            {
                return new List<Publications>();
            }
        }

        public async Task<List<Publications>> GetWinner(int idPais, string owner, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await this._consumer.GetWinner(idPais, owner,filter, pagination, token);
            }catch(Exception)
            {
                return new List<Publications>();
            }
        }

        #endregion

        #region Mutaciones

        public async Task<Publications> OfferPublication(Offert offert, string token = "")
        {
            try
            {
                return await this._consumer.OfferPublication(offert, token);
            }catch(Exception)
            {
                return new Publications();
            }
        }

        public async Task<Publications> PostponeInvoice(UpdateInvoice invoice, string token = "")
        {
            try
            {
                return await this._consumer.PostponeInvoice(invoice,token);
            }
            catch (Exception)
            {
                return new Publications();
            }
        }

        public async Task<Publications> PublicationsInvoice(UpdateInvoice invoice, string token = "")
        {
            try
            {
                return await this._consumer.PublicationsInvoice(invoice,token);
            }
            catch (Exception)
            {
                return new Publications();
            }
        }

        public async Task<Publications> RejectOffer(UpdateInvoice invoice, string token = "")
        {
            try
            {
                return await this._consumer.RejectOffer(invoice,token);
            }
            catch (Exception)
            {
                return new Publications();
            }
        }

        public async Task<Publications> SellInvoice(UpdateInvoice invoice, string token = "")
        {
            try
            {
                return await this._consumer.SellInvoice(invoice,token);
            }
            catch (Exception)
            {
                return new Publications();
            }
        }

        public async Task<Publications> AceptOffert(UpdateInvoice invoice, string token = "")
        {
            try
            {
                return await this._consumer.AceptOffert(invoice,token);
            }catch(Exception e)
            {
                return new Publications() { Error = e };
            }
        }

        public async Task<Publications> RejectOfferMarket(UpdateInvoice invoice,string token = "")
        {
            try
            {
                return await this._consumer.RejectOfferMarket(invoice,token);
            }
            catch (Exception)
            {
                return new Publications();
            }
        }

        public async Task<Auctions> CreateAuction(string country, string token = "")
        {
            try
            {
                return await this._consumer.CreateAuction(country, token);
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message};
            }
        }

        public async Task<Auctions> OpenAuction(string country, string token = "")
        {
            try
            {
                return await this._consumer.OpenAuction(country, token);
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }
        }

        public async Task<Auctions> EndingAuction(string country,string token = "")
        {
            try
            {
                return await this._consumer.EndingAuction(country,token);
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }
        }

        public async Task<Auctions> ConciliationAuction(string country, string token = "")
        {
            try
            {
                return await this._consumer.ConciliationAuction(country, token);
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }
        }
        
        public async Task<Auctions> PaymentsAuction(string country, string token = "")
        {
            try
            {
                return await this._consumer.PaymentsAuction(country, token);
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }
        }

        public async Task<Auctions> CloseAuctionAsync(string country, string token = "")
        {
            try
            {
                return await this._consumer.CloseAuction(country, token);
            }
            catch (Exception e)
            {
                return new Auctions() { Error = e.Message };
            }
        }
        #endregion

        #region Mutaciones para opciones "All"
        public async Task<List<Publications>> SellAllInvoices(List<UpdateInvoice> ofertas, string token = "")
        {
            try
            {
                return await this._consumer.SellAllInvoices(ofertas,token);
            }catch(Exception e)
            {
                return new List<Publications>() {
                    new Publications()
                    {
                        Error = e
                    }
                };
            }
        }

        public async Task<List<Publications>> RejectAllInvoices(List<UpdateInvoice> ofertas, string token = "")
        {
            try
            {
                return await this._consumer.RejectAllInvoices(ofertas,token);
            }
            catch (Exception e)
            {
                return new List<Publications>() {
                    new Publications()
                    {
                        Error = e
                    }
                };
            }
        }

        public async Task<List<Publications>> PosponeAllInvoices(List<UpdateInvoice> ofertas, string token = "")
        {
            try
            {
                return await this._consumer.PosponeAllInvoices(ofertas,token);
            }
            catch (Exception e)
            {
                return new List<Publications>() {
                    new Publications()
                    {
                        Error = e
                    }
                };
            }
        }

        public async Task<List<Publications>> PublicationsAllInvoices(List<UpdateInvoice> ofertas, string token = "")
        {
            try
            {
                return await this._consumer.PublicationsAllInvoices(ofertas,token);
            }
            catch (Exception e)
            {
                return new List<Publications>() {
                    new Publications()
                    {
                        Error = e
                    }
                };
            }
        }

        public async Task<List<Publications>> AceptOffertAll(List<UpdateInvoice> ofertas, string token)
        {
            try
            {
                return await this._consumer.AceptOffertAll(ofertas,token);
            }
            catch (Exception e)
            {
                return new List<Publications>() {
                    new Publications()
                    {
                        Error = e
                    }
                };
            }
        }

        #endregion

    }
}
