using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Services
{
    public interface IPeopleService
    {
        Task<Dashboard> GetDashboard(string topic, int currency, string token);

        Task<List<People>> GetSuppliers(string idPais, string idCliente, string token = "");
        //Venta Directa Proveedor
        Task<List<People>> GetDebtors(string idCliente, string token = "");

        //Filtros Invoices
        Task<List<ResponseProveedores>> GetDeptors(string idPersona, string token = "");
        Task<List<ResponseProveedores>> GetDeptorsForConfirmant(string idPersona, string token = "");
        Task<List<ResponseProveedores>> GetSupplierForConfirmant(string idPersona, string token = "");
        Task<List<ResponseProveedores>> GetSupplierForFactor(string idPersona, string token = "");
        Task<List<Prospecto>> IsFintech(int country, string token = "");



        //-------------------- Nuevas Consultas --------------------------------------------------
        #region Registros
        Task<string> RegisterDebtorTF(Persons registro);
        Task<string> RegisterSupplierTF(Persons registro);
        Task<string> RegisterFactorTF(Persons registro);
        Task<string> RegisterConfirmantTF(Persons registro, string token);
        Task<Prospecto> UpdateProfileTF(Profiles perfil, string token);
        Task<Prospecto> UpdateAsociadoTF(Profiles perfil, string token);
        #endregion

        #region Consultas Registros
        Task<Prospecto> RegisterById(ParamProspecto registro, string token);
        Task<Prospecto> RegisterById(ParamProspecto registro);
        Task<Prospecto> ConsultaAsociados(ParamProspecto registro);
        Task<ListDocuments> RegisterByDocument(ParamProspecto registro);
        Task<Prospecto> ConsultaEmails(string email, int? country = null);
        Task<AccountRespond> ConsultaAccount(string entity, string accountNumber, string token = null);
        #endregion

        #region Mutaciones de Invitacion rechazo Cancelar y Toggle Principaes Clientes y Proveedores
        Task<Guest> AcceptInvitation(ParamGuests guest, string token);
        Task<Guest> CancelInvitation(ParamGuests guest, string token);
        Task<Guest> ToggleInvitation(ParamGuests guest, string token);
        Task<Guest> RejectInvitation(ParamGuests guest, string token);
        Task<Guest> SendInvitation(ParamGuests guest, string token);
        #endregion

        #region Contrato Marco y Termino y condiciones
        Task<Prospecto> ConsultaContratoAsync(ParamProspecto person, string token);
        Task<Agreements> MutacionContratoAsync(AcceptanceAgreements contrato, string token);
        #endregion
        //Vista Verificar Datos
        #region ConsultaDatosParaVerificarAsync
        Task<Verification> ConsultaDatosParaVerificarAsync(string confirmant, filterInvoice filter, Pagination pagination, string token);
        Task<string> MutacionDatosParaVerificarAsync(string id, ApproveVerification verification, string token);

        #endregion

        #region ConsultaDatosParaSegmentarAsync
        Task<Prospectos> ConsultaDatosParaSegmentarAsync(string confirmant, filterInvoice filter, Pagination pagination, string token);
        Task<string> MutacionSegmentacionAsync(string id, SegmentPerson segmentado, string token);
        #endregion

        //Vista Limite de Cuentas
        #region Consulta de EjecutivoCuentas y Mutacion
        Task<Prospectos> ConsultaDatosEjecutivoCuentasAsync(ParamCreditLimit limiteCredito, string token);
        Task<string> MutacionLimiteCuentaAsync(AllocateQuota limiteCredito, string token);
        #endregion

        //Vista Consulta Banco Cliente
        Task<Prospectos> GetConsultaClientConfirmant(ParamClienteOFConfirmant listaClientes, string token);
        Task<Prospectos> GetConsultaFinaciamientoConfirmant(ParamClienteOFConfirmant listaClientes, string token);
        
        Task<Prospecto> GetDetalleClientConfirmant(ParamProspecto person, string token);
        Task<Prospecto> GetDetailQuotas(ParamProspecto person, string token);
        //Vista Gestion de Bancos BackOffice
        Task<Entity> MakeEntityAllied(Entity entity, int idCountry , string token);
        //Vista Consulta User BackOffice
        Task<Prospectos> GetConsultaUsersBackOffice(ParamProspecto person, string token);
        Task<ClienteOfConfirmant> GetDetalleUsersBackOffice(string user, string token);

        Task<List<AccountRespond>> GetConsultaAccountsPeople(ParamProspecto person, string token);

        #region Entity
        Task<User> Entity(string person);
        #endregion
    }

}
