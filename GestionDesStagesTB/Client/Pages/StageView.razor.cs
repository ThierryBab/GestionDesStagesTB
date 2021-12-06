using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
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

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public List<Stage> Stages { get; set; } = new List<Stage>();
        public ZoneRecherche Rechercher { get; set; } = new ZoneRecherche();

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

        protected async Task DeleteStage(Guid stageId)
        {
            bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Souhaitez-vous supprimer définitivement cette ligne?");
            if (confirmed)
            {
                await StageDataService.DeleteStage(stageId);
                // Appel du service pour obtenir la liste des stages d'une entreprise précise
                Stages = (await StageDataService.GetAllStages(await ObtenirClaim("sub"))).ToList();
            }
        }

        protected async Task SoumettreRecherche()
        {
            // S'assurer de la présence d'une valeur à chercher
            if (!string.IsNullOrEmpty(Rechercher.ValeurRecherchee))
            {
                // Appel du service pour obtenir la liste de TOUS les stages actifs et appliquer un filtre pour obtenir les stages selon un mot dans la description ou le titre du stage
                Stages = (await StageDataService.GetAllStages()).Where(e => e.Description.Contains(Rechercher.ValeurRecherchee.Trim()) || e.Titre.Contains(Rechercher.ValeurRecherchee.Trim())).ToList();
            }
            else
            {
                //Si aucune valeur afficher toutes les offres de stage sans condition
                await AnnulerRecherche();
            }
        }

        protected async Task AnnulerRecherche()
        {
            // Vider la zone de recherche
            Rechercher.ValeurRecherchee = string.Empty;
            // Appel du service pour obtenir la liste de TOUS les stages actifs (sans condition)
            Stages = (await StageDataService.GetAllStages()).ToList();
        }

    }
}


