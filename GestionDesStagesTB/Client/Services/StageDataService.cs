using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
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

        public StageDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<IEnumerable<Stage>> GetAllStages()
        {
            try
            {
                return await JsonSerializer.DeserializeAsync<IEnumerable<Stage>>
                    (await _httpClient.GetStreamAsync("api/stage"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                // Logging ici...
            }
            return null;
        }
    }
}
