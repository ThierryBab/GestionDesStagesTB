using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Pages
{
    public partial class CoordonateurEdit
    {
        [Inject]
        public ICoordonateurDataService CoordonateurDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

        public Coordonateur Coordonateur { get; set; } = new Coordonateur();

        //[Inject]
        //public IFichierDataService FichierDataService { get; set; }

        [Inject]
        public ILogger<CoordonateurEdit> Logger { get; set; }

        //private List<File> files = new();
        //private List<UploadResult> uploadResults = new();
        //private int maxAllowedFiles = 3;

        protected override async Task OnInitializedAsync()
        {
            // Vérifier si la fiche de l'étudiant existe
            // Si l'étudiant n'existe pas une instance vide Etudiant est retournée par le data service
            Coordonateur = await CoordonateurDataService.GetCoordonateurById(await ObtenirClaim("sub"));
        }

        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(Coordonateur.Id)) //new student
            {
                // Obtenir du tableau des revendications le Id de l'utilisateur en cours
                Coordonateur.Id = new Guid(await ObtenirClaim("sub")).ToString();
                // Appel du service pour sauvegardeer le nouvel etudiant dans la base de données.
                await CoordonateurDataService.AddCoordonateur(Coordonateur);
            }

            else
            {
                // Appel du service pour mettre à jour l'etudiant existant dans la base de données.
                await CoordonateurDataService.UpdateCoordonateur(Coordonateur);
            }
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

        protected void HandleInvalidSubmit()
        {
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
