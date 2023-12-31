﻿using CheckInSKP.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSKP.Application.Staff.Commands.UpdateStaff
{
    public record UpdateStaffOccupationCommand : IRequest
    {
        public Guid StaffId { get; init; }
        public bool IsPreoccupied { get; init; }
    }

    public class UpdateStaffOccupationCommandHandler : IRequestHandler<UpdateStaffOccupationCommand>
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateStaffOccupationCommandHandler(IStaffRepository staffRepository, IUnitOfWork unitOfWork)
        {
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Handle(UpdateStaffOccupationCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.StaffAggregate.Staff staff = await _staffRepository.GetByIdAsync(request.StaffId) ?? throw new Exception($"Staff with id {request.StaffId} not found");
            staff.UpdateOccupation(request.IsPreoccupied);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return;
        }
    }
}
