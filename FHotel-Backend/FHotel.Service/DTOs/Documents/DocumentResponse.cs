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
    }
}
