using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class common
    {
        //public static string MyConnection()
        //{
        //    return @"Server=BPDWEBSERVER\SQLEXPRESS;Database=IMS;Uid=sa;pwd=1;";
        //}
        public static string MyConnection()
        {
             //return @"Data Source=192.168.2.1\PGAS;initial catalog=IMS;Password=(@/51u0#2@3n8D0e1L1#0u1R;Persist Security Info=True;User ID=pgasIS;";
            //return @"Data Source=SMUGGLER-PC\SA;initial catalog=IMS;Password=12345;Persist Security Info=True;User ID=sa;";
            return @"Data Source=10.100.100.4;initial catalog=IMS;Password=pimo@123;Persist Security Info=True;User ID=sa;";

        }
        public static string livecon()
        {
            // return @"Data Source=192.168.2.1\PGAS;initial catalog=IMS;Password=(@/51u0#2@3n8D0e1L1#0u1R;Persist Security Info=True;User ID=pgasIS;";
          //   return @"Data Source=SMUGGLER-PC\SA;initial catalog=IMS;Password=12345;Persist Security Info=True;User ID=sa;";
            return @"Data Source=10.100.100.4;initial catalog=IMS;Password=pimo@123;Persist Security Info=True;User ID=sa;";

        }
    }
}