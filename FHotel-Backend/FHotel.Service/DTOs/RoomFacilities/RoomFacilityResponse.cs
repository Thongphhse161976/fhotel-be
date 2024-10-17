using FHotel.Repository.Models;
using FHotel.Service.DTOs.Facilities;
using FHotel.Services.DTOs.RoomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomFacilities
{
    public class RoomFacilityResponse
    {
        public Guid RoomFacilityId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public Guid? FacilityId { get; set; }

        public virtual FacilityResponse? Facility { get; set; }
        public virtual RoomTypeResponse? RoomType { get; set; }
    }
}
