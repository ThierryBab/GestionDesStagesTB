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
    public class EntrepriseDataService : IEntrepriseDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EntrepriseDataService> _logger;

        public EntrepriseDataService(HttpClient httpClient, ILogger<EntrepriseDataService> logger)
        {
            _httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<Entreprise> GetEntrepriseById(string Id)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Entreprise>
                    (await _httpClient.GetStreamAsync($"api/entreprise/{Id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'obtention de données d'un enregistrement {ex}");
            }
            return null;
        }

        public async Task<Entreprise> AddEntreprise(Entreprise entreprise)
        {
            var donneesJson =
                new StringContent(JsonSerializer.Serialize(entreprise), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/entreprise", donneesJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Entreprise>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateEntreprise(Entreprise entreprise)
        {
            var stageJson =
                new StringContent(JsonSerializer.Serialize(entreprise), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/entreprise", stageJson);
        }
    }
}
