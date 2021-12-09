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
    public class CoordonateurDataService : ICoordonateurDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CoordonateurDataService> _logger;

        public CoordonateurDataService(HttpClient httpClient, ILogger<CoordonateurDataService> logger)
        {
            _httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<Coordonateur> GetCoordonateurById(string Id)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Coordonateur>
                    (await _httpClient.GetStreamAsync($"api/coordonateur/{Id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'obtention de données d'un enregistrement {ex}");
            }
            return null;
        }

        public async Task<Coordonateur> AddCoordonateur(Coordonateur coordonateur)
        {
            var donneesJson =
                new StringContent(JsonSerializer.Serialize(coordonateur), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/coordonateur", donneesJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Coordonateur>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateCoordonateur(Coordonateur coordonateur)
        {
            var stageJson =
                new StringContent(JsonSerializer.Serialize(coordonateur), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/coordonateur", stageJson);
        }
    }
}
