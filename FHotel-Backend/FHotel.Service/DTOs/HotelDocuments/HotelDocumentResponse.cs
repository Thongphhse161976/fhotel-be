using FHotel.Repository.Models;
using FHotel.Services.DTOs.Documents;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.HotelDocuments
{
    public class HotelDocumentResponse
    {
        public Guid HotelDocumentId { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid? HotelId { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual DocumentResponse? Document { get; set; }
        public virtual HotelResponse? Hotel { get; set; }
    }
}
