﻿using CheckInSKP.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSKP.Application.Staff.Commands.DeleteStaff
{
    public record DeleteStaffCommand : IRequest
    {
        public Guid StaffId { get; init; }
    }

    public class DeleteStaffCommandHandler : IRequestHandler<DeleteStaffCommand>
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteStaffCommandHandler(IStaffRepository staffRepository, IUnitOfWork unitOfWork)
        {
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.StaffAggregate.Staff staff = await _staffRepository.GetByIdAsync(request.StaffId) ?? throw new Exception($"Staff with id {request.StaffId} not found");

            await _staffRepository.RemoveAsync(staff.UserId);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return;
        }
    }
}
