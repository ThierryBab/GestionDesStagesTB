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


            builder.Services.AddScoped<IStageDataService, StageDataService>();
            builder.Services.AddScoped<IStageStatutDataService, StageStatutDataService>();
            builder.Services.AddScoped<IEtudiantDataService, EtudiantDataService>(); 
            builder.Services.AddScoped<ICoordonateurDataService, CoordonateurDataService>(); 
            builder.Services.AddScoped<IEntrepriseDataService, EntrepriseDataService>();
            builder.Services.AddScoped<IFichierDataService, FichierDataService>(); 
            await builder.Build().RunAsync();
        }
    }
}
