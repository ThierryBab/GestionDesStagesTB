using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Interfaces
{
    public interface ICoordonateurRepository
    {
        Coordonateur GetCoordonateurById(string Id);
        Coordonateur AddCoordonateur(Coordonateur coordonateur);
        Coordonateur UpdateCoordonateur(Coordonateur coordonateur);
    }
}
