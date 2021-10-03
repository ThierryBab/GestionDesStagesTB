using GestionDesStagesTB.Server.Data;
using GestionDesStagesTB.Server.Interfaces;
using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Repositories
{
    public class StageStatutRepository : IStageStatutRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public StageStatutRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<StageStatut> GetAllStageStatuts()
        {
            return _appDbContext.StageStatut;
        }
    }
}
