using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    class ValidationResources
    {
        public static bool validateTimeSuperiorString(string dateString)
        {
            DateTime date;

            try
            {
                date = DateTime.Parse(dateString);
            }
            catch (Exception)
            {
                return false;
            }


            if (date < DateTime.Now.Date) return false;

            return true;
        }

        public static bool validateTimeInferiorString(string dateString)
        {
            DateTime date;

            try
            {
                date = DateTime.Parse(dateString);
            }
            catch (Exception)
            {
                return false;
            }


            if (date > DateTime.Now.Date) return false;

            return true;
        }

        public static bool validateCharges(Invoices factura)
        {
            double suma = 0;
            if (factura.Charges == null || factura.Charges.Count == 0) return true;

            foreach (var item in factura.Charges)
            {
                if (item.Amount.Value <= 0) return false;

                suma += item.Amount.Value;

                if (suma >= factura.Original_amount) return false;

            }

            return true;
        }

        public static bool validateImg(string imgByte64)
        {

            if (String.IsNullOrEmpty(imgByte64)) return true;

            if (!imgByte64.Contains("data:image/jpeg;base64,/9j/") && !imgByte64.Contains("data:image/png;base64,iVBOR")) return false;

            return true;
        }
    }
}
