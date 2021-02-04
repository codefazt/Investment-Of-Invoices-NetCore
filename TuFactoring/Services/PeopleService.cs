using GraphQL.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringGraphql;
using TuFactoringModels;
using TuFactoring.CustomProviders;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly PeopleConsumer _consumer;
        private readonly IConfiguration _configuration;

        public PeopleService(IConfiguration configuration)
        {
            _configuration = configuration;
            _consumer = new PeopleConsumer(new GraphQLClient(_configuration["PeopleEndpoint"]));

        }

        public async Task<Dashboard> GetDashboard(string topic, int currency, string token)
        {
            try { return await _consumer.GetDashboard(topic, currency, token); }
            catch { return null; }
        }

        public async Task<User> Entity(string person)
        {
            try
            {
                return await _consumer.Entity(person);
            }
            catch
            {
                return null;
            }
        }
    
        public async Task<List<People>> GetSuppliers(string idPais, string idCliente, string token = "")
        {
            List<People> data = new List<People>();

            try
            {
                data = await _consumer.GetSuppliers(idPais, idCliente, token);
            }
            catch (Exception)
            {
                return data;
            }

            return data;
        }

        public async Task<List<People>> GetDebtors(string idCliente, string token = "")
        {
            List<People> data = new List<People>();

            try
            {
                data = await _consumer.GetDebtors(idCliente, token);
            }
            catch (Exception)
            {
                return data;
            }

            return data;
        }

        //Filtrado Consultas
        public async Task<List<ResponseProveedores>> GetDeptors(string idPersona, string token = "")
        {
            try
            {
                return await _consumer.GetDeptors(idPersona, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ResponseProveedores>> GetDeptorsForConfirmant(string idPersona, string token = "")
        {
            try
            {
                return await _consumer.GetDeptorsForConfirmant(idPersona, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ResponseProveedores>> GetSupplierForConfirmant(string idPersona, string token = "")
        {
            try
            {
                return await _consumer.GetSupplierForConfirmant(idPersona, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ResponseProveedores>> GetSupplierForFactor(string idPersona, string token = "")
        {
            try
            {
                return await _consumer.GetSupplierForFactor(idPersona, token);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Prospecto>> IsFintech(int country, string token = "")
        {
            try
            {
                return await _consumer.IsFintech(country, token);
            }
            catch
            {
                return null;
            }
        }


        public async Task<List<AccountRespond>> GetConsultaAccountsPeople(ParamProspecto person, string token)
        {
            try
            {
                return await _consumer.GetConsultaAccountsPeople(person, token);
            }
            catch
            {
                return null;
            }
        }
        //-------------------- Nuevas Consultas --------------------------------------------------
        #region Registros

        public async Task<string> RegisterDebtorTF(Persons registro)
        {
            try { return await _consumer.RegisterDebtorTF(registro); }
            catch { return null; }
        }

        public async Task<string> RegisterSupplierTF(Persons registro)
        {
            try { return await _consumer.RegisterSupplierTF(registro); }
            catch { return null; }
        }

        public async Task<string> RegisterFactorTF(Persons registro)
        {
            try { return await _consumer.RegisterFactorTF(registro); }
            catch { return null; }
        }

        public async Task<string> RegisterConfirmantTF(Persons registro, string token)
        {
            try { return await _consumer.RegisterConfirmantTF(registro, token); }
            catch { return null; }
        }
        public async Task<Prospecto> UpdateProfileTF(Profiles perfil, string token)
        {
            try { return await _consumer.UpdateProfileTF(perfil, token); }
            catch { return null; }
        }
        public async Task<Prospecto> UpdateAsociadoTF(Profiles perfil, string token)
        {
            try { return await _consumer.UpdateAsociadoTF(perfil, token); }
            catch { return null; }
        }
        #endregion

        #region Consultas Registros

        public async Task<Prospecto> RegisterById(ParamProspecto registro, string token)
        {
            try { return await _consumer.RegisterById(registro, token); }
            catch { return null; }
        }
        public async Task<Prospecto> RegisterById(ParamProspecto registro)
        {
            try { return await _consumer.RegisterById(registro); }
            catch { return null; }
        }
        public async Task<Prospecto> ConsultaAsociados(ParamProspecto registro)
        {
            try { return await _consumer.ConsultaAsociados(registro); }
            catch { return null; }
        }
        public async Task<ListDocuments> RegisterByDocument(ParamProspecto registro)
        {
            try { return await _consumer.RegisterByDocument(registro); }
            catch { return null; }
        }
        public async Task<Prospecto> ConsultaEmails(string email, int? country = null)
        {
            try { return await _consumer.ConsultaEmails(email, country); }
            catch { return null; }
        }
        public async Task<AccountRespond> ConsultaAccount(string entity, string accountNumber, string token = null)
        {
            try { return await _consumer.ConsultaAccount(entity, accountNumber, token); }
            catch { return null; }
        }
        #endregion

        #region Mutaciones de Invitacion rechazo Cancelar y Toggle Principaes Clientes y Proveedores
        public async Task<Guest> AcceptInvitation(ParamGuests guest, string token)
        {
            try { return await _consumer.AcceptInvitation(guest, token); }
            catch { return null; }
        }
        public async Task<Guest> CancelInvitation(ParamGuests guest, string token)
        {
            try { return await _consumer.CancelInvitation(guest, token); }
            catch { return null; }
        }
        public async Task<Guest> ToggleInvitation(ParamGuests guest, string token)
        {
            try { return await _consumer.ToggleInvitation(guest, token); }
            catch { return null; }
        }
        public async Task<Guest> RejectInvitation(ParamGuests guest, string token)
        {
            try { return await _consumer.RejectInvitation(guest, token); }
            catch { return null; }
        }
        public async Task<Guest> SendInvitation(ParamGuests guest, string token)
        {
            try { return await _consumer.SendInvitation(guest, token); }
            catch { return null; }
        }
        #endregion

        #region Contrato Marco y Termino y condiciones
        public async Task<Prospecto> ConsultaContratoAsync(ParamProspecto person, string token)
        {
            try
            {
                return await _consumer.ConsultaContratoAsync(person, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<Agreements> MutacionContratoAsync(AcceptanceAgreements contrato, string token)
        {
            try
            {
                return await _consumer.MutacionContratoAsync(contrato, token);
            }
            catch { return null; }
        }
        #endregion

        //Vista Verificar Datos
        #region ConsultaDatosParaVerificarAsync
        public async Task<Verification> ConsultaDatosParaVerificarAsync(string confirmant, filterInvoice filter, Pagination pagination, string token)
        {
            try
            {
                return await _consumer.ConsultaDatosParaVerificarAsync(confirmant, filter, pagination, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> MutacionDatosParaVerificarAsync(string id, ApproveVerification verification, string token)
        {
            try { return await _consumer.MutacionDatosParaVerificarAsync(id, verification, token); }
            catch { return null; }
        }
        #endregion

        #region Consulta DatosParaSegmentar y Mutacion 
        public async Task<Prospectos> ConsultaDatosParaSegmentarAsync(string confirmant, filterInvoice filter, Pagination pagination, string token)
        {
            try { return await _consumer.ConsultaDatosParaSegmentarAsync(confirmant, filter, pagination, token); }
            catch { return null; }
        }
        public async Task<string> MutacionSegmentacionAsync(string id, SegmentPerson segmentado, string token)
        {
            try { return await _consumer.MutacionSegmentacionAsync(id, segmentado, token); }
            catch { return null; }
        }
        #endregion

        #region Consulta y Mutacion de limite de Cuenta
        public async Task<Prospectos> ConsultaDatosEjecutivoCuentasAsync(ParamCreditLimit limiteCredito, string token)
        {
            try
            {
                return await _consumer.ConsultaDatosEjecutivoCuentasAsync(limiteCredito, token);
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> MutacionLimiteCuentaAsync(AllocateQuota limiteCredito, string token)
        {
            try
            {
                return await _consumer.MutacionLimiteCuentaAsync(limiteCredito, token);
            }
            catch { return null; }
        }
        #endregion

        //Vista Consulta Banco Cliente
        #region Consulta Banco Clientes
        public async Task<Prospectos> GetConsultaClientConfirmant(ParamClienteOFConfirmant listaClientes, string token)
        {
            try { return await _consumer.GetConsultaClientConfirmant(listaClientes, token); }
            catch { return null; }
        }
        public async Task<Prospectos> GetConsultaFinaciamientoConfirmant(ParamClienteOFConfirmant listaClientes, string token)
        {
            try { return await _consumer.GetConsultaFinaciamientoConfirmant(listaClientes, token); }
            catch { return null; }
        }
        public async Task<Prospecto> GetDetalleClientConfirmant(ParamProspecto person, string token)
        {
            try { return await _consumer.GetDetalleClientConfirmant(person, token); }
            catch { return null; }
        }

        public async Task<Prospecto> GetDetailQuotas(ParamProspecto person, string token)
        {
            try { return await _consumer.GetDetailQuotas(person, token); }
            catch { return null; }
        }
        #endregion

        //Vista Consulta User BackOffice
        public async Task<Prospectos> GetConsultaUsersBackOffice(ParamProspecto person, string token)
        {
            try { return await _consumer.GetConsultaUsersBackOffice(person, token); }
            catch { return null; }
        }
        public async Task<ClienteOfConfirmant> GetDetalleUsersBackOffice(string user, string token)
        {
            try { return await _consumer.GetDetalleUsersBackOffice(user, token); }
            catch { return null; }
        }

        //Vista Gestion de Bancos - BackOffice
        public async Task<Entity> MakeEntityAllied(Entity entity, int idCountry, string token)
        {
            try
            {
                return await _consumer.MakeEntityAllied(entity, idCountry, token);
            }
            catch { return null; }
        }
    }
}
