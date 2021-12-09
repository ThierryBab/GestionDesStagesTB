using GestionDesStagesTB.Server.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoordonateurController : Controller
    {
        private readonly ICoordonateurRepository _coordonateurRepository;

        public CoordonateurController(ICoordonateurRepository coordonateurRepository)
        {
            _coordonateurRepository = coordonateurRepository;
        }


        [HttpGet("{Id}")]
        public IActionResult GetCoordonateurById(string Id)
        {

            var coordonateurExiste = _coordonateurRepository.GetCoordonateurById(Id);
            if (coordonateurExiste != null)
            {
                // L'étudiant existe retourner l'entité trouvée
                return Ok(coordonateurExiste);
            }
            // L'étudiant n'existe pas retourner une instance Etudiant vide.
            // car retourner null fait bugger la DeserializeAsync dans le dataservice : The input does not contain any JSON tokens. Expected the input to start with a valid JSON token,
            return Ok(new Coordonateur());
        }

        [HttpPost]
        public IActionResult CreateCoordonateur([FromBody] Coordonateur coordonateur)
        {
            if (coordonateur == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _coordonateurRepository.AddCoordonateur(coordonateur);

            return Created("coordonateur", created);
        }

        [HttpPut]
        public IActionResult UpdateCoordonateur([FromBody] Coordonateur coordonateur)
        {
            if (coordonateur == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // S'assurer que le stage existe dans la table avant de faire la mise à jour
            var coordonateurToUpdate = _coordonateurRepository.GetCoordonateurById(coordonateur.Id.ToString());

            if (coordonateurToUpdate == null)
                return NotFound();

            _coordonateurRepository.UpdateCoordonateur(coordonateur);

            return NoContent(); //success
        }
    }
}
