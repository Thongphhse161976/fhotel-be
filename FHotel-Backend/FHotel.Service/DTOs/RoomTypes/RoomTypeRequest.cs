using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomTypes
{
    public class RoomTypeRequest
    {
        public Guid RoomTypeId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? TypeId { get; set; }
        public string? Description { get; set; }
        public decimal? RoomSize { get; set; }
        public int? TotalRooms { get; set; }
        public int? AvailableRooms { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Note { get; set; }


    }
}
