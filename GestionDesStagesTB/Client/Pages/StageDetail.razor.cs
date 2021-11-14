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
    public partial class StageDetail
    {
        [Inject]
        public IStageDataService StageDataService { get; set; }

        [Inject]
        public IEtudiantDataService EtudiantDataService { get; set; }

        [Inject]
        public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public Stage Stage { get; set; } = new Stage();

        [Parameter]
        public string StageId { get; set; }

        // Pour vérifier si le profil de l'étudiant existe afin d'afficher ou non le bouton de soumission au stage
        public Etudiant EtudiantExiste { get; set; } = new Etudiant();

        //[Parameter]
        //public string id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Tenter d'obtenir les informations du profil de l'étudiant actuellement connecté
            EtudiantExiste = await EtudiantDataService.GetEtudiantById(await ObtenirClaim("sub"));

            Stage = (await StageDataService.GetStageByStageId(StageId));
        }

        protected async Task Postuler()
        {
            // Appel du service pour inscrire l'étudiant comme candidat à l'offre de stage dans la base de données.
            // Passer en arguments le numéro du stage et le numéro de l'utilisateur actuellement connecté
            PostulerStage postulerStage = await StageDataService.PostulerStage(new PostulerStage { StageId = Stage.StageId, Id = await ObtenirClaim("sub") });
            // Vérifier le résultat
            if (postulerStage == null)
            {
                NavigationManager.NavigateTo("/ErreurConfirmationPostulerStage");
            }
            else
            {
                NavigationManager.NavigateTo("/ConfirmationPostulerStage");
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
            return user.FindFirst(c => c.Type == ClaimName)?.Value; 
        }
    }

}
