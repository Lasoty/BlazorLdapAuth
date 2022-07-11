using Novell.Directory.Ldap;
using System;

namespace BlazorLdapAuth.Server.Authentication
{
    public class LdapService : ILdapService
    {
        public LdapService()
        {

        }

        public bool ValidateUser(string userName, string password)
        {
            string hostName = "192.168.0.112";
            string userDn = $"cn={userName},dc=example,dc=com";
            //string userDn = $"uid={userName},CN=USERS,O=PZU";

            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(hostName, LdapConnection.DefaultPort);
                    connection.Bind(userDn, password);
                    if (connection.Bound)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        public bool ValidateUser(string userName, string password, out string role)
        {
            role = string.Empty;
            string hostName = "192.168.0.113";
            string userDn = $"cn={userName},dc=example,dc=com";
            //string userDn = $"uid={userName},CN=USERS,O=PZU";

            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(hostName, LdapConnection.DefaultPort);
                    connection.Bind(userDn, password);
                    if (connection.Bound)
                    {
                        role = GetUserGroup(connection);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        private string GetUserGroup(LdapConnection connection)
        {
            string result = string.Empty;
            string searchFilter = "(objectclass=*)";
            string searchBase = "DC=example,DC=com";

            LdapSearchConstraints constrains = new LdapSearchConstraints
            {
                TimeLimit = 15000
            };

            var searchResult = connection.Search
                (
                    searchBase,
                    LdapConnection.ScopeSub,
                    searchFilter,
                    null,
                    false,
                    constrains
                );

            int count = 0;
            while (searchResult.HasMore())
            {
                var nextEntry = searchResult.Next();
                var attrSet = nextEntry.GetAttributeSet();
                if (attrSet.ContainsKey("description"))
                {
                    result = attrSet["description"].StringValue;
                    break;
                }
            }

            return result;
        }
    }


    public interface ILdapService
    {
        bool ValidateUser(string userName, string password);
        bool ValidateUser(string userName, string password, out string role);
    }
}
