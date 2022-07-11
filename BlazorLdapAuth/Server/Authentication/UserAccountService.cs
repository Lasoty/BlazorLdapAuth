using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorLdapAuth.Server.Authentication
{
    public class UserAccountService
    {
        private List<UserAccount> userAccounts;

        public UserAccountService()
        {
            userAccounts = new List<UserAccount>()
            {
                new UserAccount{ UserName = "admin", Password = "admin", Role = "Administrator"},
                new UserAccount{ UserName = "user", Password = "user", Role = "User"},
            };
        }

        public UserAccount GetUserAccountByUserName(string userName)
        {
            return userAccounts.FirstOrDefault(x => x.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
