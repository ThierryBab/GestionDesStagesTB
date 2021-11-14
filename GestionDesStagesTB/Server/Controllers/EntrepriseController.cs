using GestionDesStagesTB.Server.Data;
using GestionDesStagesTB.Server.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrepriseController : Controller
    {

        private readonly IEntrepriseRepository _entrepriseRepository;

        public EntrepriseController(IEntrepriseRepository entrepriseRepository)
        {
            _entrepriseRepository = entrepriseRepository;
        }


        [HttpGet("{Id}")]
        public IActionResult GetEntrepriseById(string Id)
        {

            var entrepriseExiste = _entrepriseRepository.GetEntrepriseById(Id);
            if (entrepriseExiste != null)
            {
                // L'étudiant existe retourner l'entité trouvée
                return Ok(entrepriseExiste);
            }
            // L'étudiant n'existe pas retourner une instance Entreprise vide.
            // car retourner null fait bugger la DeserializeAsync dans le dataservice : The input does not contain any JSON tokens. Expected the input to start with a valid JSON token,
            return Ok(new Entreprise());
        }

        [HttpPost]
        public IActionResult CreateEntreprise([FromBody] Entreprise entreprise)
        {
            if (entreprise == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _entrepriseRepository.AddEntreprise(entreprise);

            return Created("entreprise", created);
        }

        [HttpPut]
        public IActionResult UpdateEntreprise([FromBody] Entreprise entreprise)
        {
            if (entreprise == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // S'assurer que le stage existe dans la table avant de faire la mise à jour
            var entrepriseToUpdate = _entrepriseRepository.GetEntrepriseById(entreprise.Id.ToString());

            if (entrepriseToUpdate == null)
                return NotFound();

            _entrepriseRepository.UpdateEntreprise(entreprise);

            return NoContent(); //success
        }
    }
}
