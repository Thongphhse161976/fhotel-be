﻿using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Cities
{
    public class CityResponse
    {
        public Guid CityId { get; set; }
        public string? CityName { get; set; }
        public string? PostalCode { get; set; }
    }
}
