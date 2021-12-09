using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Interfaces
{
    public interface ICoordonateurDataService
    {
        Task<Coordonateur> GetCoordonateurById(string Id);
        Task<Coordonateur> AddCoordonateur(Coordonateur coordonateur);
        Task UpdateCoordonateur(Coordonateur coordonateur);
    }
}
