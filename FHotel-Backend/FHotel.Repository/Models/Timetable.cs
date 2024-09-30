using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Timetable
    {
        public Guid TimetableId { get; set; }
        public Guid? ReceptionistId { get; set; }
        public DateTime? ShiftStart { get; set; }
        public DateTime? ShiftEnd { get; set; }

        public virtual User? Receptionist { get; set; }
    }
}
