using FHotel.Repository.Models;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.ReservationDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomTypes
{
    public class RoomTypeResponse
    {
        public Guid RoomTypeId { get; set; }
        public Guid? HotelId { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public decimal? RoomSize { get; set; }
        public decimal? BasePrice { get; set; }
        public int? MaxOccupancy { get; set; }
        public int? TotalRooms { get; set; }
        public int? AvailableRooms { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }

        public virtual HotelResponse? Hotel { get; set; }
    }
}
