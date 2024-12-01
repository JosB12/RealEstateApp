using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface ISalesTypeService : IGenericService<SaveSalesTypeViewModel, SaleTypeViewModel, SaleType>
    {
        Task<List<SaleTypeViewModel>> GetAllSaleTypesNamesAsync();
        Task<List<SaleTypeViewModel>> GetAllAsync();
        Task DeleteAsync(int id);
        Task EditAsync(int id, SaveSalesTypeViewModel salesTypeSaveViewModel);
        Task CreateAsync(SaveSalesTypeViewModel salesTypeSaveViewModel);
        Task<int> GetSalesTypeCountByIdAsync(int typeId);
        Task<SaveSalesTypeViewModel> GetByIdAsync(int id);
    }
}
