using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Rooms
{
    public class RoomRequest
    {
        public Guid RoomId { get; set; }
        public int? RoomNumber { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? Status { get; set; }
        public bool? IsCleaned { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }

    }
}
