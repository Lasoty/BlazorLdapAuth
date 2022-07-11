using Blazored.SessionStorage;
using BlazorLdapAuth.Client.Extensions;
using BlazorLdapAuth.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorLdapAuth.Client.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService sessionStorage;
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        const string SESSIONNAME_KEY = "UserSession";

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorage)
        {
            this.sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await sessionStorage.ReadEncryptedItemAsync<UserSession>(SESSIONNAME_KEY);
                if (userSession == null)
                    return new AuthenticationState(anonymous);

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> 
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role),
                }, "JwtAuth"));

                return new AuthenticationState(claimsPrincipal);
            }
            catch 
            {
                return new AuthenticationState(anonymous);
            }
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role),
                }));
                userSession.ExpiryTimeStamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
                await sessionStorage.SaveItemEncryptedAsync(SESSIONNAME_KEY, userSession);
            }
            else
            {
                claimsPrincipal = anonymous;
                await sessionStorage.RemoveItemAsync(SESSIONNAME_KEY);
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<string> GetToken()
        {
            var result = string.Empty;

            try
            {
                var userSession = await sessionStorage.ReadEncryptedItemAsync<UserSession>(SESSIONNAME_KEY);
                if (userSession != null && DateTime.Now < userSession.ExpiryTimeStamp)
                    result = userSession.Token;
            }
            catch
            {
            }

            return result;
        }
    }
}
