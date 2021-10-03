using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Models
{
    public class StageStatut
    {
        [Key]
        public int StageStatutId { get; set; }

        [StringLength(50)]
        public string DescriptionStatut { get; set; }

    }
}
