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

        [Parameter]
        public string StageId { get; set; }

        public Stage Stage { get; set; } = new Stage();
        public string LibelleBoutonEnregistrer { get; set; }
        public List<StageStatut> StageStatut { get; set; } = new List<StageStatut>();

        protected override async Task OnInitializedAsync()
        {
            // Appel du service pour obtenir la liste des status de stage

            StageStatut = (await StageStatutDataService.GetAllStageStatuts()).ToList();

            if (string.IsNullOrEmpty(StageId))
            {
                //add some defaults
                //Stage = new Stage { StageId = Guid.NewGuid(), DateCreation = DateTime.Now };
                Stage = new Stage { StageStatutId = 1, Salaire = false, DateCreation = DateTime.Now };
                LibelleBoutonEnregistrer = "Ajouter ce nouveau stage";
            }
            else
            {

                Stage = (await StageDataService.GetStageByStageId(StageId));
                LibelleBoutonEnregistrer = "Mettre à jour les informations du stage";
            }

            if (string.IsNullOrEmpty(StageId))
            {
                // Proposer des valeurs par défaut pour un nouveau stage
                Stage = new Stage { StageStatutId = 1, Salaire = true, DateCreation = DateTime.Now };
                LibelleBoutonEnregistrer = "Ajouter ce nouveau stage";
            }
            else
            {
                Stage = (await StageDataService.GetStageByStageId(StageId));
                LibelleBoutonEnregistrer = "Mettre à jour les informations du stage";
            }
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
                await StageDataService.UpdateStage(Stage);
                NavigationManager.NavigateTo("/stageview");
            }
        }

        protected void HandleInvalidSubmit()
        {
        }

        protected void NavigateToOverview()
        {
            if (Stage.StageId == Guid.Empty) //new
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                NavigationManager.NavigateTo("/stageview");
            }
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
