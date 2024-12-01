using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface ISalesTypeRepository : IGenericRepository<SaleType>
    {
        Task UpdateAsync(SaleType saleType);
        Task<bool> HasAnySaleTypeAsync();
        Task<int> GetSaleTypeCountByIdAsync(int saleTypeId);
    }
}
