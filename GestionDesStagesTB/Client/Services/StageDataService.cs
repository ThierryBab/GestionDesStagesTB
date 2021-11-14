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
    public class StageDataService : IStageDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StageDataService> _logger;

        public StageDataService(HttpClient httpClient, ILogger<StageDataService> logger)
        {
            _httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<Stage> AddStage(Stage stage)
        {
            var donneesJson =
                new StringContent(JsonSerializer.Serialize(stage), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/stage", donneesJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Stage>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IEnumerable<Stage>> GetAllStages(string id = null)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return await JsonSerializer.DeserializeAsync<IEnumerable<Stage>>
                        (await _httpClient.GetStreamAsync("api/stage"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    return await JsonSerializer.DeserializeAsync<IEnumerable<Stage>>
                        (await _httpClient.GetStreamAsync($"api/stage/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'obtention de données {ex}");
            }
            return null;
        }

        public async Task DeleteStage(Guid StageId)
        {
            await _httpClient.DeleteAsync($"api/stage/{StageId}");
        }

        public async Task<Stage> GetStageByStageId(string StageId)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<Stage>
                    (await _httpClient.GetStreamAsync($"api/stage/GetStageByStageId/{StageId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'obtention de données d'un enregistrement {ex}");
            }
            return null;
        }

        public async Task UpdateStage(Stage stage)
        {
            var stageJson =
                new StringContent(JsonSerializer.Serialize(stage), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/stage", stageJson);
        }

       

        public async Task<PostulerStage> PostulerStage(PostulerStage postulerStage)
        {
            var donneesJson =
                new StringContent(JsonSerializer.Serialize(postulerStage), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/stage/PostulerStage", donneesJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<PostulerStage>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IEnumerable<PostulerStage>> GetCandidaturesStageByStageId(string StageId)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<IEnumerable<PostulerStage>>
                (await _httpClient.GetStreamAsync($"api/stage/GetCandidaturesStageByStageId/{StageId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans l'obtention de données {ex}");
            }
            return null;
        }

    }
}
