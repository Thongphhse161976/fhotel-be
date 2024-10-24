using FHotel.Repository.Models;
using FHotel.Service.DTOs.RefundPolicies;
using FHotel.Services.DTOs.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Refunds
{
    public class RefundResponse
    {
        public Guid RefundId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? RefundStatus { get; set; }
        public DateTime? RefundDate { get; set; }
        public Guid? RefundPolicyId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ReservationResponse? Payment { get; set; }
        public virtual RefundPolicyResponse? RefundPolicy { get; set; }
    }
}
