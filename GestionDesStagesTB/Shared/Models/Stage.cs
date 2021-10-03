using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Models
{
    public class Stage
    {
        [Key]
        public Guid StageId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Le titre est trop long.")]
        public string Titre { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Le description est trop longue.")]
        public string Description { get; set; }

        // Clé étrangère avec la table StageStatut
        public int StageStatutId { get; set; }
        // Etabir la relation 1:N avec la clé étrangère
        public StageStatut StageStatut { get; set; }

        public bool Salaire { get; set; }

        [Required]
        [Display(Description = "")]
        public TypeTravail TypeTravail { get; set; }

        // Garder le lien entre l'utilisateur entrepreneur et le stage
        [StringLength(450)]
        public string Id { get; set; }

        public DateTime DateCreation { get; set; }
    }
}
