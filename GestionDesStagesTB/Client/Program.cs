using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;



namespace GestionDesStagesTB.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("GestionDesStagesTB.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GestionDesStagesTB.ServerAPI"));

            builder.Services.AddApiAuthorization();

            // Pour appliquer les policy
            builder.Services.AddAuthorizationCore(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(
                    GestionDesStagesTB.Shared.Policies.Policies.EstEtudiant,
                    GestionDesStagesTB.Shared.Policies.Policies.EstEtudiantPolicy());
                authorizationOptions.AddPolicy(
                    GestionDesStagesTB.Shared.Policies.Policies.EstEntreprise,
                    GestionDesStagesTB.Shared.Policies.Policies.EstEntreprisePolicy());
                authorizationOptions.AddPolicy(
                    GestionDesStagesTB.Shared.Policies.Policies.EstCoordonateur,
                    GestionDesStagesTB.Shared.Policies.Policies.EstCoordonateurPolicy());
            });

            //TODO: remplacer le port par une variable qui auto adapte

            //builder.Services.AddHttpClient<IStageDataService, StageDataService>(client => client.BaseAddress = new Uri("https://localhost:44359/"));
            //builder.Services.AddHttpClient<IStageStatutDataService, StageStatutDataService>(client => client.BaseAddress = new Uri("https://localhost:44359/"));
            //builder.Services.AddHttpClient<IEtudiantDataService, EtudiantDataService>(client => client.BaseAddress = new Uri("https://localhost:44359/"));
            //builder.Services.AddHttpClient<IEntrepriseDataService, EntrepriseDataService>(client => client.BaseAddress = new Uri("https://localhost:44359/"));

            builder.Services.AddScoped<IStageDataService, StageDataService>();
            builder.Services.AddScoped<IStageStatutDataService, StageStatutDataService>();
            builder.Services.AddScoped<IEtudiantDataService, EtudiantDataService>();
            builder.Services.AddScoped<IEntrepriseDataService, EntrepriseDataService>();

            await builder.Build().RunAsync();
        }
    }
}
