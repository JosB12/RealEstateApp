

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Model>
           where SaveViewModel : class
           where ViewModel : class
           where Model : class
    {
        Task Update(SaveViewModel vm, int id);

        Task<SaveViewModel> Add(SaveViewModel vm);

        Task<List<SaveViewModel>> AddRange(List<SaveViewModel> vm);

        Task Delete(int id);

        Task<SaveViewModel> GetByIdSaveViewModel(int id);

        Task<List<ViewModel>> GetAllListViewModel();

        IQueryable<ViewModel> GetAllQueryViewModel();
    }
}
