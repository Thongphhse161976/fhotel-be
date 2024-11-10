using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Type
    {
        public Type()
        {
            RoomTypes = new HashSet<RoomType>();
            TypePricings = new HashSet<TypePricing>();
        }

        public Guid TypeId { get; set; }
        public string? TypeName { get; set; }
        public int? MaxOccupancy { get; set; }
        public decimal? MinSize { get; set; }
        public decimal? MaxSize { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<RoomType> RoomTypes { get; set; }
        public virtual ICollection<TypePricing> TypePricings { get; set; }
    }
}
