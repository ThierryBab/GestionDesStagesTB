using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GestionDesStagesTB.Shared.Models;
using System.Net.Http.Json;

namespace GestionDesStagesTB.Client.Services
{
    public class FichierDataService : IFichierDataService
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<FichierDataService> _logger;

        public FichierDataService(HttpClient httpClient, ILogger<FichierDataService> logger)
        {
            _httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<byte[]> Telecharger(string id)
        {
            try
            {
                return (await _httpClient.GetByteArrayAsync($"api/Fichier/{id}"));
                //MemoryStream stream = new MemoryStream(fichier);
                //return stream;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'obtention d'un fichier {ex}");
            }
            return null;
        }

        public async Task<IList<UploadResult>> Verser(MultipartFormDataContent content)
        {
            List<UploadResult> uploadResults = new List<UploadResult>();
            string error = string.Empty;

            try
            {
                var response = await _httpClient.PostAsync("api/Fichier", content);
                if (response.IsSuccessStatusCode)
                {
                    // Installer le package Microsoft.AspNetCore.Http.Extensions
                    var newUploadResults = await response.Content
                        .ReadFromJsonAsync<IList<UploadResult>>();

                    uploadResults = uploadResults.Concat(newUploadResults).ToList();
                    return uploadResults;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'envoi des fichiers {ex} {error}");
            }
            return null;
        }
    }
}
