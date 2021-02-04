using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Services
{
    public interface IPaymentService
    {

        //Vistas Pagar Facturas
        Task<AlliedAccount> ConsultaBanksAsociadosAsync(string idPais);
        Task<List<Receipts>> ConsultaFacturasAsync(string payer, string country,string abbreviation, string state, string date, string token = "" );
        Task<List<Receipts>> ConsultaFacturasAsyncFactor(string payer, string country, string abbreviation, string state, string token = "");
        Task<Receipts> MutacionPagoFacturasAsync(Payment pago, string token = "");

        #region AplicacionPagos
        Task<List<Receipts>> GetAplicationPays(string country, string idEntity, string token = "", string abbreviation = "");
        Task<Receipts> MutationPayReceipt(Payment pago,string token);
        #endregion
        //Vistas Operativo
        #region Conciliacion
        Task<ListPayments> ConsultaConciliacionsAsync(ParamPayments conciliar, string token);
        Task<string> GuardarArchivoAsync(ParamConciliarArchivo data, string token);
        Task<PaymentsNotConciliated> ConsultaMovementsAsync(ParamConciliarMovements param, string token);
        Task<TuFactoringModels.nuevaVersion.Payments> ConciliarUsuarioAsync(string conciliar, string movement, string token);
        Task<TuFactoringModels.nuevaVersion.Payments> BloquearUsuarioAsync(string conciliar, string token);
        Task<TuFactoringModels.nuevaVersion.Payments> MovementAsync(string movement, string token);
        #endregion

        #region PaymentQueryReceipts
        //Backoffice
        Task<List<Receipts>> ReceiptsQueryBackoffice(string state, string country, filterInvoice filter = null, Pagination pagination = null, string token = "");
        //Confirmant
        Task<List<Receipts>> ReceiptsQueryConfirmant(ParamConsultaPeyments param, filterInvoice filter = null, Pagination pagination = null, string token = "");
        #endregion
    }
}
