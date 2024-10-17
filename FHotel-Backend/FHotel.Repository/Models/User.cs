using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
            HotelStaffs = new HashSet<HotelStaff>();
            Hotels = new HashSet<Hotel>();
            Reservations = new HashSet<Reservation>();
            Timetables = new HashSet<Timetable>();
        }

        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Image { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Sex { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual Role? Role { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<HotelStaff> HotelStaffs { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Timetable> Timetables { get; set; }
    }
}
