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
    }

    public interface ILdapService
    {
        bool ValidateUser(string userName, string password);
    }
}
