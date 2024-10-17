using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomFacilities
{
    public class RoomFacilityRequest
    {

        public Guid? RoomTypeId { get; set; }
        public Guid? FacilityId { get; set; }
    }
}
