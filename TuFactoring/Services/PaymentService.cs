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
    public class PaymentService : IPaymentService
    {
        private readonly PaymentConsumer _consumer;
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            _consumer = new PaymentConsumer(new GraphQLClient(_configuration["PaymentEndpoint"]));

        }

        //Vistas Pago de Facturas
        public async Task<AlliedAccount> ConsultaBanksAsociadosAsync(string idPais)
        {
            try
            {
                return await _consumer.ConsultaBanksAsociadosAsync(idPais);
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<Receipts>> ConsultaFacturasAsync(string id, string country,string abbreviation, string state, string date, string token = "")
        {
            try
            {
                return await _consumer.ConsultaFacturasAsync(id, country,abbreviation, state, date, token);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Receipts>> ConsultaFacturasAsyncFactor(string id, string country, string abbreviation, string state, string token = "")
        {
            try
            {
                return await _consumer.ConsultaFacturasAsyncFactor(id, country, abbreviation, state, token);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Receipts> MutacionPagoFacturasAsync(Payment pago, string token = "")
        {
            try
            {
                return await _consumer.MutacionPagoFacturasAsync(pago, token);
            }
            catch
            {
                return null;
            }
        }

        //Vistas Aplicacion de Pagos
        public async Task<List<Receipts>> GetAplicationPays(string country, string idEntity, string token = "", string abbreviation = "")
        {
            try
            {
                return await _consumer.GetAplicationPays(country, idEntity,token,abbreviation);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Receipts> MutationPayReceipt(Payment pago, string token = "")
        {
            try
            {
                return await _consumer.MutationPayReceipt(pago,token);
            }
            catch
            {
                return null;
            }
        }



        //Vistas Operativo
        public async Task<ListPayments> ConsultaConciliacionsAsync(ParamPayments conciliar, string token)
        {
            try
            {
                return await _consumer.ConsultaConciliacionsAsync(conciliar, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<PaymentsNotConciliated> ConsultaMovementsAsync(ParamConciliarMovements param, string token)
        {
            try
            {
                return await _consumer.ConsultaMovementsAsync(param, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> GuardarArchivoAsync(ParamConciliarArchivo data, string token)
        {
            try
            {
                return await _consumer.GuardarArchivoAsync(data, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<TuFactoringModels.nuevaVersion.Payments> ConciliarUsuarioAsync(string conciliar, string movement, string token)
        {
            try
            {
                return await _consumer.ConciliarUsuarioAsync(conciliar, movement, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<TuFactoringModels.nuevaVersion.Payments> BloquearUsuarioAsync(string conciliar, string token)
        {
            try
            {
                return await _consumer.BloquearUsuarioAsync(conciliar, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<TuFactoringModels.nuevaVersion.Payments> MovementAsync(string movement, string token)
        {
            try
            {
                return await _consumer.MovementAsync(movement, token);
            }
            catch
            {
                return null;
            }
        }
        //Payment Query for Backoffice
        public async Task<List<Receipts>> ReceiptsQueryBackoffice(string state,string country, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await _consumer.ReceiptsQueryBackoffice(state,country, filter, pagination, token);
            }
            catch
            {
                return null;
            }
        }

        //Payment Query for Confirmant
        public async Task<List<Receipts>> ReceiptsQueryConfirmant(ParamConsultaPeyments param, filterInvoice filter = null, Pagination pagination = null, string token = "")
        {
            try
            {
                return await _consumer.ReceiptsQueryConfirmant(param, filter, pagination, token);
            }
            catch
            {
                return null;
            }
        }
    }
}
