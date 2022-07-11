using BlazorLdapAuth.Client.Authentication;
using BlazorLdapAuth.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorLdapAuth.Client.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        LoginRequest request = new LoginRequest();

        public async Task OnAuthenticate()
        {
            var loginResponse = await HttpClient.PostAsJsonAsync("/api/Account/Login", request);

            if (loginResponse.IsSuccessStatusCode)
            {
                var userSession = await loginResponse.Content.ReadFromJsonAsync<UserSession>();
                await ((CustomAuthenticationStateProvider)AuthStateProvider).UpdateAuthenticationState(userSession);
                NavigationManager.NavigateTo("/", true);
            }
            else if (loginResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                await JsRuntime.InvokeVoidAsync("alert", "Błędna nazwa użytkownika lub hasło.");
            }
        }
    }
}
