using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface ISalesTypeService : IGenericService<SaveSalesTypeViewModel, SaleTypeViewModel, SaleType>
    {
        Task<List<SaleTypeViewModel>> GetAllSaleTypesNamesAsync();
    }
}
