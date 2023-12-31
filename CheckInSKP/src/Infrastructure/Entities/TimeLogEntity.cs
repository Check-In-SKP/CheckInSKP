﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CheckInSKP.Infrastructure.Entities
{
    public class TimeLogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public int TimeTypeId { get; set; }

        [ForeignKey(nameof(TimeTypeId))]
        public TimeTypeEntity? TimeType { get; set; }

        public Guid StaffId { get; set; }

        [ForeignKey(nameof(StaffId))]
        public StaffEntity? Staff { get; set; }
    }
}
