using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IImprovementService : IGenericService<ImprovementViewModel, SaveImprovementViewModel, Improvement>
    {
        Task<List<ImprovementViewModel>> GetAllImprovementsNamesAsync();
        Task DeleteAsync(int id);
        Task<List<ImprovementViewModel>> GetAllAsync();
        Task<SaveImprovementViewModel> GetByIdAsync(int id);
        Task CreateAsync(SaveImprovementViewModel SaveImprovementViewModel);
        Task EditAsync(int id, SaveImprovementViewModel SaveImprovementViewModel);
    }
}
