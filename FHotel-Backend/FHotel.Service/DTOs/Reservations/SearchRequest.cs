using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Reservations
{
    public class SearchRequest
    {
        public Guid UserId { get; set; }
        public string? Query { get; set; }
    }

}
