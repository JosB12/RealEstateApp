using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;

namespace RealEstateApp.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Model> : IGenericService<SaveViewModel, ViewModel, Model>
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
        {
            private readonly IGenericRepository<Model> _repository;
            private readonly IMapper _mapper;

            public GenericService(IGenericRepository<Model> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<SaveViewModel> Add(SaveViewModel vm)
            {
                var entity = _mapper.Map<Model>(vm);
                var addedEntity = await _repository.AddAsync(entity);
                return _mapper.Map<SaveViewModel>(addedEntity);
            }

            public async Task<List<SaveViewModel>> AddRange(List<SaveViewModel> vms)
            {
                var entities = _mapper.Map<List<Model>>(vms);
                var addedEntities = await _repository.AddRangeAsync(entities);
                return _mapper.Map<List<SaveViewModel>>(addedEntities);
            }

            public async Task Delete(int id)
            {
                await _repository.DeleteAsync(id);
            }

            public async Task<SaveViewModel> GetByIdSaveViewModel(int id)
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    throw new KeyNotFoundException($"Entity with id {id} not found");

                return _mapper.Map<SaveViewModel>(entity);
            }

            public async Task<List<ViewModel>> GetAllListViewModel()
            {
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<List<ViewModel>>(entities);
            }

            public IQueryable<ViewModel> GetAllQueryViewModel()
            {
                var query = _repository.GetAllAsQueryable();
                return query.Select(entity => _mapper.Map<ViewModel>(entity));
            }

            public async Task Update(SaveViewModel vm, int id)
            {
                var entity = _mapper.Map<Model>(vm);
                await _repository.UpdateAsync(entity, id);
            }
    }
}
