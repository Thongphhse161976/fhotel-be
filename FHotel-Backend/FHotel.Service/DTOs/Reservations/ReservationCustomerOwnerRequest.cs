using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Reservations
{
    public class ReservationCustomerOwnerRequest
    {
        public Guid ownerId { get; set; }
    }
}
