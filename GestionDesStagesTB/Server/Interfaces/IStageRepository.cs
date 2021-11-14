using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Interfaces
{
    public interface IStageRepository
    {

        Stage AddStage(Stage stage);

        IEnumerable<Stage> GetAllStages();

        IEnumerable<Stage> GetAllStagesById(string id);

        Stage GetStageByStageId(string StageId);

        void DeleteStage(Guid StageId);

        Stage UpdateStage(Stage stage);

        PostulerStage PostulerStage(PostulerStage postulerStage);
    }
}
