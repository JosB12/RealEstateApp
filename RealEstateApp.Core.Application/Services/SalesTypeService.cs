using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Services
{
    public class SalesTypeService : GenericService<SaveSalesTypeViewModel, SaleTypeViewModel, SaleType>, ISalesTypeService
    {
        private readonly ISalesTypeRepository _salesTypeRepository;
        private readonly IMapper _mapper;
        public SalesTypeService(ISalesTypeRepository salesTypeRepository, IMapper mapper) : base(salesTypeRepository, mapper)
        {
            _salesTypeRepository = salesTypeRepository;
        }
        public async Task<List<SaleTypeViewModel>> GetAllSaleTypesNamesAsync()
        {
            var saleTypes = await _salesTypeRepository.GetAllAsync();
            return saleTypes.Select(s => new SaleTypeViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}
