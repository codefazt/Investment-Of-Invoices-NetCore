using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.Interface;
using TuFactoringModels.Validation;

namespace TuFactoringModels
{
    public class Utils
    {
        public string Value { get; set; }
        public string ChangeFrom { get; set; }
        public string Program { get; set; }


        public static Utils getStatus(string param)
        {
            Utils resp = new Utils();

            resp.Value = param;
            resp.ChangeFrom = param;

            switch (param)
            {
                case "rejectedForClient":
                    resp.Value = "rejected";
                    break;
                case "confirmedToOverdue":
                    resp.Value = "confirmed,offered,released,published,sold,paid,processing,postponed,overdue";
                    break;
                case "confirmedForClient":
                    resp.Value = "confirmed";
                    break;
                case "soldToOverdue":
                    resp.Value = "paid,overdue";
                    break;
                case "soldToOverdueSupplier":
                    resp.Value = "paid,overdue";
                    break;
                case "offeredBank":
                    resp.Value = "offered";
                    break;
                case "soldBank":
                    resp.Value = "sold";
                    break;
                case "soldSupplier":
                    resp.Value = "sold";
                    break;
                case "finalizeBank":
                    resp.Value = "finalize";
                    break;
            }

            return resp;
        }

        public static bool filterIsEmpty(filterInvoice filter)
        {
            return new FilterEmptyValidation().Validate(filter).IsValid;
        }

        public static void setStatus(IStatus obj, string participant, string change = null)
        {
            
            if (!String.IsNullOrEmpty(change))
            {
                obj.StateMostrar = change;
                return;
            }
            
            switch (participant)
            {
                case "DEBTOR": obj.StateMostrar = Utils.setStatusToQueryDebtor(obj); 
                    break;
                case "SUPPLIER": obj.StateMostrar = Utils.setStatusToQuerySupplier(obj); 
                    break;
                case "CONFIRMANT": obj.StateMostrar = Utils.setStatusToQueryConfirmant(obj); 
                    break;
                case "FACTOR": obj.StateMostrar = Utils.setStatusToQueryFactor(obj.State); 
                    break;
                case "BACKOFFICE": obj.StateMostrar = Utils.setStatusToQueryBackoffice(obj.State); 
                    break;
                default: obj.StateMostrar = obj.State;
                    break;
            }

        }

        private static string setStatusToQueryDebtor(IStatus obj)
        {
            string resp;

            switch (obj.State)
            {
                case "draft":
                    resp = "draft";
                    break;
                case "posted":
                    resp = "posted";
                    break;
                case "rejected":
                    resp = "rejected";
                    break;
                case "confirmed":

                    if(obj.Participant == "SUPPLIER")
                    {
                        resp = "confirmedForClient";
                    }
                    else
                    {
                        resp = "confirmedToOverdue";
                    }
                        
                    break;
                case "offered":
                    resp = "confirmedToOverdue";
                    break;
                case "released":
                    resp = "confirmedToOverdue";
                    break;
                case "published":
                    resp = "confirmedToOverdue";
                    break;
                case "sold":
                    resp = "confirmedToOverdue";
                    break;
                case "paid":
                    resp = "confirmedToOverdue";
                    break;
                case "processing":
                    resp = "confirmedToOverdue";
                    break;
                case "postponed":
                    resp = "confirmedToOverdue";
                    break;
                case "overdue":
                    resp = "confirmedToOverdue";
                    break;
                case "finalize":
                    resp = "finalizeBank";
                    break;
                default:
                    resp = obj.State;
                    break;
            }


            return resp;
        }

        private static string setStatusToQuerySupplier(IStatus obj)
        {
            string resp;

            switch (obj.State)
            {
                case "posted":
                    resp = "posted";
                    break;
                case "rejected":
                    resp = "rejectedForClient";
                    break;
                case "confirmed":

                    if (obj.Participant == "SUPPLIER")
                    {
                        resp = "confirmedForClient";
                    }
                    else
                    {
                        resp = "confirmedToOverdue";
                    }

                    break;
                case "offered":
                    resp = "offeredBank";
                    break;
                case "released":
                    resp = "released";
                    break;
                case "published":
                    resp = "published";
                    break;
                case "sold":
                    resp = "soldSupplier";
                    break;
                case "paid":
                    resp = "soldToOverdueSupplier";
                    break;
                case "processing":
                    resp = "processing";
                    break;
                case "postponed":
                    resp = "confirmedToOverdue";
                    break;
                case "overdue":
                    resp = "confirmedToOverdue";
                    break;
                case "finalize":
                    resp = "finalizeBank";
                    break;
                default:
                    resp = obj.State;
                    break;
            }

            return resp;
        }

        private static string setStatusToQueryConfirmant(IStatus obj)
        {
            string resp;

            switch (obj.State)
            {
                case "posted":
                    resp = "posted";
                    break;
                case "rejected":
                    resp = "rejectedForClient";
                    break;
                case "confirmed":

                    if (obj.Participant == "SUPPLIER")
                    {
                        resp = "confirmedForClient";
                    }
                    else
                    {
                        resp = "confirmedToOverdue";
                    }
                    break;
                case "offered":
                    resp = "offeredBank";
                    break;
                case "released":
                    resp = "confirmedToOverdue";
                    break;
                case "published":
                    resp = "confirmedToOverdue";
                    break;
                case "sold":
                    resp = "soldBank";
                    break;
                case "paid":
                    resp = "soldToOverdue";
                    break;
                case "processing":
                    resp = "processing";
                    break;
                case "postponed":
                    resp = "confirmedToOverdue";
                    break;
                case "overdue":
                    resp = "confirmedToOverdue";
                    break;
                case "finalize":
                    resp = "finalizeBank";
                    break;
                default:
                    resp = obj.State;
                    break;
            }

            return resp;
        }

        private static string setStatusToQueryFactor(string status)
        {
            string resp;

            switch (status)
            {
                case "sold":
                    resp = "soldBank";
                    break;
                case "processing":
                    resp = "processing";
                    break;
                case "paid":
                    resp = "soldToOverdue";
                    break;
                case "postponed":
                    resp = "soldToOverdue";
                    break;
                case "overdue":
                    resp = "soldToOverdue";
                    break;
                case "finalize":
                    resp = "finalizeBank";
                    break;
                default:
                    resp = status;
                    break;
            }

            return resp;
        }

        private static string setStatusToQueryBackoffice(string status)
        {
            string resp;

            switch (status)
            {
                case "draft":
                    resp = "draft";
                    break;
                case "revised":
                    resp = "revised";
                    break;
                case "posted":
                    resp = "posted";
                    break;
                case "confirmed":
                    resp = "confirmedToOverdue";
                    break;
                case "offered":
                    resp = "confirmedToOverdue";
                    break;
                case "released":
                    resp = "released";
                    break;
                case "published":
                    resp = "published";
                    break;
                case "sold":
                    resp = "soldSupplier";
                    break;
                case "paid":
                    resp = "soldToOverdueSupplier";
                    break;
                case "processing":
                    resp = "processing";
                    break;
                case "postponed":
                    resp = "confirmedToOverdue";
                    break;
                case "overdue":
                    resp = "confirmedToOverdue";
                    break;
                case "finalize":
                    resp = "finalizeBank";
                    break;
                default:
                    resp = status;
                    break;
            }

            return resp;
        }


    }
}
