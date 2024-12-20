﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.TypePricings
{
    public class TypePricingCreateRequest
    {
        public Guid? TypeId { get; set; } 
        public Guid? DistrictId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Description { get; set; }
        public decimal? PercentageIncrease { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
