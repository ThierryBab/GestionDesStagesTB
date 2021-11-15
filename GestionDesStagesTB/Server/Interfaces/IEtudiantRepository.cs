using GestionDesStagesTB.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Interfaces
{
    public interface IEtudiantRepository
    {
        Etudiant GetEtudiantById(string Id);

        Etudiant AddEtudiant(Etudiant etudiant);

        Etudiant UpdateEtudiant(Etudiant etudiant);

        PieceJointe AddPieceJointe(PieceJointe PieceJointe);

        IEnumerable<PieceJointe> GetAllPiecesJointes(string id);
    }
}
