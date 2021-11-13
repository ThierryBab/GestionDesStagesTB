using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Services
{
    public class EtudiantDataService : IEtudiantDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EtudiantDataService> _logger;

        public EtudiantDataService(HttpClient httpClient, ILogger<EtudiantDataService> logger)
        {
            _httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<Etudiant> GetEtudiantById(string Id)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Etudiant>
                    (await _httpClient.GetStreamAsync($"api/etudiant/{Id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'obtention de données d'un enregistrement {ex}");
            }
            return null;
        }

        public async Task<Etudiant> AddEtudiant(Etudiant etudiant)
        {
            var donneesJson =
                new StringContent(JsonSerializer.Serialize(etudiant), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/etudiant", donneesJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Etudiant>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateEtudiant(Etudiant etudiant)
        {
            var stageJson =
                new StringContent(JsonSerializer.Serialize(etudiant), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/etudiant", stageJson);
        }
    }
}
