using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Timetable
    {
        public Guid TimetableId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? StaffId { get; set; }
        public DateTime? ShiftDate { get; set; }
        public DateTime? ShiftStart { get; set; }
        public DateTime? ShiftEnd { get; set; }
        public string? Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual User? Staff { get; set; }
    }
}
