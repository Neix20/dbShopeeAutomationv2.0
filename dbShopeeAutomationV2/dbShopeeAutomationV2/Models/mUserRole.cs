using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class mUserRole
    {
        public mUserRole(string value)
        {
            this.value = value;
        }

        public mUserRole()
        {

        }

        public string value { get; set; }
    }
}