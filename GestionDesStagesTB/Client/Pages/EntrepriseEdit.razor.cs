using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Pages
{
    public partial class EntrepriseEdit
    {
        [Inject]
    public IEntrepriseDataService EntrepriseDataService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }

    public Entreprise Entreprise { get; set; } = new Entreprise();


    protected override async Task OnInitializedAsync()
    {
        // Vérifier si la fiche de l'étudiant existe
        // Si l'étudiant n'existe pas une instance vide Etudiant est retournée par le data service
        Entreprise = (await EntrepriseDataService.GetEntrepriseById(await ObtenirClaim("sub")));
    }

    protected async Task HandleValidSubmit()
    {
        if (string.IsNullOrEmpty(Entreprise.Id)) //new student
        {
            // Obtenir du tableau des revendications le Id de l'utilisateur en cours
            Entreprise.Id = new Guid(await ObtenirClaim("sub")).ToString();
            // Appel du service pour sauvegardeer le nouvel etudiant dans la base de données.
            await EntrepriseDataService.AddEntreprise(Entreprise);
        }

        else
        {
            // Appel du service pour mettre à jour l'etudiant existant dans la base de données.
            await EntrepriseDataService.UpdateEntreprise(Entreprise);
        }
        NavigationManager.NavigateTo("/");
    }

    /// <summary>
    /// Pour obtenir un claim : sid (Id de l'utilisateur actuel), sub, auth_time, idp, amr, role, preffered_username, name
    /// </summary>
    /// <param name="ClaimName"></param>
    /// <returns></returns>
    private async Task<string> ObtenirClaim(string ClaimName)
    {
        // Obtenir tous les revendications (Claims) de l'utilisateur actuellement connecté.            
        var authstate = await GetAuthenticationStateAsync.GetAuthenticationStateAsync();
        var user = authstate.User;
        IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();
        // Mettre les revendications dans un tableau
        _claims = user.Claims;
        // Obtenir du tableau des revendications le Id de l'utilisateur en cours
        return user.FindFirst(c => c.Type == ClaimName)?.Value; ;
    }

    protected void HandleInvalidSubmit()
    {
    }

    protected void NavigateToOverview()
    {
        NavigationManager.NavigateTo("/");
    }
}
}

