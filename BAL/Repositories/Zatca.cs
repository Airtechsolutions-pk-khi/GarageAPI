using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositories
{
    class Zatca
    {
        public string ZatcaInvoiceQR(string sellername, string taxno, string vat, string date, string Grandtotal)
        {
            //declaration for invoice data
            //var sellername = "Garage";
            //var taxno = "012345678987";
            //var date = "2021-11-28 02:27 PM";
            //var Grandtotal = "230";
            //var vat = "30";

            //generate 5 TLVs Bytes Array and Concatinate All TLVs

            List<byte> tlvArray = new List<byte>();
            tlvArray.AddRange(CombineTwoArrays(1, Encoding.UTF8.GetBytes(sellername)));
            tlvArray.AddRange(CombineTwoArrays(2, Encoding.UTF8.GetBytes(taxno)));
            tlvArray.AddRange(CombineTwoArrays(3, Encoding.UTF8.GetBytes(date)));
            tlvArray.AddRange(CombineTwoArrays(4, Encoding.UTF8.GetBytes(Grandtotal)));
            tlvArray.AddRange(CombineTwoArrays(5, Encoding.UTF8.GetBytes(vat)));
            return Convert.ToBase64String(tlvArray.ToArray());
        }


        //General funtion for Concatinate 2 byte arrays
        private static byte[] CombineTwoArrays(int id, byte[] Value)
        {
            byte[] val = new byte[2 + Value.Length];
            val[0] = (byte)id;
            val[1] = (byte)Value.Length;
            Value.CopyTo(val, 2);
            return val;
        }
    }
}
