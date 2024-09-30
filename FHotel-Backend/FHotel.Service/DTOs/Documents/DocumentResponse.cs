using FHotel.Repository.Models;
using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Documents
{
    public class DocumentResponse
    {
        public Guid DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UserId { get; set; }

        public virtual UserResponse? User { get; set; }
    }
}
