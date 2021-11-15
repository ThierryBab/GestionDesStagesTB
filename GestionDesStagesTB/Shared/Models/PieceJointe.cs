using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Models
{
    public class PieceJointe
    {
        [Key]
        public int PieceJointeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        [Required]
        [StringLength(100)]
        public string StoredFileName { get; set; }

        [StringLength(450)]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public Etudiant Etudiant { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateVersee { get; set; }
    }
}
