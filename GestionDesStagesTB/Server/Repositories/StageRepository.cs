using GestionDesStagesTB.Server.Data;
using GestionDesStagesTB.Server.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Repositories
{
    public class StageRepository : IStageRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public StageRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Stage AddStage(Stage stage)
        {
            var addedEntity = _appDbContext.Stage.Add(stage);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public IEnumerable<Stage> GetAllStages()
        {
            // Obtenir TOUS (n'importe quelle entreprise) les stages actifs
            //Mode decroissant pour show la liste des stages
            return _appDbContext.Stage.Where(c => c.StageStatutId == 1).Include(c => c.StageStatut).OrderByDescending(t => t.DateCreation);
        }


    }
}
