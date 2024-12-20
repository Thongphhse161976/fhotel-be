﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Services
{
    public class ServiceCreateRequest
    { 
        
        public string? ServiceName { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public string? Image { get; set; }

        public bool? IsActive { get; set; }
    }
}
