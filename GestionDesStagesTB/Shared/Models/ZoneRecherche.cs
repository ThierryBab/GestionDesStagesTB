using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Models
{
    public class ZoneRecherche
    {
        [StringLength(200)]
        public string ValeurRecherchee { get; set; }
    }
}
