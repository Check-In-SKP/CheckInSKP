﻿using CheckInMonitorAPI.Data.Repositories.Interfaces;
using CheckInMonitorAPI.Data.Repositories.UnitOfWork.Interfaces;
using CheckInMonitorAPI.Models.Entities;
using CheckInMonitorAPI.Services.Interfaces;

namespace CheckInMonitorAPI.Services.Implementations
{
    public class TimeTypeService : GenericService<User, int>, ITimeTypeService
    {
        private readonly IGenericRepository<User, int> _repository;

        public TimeTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = unitOfWork.GetRepository<User, int>();
        }

        // Other implementations here
    }
}
