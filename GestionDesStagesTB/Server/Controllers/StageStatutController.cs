using GestionDesStagesTB.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StageStatutController : Controller
    {
        private readonly IStageStatutRepository _stageStatutRepository;
        private readonly IStageRepository _stageRepository;

        public StageStatutController(IStageStatutRepository stageStatutRepository)
        {
            _stageStatutRepository = stageStatutRepository;
        }

        [HttpGet]
        public IActionResult GetAllStageStatuts()
        {
            return Ok(_stageStatutRepository.GetAllStageStatuts());
        }


    }
}
