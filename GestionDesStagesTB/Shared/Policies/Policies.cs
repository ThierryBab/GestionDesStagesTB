using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Shared.Policies
{
    public  static class Policies
    {
        public const string EstEtudiant = "EstEtudiant";
        public static AuthorizationPolicy EstEtudiantPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                //.RequireClaim("Statut", "Milieu")
                .RequireRole("Etudiant")
                .Build();
        }
    }
}
