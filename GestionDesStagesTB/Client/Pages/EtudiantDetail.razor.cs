using GestionDesStagesTB.Client.Interfaces;
using GestionDesStagesTB.Client.Services;
using GestionDesStagesTB.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Pages
{
    public partial class EtudiantDetail
    {
        [Inject]
        public IEtudiantDataService EtudiantDataService { get; set; }

        [Inject]
        public IFichierDataService FichierDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Inject]
        public ILogger<EtudiantDetail> Logger { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public string Id { get; set; }

        public Etudiant Etudiant { get; set; } = new Etudiant();

        public List<PieceJointe> PiecesJointes { get; set; } = new List<PieceJointe>();

        protected override async Task OnInitializedAsync()
        {
            // Vérifier si la fiche de l'étudiant existe
            // Si l'étudiant n'existe pas une instance vide Etudiant est retournée par le data service
            Etudiant = (await EtudiantDataService.GetEtudiantById(Id));
            // Appel du service pour obtenir la liste des pièces jointes
            PiecesJointes = (await EtudiantDataService.GetAllPiecesJointes(Id)).ToList();
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/");
        }


        protected void NavigateToStageEdit()
        {
            NavigationManager.NavigateTo("/");
        }

    }
}
