using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Interfaces
{
    public interface IStageDataService
    {
        Task<Stage> AddStage(Stage stage);

        Task<IEnumerable<Stage>> GetAllStages(string id = null);
        Task DeleteStage(Guid stageId);
        Task<Stage> GetStageByStageId(string stageId);
        Task UpdateStage(Stage stage);
        Task<PostulerStage> PostulerStage(PostulerStage postulerStage);
    }

}
