using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Documents
{
    public class DocumentRequest
    {
        public Guid DocumentId { get; set; }
        public string? DocumentName { get; set; }
    }
}
