using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Client.Services;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Pages
{
    public partial class PiecesJointes
    {
        [Inject]
        public IEtudiantDataService EtudiantDataService { get; set; }

        [Inject]
        public IFichierDataService FichierDataService { get; set; }

        [Inject]
        public ILogger<EtudiantEdit> Logger { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public string Id { get; set; }


        [Parameter]
        public bool VerserFichier { get; set; } = false;

        public List<PieceJointe> PiecesJtes { get; set; } = new List<PieceJointe>();

        private List<File> files = new();
        private List<UploadResult> uploadResults = new();
        private int maxAllowedFiles = 3;

        // Source  : https://docs.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-5.0&pivots=webassembly

        protected override async Task OnInitializedAsync()
        {
            // Appel du service pour obtenir la liste des pièces jointes
            PiecesJtes = (await EtudiantDataService.GetAllPiecesJointes(Id)).ToList();
        }

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
                // Verser les pièces jointes (fichiers physiques) côté serveur
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
                            await EtudiantDataService.AddPieceJointe(new PieceJointe { Id = Id, FileName = file.FileName, StoredFileName = file.StoredFileName, DateVersee = System.DateTime.Now });
                        }
                    }
                }
                catch (Exception ee)
                {

                }

            }
            // Appel du service pour obtenir la liste des pièces jointes
            PiecesJtes = (await EtudiantDataService.GetAllPiecesJointes(Id)).ToList();
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
    }
}
