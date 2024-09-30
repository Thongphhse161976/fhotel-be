using FHotel.Repository.Models;
using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Timetable
{
    public class TimetableResponse
    {
        public Guid TimetableId { get; set; }
        public Guid? ReceptionistId { get; set; }
        public DateTime? ShiftStart { get; set; }
        public DateTime? ShiftEnd { get; set; }

        public virtual UserResponse? Receptionist { get; set; }
        public virtual UserResponse? RoomAttendant { get; set; }
    }
}
