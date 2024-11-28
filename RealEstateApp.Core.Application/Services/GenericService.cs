using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RealEstateApp.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity> : IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task Update(SaveViewModel vm, int id)
        {
            Entity entity = _mapper.Map<Entity>(vm);
            await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task<SaveViewModel> Add(SaveViewModel vm)
        {
            Entity entity = _mapper.Map<Entity>(vm);

            entity = await _repository.AddAsync(entity);

            SaveViewModel entityVm = _mapper.Map<SaveViewModel>(entity);

            return entityVm;
        }

        public virtual async Task Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(product);
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            SaveViewModel vm = _mapper.Map<SaveViewModel>(entity);
            return vm;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            var entityList = await _repository.GetAllAsync();

            return _mapper.Map<List<ViewModel>>(entityList);
        }
        public async Task<List<SaveViewModel>> AddRange(List<SaveViewModel> vms)
        {
            var entities = _mapper.Map<List<Entity>>(vms);
            var addedEntities = await _repository.AddRangeAsync(entities);
            return _mapper.Map<List<SaveViewModel>>(addedEntities);
        }

        public IQueryable<ViewModel> GetAllQueryViewModel()
        {
            var query = _repository.GetAllAsQueryable();
            return query.Select(entity => _mapper.Map<ViewModel>(entity));
        }

    }



}
