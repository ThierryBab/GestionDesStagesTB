using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Services
{
    public class StageStatutDataService : IStageStatutDataService
    {
        private readonly HttpClient _httpClient;

        public StageStatutDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<StageStatut>> GetAllStageStatuts()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<StageStatut>>
                (await _httpClient.GetStreamAsync($"api/stagestatut"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
 