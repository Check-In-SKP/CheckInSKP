﻿using CheckInSKP.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSKP.Application.Staff.Commands.DeleteStaffTimeLog
{
    public record DeleteStaffTimeLogCommand : IRequest
    {
        public Guid StaffId { get; init; }
        public int TimeLogId { get; init; }
    }

    public class DeleteStaffTimeLogCommandHandler : IRequestHandler<DeleteStaffTimeLogCommand>
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteStaffTimeLogCommandHandler(IStaffRepository staffRepository, IUnitOfWork unitOfWork)
        {
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task Handle(DeleteStaffTimeLogCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.StaffAggregate.Staff staff = await _staffRepository.GetByIdAsync(request.StaffId) ?? throw new Exception($"Staff with id {request.StaffId} not found");
            var timeLog = staff.TimeLogs.FirstOrDefault(t => t.Id == request.TimeLogId) ?? throw new Exception($"TimeLog with id {request.TimeLogId} not found");

            staff.RemoveTimeLog(timeLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return;
        }
    }
}
