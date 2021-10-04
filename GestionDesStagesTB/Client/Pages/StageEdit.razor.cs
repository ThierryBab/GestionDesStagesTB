using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Pages
{
    public partial class StageEdit
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

        [Inject]
        public IStageDataService StageDataService { get; set; }

        [Inject]
        public IStageStatutDataService StageStatutDataService { get; set; }


        public Stage Stage { get; set; } = new Stage();

        public List<StageStatut> StageStatut { get; set; } = new List<StageStatut>();

        protected override async Task OnInitializedAsync()
        {
            // Appel du service pour obtenir la liste des status de stage

            StageStatut = (await StageStatutDataService.GetAllStageStatuts()).ToList();

            // Proposer des valeurs par défaut pour un nouveau stage
            Stage = new Stage { StageStatutId = 1, Salaire = true, DateCreation = DateTime.Now };
        }

        protected async Task HandleValidSubmit()
        {
            if (Stage.StageId == Guid.Empty) //new
            {
                // Obtenir du tableau des revendications (CLAIMS en anglais) le Id de l'utilisateur en cours
                Stage.Id = await ObtenirClaim("sub");
                // Obtenir un nouveau GUID pour le nouveau stage
                Stage.StageId = Guid.NewGuid();
                // Appel du service pour sauvegarder le nouveau stage dans la base de données.
                await StageDataService.AddStage(Stage);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                // Appel du service pour mettre à jour le stage existant dans la base de données.
                // Retourner à l'accueil
                NavigationManager.NavigateTo("/");
            }
        }

        protected void HandleInvalidSubmit()
        {
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/");
        }


        /// <summary>
        /// Pour obtenir un claim : sid (Id de l'utilisateur actuel), sub, auth_time, idp, amr, role, preffered_username, name
        /// </summary>
        /// <param name="ClaimName"></param>
        /// <returns></returns>
        private async Task<string> ObtenirClaim(string ClaimName)
        {
            // Obtenir tous les revendications (Claims) de l'utilisateur actuellement connecté.            
            var authstate = await GetAuthenticationStateAsync.GetAuthenticationStateAsync();
            var user = authstate.User;
            IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();
            // Mettre les revendications dans un tableau
            _claims = user.Claims;
            // Obtenir du tableau des revendications le Id de l'utilisateur en cours
            return user.FindFirst(c => c.Type == ClaimName)?.Value; ;
        }
    }

}
