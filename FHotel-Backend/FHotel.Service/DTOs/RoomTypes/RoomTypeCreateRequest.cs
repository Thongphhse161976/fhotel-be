using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RoomTypes
{
    public class RoomTypeCreateRequest
    {
        public Guid? HotelId { get; set; }
        public Guid? TypeId { get; set; }
        public string? Description { get; set; }
        public decimal? RoomSize { get; set; }
        public int? TotalRooms { get; set; }
        public int? AvailableRooms { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }

    }
}
