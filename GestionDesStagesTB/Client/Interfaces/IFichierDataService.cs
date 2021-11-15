using GestionDesStagesTB.Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GestionDesStagesTB.Client.Services
{
    public interface IFichierDataService
    {
        Task<byte[]> Telecharger(string id);

        Task<IList<UploadResult>> Verser(MultipartFormDataContent content);
    }
}