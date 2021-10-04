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

        public const string EstEntreprise = "EstEntreprise";
        public static AuthorizationPolicy EstEntreprisePolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                //.RequireClaim("Statut", "Milieu")
                .RequireRole("Entreprise")
                .Build();
        }

        public const string EstCoordinateur = "EstCoordinateur";
        public static AuthorizationPolicy EstCoordinateurPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                //.RequireClaim("Statut", "Milieu")
                .RequireRole("Entreprise")
                .Build();
        }
    }
}
