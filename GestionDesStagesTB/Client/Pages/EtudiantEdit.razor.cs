using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Client.Services;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Pages
{
    public partial class EtudiantEdit
    {
        [Inject]
        public IEtudiantDataService EtudiantDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

        public Etudiant Etudiant { get; set; } = new Etudiant();

        [Inject]
        public IFichierDataService FichierDataService { get; set; }

        [Inject]
        public ILogger<EtudiantEdit> Logger { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public List<PieceJointe> PiecesJointes { get;set; }

        private List<File> files = new();
        private List<UploadResult> uploadResults = new();
        private int maxAllowedFiles = 3;

        protected override async Task OnInitializedAsync()
        {
            // Vérifier si la fiche de l'étudiant existe
            // Si l'étudiant n'existe pas une instance vide Etudiant est retournée par le data service
            Etudiant = (await EtudiantDataService.GetEtudiantById(await ObtenirClaim("sub")));
        }

        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(Etudiant.Id)) //new student
            {
                // Obtenir du tableau des revendications le Id de l'utilisateur en cours
                Etudiant.Id = new Guid(await ObtenirClaim("sub")).ToString();
                // Appel du service pour sauvegardeer le nouvel etudiant dans la base de données.
                await EtudiantDataService.AddEtudiant(Etudiant);
            }

            else
            {
                // Appel du service pour mettre à jour l'etudiant existant dans la base de données.
                await EtudiantDataService.UpdateEtudiant(Etudiant);
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
        #region DEBUT code pour la gestion du fichier à verser
        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            //shouldRender = false;
            long maxFileSize = 1024 * 1024 * 15;
            var upload = false;

            using var content = new MultipartFormDataContent();

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                if (uploadResults.SingleOrDefault(
                    f => f.FileName == file.Name) is null)
                {
                    try
                    {
                        var fileContent =
                            new StreamContent(file.OpenReadStream(maxFileSize));

                        fileContent.Headers.ContentType =
                            new MediaTypeHeaderValue(file.ContentType);

                        files.Add(new() { Name = file.Name });

                        content.Add(
                            content: fileContent,
                            name: "\"files\"",
                            fileName: file.Name);
                        upload = true;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogInformation(
                            "{FileName} not uploaded (Err: 6): {Message}",
                            file.Name, ex.Message);

                        uploadResults.Add(
                            new()
                            {
                                FileName = file.Name,
                                ErrorCode = 6,
                                Uploaded = false
                            });
                    }
                }
            }

            if (upload)
            {
                //HttpResponseMessage response = await HttpClient.PostAsync("api/Fichier", content);
                IList<UploadResult> response = await FichierDataService.Verser(content);
                try
                {
                    // Parcourir la liste des pièces jointes traitées par l'API
                    foreach (var file in response)
                    {
                        // Inscrire seulement les pièces jointes qui ont été versées avec succès
                        if (file.Uploaded)
                        {
                            // Appel du service pour ajouter la pièce jointe dans la table des pièces jointes
                            await EtudiantDataService.AddPieceJointe(new PieceJointe { Id = await ObtenirClaim("sub"), FileName = file.FileName, StoredFileName = file.StoredFileName, DateVersee = System.DateTime.Now });
                        }
                    }
                }
                catch (Exception ee)
                {

                }

            }
            // Appel du service pour obtenir la liste des pièces jointes
            PiecesJointes = (await EtudiantDataService.GetAllPiecesJointes(await ObtenirClaim("sub"))).ToList();
            //shouldRender = true;
        }
        // Source 
        //https://dev.to/j_sakamoto/implement-the-download-file-feature-on-a-blazor-webassembly-app-2f8p
        private async Task OnClickDownloadButton(string FileName)
        {
            // Please imagine the situation that the API is protected by
            // token-based authorization (non cookie-based authorization).
            //var bytes = await HttpClient.GetByteArrayAsync("api/Fichier/aaa.pdf");
            var bytes = await FichierDataService.Telecharger(FileName);
            await JsRuntime.InvokeVoidAsync(
                "downloadFromByteArray",
                new
                {
                    ByteArray = bytes,
                    FileName = FileName,
                    ContentType = "application/pdf"
                });
        }

        private class File
        {
            public string Name { get; set; }
        }
        #endregion  FIN code pour la gestion du fichier à verser
    }

}
