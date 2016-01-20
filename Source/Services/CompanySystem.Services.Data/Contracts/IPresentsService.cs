namespace CompanySystem.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Contracts;
    using CompanySystem.Data.Models.Models;
    using Server.DataTransferModels.Presents;

    public interface IPresentsService : IService
    {
        IQueryable<Present> All();

        Task<ICollection<PresentDataTransferModel>> GetAvailablePresents();
    }
}
