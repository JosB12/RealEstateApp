﻿using RealEstateApp.Core.Application.ViewModels;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IImprovementService : IGenericService<ImprovementViewModel, ImprovementViewModel, Improvement>
    {
        Task<List<ImprovementViewModel>> GetAllImprovementsNamesAsync();
    }
}
