using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_agency
{
    public class Constants
    {
        public static readonly string dbConfig = @"Data Source=DESKTOP-GJS201Q\SQLEXPRESS;Initial Catalog=ТурАгентство;Integrated Security=True";

        public struct AccountId
        {
            public enum VisitorType
            {
                Error,
                Manager,
                Client
            }

            public short id { get; set; }
            public VisitorType visitorType { get; set; }
        }

    }
}
