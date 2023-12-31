﻿using CheckInSKP.Domain.Common;
using CheckInSKP.Domain.Events.StaffEvents;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace CheckInSKP.Domain.Entities.StaffAggregate
{
    public class Staff : DomainEntity
    {
        private readonly Guid _userId;
        public Guid UserId => _userId;

        [Required, StringLength(64)]
        public string PhoneNumber { get; private set; }

        [Required, StringLength(128)]
        public string CardNumber { get; private set; }

        public bool PhoneNotification { get; private set; }
        public bool Preoccupied { get; private set; }

        public TimeOnly MeetingTime { get; private set; }

        private readonly List<TimeLog> _timeLogs = new();
        public IReadOnlyList<TimeLog> TimeLogs => _timeLogs;

        // Constructor for new Staff
        public Staff(Guid userId, string phoneNumber, string cardNumber, bool phoneNotification)
        {
            ValidateInput(phoneNumber, cardNumber);

            _userId = userId;
            PhoneNumber = phoneNumber;
            CardNumber = cardNumber;
            PhoneNotification = phoneNotification;
            Preoccupied = false;
            MeetingTime = new TimeOnly(8, 10, 0);
        }

        // Constructor for existing Staff
        public Staff(Guid userId, string phoneNumber, string cardNumber, bool phoneNotification, bool preoccupied, TimeOnly meetingTime)
        {
            ValidateInput(phoneNumber, cardNumber);

            _userId = userId;
            PhoneNumber = phoneNumber;
            CardNumber = cardNumber;
            PhoneNotification = phoneNotification;
            Preoccupied = preoccupied;
            MeetingTime = meetingTime;
        }

        private void ValidateInput(string phoneNumber, string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length > 64)
                throw new ArgumentException("Invalid phone number.", nameof(phoneNumber));
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length > 128)
                throw new ArgumentException("Invalid card number.", nameof(cardNumber));
        }

        public void AddTimeLog(TimeLog timeLog)
        {
            if (timeLog == null) throw new ArgumentNullException(nameof(timeLog));

            _timeLogs.Add(timeLog);
        }

        public void RemoveTimeLog(TimeLog timeLog)
        {
            if (timeLog == null) throw new ArgumentNullException(nameof(timeLog));
            if (!_timeLogs.Contains(timeLog)) throw new ArgumentException("Time log does not exist.");            
            
            _timeLogs.Remove(timeLog);
        }

        public void UpdateOccupation(bool isPreoccupied)
        {
            Preoccupied = isPreoccupied;
        }
        public void UpdatePhoneNumber(string newPhoneNumber)
        {
            if (string.IsNullOrEmpty(newPhoneNumber) || newPhoneNumber.Length > 64)
            {
                throw new ArgumentException("Invalid new phone number.");
            }

            PhoneNumber = newPhoneNumber;

            AddDomainEvent(new StaffPhoneNumberUpdatedEvent(UserId, PhoneNumber));
        }

        public void UpdatePhoneNotification(bool newPhoneNotification)
        {
            PhoneNotification = newPhoneNotification;
        }

        public void UpdateMeetingTime(TimeOnly newMeetingTime)
        {
            MeetingTime = newMeetingTime;

            AddDomainEvent(new StaffMeetingTimeUpdatedEvent(UserId, MeetingTime));
        }

        public void UpdateCardNumber(string newCardNumber)
        {
            if (string.IsNullOrEmpty(newCardNumber) || newCardNumber.Length > 128)
            {
                throw new ArgumentException("Invalid new card number.");
            }

            CardNumber = newCardNumber;
        }

        public void Update(string phoneNumber, string cardNumber, bool phoneNotification, bool isPreoccupied, TimeOnly meetingTime)
        {
            UpdatePhoneNumber(phoneNumber);
            UpdateCardNumber(cardNumber);
            UpdateOccupation(isPreoccupied);
            UpdatePhoneNotification(phoneNotification);
            UpdateMeetingTime(meetingTime);
        }
    }
}
