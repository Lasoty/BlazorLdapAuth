using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorLdapAuth.Shared
{
    public class UserSession
    {
        public string UserName { get; set; }

        public string Token { get; set; }

        public string Role { get; set; }

        public int ExpiresIn { get; set; }

        public DateTime ExpiryTimeStamp { get; set; }
    }
}
