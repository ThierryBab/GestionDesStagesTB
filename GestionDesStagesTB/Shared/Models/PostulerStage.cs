using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Models
{
    public class PostulerStage
    {

        [Key]
        public int PostulerStageId { get; set; }

        public Guid StageId { get; set; }
        public Stage Stage { get; set; }


        [StringLength(450)]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public Etudiant Etudiant { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePostule { get; set; }
    }
}
