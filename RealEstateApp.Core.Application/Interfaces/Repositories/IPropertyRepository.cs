﻿using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<List<PropertyViewModel>> GetPropertiesByAgentIdAsync(string agentId);
    }
}
