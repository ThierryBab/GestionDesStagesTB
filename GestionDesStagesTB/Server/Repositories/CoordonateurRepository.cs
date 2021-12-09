using GestionDesStagesTB.Server.Data;
using GestionDesStagesTB.Server.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Repositories
{
    public class CoordonateurRepository : ICoordonateurRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogger<CoordonateurRepository> _logger;

        public CoordonateurRepository(ApplicationDbContext appDbContext, ILogger<CoordonateurRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }


        public Coordonateur GetCoordonateurById(string Id)
        {
            // Obtenir la fiche de l'étudiant
            return _appDbContext.Coordonateur.AsNoTracking().FirstOrDefault(c => c.Id == Id);
        }

        public Coordonateur AddCoordonateur(Coordonateur coordonateur)
        {
            try
            {
                var addedEntity = _appDbContext.Coordonateur.Add(coordonateur);
                _appDbContext.SaveChanges();
                return addedEntity.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur dans la création d'un enregistrement {ex}");
                return null;
            }
        }

        public Coordonateur UpdateCoordonateur(Coordonateur coordonateur)
        {
            // Rechercher le stage afin d'indiquer au contexte le stage à mettre à jour
            var foundCord = _appDbContext.Coordonateur.FirstOrDefault(e => e.Id == coordonateur.Id);
            if (foundCord != null)
            {
                foundCord.Prenom = coordonateur.Prenom;
                foundCord.Nom = coordonateur.Nom;
                foundCord.Telephone = coordonateur.Telephone;
                _appDbContext.SaveChanges();
            }
            return foundCord;
        }
    }
}
