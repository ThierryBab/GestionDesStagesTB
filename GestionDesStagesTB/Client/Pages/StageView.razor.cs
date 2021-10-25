using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Pages
{
    public partial class StageView
    {
        [Inject]
        public IStageDataService StageDataService { get; set; }

        [Inject]
        public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

        public List<Stage> Stages { get; set; } = new List<Stage>();

        protected override async Task OnInitializedAsync()
        {
            // Vérifier le rôle de l'utilisateur actuellement authentifié
            if (await ObtenirClaim("role") == "Etudiant")
            {
                // Appel du service pour obtenir la liste de TOUS les stages actifs
                Stages = (await StageDataService.GetAllStages()).ToList();
            }
            else
            {
                // Appel du service pour obtenir la liste des stages d'une entreprise précise
                Stages = (await StageDataService.GetAllStages(await ObtenirClaim("sub"))).ToList();
            }
        }

        private async Task<string> ObtenirClaim(string ClaimName)
        {
            // Obtenir tous les revendications (Claims) de l'utilisateur actuellement connecté.            
            var authstate = await GetAuthenticationStateAsync.GetAuthenticationStateAsync();
            var user = authstate.User;
            IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();
            // Mettre les revendications dans un tableau
            _claims = user.Claims;
            // Obtenir du tableau des revendications le claim demandé pour l'utilisateur en cours
            // Attention s'il y a plusieurs claims du  même type (comme rôle) seul le premier est retourné FindFirst
            return user.FindFirst(c => c.Type == ClaimName)?.Value; ;
        }
    }
}


