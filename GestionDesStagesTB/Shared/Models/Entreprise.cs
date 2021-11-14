using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Models
{
    public class Entreprise
    {
        // La valeur de ce Id est la même que le Id dans la table AspNetUsers
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        [Required]
        [StringLength(300)]
        public string NomEntreprise { get; set; }


        [StringLength(100)]
        public string NomResponsable { get; set; }


        [StringLength(100)]
        public string PrenomResponsable { get; set; }

        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [StringLength(10)]
        public string PosteTelephonique { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateModification { get; set; }
    }
}
