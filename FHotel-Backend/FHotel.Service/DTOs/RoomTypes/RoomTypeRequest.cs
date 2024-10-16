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
        public Guid? HotelId { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public decimal? RoomSize { get; set; }
        public int? MaxOccupancy { get; set; }
        public int? TotalRooms { get; set; }
        public int? AvailableRooms { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }

     
    }
}
