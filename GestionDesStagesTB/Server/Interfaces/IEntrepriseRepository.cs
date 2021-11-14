using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Interfaces
{
    public interface IEntrepriseRepository
    {
        
        Entreprise GetEntrepriseById(string Id);

        Entreprise AddEntreprise(Entreprise entreprise);

        Entreprise UpdateEntreprise(Entreprise entreprise);
    }
}
