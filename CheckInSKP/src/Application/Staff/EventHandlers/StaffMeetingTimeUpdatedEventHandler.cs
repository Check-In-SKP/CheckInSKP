﻿using CheckInSKP.Domain.Events.StaffEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSKP.Application.Staff.EventHandlers
{
    public class StaffMeetingTimeUpdatedEventHandler : INotificationHandler<StaffMeetingTimeUpdatedEvent>
    {
        private readonly ILogger<StaffMeetingTimeUpdatedEventHandler> _logger;

        public StaffMeetingTimeUpdatedEventHandler(ILogger<StaffMeetingTimeUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(StaffMeetingTimeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Meeting time updated for staff {StaffId}", notification.UserId);

            return Task.CompletedTask;
        }
    }
}
