using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Models
{
    public class Coordonateur
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        [StringLength(100)]
        public string Nom { get; set; }

        [StringLength(100)]
        public string Prenom { get; set; }

        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

       
    }
}
