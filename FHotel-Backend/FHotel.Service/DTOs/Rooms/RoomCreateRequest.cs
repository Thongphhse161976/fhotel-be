using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Rooms
{
    public class RoomCreateRequest
    {
        public int? RoomNumber { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? Status { get; set; }
        public bool? IsCleaned { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
